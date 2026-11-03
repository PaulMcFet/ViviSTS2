
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace ViviMod.ViviModCode.Powers;

public class DelayedBlockPower : PowerModel
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
        // Only trigger at the beginning of Vivi's turn.
        if (!participants.Contains(Owner))
            return;

        if (!Owner.IsAlive || Amount <= 0)
            return;

        // Grant the accumulated delayed Block.
        int blockAmount = Amount;

        await CreatureCmd.GainBlock(
            Owner,
            new BlockVar(blockAmount, ValueProp.Unpowered),
            null);

        // Consume the delayed effect.
        await PowerCmd.Remove(this);
    }
}
