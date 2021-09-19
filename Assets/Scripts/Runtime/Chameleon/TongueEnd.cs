using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueEnd : MonoBehaviour
{
	public ObiCollider2D ObiColloder;
	public Rigidbody2D Rigidbody2D;
	[SerializeField] private CircleCollider2D collider;
	private bool catched = false;

	public static Action<Color> FireflyCatched;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (catched) return;

		if (collision.GetComponent<CritterController>() is var critter && critter != null)
		{
			FireflyCatched?.Invoke(critter.GetColor());
			Destroy(collision.gameObject);
			catched = true;
		}
	}
}
