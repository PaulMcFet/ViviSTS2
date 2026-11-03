
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

using ViviMod.ViviModCode.Relics;

namespace ViviMod.ViviModCode.Powers;

public class DarkSanctuaryPower : PowerModel
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
        if (!participants.Contains(Owner) || !Owner.IsAlive)
            return;

        // Base Block scales with power stacks.
        int block = (int)Amount;

        // Gain an additional 5 Block during Trance.
        var staff = Owner.Player?.Relics
            .OfType<BlackMageStaff>()
            .FirstOrDefault();

        if (staff?.IsInTrance == true)
            block += 5;

        await CreatureCmd.GainBlock(
            Owner,
            new BlockVar(block, ValueProp.Move),
            null);
    }
}
