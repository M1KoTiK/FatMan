using Godot;
using System;

public partial class FollowingCamera : Godot.Camera2D
{
	[Export] public Node2D playerNode;
	[Export] public float followSpeed = 10f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var position = Position;
		var playerPosition = playerNode.Position;
		Position = position.MoveToward(playerPosition, (float)delta * followSpeed * position.DistanceTo(playerPosition));
	}
}
