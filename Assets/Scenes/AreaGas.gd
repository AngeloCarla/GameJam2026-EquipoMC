extends Area2D

func _ready():
	body_entered.connect(_on_body_entered)
	body_exited.connect(_on_body_exited)

func _on_body_entered(body):
	print("Entró algo:", body)

func _on_body_exited(body):
	print("Salió algo:", body)
