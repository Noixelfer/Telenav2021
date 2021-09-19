using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChameleonController : MonoBehaviour
{
	private const float JUMP_DELAY = 0.2f;
	private const float ATTACK_DELAY = 1f;
	private const float COLOR_ERROR_TRESHOLD = 12f;

	[SerializeField] private Rigidbody2D body;
	[SerializeField] private new Collider2D collider;
	[SerializeField] private ChameleonModel model;
	[SerializeField] private InputData movementData;
	[SerializeField] private GroundDetector groundDetector;
	[SerializeField] private TongueController tongue;
	[SerializeField] private AnimatorController animatorController;
	[SerializeField] private SpriteRenderer renderer;

	public bool IsCamouflage { get; private set; } = false;
	public Action<Color> OnFireflyCatched;
	private CamouflageArea currentCamouflageArea = null;
	private float lastJumpTime = 0f;
	private float lastAttackTime = 0f;
	private float desiredVelocity;
	private float currentVelocity;
	private float acceleration;
	private float force;
	bool launched = false;
	bool alive = true;
	Color currentColor;

	private void Awake()
	{
		model.Reset();
		alive = true;
		movementData.JumpCallback += Jump;
		movementData.LaunchTongueCallback += PlayAttack;
		animatorController.AttackAction += LaunchTongue;
		tongue.FireflyCatchedCallback += FireflyCatched;
		tongue.Initialize(model);
		UpdateCharacterColor();
		ShadowManager.OnShadowAppearEnd += OnShadowAppearEnd;
	}

	private void OnDestroy()
	{
		movementData.JumpCallback -= Jump;
		movementData.LaunchTongueCallback -= PlayAttack;
		animatorController.AttackAction -= LaunchTongue;
		tongue.FireflyCatchedCallback -= FireflyCatched;
		ShadowManager.OnShadowAppearEnd -= OnShadowAppearEnd;
	}

	private void FireflyCatched(Color color)
	{
		model.Colors.Enqueue(new ChameleonColor(color, model.ColorLifetime));
		if (model.Colors.Count > model.ColorsSize)
		{
			model.Colors.Dequeue();
		}

		OnFireflyCatched?.Invoke(color);
		UpdateCharacterColor();
	}

	private void OnShadowAppearEnd(bool survived)
	{
		if (survived)
		{
			return;
		}

		alive = false;
		body.velocity = Vector2.zero;
		body.isKinematic = true;
		animatorController.PlayDeathAnimation();
	}

	private void UpdateCharacterColor()
	{
		var colors = model.Colors.ToArray();
		var yellowColor = new Color(1f, 1f, 0f);

		if (colors.Length == 0)
		{
			currentColor = Color.white;
		}
		else if (colors.Length == 1)
		{
			currentColor = colors[0].Color;
		}
		else if (colors[0].Color == colors[1].Color)
		{
			currentColor = colors[0].Color;
		}
		else
		{
			if ((colors[0].Color == Color.red && colors[1].Color == Color.blue) ||
				(colors[1].Color == Color.red && colors[0].Color == Color.blue))
			{
				currentColor = new Color(0.5f, 0f, 0.5f);
			}
			else if ((colors[0].Color == Color.red && colors[1].Color == yellowColor) ||
				(colors[1].Color == Color.red && colors[0].Color == yellowColor))
			{
				currentColor = new Color(1f, 0.5f, 0f);

			}
			else if ((colors[0].Color == Color.blue && colors[1].Color == yellowColor) ||
				(colors[1].Color == Color.blue && colors[0].Color == yellowColor))
			{
				currentColor = new Color(0f, 1f, 0f);
			}
			else
			{
				currentColor = Color.gray;
			}
		}

		renderer.color = currentColor;
		UpdateCamouflageState();
	}

	private void PlayAttack(InputAction.CallbackContext context)
	{
		if (Time.time - lastAttackTime <= ATTACK_DELAY)
		{
			return;
		}

		lastAttackTime = Time.time;
		animatorController.PlayAttackAnimation();
		launched = true;
	}

	private void LaunchTongue()
	{
		tongue.LaunchTongue(30f);
	}

	private void Jump()
	{
		if (groundDetector.OnGround && Time.time - lastJumpTime >= JUMP_DELAY)
		{
			body.AddForce(Vector2.up * model.JumpForce, ForceMode2D.Impulse);
			lastJumpTime = Time.time;
		}
	}

	private void Update()
	{
		if (!alive) return;
		animatorController.Update(movementData.MoveValue.x);
		UpdateColorsTimers();
	}

	private void FixedUpdate()
	{
		if (!alive) return;
		desiredVelocity = movementData.MoveValue.x * model.MovementSpeed * (groundDetector.OnGround ? 1f : model.InAirMovementPercentage);

		if (movementData.MoveValue.x != 0)
		{
			transform.localScale = movementData.MoveValue.x > 0 ? Vector3.one : new Vector3(-1, 1, 1);
		}

		currentVelocity = body.velocity.x;
		acceleration = (desiredVelocity - currentVelocity) / Time.fixedDeltaTime;
		force = body.mass * acceleration;
		body.AddForce(Vector3.right * force, ForceMode2D.Force);
	}

	private void UpdateColorsTimers()
	{
		foreach (var color in model.Colors)
		{
			color.Update(Time.deltaTime);
		}

		if (model.Colors.Count > 0f && model.Colors.Peek().TimePercentage() == 0)
		{
			model.Colors.Dequeue();
			UpdateCharacterColor();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!alive) return;
		if (collision.GetComponent<CamouflageArea>() is var area && area != null)
		{
			currentCamouflageArea = area;
			UpdateCamouflageState();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!alive) return;
		if (currentCamouflageArea != null && collision.gameObject == currentCamouflageArea.gameObject)
		{
			currentCamouflageArea = null;
			UpdateCamouflageState();
		}
	}

	private void UpdateCamouflageState()
	{
		if (currentCamouflageArea == null)
		{
			IsCamouflage = false;
			Debug.Log("Camouflage: " + IsCamouflage);
			return;
		}

		var areaColor = currentCamouflageArea.GetColor();
		var error = Mathf.Abs(areaColor.r - currentColor.r) * 255f + Mathf.Abs(areaColor.g - currentColor.g) * 255f + Mathf.Abs(areaColor.b - currentColor.b) * 255f;

		IsCamouflage = error <= COLOR_ERROR_TRESHOLD;
		Debug.Log("Camouflage: " + IsCamouflage);
	}
}
