using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CritterController : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private Light2D pointLight;
	[SerializeField] private Light2D glowLight;
	private CritterModel critterModel;
	private CritterFlyController flyController;

	public void Initialize(CritterModel model, Vector2 startPos)
	{
		critterModel = model;
		flyController = new CritterFlyController();
		flyController.Initialize(rb, critterModel, startPos);
		pointLight.color = model.Color;
		glowLight.color = model.Color;
	}

	public Color GetColor()
	{
		return critterModel.Color;
	}

	private void FixedUpdate()
	{
		flyController.FixedUpdate();
	}
}
