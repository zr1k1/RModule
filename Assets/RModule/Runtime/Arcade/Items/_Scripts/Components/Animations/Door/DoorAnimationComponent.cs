namespace RModule.Runtime.Arcade {

	using System.Collections;
	using RModule.Runtime.LeanTween;
	using UnityEngine;

	public class DoorAnimationComponent : ItemAnimationComponent, IDoorAnimationComponent {
		// Outlets
		[SerializeField] protected float p_alphaDuration = default;

		public virtual IEnumerator OpenAnimation() {
			LeanTween.alpha(gameObject, 0f, p_alphaDuration);
			yield return new WaitForSeconds(p_alphaDuration);
		}

		public virtual IEnumerator CloseAnimation() {
			LeanTween.alpha(gameObject, 1f, p_alphaDuration);
			yield return new WaitForSeconds(p_alphaDuration);
		}
	}
}