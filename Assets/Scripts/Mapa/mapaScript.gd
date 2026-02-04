extends Node2D


func _on_jugador_derrotado():
	get_tree().change_scene("res://UI/derrota.tscn")
