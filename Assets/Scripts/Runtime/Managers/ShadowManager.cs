using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

public class ShadowManager : MonoBehaviour
{
	[Header("Data")]
	[SerializeField] private ShadowData shadowData;
	[SerializeField] private LevelData levelData;
	[Header("Audio")]
	[SerializeField] private AudioSource hearthBeat;
	[SerializeField] private AudioSource voice;
	[SerializeField] private AudioClip hearthbeatSlow;
	[SerializeField] private AudioClip hearthbeatFast;
	[SerializeField] private Light2D globalLight;
	[SerializeField] private ChameleonController chameleonController;

	public static Action OnShadowAppearStart;
	public static Action<bool> OnShadowAppearEnd;
	public float AppearProgress { get; private set; } = 0f;

	private float appearTime;
	private Coroutine appearRoutine;
	private float appearDuration;
	private bool fastHearthbeat = false;

	private void Start()
	{
		appearTime = Random.Range(shadowData.MinimumAppearCooldown, shadowData.MaximumAppearCooldown) * 1.5f; //We give more time at the start before the shadow appears
	}

	private void Update()
	{
		if (appearRoutine == null && Time.time >= appearTime)
		{
			appearRoutine = StartCoroutine(Appear());
		}
	}

	private IEnumerator Appear()
	{
		OnShadowAppearStart?.Invoke();
		PrepareHearthbeatAudio();
		var percentage = Random.Range(levelData.CompletedShrines / levelData.TotalShrines, (levelData.CompletedShrines + 1) / levelData.TotalShrines);
		appearDuration = shadowData.BaseAppearCooldown * shadowData.AppearCooldownCurve.Evaluate(percentage);
		var elapsed = 0f;

		while (elapsed <= appearDuration)
		{
			elapsed += Time.deltaTime;
			AppearProgress = Mathf.Clamp01(elapsed / appearDuration);
			UpdateHearthbeat(AppearProgress);
			UpdateLight(AppearProgress);
			yield return null;
		}

		hearthBeat.Stop();
		OnShadowAppearEnd?.Invoke(chameleonController.IsCamouflage);
	}

	private void PrepareHearthbeatAudio()
	{
		fastHearthbeat = false;
		hearthBeat.volume = 0f;
		hearthBeat.pitch = 1f;
		hearthBeat.clip = hearthbeatSlow;
		hearthBeat.Play();
	}

	private void UpdateHearthbeat(float progress)
	{
		if (progress >= 0.5f && !fastHearthbeat)
		{
			hearthBeat.clip = hearthbeatFast;
			fastHearthbeat = true;
			hearthBeat.Play();
		}

		if (progress < 0.5f)
		{
			hearthBeat.volume = shadowData.HearthbeatCurve.Evaluate(progress);
		}
		else
		{
			hearthBeat.pitch = shadowData.HearthbeatCurve.Evaluate(progress);
		}
	}

	private void UpdateLight(float progress)
	{
		globalLight.intensity = 1f - 0.3f * progress;
	}
}
