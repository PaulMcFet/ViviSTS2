
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

using ViviMod.ViviModCode.Relics;

namespace ViviMod.ViviModCode.Cards;

public class CoveringFire : BlackMagicCard
{
    public override bool GainsBlock => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(6m, ValueProp.Move),
        new DynamicVar("BlockPerSpell", 3m)
    ];

    public CoveringFire()
        : base(
            1,
            CardType.Skill,
            CardRarity.Common,
            TargetType.Self)
    {
    }

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay)
    {
        // Read the number of Black Magic spells played
        // during Vivi's current turn.
        var staff = Owner.Relics
            .OfType<BlackMageStaff>()
            .FirstOrDefault();

        int spellsPlayed =
            staff?.BlackMagicSpellsThisTurn ?? 0;

        // Base Block + 3 per Black Magic spell.
        int totalBlock =
            DynamicVars.Block.IntValue +
            spellsPlayed *
            DynamicVars["BlockPerSpell"].IntValue;

        await CreatureCmd.GainBlock(
            Owner.Creature,
            totalBlock,
            ValueProp.Move,
            cardPlay);
    }

    protected override void OnUpgrade()
    {
        // Increase base Block from 6 to 9.
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}
