
using System.Collections.Generic;
using System.Diagnostics;
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

public class ViviRegenPower : PowerModel
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

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        Debug.WriteLine(
            "[ViviMod] ViviRegenPower.AfterSideTurnEnd invoked.");

        // Only activate on the owner's turn.
        if (!participants.Contains(Owner))
        {
            Debug.WriteLine(
                "[ViviMod] Regen skipped: owner not participating.");
            return;
        }

        if (!Owner.IsAlive || Amount <= 0)
        {
            Debug.WriteLine(
                "[ViviMod] Regen skipped: owner dead or duration expired.");
            return;
        }

        Debug.WriteLine(
            $"[ViviMod] Regen healing 1 HP. Remaining duration: {Amount}");

        // Heal exactly 1 HP.
        await CreatureCmd.Heal(Owner, 1m);

        // Consume one turn of Regen.
        await PowerCmd.Decrement(this);
    }
}
