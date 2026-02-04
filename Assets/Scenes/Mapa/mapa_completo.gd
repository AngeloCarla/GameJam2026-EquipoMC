extends Node2D

@export var escena_victoria: String = "res://Assets/Scenes/Victoria.tscn"

func _on_ZonaSegura_body_entered(body: Node) -> void:
	if body.is_in_group("jugador"):
		get_tree().change_scene_to_file(escena_victoria)

func _on_FiltroAire_body_entered(body: Node) -> void:
	if body.is_in_group("jugador"):
		body.aumentar_oxigeno(10)  # aumenta 10 unidades de oxígeno
		# Usamos 'get_parent().queue_free()' para eliminar el filtro que disparó la señal
		self.queue_free()
