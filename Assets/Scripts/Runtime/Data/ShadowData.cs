using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShadowData", menuName = "Properties/ShadowData", order = 3)]
public class ShadowData : ScriptableObject
{
	public float BaseAppearCooldown;
	public float MinimumAppearCooldown;
	public float MaximumAppearCooldown;
	public AnimationCurve AppearCooldownCurve;
	public AnimationCurve HearthbeatCurve;
}
