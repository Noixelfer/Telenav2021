using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueEnd : MonoBehaviour
{
	[SerializeField] private CircleCollider2D collider;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Destroy(collision.gameObject);
	}
}
