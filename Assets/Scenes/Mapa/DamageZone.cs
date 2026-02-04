using Godot;
using System;

public partial class DamageZone : Area2D
{
    // ── OBSTACULOS ──
    [Export] private float damageAmount = 5f;  // Daño al tocar
    private Timer damageTimer; // Para el daño continuo

    public override void _Ready()
    {
        // ── Colision con la DamageZone ──
        BodyEntered += OnBodyEntered; // Detecta entrada
        BodyExited += OnBodyExited; // Detecta salida

        // ── Timer para DAÑO CONTINUO ──
        damageTimer = new Timer();
        damageTimer.WaitTime = 0.5; // Tiempo de Daño
        damageTimer.OneShot = false; // Repite
        damageTimer.Timeout += DamageTick;
        AddChild(damageTimer);
    }

    private void DamageTick()
    {
        for (int i = 0; i < GetOverlappingBodies().Count; i++)
        {
            var body = GetOverlappingBodies()[i];
            if (body is Player player)
            {
                player.TakeDamage(damageAmount); // Daño al personaje
                break;
            }
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player p)
        {
            p.TakeDamage(5); // Primer daño
            damageTimer.Start(); // Daño continuo
        }
    }
    private void OnBodyExited(Node2D body)
    {
        if (body is Player)
        {
            damageTimer.Stop(); // Para el daño
        }
    }
}
