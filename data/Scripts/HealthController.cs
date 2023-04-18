using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "61a761376109cd2a1ac71a8183b3b1ece57f045b")]
public class HealthController : Component
{
	[ShowInEditor][Parameter(Tooltip = "Health points")]
	private int hp = 3;

	public int HP => hp;

	public event Action deadEvent;
	public event Action<int> hpChangedEvent;

	private bool isDead = false;

	void Init()
	{
		isDead = false;
	}

	public void TakeDamage(int value)
	{
		// set hp and invoke the controller events
		hp -= value;
		hpChangedEvent?.Invoke(hp);

		if (hp <= 0 && !isDead)
		{
			isDead = true;
			deadEvent?.Invoke();
		}
	}
}
