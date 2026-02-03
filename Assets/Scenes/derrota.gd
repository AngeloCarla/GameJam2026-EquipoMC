extends Control

func _on_jugar_pressed() -> void:
	# Cambiar a la escena del juego (ajustá la ruta según tu proyecto)
	get_tree().change_scene_to_file("res://Assets/Scenes/Mapa/MapaCompleto.tscn")

func _on_salir_pressed() -> void:
	var confirmacion = ConfirmationDialog.new()
	confirmacion.dialog_text = "¿Estás seguro de que quieres salir del juego?"
	confirmacion.add_button("Sí", true, "OK")
	confirmacion.add_button("No", false, "Cancelar")
	confirmacion.connect("confirmed", Callable(self, "_on_salir_confirmado"))
	add_child(confirmacion)
	confirmacion.popup_centered()

func _on_salir_confirmado() -> void:
	get_tree().quit()
