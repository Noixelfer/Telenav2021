using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChameleonColor
{
	private float totalTime;
	private float remainingTime;
	public Color Color;

	public ChameleonColor(Color color, float lifeTime)
	{
		Color = color;
		totalTime = lifeTime;
		remainingTime = totalTime;
	}

	public void Update(float deltaTime)
	{
		remainingTime = Mathf.Max(remainingTime - deltaTime, 0f);
	}

	public float TimePercentage()
	{
		return remainingTime / totalTime;
	}
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
	public float ColorLifetime;
	public AnimationCurve TongueDistanceCurve;
	[Range(0, 4)] public int ColorsSize;
	[HideInInspector] public Queue<ChameleonColor> Colors = new Queue<ChameleonColor>();

	public void Reset()
	{
		Colors.Clear();
	}
}
