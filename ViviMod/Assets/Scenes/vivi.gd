
using Godot;

public partial class ViviCharacter : Node2D
{
	private AnimatedSprite2D _sprite = null!;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>(
            "AnimatedSprite2D"
		);

		// Start with the idle animation.
		_sprite.Play("Idle");
	}

	public void PlayIdle()
	{
		_sprite.Play("Idle");
	}

	public void PlayCasting()
	{
		_sprite.Play("Casting");
	}

	public void PlayMagicSpell()
	{
		_sprite.Play("MagSpell");
	}

	public void PlayPhysicalAttack()
	{
		_sprite.Play("PhysAttack");
	}

	public void PlayVictory()
	{
		_sprite.Play("Start_Victory");
	}
}
