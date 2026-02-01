extends Area2D
@export var drainMultiplier = 4.0

func _on_body_entered(body: Node2D) -> void:
	# Llama el metodo en player 
	if body.has_method("SetOxygenDrainMultiplier"):
		# Cada que esta en la zona baja x4
		body.SetOxygenDrainMultiplier(drainMultiplier)
		#print("Golpeado")

func _on_body_exited(body: Node2D) -> void:
	if body.has_method("SetOxygenDrainMultiplier"):
		body.SetOxygenDrainMultiplier(1.0) # Vuelve al normal
		#print("Saliste")
