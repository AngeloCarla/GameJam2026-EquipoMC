extends Node2D

@export var escena_victoria: String = "res://Assets/Scenes/Victoria.tscn"

func _on_ZonaSegura_body_entered(body):
	if body.is_in_group("player"):
		get_tree().change_scene_to_file(escena_victoria)
