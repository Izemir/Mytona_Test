using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MobSpawner : Handler<SpawnMobMessage>
{
    [SerializeField] protected List<Mob> Prefabs;

	protected override void Awake()
	{
		base.Awake();
		EventBus.Sub(() => { EventBus<SpawnMobMessage>.Unsub(HandleMessage);},EventBus.PLAYER_DEATH);
	}

	public override void HandleMessage(SpawnMobMessage message)
	{
		var position = new Vector3(Random.value * 11 - 6,1,Random.value * 11 - 6);
		var mob = Prefabs.FirstOrDefault(i => i.GetId() == message.Type);
		if (mob == null) mob = Prefabs[0];
		Instantiate(mob, position, Quaternion.identity);
	}
}