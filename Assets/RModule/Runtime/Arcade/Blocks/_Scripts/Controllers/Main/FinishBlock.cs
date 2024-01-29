namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class FinishBlock : BaseBlock {

		public virtual void OnTriggerEnter2D(Collider2D collider) {
			IFinishBlockCollisionHandler iIFinishBlockCollisionHandler = collider.GetComponent<IFinishBlockCollisionHandler>();
			if (iIFinishBlockCollisionHandler != null) {
				PlayAnimation();
				iIFinishBlockCollisionHandler.OnContactFinishBlock(this);
			}
		}

		public virtual void PlayAnimation() {

		}
	}
}
