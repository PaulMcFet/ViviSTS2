
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using ViviMod.ViviModCode.Relics;

namespace ViviMod.ViviModCode.Cards;

public class BurningKnowledge : ViviModCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Cards", 2m),
        new DynamicVar("TrancePerCard", 10m)
    ];

    public BurningKnowledge()
        : base(
            1,
            CardType.Skill,
            CardRarity.Uncommon,
            TargetType.Self)
    {
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        var selectedCards = await CardSelectCmd.FromHand(
            choiceContext,
            Owner,
            new CardSelectorPrefs(
                CardSelectorPrefs.ExhaustSelectionPrompt,
                DynamicVars["Cards"].IntValue),
            null,
            this);

        int exhaustedCount = 0;

        foreach (var selectedCard in selectedCards)
        {
            await CardCmd.Exhaust(
                choiceContext,
                selectedCard);

            exhaustedCount++;
        }

        var staff = Owner.Relics
            .OfType<BlackMageStaff>()
            .FirstOrDefault();

        staff?.GainTrance(
            exhaustedCount *
            DynamicVars["TrancePerCard"].IntValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Cards"].UpgradeValueBy(1m);
    }
}
