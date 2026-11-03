
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

using ViviMod.ViviModCode.Cards;

namespace ViviMod.ViviModCode.Powers;

public class ArcaneRecyclingPower : PowerModel
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

    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool _)
    {
        // Only react to cards exhausted by Vivi.
        if (card.Owner.Creature != Owner)
            return;

        // Only exhausted Attacks qualify.
        if (card.Type != CardType.Attack)
            return;

        var player = Owner.Player;

        if (player == null || Amount <= 0)
            return;

        // Generate one random Black Magic card per stack.
        for (int i = 0; i < Amount; i++)
        {
            CardModel? generatedCard =
                CardFactory.GetDistinctForCombat(
                    player,
                    player.Character.CardPool.GetUnlockedCards(
                        player.UnlockState,
                        player.RunState.CardMultiplayerConstraint)
                        .Where(candidate => candidate is BlackMagicCard),
                    1,
                    player.RunState.Rng.CombatCardGeneration
                ).FirstOrDefault();

            if (generatedCard == null)
                return;

            await CardPileCmd.AddGeneratedCardToCombat(
                generatedCard,
                PileType.Hand,
                player);
        }
    }
}
