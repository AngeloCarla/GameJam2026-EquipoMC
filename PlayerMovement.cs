using Godot;
using Godot.NativeInterop;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	private float walkSpeed = 300f; // Velocidad normal
	private float runSpeed = 600f; // Velocidad corriendo

	private float gravity = 1200f; // Gravedad
	private float jumpForce = -500f; // Fuerza de salto
    public override void _PhysicsProcess(double delta)
	{
		// Movimiento Horizontal
		float horizontal = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");

		Vector2 dir = new Vector2(horizontal, 0).Normalized();

        // Detecta si el jugador está presionando Shift
        bool isRunning = Input.IsKeyPressed(Key.Shift);

		// la velocidad actual: con shift corre, sin camina
		float currentSpeed = isRunning ? runSpeed : walkSpeed; // condicion ? verdadero : falso

		Velocity = new Vector2(dir.X * currentSpeed, Velocity.Y);

		// Gravedad
		if (!IsOnFloor()) // Si no esta en el suelo
		{
			Velocity += Vector2.Down * gravity * (float)delta;
		}

		// Salto
		if(IsOnFloor() && Input.IsActionJustPressed("Jump")) // Si esta en el suelo y aprieta espacio
		{
			Velocity = new Vector2(Velocity.X, jumpForce);
		}

		MoveAndSlide();
	}
}
