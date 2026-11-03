
extends Node2D

@onready var sprite: AnimatedSprite2D = $Visuals/AnimatedSprite2D

func _ready() -> void:
	sprite.play("Idle")

func play_idle() -> void:
	sprite.play("Idle")

func play_casting() -> void:
	sprite.play("Casting")

func play_magic_spell() -> void:
	sprite.play("MagSpell")

func play_physical_attack() -> void:
	sprite.play("PhysAttack")

func play_victory() -> void:
	sprite.play("Start_Victory")
