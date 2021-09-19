using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChameleonColor
{
	public Color Color;
	public float StartTime;
}
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
	[Range(0, 4)]public int ColorsSize;
	[HideInInspector] public Queue<ChameleonColor> Colors = new Queue<ChameleonColor>();
}
