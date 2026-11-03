
using System.Collections.Generic;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

using ViviMod.ViviModCode.Powers;

namespace ViviMod.ViviModCode.Cards;

public class GarnetRegen : PartyAssistCard
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<ViviRegenPower>(3m)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<ViviRegenPower>()
    ];

    public GarnetRegen()
        : base(
            2,
            CardType.Skill,
            CardRarity.Rare,
            TargetType.Self)
    {
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        // Apply Regen for 3 turns.
        await PowerCmd.Apply<ViviRegenPower>(
            choiceContext,
            Owner.Creature,
            3m,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        // Reduce energy cost from 2 to 1.
        EnergyCost.UpgradeBy(-1);
    }
}
