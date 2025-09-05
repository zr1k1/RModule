using System;
using UnityEngine;
using UnityEngine.Events;
using RModule.Runtime.Sounds;
using RModule.Runtime.DealDamageSystem;

public class DamageDealerComponent : MonoBehaviour {
	// Delegates
	public delegate bool DealDamageCondition(GameObject gameObjectToDealDamage);

	public DealDamageCondition D_DealDamageCondition = (gameObjectToDealDamage) => { return true; };

	// Events
	public UnityEvent<DamageData> DamageDidDeal = default;

	// Outlets
	[SerializeField] protected DamageConfig p_damageConfig = default;
	[SerializeField] protected SoundEffectPlayer p_contactSfx = default;
	[SerializeField] protected ParticleSystem p_animation = default;

	public virtual void OnCollisionEnter2D(Collision2D collision) {
		var damageRecipientComponent = collision.collider.GetComponent<DamageRecipientComponent>();
		if (damageRecipientComponent != null && D_DealDamageCondition(collision.gameObject)) {		
			DealDamage(damageRecipientComponent, new DamageData {
				damageConfig = p_damageConfig,
				damageSourceGameObject = gameObject,
				point = collision.contacts[0].point
			});
		}
	}

	public virtual void OnTriggerEnter2D(Collider2D collider) {
		var damageRecipientComponent = collider.GetComponent<DamageRecipientComponent>();
		if (damageRecipientComponent != null && D_DealDamageCondition(collider.gameObject)) {		
			DealDamage(damageRecipientComponent, new DamageData {
				damageConfig = p_damageConfig,
				damageSourceGameObject = gameObject,
				point = collider.ClosestPoint(transform.position)
			});
		}
	}

	public bool CheckCanDealDamageToGameObject(GameObject go,out DamageRecipientComponent damageRecipientComponent) {
		damageRecipientComponent = go.GetComponent<DamageRecipientComponent>();

		return damageRecipientComponent != null && D_DealDamageCondition(go);
	}

	protected virtual void DealDamage(DamageRecipientComponent damageRecipientComponent, DamageData damageData) {
		if (damageRecipientComponent.TryTakeDmg(damageData)) {
			Debug.Log($"DamageDealerComponent : DealDamage from {gameObject.name} to {damageRecipientComponent.gameObject.name} dmg = {damageData.damageConfig.Damage}");
			p_contactSfx?.PlayEffect();
			TryPlayAnimation(damageData);
			DamageDidDeal?.Invoke(damageData);
		}
	}

	protected virtual void TryPlayAnimation(DamageData damageData) {
		if (p_animation != null) {
			p_animation.transform.position = damageData.point;
			p_animation.Play();
		}
	}
}
