
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using ViviMod.ViviModCode.Relics;

namespace ViviMod.ViviModCode.Cards;

public class Focus : ViviModCard
{
    private int _tranceGain = 20;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];

    public Focus()
        : base(
            1,
            CardType.Skill,
            CardRarity.Basic,
            TargetType.Self)
    {
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        // Find Vivi's Black Mage's Staff relic.
        var staff = Owner.Relics
            .OfType<BlackMageStaff>()
            .FirstOrDefault();

        // Gain 20 Trance (30 if upgraded).
        // TranceState ignores this while Trance is active.
        staff?.GainTrance(_tranceGain);

        // Draw one card.
        await CardPileCmd.Draw(
            choiceContext,
            DynamicVars.Cards.BaseValue,
            Owner);
    }

    protected override void OnUpgrade()
    {
        _tranceGain += 10;
    }
}
