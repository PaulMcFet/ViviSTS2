
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

using ViviMod.ViviModCode.Relics;

namespace ViviMod.ViviModCode.Powers;

public class MagicalResiduePower : PowerModel
{
    private bool _triggeredThisTurn;

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

    public override Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool _)
    {
        // Only respond to cards exhausted by Vivi.
        if (card.Owner.Creature != Owner)
            return Task.CompletedTask;

        // Trigger at most once per turn.
        if (_triggeredThisTurn)
            return Task.CompletedTask;

        _triggeredThisTurn = true;

        var staff = Owner.Player?.Relics
            .OfType<BlackMageStaff>()
            .FirstOrDefault();

        staff?.GainTrance((int)Amount);

        return Task.CompletedTask;
    }

    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        // Reset at the beginning of Vivi's turn.
        if (participants.Contains(Owner))
            _triggeredThisTurn = false;

        return Task.CompletedTask;
    }
}
