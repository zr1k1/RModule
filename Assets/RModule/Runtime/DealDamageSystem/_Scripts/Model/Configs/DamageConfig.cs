using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageConfig", menuName = "AppConfigs/DamageConfig", order = 1)]
public class DamageConfig : ScriptableObject {
	// Accessors
	public float Damage => _damage;

	// Outlets
	[SerializeField] float _damage = default;

	// Privats
}
