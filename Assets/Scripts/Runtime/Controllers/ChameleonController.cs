using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChameleonController : MonoBehaviour
{
	private const float JUMP_DELAY = 0.2f;

	[SerializeField] private Rigidbody2D body;
	[SerializeField] private new BoxCollider2D collider;
	[SerializeField] private ChameleonModel model;
	[SerializeField] private MovementData movementData;
	[SerializeField] private GroundDetector groundDetector;

	private float lastJumpTime = 0f;
	private float desiredVelocity;
	private float currentVelocity;
	private float acceleration;
	private float force;

	private void Awake()
	{
		movementData.JumpCallback += Jump;
	}

	private void Jump()
	{
		if (groundDetector.OnGround && Time.time - lastJumpTime >= JUMP_DELAY)
		{
			body.AddForce(Vector2.up * model.JumpForce, ForceMode2D.Impulse);
			lastJumpTime = Time.time;
		}
	}

	private void FixedUpdate()
	{
		desiredVelocity = movementData.MoveValue.x * model.MovementSpeed * (groundDetector.OnGround ? 1f : model.InAirMovementPercentage);
		currentVelocity = body.velocity.x;
		acceleration = (desiredVelocity - currentVelocity) / Time.fixedDeltaTime;
		force = body.mass * acceleration;
		body.AddForce(Vector3.right * force, ForceMode2D.Force);
	}
}
