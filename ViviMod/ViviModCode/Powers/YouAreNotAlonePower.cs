
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

using ViviMod.ViviModCode.Cards;
using ViviMod.ViviModCode.Relics;

namespace ViviMod.ViviModCode.Powers;

public class YouAreNotAlonePower : PowerModel
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
        // Only respond to Vivi's cards.
        if (cardPlay.Player != Owner.Player)
            return;

        // Only Party Assist cards qualify.
        if (cardPlay.Card is not PartyAssistCard partyCard)
            return;

        // Generated copies cannot generate further copies.
        if (partyCard.IsPartyAssistCopy)
            return;

        // Only activate during Trance.
        var player = Owner.Player;

        if (player == null || Amount <= 0)
            return;

        var staff = player.Relics
            .OfType<BlackMageStaff>()
            .FirstOrDefault();

        if (staff?.IsInTrance != true)
            return;

        // Generate one copy per stack.
        var copies = new List<CardModel>();

        for (int i = 0; i < Amount; i++)
        {
            var copy = (PartyAssistCard)partyCard.CreateClone();

            // Prevent recursive copying.
            copy.MarkAsPartyAssistCopy();

            // Copies cost 0 and disappear after use or turn end.
            copy.SetToFreeThisCombat();
            copy.AddKeyword(CardKeyword.Exhaust);
            copy.AddKeyword(CardKeyword.Ethereal);

            copies.Add(copy);
        }

        await CardPileCmd.AddGeneratedCardsToCombat(
            copies,
            PileType.Hand,
            player);
    }
}
