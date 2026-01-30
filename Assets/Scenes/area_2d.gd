extends Area2D



func _on_body_entered(body: Node2D) -> void:
	if body.has_method("EntroAlArea"):
		body.EntroAlArea()
		print("Entraste")


func _on_body_exited(body: Node2D) -> void:
	if body.has_method("SalioDelArea"):
		body.SalioDelArea()
		print("Saliste")
