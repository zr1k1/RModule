namespace RModule.Runtime.Arcade {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using RModule.Runtime.LeanTween;

	public class DestroyableWallBlock : WallBlock, IStartContactDetector<CannonBlockFireUnit> {

		// Outlets
		[SerializeField] protected float _destroyAnimationDuration = default;

		public override void Die() {
			base.Die();
			StartCoroutine(PlayAnimationAndDisableCollider());
			Destroy(gameObject, _destroyAnimationDuration);
		}

		IEnumerator PlayAnimationAndDisableCollider() {
			yield return LeanTween.alpha(gameObject, 0f, _destroyAnimationDuration);
			p_collider2D.enabled = false;
		}

		public void OnStartContact(CannonBlockFireUnit contactedObject) {
			Die();
			Destroy(contactedObject.gameObject);
		}
	}
}
