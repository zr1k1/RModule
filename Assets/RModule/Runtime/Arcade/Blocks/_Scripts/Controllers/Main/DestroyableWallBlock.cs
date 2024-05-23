namespace RModule.Runtime.Arcade {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using RModule.Runtime.LeanTween;

	public class DestroyableWallBlock : WallBlock {

		// Outlets
		[SerializeField] protected float _destroyAnimationDuration = default;

		//protected override void Start() {
		//	p_contactDetector.Setup(this);
		//}

		public override void Die() {
			base.Die();
			StartCoroutine(PlayAnimationAndDisableCollider());
			Destroy(gameObject, _destroyAnimationDuration);
		}

		IEnumerator PlayAnimationAndDisableCollider() {
			yield return LeanTween.alpha(gameObject, 0f, _destroyAnimationDuration);
			p_collider2D.enabled = false;
		}

		public override void OnStartContact(CannonBlockFireUnit contactedObject) {
			Die();
		}
	}
}
