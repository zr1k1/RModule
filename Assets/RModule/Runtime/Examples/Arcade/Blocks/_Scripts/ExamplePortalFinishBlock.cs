namespace RModule.Runtime.Arcade {

	using RModule.Runtime.LeanTween;
	using UnityEngine;

	public class PortalBlock : FinishBlock {
		// Outlets
		[SerializeField] Animator _animator = default;
		[SerializeField] GameObject _leftDoor = default;
		[SerializeField] GameObject _rightDoor = default;
		[SerializeField] float _duration = default;
		[Header("Debug")]
		[SerializeField] bool _playOpenAnimation = default;

		private void Update() {
			if (_playOpenAnimation) {
				_playOpenAnimation = false;
				PlayAnimation();
			}
		}

		public override void OnTriggerEnter2D(Collider2D collider) {
			IFinishBlockCollisionHandler iIFinishBlockCollisionHandler = collider.GetComponent<IFinishBlockCollisionHandler>();
			if (iIFinishBlockCollisionHandler != null) {
				iIFinishBlockCollisionHandler.OnContactFinishBlock(this);
			}
		}

		public void OnOpenDoorsTriggerEnter2D(Collider2D collider) {
			IFinishBlockCollisionHandler iIFinishBlockCollisionHandler = collider.GetComponent<IFinishBlockCollisionHandler>();
			if (iIFinishBlockCollisionHandler != null) {
				PlayAnimation();
			}
		}

		public override void PlayAnimation() {
			_animator.SetTrigger("open");
			LeanTween.moveLocalX(_leftDoor, -1, _duration);
			LeanTween.moveLocalX(_rightDoor, 1, _duration);
		}
	}
}