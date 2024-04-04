namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SlidingDoorAnimationComponent : MonoBehaviour, IDoorAnimationComponent {

		// Outlets
		[SerializeField] MoveGoAnimation _openDoor = default;
		[SerializeField] MoveGoAnimation _closeDoor = default;

		public IEnumerator OpenAnimation() {
			_closeDoor?.Stop();
			_openDoor?.Play();
			yield return new WaitForSeconds(_openDoor.Duration);
		}

		public IEnumerator CloseAnimation() {
			_openDoor?.Stop();
			_closeDoor?.Play();
			yield return new WaitForSeconds(_closeDoor.Duration);
		}
	}
}
