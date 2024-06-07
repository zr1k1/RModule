using System;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.DealDamageSystem;

[Serializable]
public class DamageData {
	// Acessors
	public const int MAX_DAMAGE = 9999999;

	public DamageConfig damageConfig;
	public GameObject damageSourceGameObject;

	// Optional
	public Vector3 point;

	// Privats
	int _damage = -1;

	public virtual int GetDamage() {
		if (damageConfig == null) {
			Debug.LogError("Damage config for current game object is not settuped on inspector");
			return 0;
		} else if (_damage == -1) {
			_damage = (int)damageConfig.Damage;
		}

		return _damage;
	}

	public void ModifyDamage(int amount) {
		_damage = Mathf.Clamp(_damage + amount, 0, MAX_DAMAGE);
	}
}
