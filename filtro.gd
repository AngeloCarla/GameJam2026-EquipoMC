extends Node2D
@export var cantidad_oxigeno: int = 20  # cuánto oxígeno suma este filtro

func _ready() -> void:
	connect("body_entered", Callable(self, "_on_body_entered"))

func _on_body_entered(body: Node) -> void:
	if body.is_in_group("jugador"):  # asegurate que tu Player esté en el grupo "jugador"
		body.aumentar_oxigeno(cantidad_oxigeno)
		queue_free()  # elimina el filtro de la escena
