using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace RModule.Runtime.Arcade {

	[Serializable]
	public class SpawnData {

	}

	public abstract class HeroSpawnerBlock<T0,T1> : BaseBlock where T1 : SpawnData {

		[SerializeField] protected T0 _heroPrefab = default;
		[SerializeField] protected T1 _spawnData = default;


		public abstract T0 Spawn(SpawnData spawnData = null);
	}
}

