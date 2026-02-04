extends AudioStreamPlayer

var background = preload("res://Assets/Scenes/Music/background.mp3")
var gameover = preload("res://Assets/Scenes/Music/gameover.mp3")

func _ready() -> void:
	stream = background
	play()

func audioGameOver():
	stop()
	stream = gameover
	play()
