
using MegaCrit.Sts2.Core.Entities.Cards;

namespace ViviMod.ViviModCode.Cards;

public abstract class PartyAssistCard : ViviModCard
{
    public bool IsPartyAssistCopy { get; private set; }

    protected PartyAssistCard(
        int energyCost,
        CardType cardType,
        CardRarity rarity,
        TargetType targetType)
        : base(energyCost, cardType, rarity, targetType)
    {
    }

    public void MarkAsPartyAssistCopy()
    {
        IsPartyAssistCopy = true;
    }
}
