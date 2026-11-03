
using System.Collections.Generic;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using ViviMod.ViviModCode.Powers;

namespace ViviMod.ViviModCode.Cards;

public class ArcaneBarrier : ViviModCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("BlockPerSpell", 2m)
    ];

    public ArcaneBarrier()
        : base(
            1,
            CardType.Power,
            CardRarity.Uncommon,
            TargetType.Self)
    {
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        // Apply the Block gained per Black Magic card.
        await PowerCmd.Apply<ArcaneBarrierPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["BlockPerSpell"].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        // Increase Block per spell from 2 to 3.
        DynamicVars["BlockPerSpell"].UpgradeValueBy(1m);
    }
}
