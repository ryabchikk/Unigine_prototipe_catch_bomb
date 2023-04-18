using Unigine;

[Component(PropertyGuid = "d7fe84808da6addfd1f3a1b2c29d1569cc31dfb4")]
public class DeathZone : Component
{
    [ParameterFile(Filter = ".node")]
    [ShowInEditor] private string explosionBombFx;

    [ShowInEditor][Parameter(Tooltip = "The player's health controller")]
	private HealthController healthController = null;
    
    WorldTrigger trigger;

    void enter_callback(Node incomer)
    {
		if(incomer.Name=="bomb"){	
		    AddExposion(incomer);
			incomer.DeleteLater();
            healthController.TakeDamage(1);
		}
    }

    void Init()
    {
        healthController.deadEvent += DeadEventHandler;
        trigger = node as WorldTrigger;
        if(trigger != null)
        {
            trigger.AddEnterCallback(enter_callback);
            trigger.AddLeaveCallback( leaver => Log.Message("{0} has left the trigger space", leaver.Name));
        }
    }

    private void AddExposion(Node incomer)
    {
        if(explosionBombFx != ""){
            Node explosionBomb = World.LoadNode(explosionBombFx);
            explosionBomb.WorldPosition = incomer.WorldPosition;
        }
    }
    
    private void DeadEventHandler()
	{
		// show the message
		Unigine.Console.Run("world_reload");
	}
}