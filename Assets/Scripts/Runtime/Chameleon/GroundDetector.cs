using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
	const float TIME_TRESHOLD = 0.1f;
	public bool OnGround = true;
	private float lastTimeOnGround = 0f;

	private void OnTriggerStay2D(Collider2D other)
	{
		OnGround = true;
		lastTimeOnGround = Time.time;
	}

	private void LateUpdate()
	{
		if (OnGround && Time.time - lastTimeOnGround >= TIME_TRESHOLD)
		{
			OnGround = false;
		}
	}
}
