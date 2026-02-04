extends Control



func _on_menu_pressed() -> void:
	get_tree().change_scene_to_file("res://Assets/Scenes/menu.tscn")
	


func _on_salir_pressed() -> void:
	get_tree().quit()
