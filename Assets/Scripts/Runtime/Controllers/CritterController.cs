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

	public void Initialize(CritterModel model, float startXPosition)
	{
		critterModel = model;
		flyController = new CritterFlyController();
		flyController.Initialize(rb, critterModel, startXPosition);
		pointLight.color = model.Color;
		glowLight.color = model.Color;
	}


	private void FixedUpdate()
	{
		flyController.FixedUpdate();
	}
}
