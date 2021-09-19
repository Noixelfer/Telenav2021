using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineController : MonoBehaviour
{

	const float CHANNEL_TIME = 3f;

	public Action OnActivate;

	private float startTime;
	private bool activated = false;
	private bool isActivating = false;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (activated) return;
		if (collision.GetComponent<ChameleonController>() == null) return;

		startTime = Time.time;
		isActivating = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (activated) return;
		if (collision.GetComponent<ChameleonController>() == null) return;

		isActivating = false;
	}

	private void Update()
	{
		if (!activated && isActivating && Time.time - startTime >= CHANNEL_TIME)
		{
			activated = true;
			OnActivate?.Invoke();
		}
	}
}
