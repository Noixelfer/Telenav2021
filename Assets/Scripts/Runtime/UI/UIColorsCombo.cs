using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorsCombo : MonoBehaviour
{
	private const float ROTATION_DURATION = 0.3f;
	[SerializeField] private ChameleonModel chameleonModel;
	[SerializeField] private ChameleonController chameleonController;
	[SerializeField] private RectTransform comboHolder;
	[SerializeField] private Image color1;
	[SerializeField] private Image color2;
	private bool reversed = true;
	private ChameleonColor[] colors;

	private void Awake()
	{
		chameleonController.OnFireflyCatched += FireflyCatched;
	}

	private void FireflyCatched(Color color)
	{
		RotateColors(color);
	}

	public void RotateColors(Color color)
	{
		comboHolder.DOBlendableLocalRotateBy(new Vector3(0f, 0f, 180f), ROTATION_DURATION, RotateMode.Fast);
		if (reversed)
		{
			color1.color = color;
		}
		else
		{
			color2.color = color;
		}

		reversed = !reversed;
	}

	private void Update()
	{
		colors = chameleonModel.Colors.ToArray();
		color1.transform.rotation = Quaternion.identity;
		color2.transform.rotation = Quaternion.identity;

		if (chameleonModel.Colors.Count == 2)
		{
			color1.fillAmount = reversed ? colors[0].TimePercentage() : colors[1].TimePercentage();
			color2.fillAmount = reversed ? colors[1].TimePercentage() : colors[0].TimePercentage();
		}
		else if (chameleonModel.Colors.Count == 1)
		{
			color1.fillAmount = reversed ? 0f : colors[0].TimePercentage();
			color2.fillAmount = reversed ? colors[0].TimePercentage() : 0f;
		}
		else
		{
			color1.fillAmount = 0f;
			color2.fillAmount = 0f;
		}
	}
}
