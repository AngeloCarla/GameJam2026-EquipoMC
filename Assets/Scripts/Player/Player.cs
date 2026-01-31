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
    [Export] private float currentHealth; // Vida actual
    private ProgressBar healthBar; // Barra de vida
    private bool isDead = false; // Esta MUERTO
    private bool isDeadVisual = false; // Nomas por el color

    // ── MINERALES Y EFECTOS ──
    [Export] private float pickupHealthGain = 20f; // Vida que da el mineral (Plata)
    private bool isShielded = false;  // Escudo temporal (Plomo)
    private Label effectLabel; // Contador del Escudo
    private float shieldTimeLeft = 0f; // Tiempo restante

    // ── RADAR / INTERACCION ──
    private Area2D radar; // Area de deteccion 
    private Node2D currentInteractable; // Objeto actual
    private Label interactPrompt;  // Texto interaccion

    // ── TESTEO ──
    private Color originalModulate = Colors.White; // Color original

    public override void _Ready() // Start()
    {
        // ── RADAR ──
        radar = GetNodeOrNull<Area2D>("Area2D");  // Usa GetNodeOrNull referenciar el nodo
        radar.BodyEntered += OnRadarEnter;
        radar.BodyExited += OnRadarExit;

        // ── TEXTO PARA LA INTERACCION ──
        interactPrompt = GetNodeOrNull<Label>("InteractPrompt");
        if (interactPrompt == null)
            GD.Print("ERROR: No se encontro Label 'InteractPrompt' como hijo del Player");
        else
            interactPrompt.Visible = false;  // Asegura que empiece oculto

        // ── VIDA ──
        healthBar = GetNodeOrNull<ProgressBar>("HealthBar");
        if (healthBar != null)
        {
            healthBar.MaxValue = maxHealth;
            healthBar.Value = currentHealth;
        }

        UpdateHealthDisplay();

        // ── OTROS ──
        effectLabel = GetNodeOrNull<Label>("EffectLabel");

        // Testeo para los minerales
        var visual = GetNodeOrNull<ColorRect>("ColorRect");
        if (visual != null)
            originalModulate = visual.Modulate;  // Guarda el color
    }

    public override void _PhysicsProcess(double delta)
	{
        // ── MUERTE ─
        if (isDead)
        {
                Velocity = Vector2.Zero;
                // Reset con R
                if (Input.IsActionJustPressed("reset_game"))
                {
                    ResetPlayer();
                }
                return;
        }

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

        // ── CONTADOR EFECTO PLOMO (Escudo) ──
        if (isShielded && shieldTimeLeft > 0)
        {
            shieldTimeLeft -= (float)delta;
            if (effectLabel != null)
                effectLabel.Text = $"Escudo: {shieldTimeLeft:F1}s";

            if (shieldTimeLeft <= 0)
            {
                isShielded = false;
                if (effectLabel != null)
                    effectLabel.Visible = false;
                GD.Print("Escudo terminado");
            }
        }

        // ── TEST DE DAÑO ──
        if (Input.IsActionJustPressed("damage_test"))
        {
            TakeDamage(20f);
        }

        // ── RESET ──
  
            if (Input.IsActionJustPressed("reset_game"))
            {
                ResetPlayer();
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
        if (body.IsInGroup("interactable")) // A todos los objetos del grupo Interactable
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

                interactPrompt.Text = promptText; // Asigna el texto
                interactPrompt.Visible = true; // Muetra en pantalla
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
                interactPrompt.Visible = false; // Deja de ser visible al no detectar nada
            }
        }
    }

    // ── FUNCIONDE DE LOS MINERALES ──
    // Plata
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        FlashColor(Colors.Red, 1f);
        UpdateHealthDisplay();

        if (effectLabel != null)
        {
            effectLabel.Text = $"+{amount} vida";
            effectLabel.Modulate = Colors.Lime;
            effectLabel.Visible = true;

            var tween = CreateTween();
            tween.TweenInterval(1f);  // Tiempo visible
            tween.TweenProperty(effectLabel, "modulate:a", 0f, 0.5f);  // Fade out

            // Desaparece el texto
            tween.TweenCallback(Callable.From(() =>
            {
                effectLabel.Visible = false;
                effectLabel.Modulate = Colors.White;
            }));
        }
    }
    // Plomo
    public void ActivateShield(float duration)
    {
        isShielded = true;
        shieldTimeLeft = duration;

        FlashColor(Colors.Blue, duration);
        GD.Print($"¡Proteccion por {duration}s!");

        if (effectLabel != null)
        {
            effectLabel.Modulate = Colors.LightBlue;
            effectLabel.Visible = true;
        }

        UpdateHealthDisplay();
    }
    // Testeo rapido, cambio de color: ROJO PLATA, AZUL PLOMO
    public void FlashColor(Color flashColor, float duration)
    {
        if (isDeadVisual) return;

        var visual = GetNodeOrNull<ColorRect>("ColorRect");
        if (visual == null)
        {
            GD.Print("No se encontró ColorRect para flash");
            return;
        }

        var tween = CreateTween();
        tween.TweenProperty(visual, "modulate", flashColor, 0.2f);  // Flash suave

        // Espera la duración y resetea
        GetTree().CreateTimer(duration).Timeout += () =>
        {
            if (!isDeadVisual)
            {
                var resetTween = CreateTween();
                resetTween.TweenProperty(visual, "modulate", originalModulate, 0.8f);  // Vuelta suave al original
            }
        };
    }

    // ── PROGRESS BAR ──
    public void UpdateHealthDisplay()
    {
        if (healthBar != null)
        {
            healthBar.Value = currentHealth;
            healthBar.Modulate = currentHealth < 30 ? Colors.Red : Colors.Green;
        }
        GD.Print($"Vida actual: {currentHealth}/{maxHealth}");  // Consola siempre
    }

    // ── DAÑO Y MUERTE ──
    public void TakeDamage(float amount)
    {
        if (isShielded)
        {
            GD.Print("¡Tienes proteccion!");
            return;
        }

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        GD.Print($"¡Recibiste daño! Vida:{currentHealth}");
        FlashColor(Colors.Red, 0.5f);
        UpdateHealthDisplay();

        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        GD.Print("Perdiste");
        isDead = true;
        isDeadVisual = true;

        // Cambio de color
        var visual = GetNodeOrNull<ColorRect>("ColorRect");
        if (visual != null)
        {
            visual.Modulate = Colors.DarkRed;  // Rojo oscuro fijo
        }

        // Barra roja total
        if (healthBar != null)
        {
            healthBar.Modulate = Colors.DarkRed;
            healthBar.Value = 0;
        }
    }

    private void ResetPlayer()
    {
        isDead = false;
        isDeadVisual = false;
        currentHealth = maxHealth;  // Vida llena
        isShielded = false;
        shieldTimeLeft = 0f;

        // Colores originales
        var visual = GetNodeOrNull<ColorRect>("ColorRect");
        if (visual != null)
            visual.Modulate = originalModulate;

        // Barra verde
        if (healthBar != null)
        {
            healthBar.Modulate = Colors.Green;
            healthBar.Value = currentHealth;
        }

        // Labels ocultos
        if (effectLabel != null)
            effectLabel.Visible = false;
        if (interactPrompt != null)
            interactPrompt.Visible = false;

        UpdateHealthDisplay();
        GD.Print("¡REVIVIDO!");
    }
}
