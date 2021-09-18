using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CritterGeneratorData", menuName = "Properties/CritterGeneratorData", order = 1)]
public class CritterGeneratorData : ScriptableObject
{
	public Color[] Colors;
	public float SpeedMin;
	public float SpeedMax;
	public float DirectionChangeTimeMin;
	public float DirectionChangeTimeMax;
	[Range(-5, 5)] public float FactorE;
	[Range(-5, 5)] public float FactorPI;
	[Range(-5, 5)] public float Factor1;
	[Range(-5, 5)] public float FactorTotal;
	[Range(-5, 5)] public float ScaleEMin;
	[Range(-5, 5)] public float ScaleEMax;
	[Range(-5, 5)] public float ScalePIMin;
	[Range(-5, 5)] public float ScalePIMax;
	[Range(-5, 5)] public float Scale1Min;
	[Range(-5, 5)] public float Scale1Max;
}
