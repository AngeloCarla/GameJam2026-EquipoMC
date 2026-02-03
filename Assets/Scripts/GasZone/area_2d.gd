extends Area2D
@export var drainMultiplier = 4.0

func _ready():
	# CONECTA LOS SIGNALS (OBLIGATORIO)
	body_entered.connect(_on_body_entered)
	body_exited.connect(_on_body_exited)
	
func _on_body_entered(body: Node2D) -> void:
	# Llama el metodo en player 
	if body.is_in_group("player") or body.get_class() == "CharacterBody2D":
		if body.has_method("SetOxygenDrainMultiplier"):
			body.SetOxygenDrainMultiplier(drainMultiplier)
			print("Golpeado")

func _on_body_exited(body: Node2D) -> void:
	if body.has_method("SetOxygenDrainMultiplier"):
		body.SetOxygenDrainMultiplier(1.0) # Vuelve al normal
		print("Saliste")
