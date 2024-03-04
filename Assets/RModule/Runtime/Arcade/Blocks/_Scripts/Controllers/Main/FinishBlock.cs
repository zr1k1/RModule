namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class FinishBlock : BaseBlock {

		protected override void Start() {
			p_contactDetector.Setup(this);
		}

		public virtual void PlayAnimation() {

		}

		public override void OnStartContact(GameObject userGo) {
			base.OnStartContact(userGo);
			PlayAnimation();
		}
	}
}
