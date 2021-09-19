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
	[SerializeField] private ObiCollider2D tongueEndPrefab;
	[SerializeField] private Material material;
	[SerializeField] private ObiRopeSection section;
	[SerializeField] private Rigidbody2D tongueEndRigidbody;
	[SerializeField] private ObiRope rope;
	[SerializeField] private ObiRopeBlueprint blueprint;
	[SerializeField] private ObiRopeExtrudedRenderer ropeRenderer;
	[SerializeField] private ObiRopeCursor cursor;
	private Coroutine launchTongueRoutine;

	private ChameleonModel model;
	private float startTime;
	private Vector2 targetPos;
	private float elapsed;
	private float attackPercentage = 0f;

	private void Awake()
	{
		ropeRenderer.enabled = false;
	}

	private void OnDestroy()
	{
		DestroyImmediate(blueprint);
	}

	public void Initialize(ChameleonModel model)
	{
		this.model = model;
	}

	public void LaunchTongue(float force)
	{
		Vector3 mouse = Mouse.current.position.ReadValue();
		mouse.z = transform.position.z - Camera.main.transform.position.z;
		Vector3 mouseInScene = Camera.main.ScreenToWorldPoint(mouse);
		var direction = (mouseInScene - transform.position).normalized;
		targetPos = mouseInScene;

		tongueEndRigidbody.isKinematic = false;
		//tongueEndRigidbody.AddForce(direction * force, ForceMode2D.Impulse);
		if (launchTongueRoutine != null) StopCoroutine(launchTongueRoutine);
		launchTongueRoutine = StartCoroutine(LaunchTongueRoutine(direction));
	}

	private IEnumerator LaunchTongueRoutine(Vector3 direction)
	{
		var attackTime = model.AttackTime * 0.66f;
		var retractTime = model.AttackTime * 0.33f;
		startTime = Time.time;
		elapsed = 0f;
		attackPercentage = 0f;
		var currentTongueDist = 0f;
		ropeRenderer.enabled = true;

		while (attackPercentage < 1f)
		{
			elapsed += Time.deltaTime;
			attackPercentage = elapsed / attackTime;
			var realPercentage = model.TongueDistanceCurve.Evaluate(attackPercentage);
			var targetDist = model.MaxTongueDistance * realPercentage;
			var deltaDist = targetDist - currentTongueDist;
			currentTongueDist = targetDist;
			cursor.ChangeLength(rope.restLength + deltaDist);
			tongueEndRigidbody.MovePosition(transform.position + direction * model.MaxTongueDistance * realPercentage);
			yield return null;
		}

		var retractPercentage = 0f;
		elapsed = 0f;

		while (retractPercentage < 1f)
		{
			elapsed += Time.deltaTime;
			retractPercentage = elapsed / retractTime;
			if (rope.restLength > 0 && rope.elements != null && rope.elements.Count > 0)
			{
				cursor.ChangeLength(Mathf.Max(model.MaxTongueDistance * (1f - retractPercentage), 0.1f));
				tongueEndRigidbody.MovePosition(transform.position + direction * model.MaxTongueDistance * (1f - retractPercentage));
			}
			yield return null;
		}

		ropeRenderer.enabled = false;
	}
}
