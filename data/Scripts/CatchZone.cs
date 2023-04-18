using Unigine;

[Component(PropertyGuid = "400e886098e2203f3eeab0922ef616c9e5a33194")]
public class CatchZone : Component
{
    WorldTrigger trigger;
    
    [ShowInEditor][Parameter(Tooltip = "Score points")]
	private ScoreController scoreController = null;
    
    void enter_callback(Node incomer)
    {
		if(incomer.Name=="bomb"){			
			incomer.DeleteLater();
            scoreController.AddPoints(1);
		}
    }

    void Init()
    {
        trigger = node as WorldTrigger;
        if(trigger != null)
        {
            trigger.AddEnterCallback(enter_callback);
        }
    }
}