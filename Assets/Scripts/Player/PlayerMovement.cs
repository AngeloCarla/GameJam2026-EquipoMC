using Godot;
using Godot.NativeInterop;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[ExportGroup("Oxígeno")]
	[Export] private int maxOxygen = 100;   // cantidad máxima
	private float currentOxygen;              // oxígeno actual
	private float walkDrainRate = 1f;   // oxígeno por segundo en reposo o caminando
	private float runDrainMultiplier = 4f; // multiplicador si corre
	
	private ProgressBar OxygenBar;
	
	[ExportGroup("Movimiento")]
	[Export] private float walkSpeed = 10f; // Velocidad normal
	[Export] private float runSpeed = 5f; // Velocidad corriendo

	[ExportGroup("Salto")]
	[Export] private float jumpForce = -10f; // Fuerza de salto
	[Export] private float runJumpMultiplier = 1.5f; // x1.5 mas alto el salto al correr
	[Export] private float runJumpBoostX = 10f; // Impulso extra al saltar

	[ExportGroup("Fisica")]
	[Export] private float gravity = 3f; // Gravedad
	
	public override void _Ready()
	{
	currentOxygen = maxOxygen;
	// Busca el nodo OxygenBar en la escena (ajustá la ruta según tu jerarquía real) 
	OxygenBar = GetNode<ProgressBar>("../HUD/OxygenBar");
	 OxygenBar.MaxValue = maxOxygen;
	OxygenBar.Value = currentOxygen;
	}

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
		// ── Oxígeno ──
		float drain = walkDrainRate;
		if (Input.IsKeyPressed(Key.Shift)) // si corre
		{
			 drain *= runDrainMultiplier;
			}
			currentOxygen -= drain * (float)delta;
			currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
			
			if (OxygenBar != null)
			{
				OxygenBar.Value = currentOxygen;
				//GD.Print(OxygenBar.Value);
			
			}
			}
	public void EntroAlArea()
{
	GD.Print("Estoy dentro");
}
	public void SalioDelArea() {
		
		 GD.Print("Estoy fuera");
 }		 
}
