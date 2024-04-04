namespace RModule.Runtime.Arcade {

	using System.Collections;
	using UnityEngine;
	using UnityEngine.Events;

	public class Door : BaseBlock, IDoor, IDoorAnimationComponent {
		// Events
		public UnityEvent DidOpened = default;
		public UnityEvent DidClosed = default;

		// Outlets
		[SerializeField] protected GameObject p_doorAnimationComponent = default;

		// Privats
		protected bool _isOpen;
		IDoorAnimationComponent _doorAnimationComponent;

		protected override void Start() {
			p_contactDetector.Setup(this);
			_doorAnimationComponent = p_doorAnimationComponent.GetComponent<IDoorAnimationComponent>();
			if (_doorAnimationComponent == null)
				Debug.LogError($"Setup game object to doorAnimationComponent in inspector with realized IDoorAnimationComponent interface");
		}

		public virtual void Open() {
			if (_isOpen)
				return;
			Debug.Log($"Door : Open");
			StartCoroutine(ChangeState(true));
		}

		public virtual void Close() {
			if (!_isOpen)
				return;
			Debug.Log($"Door : Close");
			StartCoroutine(ChangeState(false));
		}

		IEnumerator ChangeState(bool open) {
			_isOpen = open;

			if (_isOpen)
				DidOpened?.Invoke();
			else
				DidClosed?.Invoke();

			yield return open ? OpenAnimation() : CloseAnimation();
			p_collider2D.enabled = !open;
		}

		public IEnumerator OpenAnimation() {
			yield return _doorAnimationComponent?.OpenAnimation();
		}

		public IEnumerator CloseAnimation() {
			yield return _doorAnimationComponent?.CloseAnimation();
		}
	}
}