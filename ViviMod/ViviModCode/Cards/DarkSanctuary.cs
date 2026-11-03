
using System.Collections.Generic;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using ViviMod.ViviModCode.Powers;

namespace ViviMod.ViviModCode.Cards;

public class DarkSanctuary : ViviModCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("BlockPerTurn", 5m)
    ];

    public DarkSanctuary()
        : base(
            2,
            CardType.Power,
            CardRarity.Rare,
            TargetType.Self)
    {
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        await PowerCmd.Apply<DarkSanctuaryPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["BlockPerTurn"].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["BlockPerTurn"].UpgradeValueBy(2m);
    }
}
