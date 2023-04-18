using System;
using Unigine;

[Component(PropertyGuid = "f37d9d81082a58ec350f7f5b8ad9acfee937f045")]
public class MoveGun : Component
{
	[ShowInEditor] private float speedY;
	[ShowInEditor] private float speedX;
	[ShowInEditor] private int chanceToSwitchTrajectory;

	private const float MAX_TRANSLATE_COORDINATE = 5f;
	
	private void Update()
	{
		ChangeDirectionY();
		ChangeDirectionX();
		TranslateGun();
		RandomlyChangeDiretcion(ref speedY);
		RandomlyChangeDiretcion(ref speedX);
	}

	private void TranslateGun()
	{
		vec3 directionY = node.GetWorldDirection(MathLib.AXIS.Y);
		vec3 directionX = node.GetWorldDirection(MathLib.AXIS.X);

		node.WorldPosition += directionY * speedY * Game.IFps;
		node.WorldPosition += directionX *speedX * Game.IFps;
	}
	
	private void ChangeDirectionY()
	{
		if(node.WorldPosition.y >= MAX_TRANSLATE_COORDINATE){
			speedY = -MathF.Abs(speedY);
		}
		else if(node.WorldPosition.y <= MAX_TRANSLATE_COORDINATE *-1){
			speedY = MathF.Abs(speedY);
		}
	}

	private void ChangeDirectionX()
	{
		if(node.WorldPosition.x >= MAX_TRANSLATE_COORDINATE){
			speedX = -MathF.Abs(speedX);
		}
		else if(node.WorldPosition.x <= MAX_TRANSLATE_COORDINATE *-1){
			speedX = MathF.Abs(speedX);
		}
	}

	private void RandomlyChangeDiretcion(ref float  gunSpeed)
	{
		System.Random rnd = new System.Random();
		int value = rnd.Next(0, 100);
		if ( value < chanceToSwitchTrajectory) {
			gunSpeed*=-1;
		}
		
	}
}