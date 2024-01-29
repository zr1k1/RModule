namespace RModule.Runtime.Arcade {

	using RModule.Runtime.LeanTween;
	using UnityEngine;

	public class BlockRotater : MonoBehaviour {
		// Outlets
		[SerializeField] Transform _rotateAroundTr = default;
		[SerializeField] float _360RotateDuration = default;
		[SerializeField] bool _clockwise = default;

		void OnEnable() {
			LeanTween.rotateAround(_rotateAroundTr.gameObject, Vector3.forward, 360 * (_clockwise ? -1 : 1), _360RotateDuration).setLoopClamp();
		}

		public void ChangeParentForRotate(GameObject go) {
			go.transform.SetParent(_rotateAroundTr);
		}
	}
}