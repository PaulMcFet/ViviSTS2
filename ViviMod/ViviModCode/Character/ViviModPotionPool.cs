using BaseLib.Abstracts;
using ViviMod.ViviModCode.Extensions;
using Godot;

namespace ViviMod.ViviModCode.Character;

public class ViviModPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => ViviMod.Color;
    

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}