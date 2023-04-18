using System;
using System.Collections;
using System.Collections.Generic;
using Unigine;

[Component(PropertyGuid = "cc6e82e56e8fd7002866de5ffd89183218eb19e9")]
public class GenerateBombs : Component
{
	[ParameterFile(Filter = ".node")]
	[ShowInEditor] private string bombNode;

	[ShowInEditor] private List<Node> spawnPoints;
	[ShowInEditor] private float spawnBetweenTimer = 2.0f;

	private float spawnTimer;
	
	private void Init()
	{
		spawnTimer = spawnBetweenTimer;
	}
	
	private void Update()
	{
		 spawnTimer -= Game.IFps;
		 
		 if (spawnTimer < 0){
			spawnTimer= spawnBetweenTimer;

			foreach(Node point in spawnPoints)
			{
				Node spawnedBomb = World.LoadNode(bombNode);
				
				spawnedBomb.WorldTransform = point.WorldTransform;
				spawnedBomb.Enabled=true;
			}
		 }
		
	}
}