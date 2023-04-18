using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "da190f00c2aeb65f8b3f103203f8b1e9e6b3d234")]
public class ScoreWidget : Component
{
	[ShowInEditor][Parameter(Tooltip = "The player's health controller")]
	private ScoreController scoreController = null;

	[ShowInEditor][Parameter(Tooltip = "Text to be displayed (should contain {HP} as a placeholder for health points)")]
	private string textPattern = "Score: {SCORE}";

	[ShowInEditor][Parameter(Tooltip = "Text field for display")]
	private ObjectText textField = null;

	void Init()
	{
		// use the handler to display health points
		scoreController.scoreChangedEvent += ScoreChangedHandler;

		textField.Text = textPattern.Replace("{SCORE}", scoreController.SCORE.ToString());
	}

	void Shutdown()
	{
		// remove the handler
		scoreController.scoreChangedEvent -= ScoreChangedHandler;
	}

	private void ScoreChangedHandler(int score)
	{
		// update text in the field
		textField.Text = textPattern.Replace("{SCORE}", score.ToString());
	}
}