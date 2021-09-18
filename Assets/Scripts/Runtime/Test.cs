using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	private CritterSpawner critterSpawner;

	// Start is called before the first frame update
	void Start()
	{
		critterSpawner = new CritterSpawner();
		for (int i = 0; i < 20; i++)
		{
			critterSpawner.CreateCritter();
		}
	}
}
