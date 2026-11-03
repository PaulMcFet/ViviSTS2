
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BaseLib.Abstracts;
using BaseLib.Utils;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Combat;


using ViviMod.ViviModCode.Cards;
using ViviMod.ViviModCode.Character;
using ViviMod.ViviModCode.Mechanics;
using ViviMod.ViviModCode.Powers;

namespace ViviMod.ViviModCode.Relics;

[Pool(typeof(ViviModRelicPool))]
public class BlackMageStaff : CustomRelicModel
{
    // Tracks current Trance charge and active Trance state.
    private TranceState _trance = new();

    // Tracks different Black Magic spells played this combat.
    private BlackMagicHistory _blackMagicHistory = new();

    // Counts all Black Magic cards played during the current turn.
    // Includes Trance-generated copies.
    private int _blackMagicSpellsThisTurn;

    public int TranceCharge => _trance.Charge;

    public bool IsInTrance => _trance.IsActive;

    public int UniqueBlackMagicSpellsPlayed =>
        _blackMagicHistory.UniqueSpellCount;

    public int BlackMagicSpellsThisTurn =>
        _blackMagicSpellsThisTurn;

    /// <summary>
    /// Requests Trance charge from another card or effect.
    /// TranceState determines whether the charge is allowed.
    /// </summary>
    public void GainTrance(int amount)
    {
        _trance.GainCharge(amount);
        InvokeDisplayAmountChanged();
    }

    /// <summary>
    /// Number of copies generated per Trance activation.
    /// Normal relic: 1.
    /// Upgraded relic: 2.
    /// </summary>
    protected virtual int CopiesPerActivation => 1;

    /// <summary>
    /// Charge consumed when Black Magic activates Doublecast.
    /// Trance Efficiency reduces the cost from 20 to 15.
    /// </summary>
    protected virtual int ActivationChargeCost
    {
        get
        {
            bool hasTranceEfficiency = Owner.Creature.Powers
                .OfType<TranceEfficiencyPower>()
                .Any();

            return hasTranceEfficiency ? 15 : 20;
        }
    }

    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    public override bool ShowCounter => true;

    public override int DisplayAmount =>
        _trance.Charge;

    /// <summary>
    /// Reset Trance, spell history and turn counter
    /// at the beginning of each combat.
    /// </summary>
    public override Task BeforeCombatStart()
    {
        _trance.Reset();
        _blackMagicHistory.Reset();
        _blackMagicSpellsThisTurn = 0;

        InvokeDisplayAmountChanged();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Reset the Black Magic spell counter at the start
    /// of each of Vivi's turns.
    /// </summary>
    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (participants.Contains(Owner.Creature))
        {
            _blackMagicSpellsThisTurn = 0;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gain 5 Trance charge per point of actual HP lost.
    /// </summary>
    public override Task AfterCurrentHpChanged(
        Creature creature,
        decimal delta)
    {
        if (creature != Owner.Creature || delta >= 0)
            return Task.CompletedTask;

        int hpLost = (int)Math.Ceiling(-delta);

        _trance.OnDamageTaken(hpLost);

        InvokeDisplayAmountChanged();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles Black Magic turn counting, unique spell
    /// history, Trance charging and Doublecast copies.
    /// </summary>
    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        // Only react to cards played by Vivi.
        if (cardPlay.Player != Owner)
            return;

        // Only Black Magic cards interact with this relic.
        if (cardPlay.Card is not BlackMagicCard spell)
            return;

        // Count every Black Magic card played this turn.
        // Generated Trance copies count towards card synergies.
        _blackMagicSpellsThisTurn++;

        // Record the spell type for Freya Dragon's Crest.
        // Repeated plays of the same spell count only once.
        _blackMagicHistory.Record(spell);

        // Charge Trance if eligible.
        // Relic-generated copies never charge Trance.
        bool enteredTrance =
            _trance.OnBlackMagicPlayed(spell.IsTranceCopy);

        InvokeDisplayAmountChanged();

        // The spell that enters Trance does not Doublecast.
        // Relic-generated copies cannot trigger Doublecast.
        // No copies are generated outside active Trance.
        if (enteredTrance ||
            spell.IsTranceCopy ||
            !_trance.IsActive)
        {
            return;
        }

        if (Owner.Creature.CombatState == null)
            return;

        // ActivationChargeCost automatically accounts
        // for the Trance Efficiency Power.
        if (!_trance.TrySpendActivation(
            spell.IsTranceCopy,
            ActivationChargeCost))
        {
            return;
        }

        InvokeDisplayAmountChanged();

        var copies = new List<CardModel>();

        for (int i = 0; i < CopiesPerActivation; i++)
        {
            var copy = (BlackMagicCard)spell.CreateClone();

            // Prevent further Trance charging
            // or recursive copy generation.
            copy.MarkAsTranceCopy();

            // Generated copies cost 0 for this combat.
            copy.SetToFreeThisCombat();

            // Exhaust when played.
            copy.AddKeyword(CardKeyword.Exhaust);

            // Exhaust at end of turn if unplayed.
            copy.AddKeyword(CardKeyword.Ethereal);

            copies.Add(copy);
        }

        // Add generated cards directly to Vivi's hand.
        await CardPileCmd.AddGeneratedCardsToCombat(
            copies,
            PileType.Hand,
            Owner);
    }

    /// <summary>
    /// Clear combat-specific state after combat.
    /// </summary>
    public override Task AfterCombatEnd(CombatRoom room)
    {
        _trance.Reset();
        _blackMagicHistory.Reset();
        _blackMagicSpellsThisTurn = 0;

        InvokeDisplayAmountChanged();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Ensure cloned relics have independent mutable state.
    /// The integer turn counter is copied by value.
    /// </summary>
    protected override void DeepCloneFields()
    {
        base.DeepCloneFields();

        _trance = _trance.Clone();
        _blackMagicHistory = _blackMagicHistory.Clone();
    }
}
