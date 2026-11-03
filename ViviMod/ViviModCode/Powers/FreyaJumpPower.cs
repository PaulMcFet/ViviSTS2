
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
using MegaCrit.Sts2.Core.ValueProps;

namespace ViviMod.ViviModCode.Powers;

public class FreyaJumpPower : PowerModel
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

        // Find all living enemies.
        var enemies = combatState
            .GetOpponentsOf(Owner)
            .Where(enemy => enemy.IsAlive)
            .ToList();

        // Consume the effect if no enemies remain.
        if (enemies.Count == 0)
        {
            await PowerCmd.Remove(this);
            return;
        }

        // Store the delayed damage amount.
        int damage = Amount;

        // Choose a random living enemy.
        Creature? target = player.RunState.Rng
            .CombatTargets.NextItem(enemies);

        // Protect against a null target.
        if (target == null)
        {
            await PowerCmd.Remove(this);
            return;
        }

        // Deal the delayed damage.
        await CreatureCmd.Damage(
            new ThrowingPlayerChoiceContext(),
            target,
            damage,
            ValueProp.Unpowered,
            null,
            null);

        // Consume the delayed effect.
        await PowerCmd.Remove(this);
    }
}
