namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class WeaponConsumableItem : ConsumableItem, IPickable, IHandTransformer {
		// Outlets
		[SerializeField] Weapon _weaponPrefab = default;

		public interface IWeaponUser {
			bool TryTakeWeapon(Weapon weapon);
		}

		public void PickUp(GameObject pickerGo) {
			TryPlaySound();
		}

		public void Drop(GameObject droperGo) {
		}

		public override void Consume(GameObject consumer) {
			base.Consume(consumer);
			var iWeaponUser = consumer.GetComponent<IWeaponUser>();
			if (iWeaponUser != null) {
				if (iWeaponUser.TryTakeWeapon(Instantiate(_weaponPrefab)))
					gameObject.SetActive(false);
			}
		}
	}
}
