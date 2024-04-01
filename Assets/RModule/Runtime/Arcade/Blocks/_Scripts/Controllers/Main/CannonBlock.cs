namespace RModule.Runtime.Arcade {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using RModule.Runtime.Utils.Vectors;
	using RModule.Runtime.LeanTween;


	public class CannonBlock : BaseBlock {
		public bool IsReady => _isReady;

		[SerializeField] Transform _parent = default;
		[SerializeField] StopObstacleObjMover _stopObstacleObjMover = default;
		[SerializeField] float reloadTime = default;
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

				Quaternion rotation = Quaternion.AngleAxis(_parent.localEulerAngles.z, Vector3.forward);
				float angle = transform.localEulerAngles.z;
				var finallyDirection = rotation * _direction;
				fireUnit.transform.localEulerAngles = _parent.localEulerAngles;
				fireUnit.GetComponent<ViewDirectionController>().ChangeDirection(finallyDirection);
				LeanTween.move(fireUnit.gameObject, transform.position + finallyDirection * 10, 1f).setOnComplete(fireUnit.Die);
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
