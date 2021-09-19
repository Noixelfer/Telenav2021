using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class InGameUI : MonoBehaviour
{
	[SerializeField] private ShadowManager shadowManager;
	[SerializeField] private Image vignette;
	[SerializeField] private AnimationCurve vignetteCurve;
	[SerializeField] private CanvasGroup endScreen;
	[SerializeField] private GameObject colorCombo;
	[SerializeField] private Button tryAgain;

	private bool shadowAppearing = false;
	private Color vignetteColor = Color.white;

	private void Awake()
	{
		ShadowManager.OnShadowAppearStart += ShadowAppearStarted;
		ShadowManager.OnShadowAppearEnd += ShadowAppearEnded;
		tryAgain.onClick.AddListener(TryAgain);
		endScreen.gameObject.SetActive(false);
		vignette.enabled = false;
	}

	private void TryAgain()
	{
		SceneManager.LoadScene("EnvironmentScene");
	}

	private void OnDestroy()
	{
		ShadowManager.OnShadowAppearStart -= ShadowAppearStarted;
		ShadowManager.OnShadowAppearEnd -= ShadowAppearEnded;
	}

	private void ShadowAppearEnded(bool survived)
	{
		if (!survived)
		{
			ShowEndScreen();
			return;
		}
		shadowAppearing = false;
		vignette.enabled = false;
	}

	private void ShowEndScreen()
	{
		endScreen.alpha = 0f;
		endScreen.DOFade(1f, 0.5f);
		colorCombo.gameObject.SetActive(false);
		endScreen.gameObject.SetActive(true);
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
