
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace ViviMod.ViviModCode.Powers;

public class DrawNextTurnPower : PowerModel
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

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        // Only trigger at the start of Vivi's turn.
        if (!participants.Contains(Owner))
            return;

        if (!Owner.IsAlive || Amount <= 0)
            return;

        var player = Owner.Player;

        if (player == null)
            return;

        // Draw the accumulated number of cards.
        await CardPileCmd.Draw(
            new ThrowingPlayerChoiceContext(),
            Amount,
            player);

        // Consume the delayed draw effect.
        await PowerCmd.Remove(this);
    }
}
