extends Node2D

var oxigeno: int = 100
var timer: float = 0.0
var velocidad_disminucion: float = 1.0  # Parámetro para controlar la velocidad de disminución

func _ready() -> void:
	# Inicializamos la barra con el valor máximo
	$HealthBar.max_value = oxigeno
	$HealthBar.value = oxigeno

func _process(delta: float) -> void:
	controlar_oxigeno(delta)

func controlar_oxigeno(delta: float) -> void:
	# Disminuir oxígeno con el tiempo
	timer += delta
	if timer >= 1.0:
		oxigeno -= velocidad_disminucion
		timer = 0.0

	# Asegurarnos de que oxigeno no sea menor que 0
	oxigeno = max(0, oxigeno)

	# Actualizar la barra
	$HealthBar.value = oxigeno

	# Verificar si el oxígeno se acaba
	if oxigeno <= 0:
		game_over()

func game_over() -> void:
	# Mostrar cartel de derrota
	print("¡Oxígeno agotado!")  # Mensaje de debug
	get_tree().change_scene_to_file("res://Assets/Scenes/Derrota.tscn")
