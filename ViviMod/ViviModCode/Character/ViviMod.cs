using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using ViviMod.ViviModCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using ViviMod.ViviModCode.Cards;
using ViviMod.ViviModCode.Relics;

namespace ViviMod.ViviModCode.Character;

public class ViviMod : PlaceholderCharacterModel
{
    public ViviMod()
    {
        const string path = "res://Assets/Scenes/Vivi.tscn";

        GD.Print("[ViviMod] Scene exists: " + ResourceLoader.Exists(path));

        var scene = ResourceLoader.Load<PackedScene>(path);

        GD.Print("[ViviMod] Scene loaded: " + (scene != null));

        if (scene != null)
        {
            var instance = scene.Instantiate();

            GD.Print("[ViviMod] Scene instantiated: " + (instance != null));
            GD.Print("[ViviMod] Root type: " + instance.GetType().Name);

            instance.QueueFree();
        }
    }
    public const string CharacterId = "ViviMod";
    
    public static readonly Color Color = new("4097f5");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 65;
    public override string CustomVisualPath
    {
        get
        {
            GD.Print("[ViviMod] CustomVisualPath accessed");
            GD.Print("[ViviMod] Returning: res://Assets/Scenes/Vivi.tscn");
            return "res://Assets/Scenes/Vivi.tscn";
        }
    }
    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),
        ModelDb.Card<StrikeIronclad>(),

        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),
        ModelDb.Card<DefendIronclad>(),

        ModelDb.Card<Fire>(),
        ModelDb.Card<Focus>()

    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BlackMageStaff>()
    ];   
    
    public override CardPoolModel CardPool => ModelDb.CardPool<ViviModCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<ViviModRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<ViviModPotionPool>();
    
    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets. 
        These are just some of the simplest assets, given some placeholders to differentiate your character with. 
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}