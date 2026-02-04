using Godot;

public partial class MapaCompleto : Node2D
{
	private Player player;
	private Area2D metaZone;

	public override void _Ready()
	{
		// Player (aunque el nodo se llame CharacterBody2D)
		player = GetNode<Player>("CharacterBody2D");
		player.JugadorDerrotado += OnJugadorDerrotado;

		// Meta (zona de victoria)
		metaZone = GetNode<Area2D>("MetaZone");
		metaZone.Connect("victoria_alcanzada", new Callable(this, nameof(OnVictoria)));
	}

	// ───── DERROTA ─────
	private void OnJugadorDerrotado()
	{
		GD.Print("Jugador derrotado, cambiando escena...");
		CallDeferred(nameof(CambiarAEscenaDerrota));
	}

	private void CambiarAEscenaDerrota()
	{
		GetTree().ChangeSceneToFile("res://Assets/Scenes/victoria y derrota/derrota.tscn");
	}

	// ───── VICTORIA ─────
	private void OnVictoria()
	{
		GD.Print("Victoria alcanzada, cambiando escena...");
		CallDeferred(nameof(CambiarAEscenaVictoria));
	}

	private void CambiarAEscenaVictoria()
	{
		GetTree().ChangeSceneToFile("res://Assets/Scenes/victoria y derrota/victoria.tscn");
	}
}
