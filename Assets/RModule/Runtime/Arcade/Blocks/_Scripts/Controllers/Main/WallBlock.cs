namespace RModule.Runtime.Arcade {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using RModule.Runtime.LeanTween;

	public class WallBlock : BaseBlock, IDamagable {

		// Outlets
		[SerializeField] float _destroyAnimationDuration = default;

		// Interfaces
		public interface IWallDestroyer { }

		public bool TryTakeDmg(DamageData damageData) {
			var iWallDestroyer = damageData.damageSourceGameObject.GetComponent<IWallDestroyer>();
			if (iWallDestroyer != null) {
				Die();
				return true;
			}
			return false;
		}

		public override void Die() {
			base.Die();
			StartCoroutine(PlayAnimationAndDisableCollider());
			Destroy(gameObject, _destroyAnimationDuration);
		}

		IEnumerator PlayAnimationAndDisableCollider() {
			yield return LeanTween.alpha(gameObject, 0f, _destroyAnimationDuration);
			p_collider2D.enabled = false;
		}
	}
}
