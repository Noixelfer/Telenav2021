using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
	[SerializeField] private ShadowManager shadowManager;
	[SerializeField] private Image vignette;
	[SerializeField] private AnimationCurve vignetteCurve;
	private bool shadowAppearing = false;
	private Color vignetteColor = Color.white;

	private void Awake()
	{
		ShadowManager.OnShadowAppearStart += ShadowAppearStarted;
		ShadowManager.OnShadowAppearEnd += ShadowAppearEnded;
		vignette.enabled = false;
	}

	private void OnDestroy()
	{
		ShadowManager.OnShadowAppearStart -= ShadowAppearStarted;
		ShadowManager.OnShadowAppearEnd -= ShadowAppearEnded;
	}

	private void ShadowAppearEnded(bool survived)
	{
		if (!survived) return;
		shadowAppearing = false;
		vignette.enabled = false;
	}

	private void ShadowAppearStarted()
	{
		shadowAppearing = true;
		vignetteColor.a = 0f;
		vignette.color = vignetteColor;
		vignette.enabled = true;
	}

	private void Update()
	{
		if (shadowAppearing)
		{
			vignetteColor.a = vignetteCurve.Evaluate(shadowManager.AppearProgress);
			vignette.color = vignetteColor;
		}
	}
}
