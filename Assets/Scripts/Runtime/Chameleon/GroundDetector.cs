using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
	public bool OnGround = true;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		OnGround = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		OnGround = false;
	}
}
