using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CritterFlyController
{
	private CritterModel model;
	private float flyYvalue;
	private Rigidbody2D critterRb;
	private float xPosition;
	private float yPosition;
	private bool moveRight;
	private float nextDiretionChangeTime = 0f;

	public void Initialize(Rigidbody2D critterRb, CritterModel model, float xPosition)
	{
		this.model = model;
		this.critterRb = critterRb;
		this.xPosition = xPosition;
		moveRight = Random.value <= 0.5f;

		if (moveRight)
		{
			Flip();
		}

		nextDiretionChangeTime = Time.time + Random.Range(model.DirectionChangeTimeMin, model.DirectionChangeTimeMax);
		flyYvalue = 0f;
		MoveToInitialPosition(xPosition);
	}

	public void FixedUpdate()
	{
		flyYvalue += Time.fixedDeltaTime * model.Speed;
		if (Time.time >= nextDiretionChangeTime)
		{
			ChangeDirection();
		}

		xPosition += (moveRight ? 1 : -1) * model.Speed * Time.deltaTime;
		yPosition = GetFlyAltitude(model.FlySeed + flyYvalue);
		critterRb.MovePosition(new Vector2(xPosition, yPosition));
	}

	private void ChangeDirection()
	{
		moveRight = !moveRight;
		Flip();
		nextDiretionChangeTime = Time.time + Random.Range(model.DirectionChangeTimeMin, model.DirectionChangeTimeMax);
	}

	private void Flip()
	{
		var localScale = critterRb.transform.localScale;
		localScale.x *= -1f;
		critterRb.transform.localScale = localScale;
	}

	private void MoveToInitialPosition(float xPosition)
	{
		yPosition = GetFlyAltitude(model.FlySeed);
		critterRb.MovePosition(new Vector2(xPosition, yPosition));
	}

	//Random function for getting the fly altitude for the critter
	//Source: https://www.geogebra.org/graphing/yzgxvd8q 
	private float GetFlyAltitude(float x)
	{
		return model.FactorTotal * (model.Factor1 * Mathf.Sin(model.Scale1 * x) + model.FactorE * Mathf.Sin(model.ScaleE * Mathf.Epsilon * x) + model.FactorPI * Mathf.Sin(model.ScalePI * Mathf.PI * x));
	}
}
