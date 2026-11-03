using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using ViviMod.ViviModCode.Character;
using ViviMod.ViviModCode.Extensions;

namespace ViviMod.ViviModCode.Potions;

[Pool(typeof(ViviModPotionPool))]
public abstract class ViviModPotion : CustomPotionModel
{
	public override string? CustomPackedImagePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
	public override string? CustomPackedOutlinePath =>
		$"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}