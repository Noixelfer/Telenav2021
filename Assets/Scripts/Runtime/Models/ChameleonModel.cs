using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChameleonModel", menuName = "Properties/ChameleonModel", order = 2)]
public class ChameleonModel : ScriptableObject
{
	public float MovementSpeed;
	public float JumpForce;
	[Range(0, 1)] public float InAirMovementPercentage;
	public float MaxTongueDistance;
	public float TongueChargeTime;
	public float AttackTime;
	public AnimationCurve TongueDistanceCurve;
}
