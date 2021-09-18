using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TongueController : MonoBehaviour
{
	const float DURATION = 0.3f;
	const float TONGUE_SPEED = 3f;

	[SerializeField] private ObiSolver solver;
	[SerializeField] private ObiCollider2D mouth;
	[SerializeField] private ObiCollider2D tongueEnd;
	[SerializeField] private Material material;
	[SerializeField] private ObiRopeSection section;
	[SerializeField] private Rigidbody2D tongueEndRigidbody;
	[SerializeField] private ObiRope rope;
	[SerializeField] private ObiRopeBlueprint blueprint;
	[SerializeField] private ObiRopeExtrudedRenderer ropeRenderer;
	[SerializeField] private ObiRopeCursor cursor;

	private float startTime;
	private Vector2 targetPos;

	private void Awake()
	{
	}

	private void OnDestroy()
	{
		DestroyImmediate(blueprint);
	}

	public void LaunchTongue(float force)
	{
		Vector3 mouse = Mouse.current.position.ReadValue();
		mouse.z = transform.position.z - Camera.main.transform.position.z;
		Vector3 mouseInScene = Camera.main.ScreenToWorldPoint(mouse);
		var go = new GameObject("Pos");
		go.transform.position = mouseInScene;
		var direction = (mouseInScene - transform.position).normalized;
		targetPos = mouseInScene;

		tongueEndRigidbody.isKinematic = false;
		//tongueEndRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
		StartCoroutine(LaunchTongueRoutine(direction));
	}

	private IEnumerator LaunchTongueRoutine(Vector3 direction)
	{
		startTime = Time.time;

		while (Time.time - startTime <= DURATION)
		{
			cursor.ChangeLength(rope.restLength + TONGUE_SPEED * Time.deltaTime);
			tongueEndRigidbody.position = tongueEndRigidbody.transform.position + direction * TONGUE_SPEED * Time.deltaTime;
			yield return null;
		}
	}
}
