namespace RModule.Runtime.Arcade {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using RModule.Runtime.Utils.Vectors;
	using RModule.Runtime.LeanTween;


	public class CannonBlock : BaseBlock {
		public bool IsReady => _isReady;

		[SerializeField] StopObstacleObjMover _stopObstacleObjMover = default;
		[SerializeField] float reloadTime = default;
		//[SerializeField] Degrees90DirectionsCalculator _degrees90DirectionsCalculator = default;
		[SerializeField] Vector2 _direction = default;

		bool _isReady = true;

		protected override void Start() {
			p_contactDetector.Setup(this);
		}

		public void Fire(FireUnit fireUnit) {
			if (_isReady) {
				_stopObstacleObjMover.SetObjToMoveTransfrm(fireUnit.transform);
				_stopObstacleObjMover.SetSize(fireUnit.GetSize());
				_stopObstacleObjMover.MoveTo(_direction);

				LeanTween.move(fireUnit.gameObject, transform.position + (Vector3)_direction * 10, 1f).setOnComplete(fireUnit.Die);
				//_stopObstacleObjMover.MoveDidEnd.AddListener(fireUnit.Die);
				fireUnit.Use(gameObject);
				StartCoroutine(Reload());
			}
		}

		IEnumerator Reload() {
			_isReady = false;
			yield return new WaitForSeconds(reloadTime);
			_isReady = true;
		}
	}
}
