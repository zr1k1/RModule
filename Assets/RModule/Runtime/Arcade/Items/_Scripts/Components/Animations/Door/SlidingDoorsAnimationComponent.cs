namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SlidingDoorsAnimationComponent : MonoBehaviour, IDoorAnimationComponent {

		// Outlets
		[SerializeField] List<SlidingDoorAnimationComponent> _slidingDoorAnimationComponents = default;

		public IEnumerator OpenAnimation() {
			if (_slidingDoorAnimationComponents.Count > 0) {
				for (int i = 0; i < _slidingDoorAnimationComponents.Count - 1; i++) {
					StartCoroutine(_slidingDoorAnimationComponents[i].OpenAnimation());
				}
				yield return _slidingDoorAnimationComponents[^1].OpenAnimation();
			}

			yield return null;
		}

		public IEnumerator CloseAnimation() {
			if (_slidingDoorAnimationComponents.Count > 0) {
				for (int i = 0; i < _slidingDoorAnimationComponents.Count - 1; i++) {
					StartCoroutine(_slidingDoorAnimationComponents[i].CloseAnimation());
				}
				yield return _slidingDoorAnimationComponents[^1].CloseAnimation();
			}

			yield return null;
		}
	}
}
