extends Node2D

@export var oxigeno_maximo: int = 100
var oxigeno: int = oxigeno_maximo
var timer: float = 0.0
@export var velocidad_disminucion: float = 1.0  # Parámetro para controlar la velocidad de disminución

func _ready() -> void:
	# Inicializamos la barra con el valor máximo
	$HealthBar.max_value = oxigeno_maximo
	$HealthBar.value = oxigeno

func _process(delta: float) -> void:
	controlar_oxigeno(delta)

func controlar_oxigeno(delta: float) -> void:
	# Disminuir oxígeno con el tiempo
	timer += delta
	if timer >= 1.0:
		oxigeno -= int(velocidad_disminucion)  # aseguramos que reste enteros
		timer = 0.0

	# Asegurarnos de que oxígeno no sea menor que 0
	oxigeno = max(0, oxigeno)

	# Actualizar la barra
	$HealthBar.value = oxigeno

	# Verificar si el oxígeno se acaba
	if oxigeno <= 0:
		game_over()

func aumentar_oxigeno(cantidad: int) -> void:
	oxigeno = min(oxigeno_maximo, oxigeno + cantidad)
	$HealthBar.value = oxigeno

func game_over() -> void:
	# Mostrar cartel de derrota
	print("¡Oxígeno agotado!")  # Mensaje de debug
	get_tree().change_scene_to_file("res://Assets/Scenes/Derrota.tscn")
