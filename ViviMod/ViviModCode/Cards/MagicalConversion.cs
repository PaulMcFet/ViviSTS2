
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ViviMod.ViviModCode.Cards;

public class MagicalConversion : ViviModCard
{
    public MagicalConversion()
        : base(
            0,
            CardType.Skill,
            CardRarity.Common,
            TargetType.Self)
    {
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        // Choose an Attack from the hand.
        var selected = (await CardSelectCmd.FromHand(
            choiceContext,
            Owner,
            new CardSelectorPrefs(
                CardSelectorPrefs.ExhaustSelectionPrompt,
                1),
            card => card.Type == CardType.Attack,
            this
        )).FirstOrDefault();

        if (selected == null)
            return;

        // Exhaust the selected Attack.
        await CardCmd.Exhaust(choiceContext, selected);

        // Generate a random Common Black Magic card.
        CardModel? generated = CardFactory.GetDistinctForCombat(
            Owner,
            Owner.Character.CardPool.GetUnlockedCards(
                Owner.UnlockState,
                Owner.RunState.CardMultiplayerConstraint)
                .Where(card =>
                    card is BlackMagicCard &&
                    card.Rarity == CardRarity.Common),
            1,
            Owner.RunState.Rng.CombatCardGeneration
        ).FirstOrDefault();

        if (generated == null)
            return;

        if (IsUpgraded)
        {
            generated.EnergyCost.AddThisTurn(-1);
        }

        await CardPileCmd.AddGeneratedCardToCombat(
            generated,
            PileType.Hand,
            Owner);
    }
}
