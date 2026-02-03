using Godot;
using System;

public partial class GasMaskFilter : StaticBody2D
{
	[Export] private float oxygenAmount = 10f;

	public void Interact(Player player)
	{
		GD.Print("¡Encontraste un filtro nuevo!");
		player.AddOxygen(oxygenAmount);

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
