using Godot;
using Godot.NativeInterop;
using System;

public partial class Player : CharacterBody2D
{
    // ── T2: Movimiento del Jugador ──
    // ── MOVIMIENTO ──
    [ExportGroup("Movimiento")]
	[Export] private float walkSpeed = 300f; // Velocidad normal
	[Export] private float runSpeed = 600f; // Velocidad corriendo (shift)

    // ── SALTO ──
    [ExportGroup("Salto")]
	[Export] private float jumpForce = -500f; // Fuerza de salto
	[Export] private float runJumpMultiplier = 1.5f; // x1.5 mas alto el salto al correr
	[Export] private float runJumpBoostX = 200f; // Impulso extra al saltar

    // ── FISICA ──
    [ExportGroup("Fisica")]
    [Export] private float gravity = 1200f; // Gravedad

    // ── T6: Recoger Objetos ──
    // ── VIDA ──
    [ExportGroup("Vida")]
    [Export] private float maxHealth = 100f; // Vida total
    [Export] private float currentHealth = 100f; // Vida actual
    [Export] private float pickupHealthGain = 20f; // Vida que da el mineral (Plata)
    [Export] private bool isShielded = false;  // Escudo temporal (Plomo)

    // ── RADAR / INTERACCION ──
    private Area2D radar; // Area de deteccion 
    private Node2D currentInteractable; // Objeto actual
    private Label interactPrompt;  // Texto interaccion

    public override void _Ready()
    {
        // ── RADAR ──
        radar = GetNodeOrNull<Area2D>("Area2D");  // Usa GetNodeOrNull para no tirar error si falla
        radar.BodyEntered += OnRadarEnter;
        radar.BodyExited += OnRadarExit;

        // ── TEXTO PARA LA INTERACCION ──
        interactPrompt = GetNodeOrNull<Label>("InteractPrompt");
        if (interactPrompt == null)
        {
            GD.Print("ERROR: No se encontro Label 'InteractPrompt' como hijo del Player");
        }
        else
        {
            GD.Print("Prompt Label encontrado OK");
            interactPrompt.Visible = false;  // Asegura que empiece oculto
        }
    }

    public override void _PhysicsProcess(double delta)
	{
        // ── ENTRADA DEL JUGADOR ──
        Vector2 dir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down"); //(Input.GetVector maneja normalized auto)
        bool isRunning = Input.IsKeyPressed(Key.Shift); // Detecta si el jugador está presionando Shift
        // la velocidad actual: con shift corre, sin camina
        float currentSpeed = isRunning ? runSpeed : walkSpeed; // condicion ? verdadero : falso

        // Aplicamos velocidad horizontal basada en input actual
        Velocity = new Vector2(dir.X * currentSpeed, Velocity.Y);

        // ── GRAVEDAD (si no esta en el suelo) ──
        if (!IsOnFloor())
		{
			Velocity += Vector2.Down * gravity * (float)delta;
		}

        // ── SALTO ──
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

        // Aplicar movimiento y manejar colisiones
        MoveAndSlide();

        // ── INTERACCION CON E ──
        if (Input.IsActionJustPressed("interact") && currentInteractable != null)
        {
            if (currentInteractable is Mena mena)
            {
                mena.Extract();
            }
            else if (currentInteractable is PlataPickup plata)
            {
                plata.Interact(this);
            }
            else if (currentInteractable is PlomoPickup plomo)
            {
                plomo.Interact(this);
            }
        }
    }

    // ── FUNCIONES DEL RADAR ──
    private void OnRadarEnter(Node2D body)
    {
        if (body.IsInGroup("interactable"))
        {
            currentInteractable = body;

            if (interactPrompt != null)
            {
                string promptText = "E para interactuar";

                if (body is Mena)
                    promptText = "E para excavar";
                else if (body is PlataPickup)
                    promptText = "E para usar";
                else if (body is PlomoPickup)
                    promptText = "E para usar";

                interactPrompt.Text = promptText;
                interactPrompt.Visible = true;
            }
        }
    }   
    private void OnRadarExit(Node2D body)
    {
        if (body == currentInteractable)
        {
            currentInteractable = null;
            if (interactPrompt != null)
            {
                interactPrompt.Visible = false;
            }
        }
    }

    // ── FUNCIONDE DE LOS MINERALES ──
    // Plata
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        GD.Print($"¡+{amount} vida! Total: {currentHealth}");
    }
    // Plomo
    public void ActivateShield(float duration)
    {
        isShielded = true;
        GetTree().CreateTimer(duration).Timeout += () => isShielded = false;
        GD.Print($"¡Escudo {duration}s!");
    }
}
