using Godot;
using System;

public partial class FollowingCamera : Godot.Camera2D
{
	[Export] public Node2D playerNode;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position = playerNode.Position;
	}
}
