using Unigine;

[Component(PropertyGuid = "e15dc917af90538ad0862d61b7cce74ccc6afa3b")]
public class Basket : Component
{
	
	[ShowInEditor] private float speed;
	
	private void Update()
	{
		HandleKeys();
	}
	
	private void Move(MathLib.AXIS axis, float speed)
	{
		node.WorldPosition += node.GetWorldDirection(axis) * speed * Game.IFps;
	}
	
	private void HandleKeys()
	{
		if(Input.IsKeyPressed(Input.KEY.W)){
			Move(MathLib.AXIS.Y,speed);
		}
		if(Input.IsKeyPressed(Input.KEY.S)){
			Move(MathLib.AXIS.Y,-speed);
		}

		if(Input.IsKeyPressed(Input.KEY.D)){
			Move(MathLib.AXIS.X,speed);
		}
		if(Input.IsKeyPressed(Input.KEY.A)){
			Move(MathLib.AXIS.X,-speed);
		}
	}
}