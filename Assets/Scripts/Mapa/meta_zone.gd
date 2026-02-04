extends Area2D

signal victoria_alcanzada

func _ready():
	body_entered.connect(_on_body_entered)

func _on_body_entered(body):
	if body is CharacterBody2D:
		print("Meta alcanzada")
		emit_signal("victoria_alcanzada")
