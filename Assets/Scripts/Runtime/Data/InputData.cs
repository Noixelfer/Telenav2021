using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[CreateAssetMenu(fileName = "MovementData", menuName = "Properties/MovementData", order = 2)]

public class InputData : ScriptableObject
{
	[HideInInspector] public Vector2 MoveValue;
	public event Action JumpCallback;
	public event Action<CallbackContext> LaunchTongueCallback;

	public void Move(CallbackContext context)
	{
		MoveValue = context.ReadValue<Vector2>();
	}

	public void Jump(CallbackContext context)
	{
		JumpCallback?.Invoke();
	}

	public void LaunchTongue(CallbackContext context)
	{
		LaunchTongueCallback?.Invoke(context);
	}
}
