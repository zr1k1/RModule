namespace RModule.Runtime.Arcade {
	using System;

	using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

	public class Ammo : Item, IPickable, IValueable<int> {
		public FireUnit FireUnitPrefab => _fireUnitPrefab;

		[SerializeField] FireUnit _fireUnitPrefab = default;
		[SerializeField] AmmoData _ammoData = default;

		// Classes
		[Serializable]
		public class AmmoData {
			public int FireUnitsCount = 1;
		}

		protected override void Start() {
			p_contactDetector.Setup(this);
		}

		public override void Destroy() {
			base.Destroy();

			enabled = false;
			p_collider2D.enabled = false;
			Destroy(gameObject, 0);
		}

		public virtual void Drop(GameObject dropperGo) {
		}

		public virtual void PickUp(GameObject pickerGo) {
		}

		public int GetValue() {
			return _ammoData.FireUnitsCount;
		}
	}
}