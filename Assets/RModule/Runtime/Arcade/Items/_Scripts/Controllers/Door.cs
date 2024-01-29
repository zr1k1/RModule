namespace RModule.Runtime.Arcade {

	using System.Collections;
	using UnityEngine;
	using UnityEngine.Events;

	public class Door : Item, IDoor, IDoorAnimationComponent {
		// Events
		public UnityEvent DidOpened = default;
		public UnityEvent DidClosed = default;

		// Privats
		protected bool _isOpen = default;

		public virtual void Open() {
			if (_isOpen)
				return;
			Debug.Log($"Door : Open");
			StartCoroutine(WaitDorAnimationEnd(true));
		}

		public virtual void Close() {
			if (!_isOpen)
				return;
			Debug.Log($"Door : Close");
			StartCoroutine(WaitDorAnimationEnd(false));
		}

		public virtual IEnumerator OpenAnimation() {
			yield return ((IDoorAnimationComponent)p_itemAnimationComponent)?.OpenAnimation();
		}

		public virtual IEnumerator CloseAnimation() {
			yield return ((IDoorAnimationComponent)p_itemAnimationComponent)?.CloseAnimation();
		}

		IEnumerator WaitDorAnimationEnd(bool open) {
			p_collider2D.enabled = !open;
			yield return open ? OpenAnimation() : CloseAnimation();
			_isOpen = open;

			if (_isOpen)
				DidOpened?.Invoke();
			else
				DidClosed?.Invoke();
		}
	}
}