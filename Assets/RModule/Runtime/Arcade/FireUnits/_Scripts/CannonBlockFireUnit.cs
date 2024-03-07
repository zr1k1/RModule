namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class CannonBlockFireUnit : FireUnit {

		protected override void Start() {
			p_contactDetector.Setup(this);
		}
	}
}