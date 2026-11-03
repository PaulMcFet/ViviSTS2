
using Godot;

public partial class ViviAnimationController : Node2D
{
	private AnimatedSprite2D _sprite = null!;

	public override void _Ready()
	{
		GD.Print("[ViviMod] Animation controller loaded!");

		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_sprite.Play("Idle");
	}
}
