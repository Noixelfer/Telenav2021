using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterController : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;
	private CritterModel critterModel;
	private CritterFlyController flyController;

	public void Initialize(CritterModel model, float startXPosition)
	{
		critterModel = model;
		flyController = new CritterFlyController();
		flyController.Initialize(rb, critterModel, startXPosition);
	}

	private void FixedUpdate()
	{
		flyController.FixedUpdate();
	}
}
