using Godot;
using System;

public partial class Mena : StaticBody2D
{
	[Export] private PackedScene pickupScene = null;  // Asigna oel mineral en Inspector
    [Export] private string tipoMineral = "plata"; // Por las dudas
    public void Extract()
    {
        GD.Print($"¡EXTRAYENDO mina de {tipoMineral}!");

        if (pickupScene == null)
        {
            GD.Print("ERROR: No se asigno ninguna escena de pickup en Inspector");
            return;
        }

        var pickup = pickupScene.Instantiate<Node2D>();
        pickup.GlobalPosition = GlobalPosition;
        GetParent().AddChild(pickup);

        QueueFree();  // Mina desaparece
    }
}
