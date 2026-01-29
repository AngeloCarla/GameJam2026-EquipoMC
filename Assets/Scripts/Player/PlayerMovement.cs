using Godot;
using Godot.NativeInterop;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[ExportGroup("Movimiento")]
	[Export] private float walkSpeed = 300f; // Velocidad normal
	[Export] private float runSpeed = 600f; // Velocidad corriendo

	[ExportGroup("Salto")]
	[Export] private float jumpForce = -500f; // Fuerza de salto
	[Export] private float runJumpMultiplier = 1.5f; // x1.5 mas alto el salto al correr
	[Export] private float runJumpBoostX = 200f; // Impulso extra al saltar

	[ExportGroup("Fisica")]
    [Export] private float gravity = 1200f; // Gravedad

    public override void _PhysicsProcess(double delta)
	{
        // ── Entrada del jugador ──
        Vector2 dir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down"); //(Input.GetVector maneja normalized auto)
        bool isRunning = Input.IsKeyPressed(Key.Shift); // Detecta si el jugador está presionando Shift
        // la velocidad actual: con shift corre, sin camina
        float currentSpeed = isRunning ? runSpeed : walkSpeed; // condicion ? verdadero : falso

        // Aplicamos velocidad horizontal basada en input actual
        Velocity = new Vector2(dir.X * currentSpeed, Velocity.Y);

        // ── Gravedad (si no esta en el suelo) ──
        if (!IsOnFloor())
		{
			Velocity += Vector2.Down * gravity * (float)delta;
		}

        // ── Salto ──
        if (IsOnFloor() && Input.IsActionJustPressed("Jump"))
		{
			float jumpY = jumpForce; // Altura base
            Vector2 newVel = Velocity; // Copia de Velocidad

			// Si corre al saltar
			if (isRunning)
			{
				jumpY *= runJumpMultiplier; // Salto mas alto si estas corriendo

                // Impulso horizontal basado en la velocidad actual
                float boostDir = Mathf.Sign(Velocity.X);  // -1, 0 o +1 segun hacia donde ibas
                float boost = boostDir * runJumpBoostX;
                newVel.X += boost;
            }

            newVel.Y = jumpY; // Aplicamos la altura del salto
            Velocity = newVel; // Asignamos todo de una vez

        }

        // ── Aplicar movimiento y manejar colisiones ──
        MoveAndSlide();
	}
}
