namespace RModule.Runtime.Arcade {

	using UnityEngine;
	using UnityEngine.Events;

	public class ActionButton : UnPickableItem, IUseable {
		// Outlets
		[SerializeField] UnityEvent DidUse = default;

		[SerializeField] Transform _pressedBtn = default;
		[SerializeField] Vector2 _pressedStateLocalPosition = default;
		[SerializeField] bool _startAtStatePressed = default;

		//Privats
		bool _isPressed;

		protected override void Awake() {
			base.Awake();
			_isPressed = _startAtStatePressed;
			if (_isPressed)
				_pressedBtn.localPosition = _pressedStateLocalPosition;
		}

		public void Use(GameObject userGo) {
			Debug.Log($"ActionButton : Use");
			if (_isPressed)
				return;

			Press();
			DidUse?.Invoke();
		}

		public void Press() {
			_isPressed = true;
			_pressedBtn.localPosition = _pressedStateLocalPosition;
			TryPlaySound();
		}

		public void UnPress() {
			_isPressed = false;
			_pressedBtn.localPosition = Vector2.zero;
		}

		protected override void TryPlaySound() {
			if (_isPressed)
				base.TryPlaySound();
		}
	}
}
