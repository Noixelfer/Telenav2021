using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[CreateAssetMenu(fileName = "MovementData", menuName = "Properties/MovementData", order = 2)]

public class MovementData : ScriptableObject
{
	[HideInInspector] public Vector2 MoveValue;
	public event Action JumpCallback;

	public void Move(CallbackContext context)
	{
		MoveValue = context.ReadValue<Vector2>();
	}

	public void Jump(CallbackContext context)
	{
		JumpCallback?.Invoke();
	}
}
