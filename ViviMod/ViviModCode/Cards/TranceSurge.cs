
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using ViviMod.ViviModCode.Relics;

namespace ViviMod.ViviModCode.Cards;

public class TranceSurge : ViviModCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("TranceGain", 50m)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];

    public TranceSurge()
        : base(
            2,
            CardType.Skill,
            CardRarity.Rare,
            TargetType.Self)
    {
    }

    protected override Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        var staff = Owner.Relics
            .OfType<BlackMageStaff>()
            .FirstOrDefault();

        staff?.GainTrance(
            (int)DynamicVars["TranceGain"].BaseValue);

        return Task.CompletedTask;
    }


    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
