using System;
using UnityEngine;
using UnityEngine.Events;
using RModule.Runtime.Sounds;

public class DamageDealerComponent : MonoBehaviour {
	// Delegates
	public delegate bool DealDamageCondition();
	public DealDamageCondition D_DealDamageCondition = () => { return true; };

	// Events
	public UnityEvent<DamageData> DamageDidDeal = default;

	// Outlets
	[SerializeField] protected DamageConfig p_damageConfig = default;
	[SerializeField] protected SoundEffectPlayer p_contactSfx = default;
	[SerializeField] protected ParticleSystem p_animation = default;

	public virtual void OnCollisionEnter2D(Collision2D collision) {
		var damageRecipientComponent = collision.collider.GetComponent<DamageRecipientComponent>();
		if (damageRecipientComponent != null && D_DealDamageCondition()) {
			DealDamage(damageRecipientComponent, new DamageData {
				damageConfig = p_damageConfig,
				damageSourceGameObject = gameObject,
				point = collision.contacts[0].point
			});
		}
	}

	public virtual void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log($"DamageDealerComponent : OnTriggerEnter2D {collider.gameObject.name}");
		var damageRecipientComponent = collider.GetComponent<DamageRecipientComponent>();
		if (damageRecipientComponent != null && D_DealDamageCondition()) {
			DealDamage(damageRecipientComponent, new DamageData {
				damageConfig = p_damageConfig,
				damageSourceGameObject = gameObject,
				point = collider.ClosestPoint(transform.position)
			});
		}
	}

	protected virtual void DealDamage(DamageRecipientComponent damageRecipientComponent, DamageData damageData) {
		if (((IDamagable)damageRecipientComponent).TryTakeDmg(damageData)) {
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
