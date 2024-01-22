using System;
using UnityEngine;

[Serializable]
public class DamageData {
	// Acessors
	public DamageConfig damageConfig;
	public GameObject damageSourceGameObject;

	// Optional
	public Vector3 point;

	public virtual float GetDamage() {
		if (damageConfig == null) {
			Debug.LogError("Damage config for current game object is not settuped on inspector");
			return 0;
		}
		return damageConfig.Damage;
	}
}
