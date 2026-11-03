
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

public class SteinerMagicSword : PartyAssistCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10m, ValueProp.Move),
        new DynamicVar("DamagePerSpell", 5m)
    ];

    public SteinerMagicSword()
        : base(
            2,
            CardType.Attack,
            CardRarity.Uncommon,
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

        // Count all Black Magic cards played this turn,
        // including generated Trance copies.
        int spellsPlayed =
            staff?.BlackMagicSpellsThisTurn ?? 0;

        // Base damage + 5 damage per Black Magic spell.
        // Upgraded: +7 damage per spell.
        decimal totalDamage =
            DynamicVars.Damage.BaseValue +
            spellsPlayed *
            DynamicVars["DamagePerSpell"].BaseValue;

        await DamageCmd
            .Attack(totalDamage)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        // Upgrade scaling from +5 to +7 per spell.
        DynamicVars["DamagePerSpell"].UpgradeValueBy(2m);
    }
}
