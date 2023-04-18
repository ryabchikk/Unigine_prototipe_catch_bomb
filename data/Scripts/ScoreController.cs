using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "88266ceb18bb1d4ee83a8455446e89b03318114f")]
public class ScoreController : Component
{
	[ShowInEditor][Parameter(Tooltip = "Score points")]
	private int score = 0;

	public int SCORE => score;

	public event Action<int> scoreChangedEvent;

	public void AddPoints(int value)
	{
		// set hp and invoke the controller events
		score += value;
		scoreChangedEvent?.Invoke(score);
	}
}