using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpawnArea : MonoBehaviour
{
	const float MIN_SPAWN_DELAY = 0.7f;
	const float MAX_SPAWN_DELAY = 2f;

	[SerializeField] private BoxCollider2D area;
	[SerializeField] private int firefliesCount;
	private static CritterSpawner spawner;
	private int numberOfFlies;
	private float lastSpawnTime = 0f;
	private float spawnDelay;

	private void Awake()
	{
		if (spawner == null)
		{
			spawner = new CritterSpawner();
		}

		spawnDelay = Random.Range(MIN_SPAWN_DELAY, MAX_SPAWN_DELAY);
	}

	private void Start()
	{
		for (int i = 0; i < numberOfFlies; i++)
		{
			spawner.CreateCritter(GetRandomPosition(), transform);
		}
	}

	private void Update()
	{
		if (Time.time - lastSpawnTime >= spawnDelay && transform.childCount < firefliesCount)
		{
			spawner.CreateCritter(GetRandomPosition(), transform);
			lastSpawnTime = Time.time;
			spawnDelay = Random.Range(MIN_SPAWN_DELAY, MAX_SPAWN_DELAY);
		}
	}

	private Vector2 GetRandomPosition()
	{
		return new Vector2(area.transform.position.x + Random.Range(-area.size.x / 2f, area.size.x / 2f),
						   area.transform.position.y + Random.Range(-area.size.y / 2f, area.size.y / 2f));
	}
}
