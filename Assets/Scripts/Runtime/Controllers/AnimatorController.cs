﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	const string SPEED = "Speed";
	const string ATTACK = "Attack";

	public Action AttackAction;
	[SerializeField] private Animator animator;

	public void PlayAttackAnimation()
	{
		animator.SetTrigger(ATTACK);
	}

	public void Attack()
	{
		AttackAction?.Invoke();
	}

	public void Update(float characterSpeed)
	{
		animator.SetFloat(SPEED, Mathf.Abs(characterSpeed));
	}
}
