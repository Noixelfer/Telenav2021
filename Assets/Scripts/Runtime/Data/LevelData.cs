using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Properties/LevelData", order = 4)]
public class LevelData : ScriptableObject
{
	private int completedShrines = 0;
	public int CompletedShrines
	{
		get
		{
			return completedShrines;
		}

		set
		{
			completedShrines = value;
			OnShrineCompleted?.Invoke();
		}
	}

	public int TotalShrines;
	public Action OnShrineCompleted;
}
