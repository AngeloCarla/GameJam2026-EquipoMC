using Godot;
using System;

public partial class PlataPickup : StaticBody2D
{
    [Export] private float healAmount = 20f; // Vida que da

    public void Interact(Player player)
    {
        GD.Print("¡Encontraste PLATA! Sirve para crear medicina");
        player.Heal(healAmount);

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
