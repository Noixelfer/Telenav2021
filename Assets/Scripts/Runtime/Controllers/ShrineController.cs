using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;

public class ShrineController : MonoBehaviour
{
	[SerializeField] private Light2D beam;
	const float CHANNEL_TIME = 0.5f;

	public Action OnActivate;

	private float startTime;
	private bool activated = false;
	private bool isActivating = false;

	private void Awake()
	{
		beam.gameObject.SetActive(false);
	}

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
			beam.gameObject.SetActive(true);
			OnActivate?.Invoke();
		}
	}
}
