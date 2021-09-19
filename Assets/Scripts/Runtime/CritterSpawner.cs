using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpawner
{
	private CritterGeneratorData generatorData;
	private CritterController critterPrefab;

	public CritterSpawner()
	{
		generatorData = Resources.Load<CritterGeneratorData>(Paths.CRITTER_GENERATOR_DATA);
		critterPrefab = Resources.Load<CritterController>(Paths.FIREFLY_PREFAB);
	}

	public CritterController CreateCritter(Vector2 spawnPosition, Transform parent)
	{
		var critter = MonoBehaviour.Instantiate(critterPrefab, parent);
		var critterModel = new CritterModel();
		critterModel.Color = generatorData.Colors[Random.Range(0, generatorData.Colors.Length)];
		critterModel.Speed = Random.Range(generatorData.SpeedMin, generatorData.SpeedMax);
		critterModel.FlySeed = Random.Range(0f, 10000f);
		critterModel.DirectionChangeTimeMin = generatorData.DirectionChangeTimeMin;
		critterModel.DirectionChangeTimeMax = generatorData.DirectionChangeTimeMax;
		critterModel.FactorE = generatorData.FactorE;
		critterModel.FactorPI = generatorData.FactorPI;
		critterModel.Factor1 = generatorData.Factor1;
		critterModel.FactorTotal = generatorData.FactorTotal;
		critterModel.ScaleE = Random.Range(generatorData.ScaleEMin, generatorData.ScaleEMax);
		critterModel.ScalePI = Random.Range(generatorData.ScalePIMin, generatorData.ScalePIMax);
		critterModel.Scale1 = Random.Range(generatorData.Scale1Min, generatorData.Scale1Max);
		critter.Initialize(critterModel, spawnPosition);

		return critter;
	}
}
