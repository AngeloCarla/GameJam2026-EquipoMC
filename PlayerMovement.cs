using Godot;
using Godot.NativeInterop;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	private float walkSpeed = 300f;
	private float runSpeed = 600f;

    public override void _PhysicsProcess(double delta)
	{
		float horizontal = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");

		Vector2 dir = new Vector2(horizontal, 0).Normalized();

		bool isRunning = Input.IsKeyPressed(Key.Shift);

		float currentSpeed = isRunning ? runSpeed : walkSpeed;

		Velocity = dir * currentSpeed;
		MoveAndSlide();
	}
}
