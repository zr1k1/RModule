namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class ConsumableItem : Item, IPickable {

		protected override void Start() {
			p_contactDetector.Setup(this);
		}

		public virtual void Drop(GameObject dropperGo) {
		}

		public virtual void PickUp(GameObject pickerGo) {
			TryPlaySound();
		}

		public virtual void Consume(GameObject consumer) {
			gameObject.SetActive(false);
		}
	}

}
