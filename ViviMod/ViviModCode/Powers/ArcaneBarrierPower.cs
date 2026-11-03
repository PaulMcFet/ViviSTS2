
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

using ViviMod.ViviModCode.Cards;

namespace ViviMod.ViviModCode.Powers;

public class ArcaneBarrierPower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType =>
        PowerStackType.Counter;

    public override LocString Description
    {
        get
        {
            var description = base.Description;
            description.Add("Amount", Amount);
            return description;
        }
    }

    public override async Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        // Only trigger for Vivi's cards.
        if (cardPlay.Player != Owner.Player)
            return;

        // Only Black Magic cards qualify.
        if (cardPlay.Card is not BlackMagicCard)
            return;

        // Do not trigger if Vivi is dead or the power is inactive.
        if (!Owner.IsAlive || Amount <= 0)
            return;

        // Gain Block equal to the current power stacks.
        await CreatureCmd.GainBlock(
            Owner,
            new BlockVar(Amount, ValueProp.Unpowered),
            null);
    }
}
