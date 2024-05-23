namespace RModule.Runtime.Arcade {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class FieldItem : ConsumableItem {

		[SerializeField] Buff _buffPrefab = default;

		// Privats

		protected override void Start() {
			p_contactDetector.Setup(this);
		}

		public override void Drop(GameObject dropperGo) {
			base.Drop(dropperGo);
		}

		public override void PickUp(GameObject pickerGo) {
			base.PickUp(pickerGo);
		}

		public override void Consume(GameObject consumer) {
			base.Consume(consumer);

			var iBuffUser = consumer.GetComponent<IBuffUser>();
			if (iBuffUser != null) {
				iBuffUser?.ApplyBuff(Instantiate(_buffPrefab));
			}
		}
	}
}
