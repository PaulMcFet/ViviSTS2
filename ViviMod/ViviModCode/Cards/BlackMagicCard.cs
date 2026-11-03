
using MegaCrit.Sts2.Core.Entities.Cards;

namespace ViviMod.ViviModCode.Cards;

public abstract class BlackMagicCard(
    int cost,
    CardType type,
    CardRarity rarity,
    TargetType target)
    : ViviModCard(cost, type, rarity, target)
{
    public bool IsTranceCopy { get; private set; }

    public void MarkAsTranceCopy()
    {
        IsTranceCopy = true;
    }
}
