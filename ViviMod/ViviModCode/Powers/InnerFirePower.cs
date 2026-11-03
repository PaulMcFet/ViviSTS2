
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

using ViviMod.ViviModCode.Cards;
using ViviMod.ViviModCode.Relics;

namespace ViviMod.ViviModCode.Powers;

public class InnerFirePower : PowerModel
{
    private int _spellCount;

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

    public override Task AfterCardPlayed(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        // Only count Vivi's cards.
        if (cardPlay.Player != Owner.Player)
            return Task.CompletedTask;

        // Only Black Magic spells qualify.
        if (cardPlay.Card is not BlackMagicCard spell)
            return Task.CompletedTask;

        // Trance-generated copies cannot contribute.
        if (spell.IsTranceCopy)
            return Task.CompletedTask;

        var staff = Owner.Player?.Relics
            .OfType<BlackMageStaff>()
            .FirstOrDefault();

        // No additional charge while Trance is active.
        if (staff == null || staff.IsInTrance)
            return Task.CompletedTask;

        _spellCount++;

        // Every third qualifying spell grants charge.
        if (_spellCount >= 3)
        {
            _spellCount = 0;
            staff.GainTrance((int)Amount);
        }

        return Task.CompletedTask;
    }
}
