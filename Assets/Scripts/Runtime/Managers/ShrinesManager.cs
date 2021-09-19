using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinesManager : MonoBehaviour
{
	[SerializeField] private List<ShrineController> shrines;
	private int activatedShrinesCount = 0;

	private void Awake()
	{
		foreach (var shrine in shrines)
		{
			shrine.OnActivate += ShrineActivated;
		}
	}

	private void ShrineActivated()
	{
		activatedShrinesCount++;
		Debug.LogError("SHRINEEEE ACTIVATED");
		if (activatedShrinesCount >= shrines.Count)
		{
			//Win
		}
	}
}
