namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class CollectedItem : Item, IPickable {
		//Accessors
		public int Value => _value;

		// Outlets
		[SerializeField] protected int _value = 1;

		public virtual void PickUp(GameObject pickerGo) {
			Destroy();
		}

		public virtual void Drop(GameObject droperGo) {
		}
	}
}
