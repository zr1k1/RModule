namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class CannonBlockFireUnit : FireUnit, IStartContactDetector<WallBlock> {

		protected override void Start() {
			p_contactDetector.Setup(this);
		}

		public void OnStartContact(WallBlock contactedObject) {
			Die();
		}
	}
}