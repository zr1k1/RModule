namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class CollectedItem : Item, IPickable {

		public virtual void PickUp(GameObject pickerGo) {
			p_sfx.PlayEffect();
			gameObject.SetActive(false);
		}

		public virtual void Drop(GameObject droperGo) {
		}
	}
}
