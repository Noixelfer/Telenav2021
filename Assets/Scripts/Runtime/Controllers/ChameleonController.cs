using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChameleonController : MonoBehaviour
{
	private const float JUMP_DELAY = 0.2f;
	private const float ATTACK_DELAY = 1f;

	[SerializeField] private Rigidbody2D body;
	[SerializeField] private new BoxCollider2D collider;
	[SerializeField] private ChameleonModel model;
	[SerializeField] private InputData movementData;
	[SerializeField] private GroundDetector groundDetector;
	[SerializeField] private TongueController tongue;
	[SerializeField] private AnimatorController animatorController;

	private float lastJumpTime = 0f;
	private float lastAttackTime = 0f;
	private float desiredVelocity;
	private float currentVelocity;
	private float acceleration;
	private float force;
	bool launched = false;

	private void Awake()
	{
		movementData.JumpCallback += Jump;
		movementData.LaunchTongueCallback += PlayAttack;
		animatorController.AttackAction += LaunchTongue;
		tongue.Initialize(model);
	}

	private void OnDestroy()
	{
		movementData.JumpCallback -= Jump;
		movementData.LaunchTongueCallback -= PlayAttack;
		animatorController.AttackAction -= LaunchTongue;
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
		animatorController.Update(movementData.MoveValue.x);
	}

	private void FixedUpdate()
	{
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
}
