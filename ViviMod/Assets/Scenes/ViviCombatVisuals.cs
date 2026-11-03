
using Godot;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace ViviMod.ViviModCode.Character;

public partial class ViviCombatVisuals : NCreatureVisuals
{
	private AnimatedSprite2D _sprite = null!;

	public override void _Ready()
	{
		GD.Print("[ViviMod] ViviCombatVisuals._Ready() executed");

		base._Ready();

		_sprite = GetNode<AnimatedSprite2D>(
			"Visuals/AnimatedSprite2D"
		);

		_sprite.Play("Idle");
	}
}
