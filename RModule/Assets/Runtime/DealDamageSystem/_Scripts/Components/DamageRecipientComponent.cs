using UnityEngine;

public class DamageRecipientComponent : MonoBehaviour, IDamagable {

	// Outlets
	[Tooltip("Object with implemented IDamagable interface for take damage")]
	[SerializeField] GameObject _damageTakerGameObject = default;

	[Header("Animation")]
	[SerializeField] protected ParticleSystem p_animation = default;

	public virtual bool TryTakeDmg(DamageData damageData) {
		if (_damageTakerGameObject != null) {
			var iDamagable = _damageTakerGameObject.GetComponent<IDamagable>();
			if (iDamagable != null) {
				if (iDamagable.TryTakeDmg(damageData)) {
					TryPlayAnimation(damageData);
					return true;
				}				
			}
		};

		return false;
	}

	protected virtual void TryPlayAnimation(DamageData damageData) {
		if (p_animation != null) {
			p_animation.transform.position = damageData.point;
			p_animation.Play();
		}
	}
}