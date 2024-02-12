namespace RModule.Runtime.Arcade {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class FieldItem : ConsumableItem {

		[SerializeField] Buff _buffPrefab = default;

		// Privats
		Buff _buff;

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
				_buff = Instantiate(_buffPrefab);
				iBuffUser?.ApplyBuff(_buff);
			}
		}
	}
}
