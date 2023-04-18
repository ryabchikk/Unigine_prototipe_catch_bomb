using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "3655d39cdb56da4b4c24eb2ec78138b56443f290")]
public class HpWidget : Component
{
	[ShowInEditor][Parameter(Tooltip = "The player's health controller")]
	private HealthController healthController = null;

	[ShowInEditor][Parameter(Tooltip = "Text to be displayed (should contain {HP} as a placeholder for health points)")]
	private string textPattern = "PLAYER HP: {HP}";

	[ShowInEditor][Parameter(Tooltip = "Text field for display")]
	private ObjectText textField = null;

	void Init()
	{
		// use the handler to display health points
		healthController.hpChangedEvent += HpChangedHandler;

		textField.Text = textPattern.Replace("{HP}", healthController.HP.ToString());
	}

	void Shutdown()
	{
		// remove the handler
		healthController.hpChangedEvent -= HpChangedHandler;
	}

	private void HpChangedHandler(int hp)
	{
		// update text in the field
		textField.Text = textPattern.Replace("{HP}", hp.ToString());
	}
}
