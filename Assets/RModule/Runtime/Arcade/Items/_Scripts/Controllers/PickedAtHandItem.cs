namespace RModule.Runtime.Arcade {

	using UnityEngine;
	using UnityEngine.Events;

	public class PickedAtHandItem : Item, IPickable {

		public UnityEvent DidTap = default;
		public UnityEvent DidPicked = default;

		// Privats

		public virtual void Drop(GameObject dropperGo) {
			DidTap = null;
		}

		public virtual void PickUp(GameObject pickerGo) {
			p_rigidbody2D.constraints = RigidbodyConstraints2D.None;
			DidPicked?.Invoke();
		}

		protected virtual void OnMouseUpAsButton() {
			DidTap?.Invoke();
		}
	}
}
