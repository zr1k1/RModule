namespace RModule.Runtime.DealDamageSystem {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System;

	[CreateAssetMenu(fileName = "DamageConfig", menuName = "RModule/Examples/AppConfigs/DamageConfig", order = 1)]
	public class DamageConfig : ScriptableObject {
		// Accessors
		public float Damage => _damage;

		// Outlets
		[SerializeField] protected float _damage = default;

		// Privats
	}
}
