using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRunner : MonoBehaviour
{
	[SerializeField] protected int MaxMobCount = 5;
	[SerializeField] protected float SpawnInterval = 2f;
	
	private List<LevelData> _levelDatas = null;

	private int _mobsCount = 0;
	
	
	private void Awake()
	{
		_levelDatas = new List<LevelData>(Resources.LoadAll<LevelData>("Data"));
		EventBus.Sub(MobKilled, EventBus.MOB_KILLED);
		EventBus<SpawnMobMessage>.Sub(MobSpawned);
		EventBus<LoadLevelMessage>.Sub(LoadLevelMessage);
		
	}

	private void Start()
	{
		EventBus<LoadLevelMessage>.Pub(new LoadLevelMessage(0));
	}

	private void OnDestroy()
	{
		EventBus.Unsub(MobKilled, EventBus.MOB_KILLED);
		EventBus<SpawnMobMessage>.Unsub(MobSpawned);
		EventBus<LoadLevelMessage>.Unsub(LoadLevelMessage);
	}

	public void LoadLevel(int index)
	{
		var level = _levelDatas.Find(l => l.Index == index);
		if (level == null)
		{
			EventBus.Pub(EventBus.PLAYER_WON);
			return;
		}

		StartCoroutine(Waves(level.WaveDatas, level.WaveInterval, level.Index));
		EventBus<FieldCreateMessage>.Pub(new FieldCreateMessage()
		{
			Field = level.GetMap()
		});
	}

	private void MobKilled()
	{
		_mobsCount--;
	}
	
	public void MobSpawned(SpawnMobMessage message)
	{
		_mobsCount++;
	}

	private void LoadLevelMessage(LoadLevelMessage message)
	{
		LoadLevel(message.LevelIndex);
	}

	private IEnumerator Waves(List<WaveData> waveDatas, float interval, int level)
	{
		foreach (var waveData in waveDatas)
		{
			foreach (var mobData in waveData.WaveMobNCount)
			{
				for (int i = 0; i < mobData.y; i++)
				{
					while (_mobsCount >= MaxMobCount)
					{
						yield return new WaitForSeconds(SpawnInterval);
					}

					EventBus<SpawnMobMessage>.Pub(new SpawnMobMessage()
					{
						Type = mobData.x
					});
					yield return new WaitForSeconds(SpawnInterval);
				}
			}
			yield return  new WaitForSeconds(interval);
		}
		yield return  new WaitForSeconds(interval);
		EventBus<LoadLevelMessage>.Pub(new LoadLevelMessage(level + 1));
	} 
}