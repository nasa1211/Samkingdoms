using UnityEngine;
using System;
using System.Collections.Generic;

public sealed class GameData
{
	private static volatile GameData uniqueInstance;
	private static object _lock = new System.Object();

	private GameData() {}

	public static GameData Instance
	{
		get
		{
			if(uniqueInstance == null)
			{
				lock(_lock)
				{
					if(uniqueInstance == null)
					{
						uniqueInstance = new GameData();
					}
				}
			}
			return uniqueInstance;
		}
	}

	public InGameManager inGameMananger;
	public float targetWidth = 0, targetHeight = 640f;

	public List<Player> Hero = new List<Player>();
	public List<Player> Enemy = new List<Player>();

}
