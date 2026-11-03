
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

using ViviMod.ViviModCode.Relics;

namespace ViviMod.ViviModCode.Cards;

public class FreyaDragonCrest : PartyAssistCard
{
    private const int MaxUniqueSpells = 5;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8m, ValueProp.Move)
    ];

    public FreyaDragonCrest()
        : base(
            2,
            CardType.Attack,
            CardRarity.Rare,
            TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);

        // Find Vivi's Black Mage Staff.
        var staff = Owner.Relics
            .OfType<BlackMageStaff>()
            .FirstOrDefault();

        // Count unique Black Magic spells played this combat.
        int uniqueSpells =
            staff?.UniqueBlackMagicSpellsPlayed ?? 0;

        // Count no more than five unique spells.
        int spellCount = Math.Min(
            Math.Max(uniqueSpells, 0),
            MaxUniqueSpells);

        // Calculate the total damage.
        decimal totalDamage =
            DynamicVars.Damage.BaseValue * spellCount;

        // No Black Magic played means no damage.
        if (totalDamage <= 0)
            return;

        // Deal the calculated damage.
        await DamageCmd
            .Attack(totalDamage)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        // Increase damage per unique spell from 8 to 10.
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
