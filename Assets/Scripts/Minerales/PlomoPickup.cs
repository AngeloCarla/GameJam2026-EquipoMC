using Godot;
using System;

public partial class PlomoPickup : StaticBody2D
{
	[Export] private float durationEffect = 5f; // Duracion del efecto	
	public void Interact(Player player)
	{
		GD.Print("¡Encontraste PLOMO! Sirve para protegerse de los gases toxicos");
		player.ActivateShield(durationEffect);

		PoofEffect();
	}

	private void PoofEffect()
	{
        var tween = CreateTween();
        tween.TweenProperty(this, "scale", Vector2.One * 1.5f, 0.2f);
        tween.Parallel().TweenProperty(this, "modulate:a", 0f, 0.3f);
        tween.TweenCallback(Callable.From(QueueFree));
    }
}
