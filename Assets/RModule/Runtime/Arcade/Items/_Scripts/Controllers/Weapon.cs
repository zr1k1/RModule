using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	protected DamageDealerComponent _damageDealerComponent = default;

	protected virtual void Start() {
		_damageDealerComponent = GetComponent<DamageDealerComponent>();
		_damageDealerComponent.DamageDidDeal.AddListener(OnDealDamage);
	}

	public virtual void OnDealDamage(DamageData damageData) {
		Destroy();
	}

	public virtual void Destroy() {
		Destroy(gameObject);
	}
}
