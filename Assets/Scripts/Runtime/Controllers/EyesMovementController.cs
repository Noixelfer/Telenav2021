using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesMovementController : MonoBehaviour
{
	[SerializeField] private float MinDelay = 2f;
	[SerializeField] private float MaxDelay = 3f;
	[SerializeField] private Transform eyesTransform;
	[SerializeField] private BoxCollider2D eyesBounds;
	private float changeEyesTime;

	private void Awake()
	{
		changeEyesTime = Time.time + Random.Range(MinDelay, MaxDelay);
	}

	private void Update()
	{
		if (Time.time >= changeEyesTime)
		{
			changeEyesTime = Time.time + Random.Range(MinDelay, MaxDelay);
			eyesTransform.localPosition = GetRandomPositionInsideBounds(eyesBounds);
		}
	}

	private Vector2 GetRandomPositionInsideBounds(BoxCollider2D coll)
	{
		return new Vector2(Random.Range(-coll.size.x / 2f, coll.size.x / 2f),
						   Random.Range(-coll.size.x / 2f, coll.size.y / 2f));
	}
}
