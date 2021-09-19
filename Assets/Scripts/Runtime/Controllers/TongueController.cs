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
	[SerializeField] private TongueEnd tongueEndPrefab;
	[SerializeField] private Material material;
	[SerializeField] private ObiRopeSection section;
	[SerializeField] private ObiRope rope;
	[SerializeField] private ObiRopeBlueprint blueprint;
	[SerializeField] private ObiRopeExtrudedRenderer ropeRenderer;
	[SerializeField] private ObiRopeCursor cursor;
	[SerializeField] private ObiParticleAttachment tongueEndAttachment;
	private Coroutine launchTongueRoutine;
	private TongueEnd tongEndInstance;

	private ChameleonModel model;
	private float startTime;
	private Vector2 targetPos;
	private float elapsed;
	private float attackPercentage = 0f;
	private bool fireflyCatced = false;

	public Action<Color> FireflyCatchedCallback;

	private void Awake()
	{
		ropeRenderer.enabled = false;
		TongueEnd.FireflyCatched += FireflyCatched;
	}

	private void FireflyCatched(Color color)
	{
		fireflyCatced = true;
		FireflyCatchedCallback?.Invoke(color);
	}

	private void OnDestroy()
	{
		DestroyImmediate(blueprint);
		TongueEnd.FireflyCatched -= FireflyCatched;
	}

	public void Initialize(ChameleonModel model)
	{
		this.model = model;
	}

	public void LaunchTongue(float force)
	{
		fireflyCatced = false;
		Vector3 mouse = Mouse.current.position.ReadValue();
		mouse.z = transform.position.z - Camera.main.transform.position.z;
		Vector3 mouseInScene = Camera.main.ScreenToWorldPoint(mouse);
		var direction = (mouseInScene - transform.position).normalized;

		if (launchTongueRoutine != null)
		{
			StopCoroutine(launchTongueRoutine);
			if (tongEndInstance != null && tongEndInstance.gameObject != null)
			{
				Destroy(tongEndInstance.gameObject);
				tongueEndAttachment.target = null;
				rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
			}
		}

		tongEndInstance = Instantiate(tongueEndPrefab, transform.position + direction * 0.1f, Quaternion.identity);
		tongueEndAttachment.target = tongEndInstance.transform;
		targetPos = mouseInScene;
		//tongueEndRigidbody.AddForce(direction * force, ForceMode2D.Impulse)
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

		while (attackPercentage < 1f && !fireflyCatced)
		{
			elapsed += Time.deltaTime;
			attackPercentage = elapsed / attackTime;
			var realPercentage = model.TongueDistanceCurve.Evaluate(attackPercentage);
			var targetDist = model.MaxTongueDistance * realPercentage;
			var deltaDist = targetDist - currentTongueDist;
			currentTongueDist = targetDist;
			cursor.ChangeLength(rope.restLength + deltaDist);
			tongEndInstance.Rigidbody2D.MovePosition(transform.position + direction * model.MaxTongueDistance * realPercentage);
			yield return null;
		}

		var retractPercentage = 0f;
		elapsed = 0f;

		if (fireflyCatced)
		{
			retractTime *= attackPercentage;
		}

		while (retractPercentage < 1f)
		{
			elapsed += Time.deltaTime;
			retractPercentage = elapsed / retractTime;
			if (rope.restLength > 0 && rope.elements != null && rope.elements.Count > 0)
			{
				cursor.ChangeLength(Mathf.Max(model.MaxTongueDistance * (1f - retractPercentage), 0.1f));
				tongEndInstance.Rigidbody2D.MovePosition(transform.position + direction * model.MaxTongueDistance * (1f - retractPercentage));
			}
			yield return null;
		}

		Destroy(tongEndInstance.gameObject);
		tongueEndAttachment.target = null;
		rope.SetConstraintsDirty(Oni.ConstraintType.Pin);
		fireflyCatced = false;
		ropeRenderer.enabled = false;
	}
}
