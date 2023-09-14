using Godot;
using System;

public partial class CharacterBody2D : Godot.CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -700.0f;
	private AnimatedSprite2D animatedSprite;
	

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	//public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public float gravity = 1600;
	public float coyoteTime = 0.25f;
	
	private bool CanJump => currentCoyoteTime > 0;
	
	private float currentCoyoteTime;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	public override void _Process(double delta)
	{
		if (Velocity.X > 0)
		{
			animatedSprite.FlipH = false;
		}
		else if (Velocity.X < 0)
		{
			animatedSprite.FlipH = true;			
		}
		
		if (Velocity.Y < 0) 
		{
			animatedSprite.Play("jump");
		}
		else if (Velocity.Y > 0) 
		{
			animatedSprite.Play("fall");
		}
		else if (Velocity.X > 0)
		{
			animatedSprite.Play("walk");
		}
		else if (Velocity.X < 0)
		{
			animatedSprite.Play("walk");			
		}
		else if (Input.IsActionPressed("ui_down")) 
		{
			animatedSprite.Play("crouch");
		}
		else
		{
			animatedSprite.Play("idle");
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && CanJump)
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed * 0.1f);
		}

		Velocity = velocity;
		MoveAndSlide();
		
		if (IsOnFloor()) 
		{
			currentCoyoteTime = coyoteTime;
		}
		else if (currentCoyoteTime > 0)
		{
			currentCoyoteTime -= (float)delta;
		}
	}
}
