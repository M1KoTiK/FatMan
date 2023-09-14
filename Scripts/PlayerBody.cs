using Godot;
using System;

public partial class PlayerBody : CharacterBody2D
{
    [Export] public float gravity = 980;
    [Export] public float terminalVelocity = 500;
    [Export] public float topSpeed = 300;
    [Export] public float groundAcceleration = 200;
    [Export] public float airAcceleration = 70;
    [Export] public float initialJumpVelocity = -400;
    [Export] public float jumpGravity = 500;
    [Export] public float jumpTime = 0.5f;
    [Export] public float coyoteTime = 0.1f;
    [Export] public float jumpBufferTime = 0.1f;

    private float currentJumpTime = 0;
    private float currentCoyoteTime = 0;
    private float currentJumpBufferTime = 0;
    private float HorizontalInput
    {
        get
        {
            return Input.GetAxis("ui_left", "ui_right");
        }
    }
    private Vector2 TargetVelocity
    {
        get
        {
            return new Vector2(HorizontalInput * topSpeed, terminalVelocity);
        }
    }
    private bool CanJump
    {
        get
        {
            return currentCoyoteTime > 0;
        }
    }
    private bool IsJumping
    {
        get
        {
            return currentJumpTime > 0;
        }
    }
    private float HorizontalAcceleration
    {
        get
        {
            if (IsOnFloor())
            {
                return groundAcceleration;
            }
            else
            {
                return airAcceleration;
            }
        }
    }
    private float VerticalAcceleration
    {
        get
        {
            if (IsJumping)
            {
                return jumpGravity;
            }
            else
            {
                return gravity;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var horizontalVelocity = Velocity.X;
        var verticalVelocity = Velocity.Y;

        var collisionCount = GetSlideCollisionCount();
        for (int i = 0; i < collisionCount; i++)
        {
            var normal = GetSlideCollision(i).GetNormal();
            var velocity = new Vector2(horizontalVelocity, verticalVelocity);
            var dotProduct = Mathf.Clamp(normal.Dot(velocity), float.MinValue, 0);
            horizontalVelocity -= normal.X * dotProduct;
            verticalVelocity -= normal.Y * dotProduct;
        }
        var targetVelocity = TargetVelocity;
        var targetHorizontalVelocity = targetVelocity.X;
        var targetVerticalVelocity = targetVelocity.Y;

        var horizontalAcceleration = HorizontalAcceleration;
        var verticalAcceleration = VerticalAcceleration;

        horizontalVelocity = Mathf.MoveToward(horizontalVelocity, targetHorizontalVelocity, horizontalAcceleration * (float)delta);
        verticalVelocity = Mathf.MoveToward(verticalVelocity, targetVerticalVelocity, verticalAcceleration * (float)delta);
        if (Input.IsActionJustPressed("ui_accept"))
        {
            currentJumpBufferTime = jumpBufferTime;
        }
        if (IsOnFloor())
        {
            currentCoyoteTime = coyoteTime;
        }
        if (currentJumpBufferTime > 0 && CanJump)
        {
            currentJumpTime = jumpTime;
            currentJumpBufferTime = 0;
            verticalVelocity = initialJumpVelocity;
        }
        if (!Input.IsActionPressed("ui_accept") || verticalVelocity > jumpTime)
        {
            currentJumpTime = 0;
        }
        currentJumpTime = Mathf.MoveToward(currentJumpTime, 0, (float)delta);
        currentCoyoteTime = Mathf.MoveToward(currentCoyoteTime, 0, (float)delta);
        currentJumpBufferTime = Mathf.MoveToward(currentJumpBufferTime, 0, (float)delta);

        Velocity = new Vector2(horizontalVelocity, verticalVelocity);

        MoveAndSlide();
    }
}
