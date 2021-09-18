using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	const string SPEED_KEY = "Speed";

	[SerializeField] private Animator animator;

	public void Update(float characterSpeed)
	{
		animator.SetFloat(SPEED_KEY, Mathf.Abs(characterSpeed));
	}
}
