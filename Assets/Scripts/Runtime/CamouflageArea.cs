using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamouflageArea : MonoBehaviour
{
	[SerializeField] private Color color;

	public Color GetColor()
	{
		return color;
	}
}
