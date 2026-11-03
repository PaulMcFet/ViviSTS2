
using Godot;

public partial class Vivi : Node2D
{
	private AnimatedSprite2D _sprite = null!;

	public override void _Ready()
	{
		GD.Print("[ViviMod] ViviCharacter root script is running");

		_sprite = GetNode<AnimatedSprite2D>(
			"Visuals/AnimatedSprite2D"
		);

		_sprite.Play("Idle");
	}

	public void PlayAnimation(string animation)
	{
		if (!_sprite.SpriteFrames.HasAnimation(animation))
		{
			GD.PrintErr(
				$"[Vivi] Animation not found: {animation}"
			);
			return;
		}

		GD.Print($"[Vivi] Playing animation: {animation}");
		_sprite.Play(animation);
	}

	private void OnAnimationFinished()
	{
		if (_sprite.Animation != "Idle")
		{
			PlayAnimation("Idle");
		}
	}
}
