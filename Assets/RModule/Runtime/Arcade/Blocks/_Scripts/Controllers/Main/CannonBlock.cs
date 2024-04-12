namespace RModule.Runtime.Arcade {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using RModule.Runtime.Utils.Vectors;
	using RModule.Runtime.LeanTween;


	public class CannonBlock : BaseBlock {
		public bool IsReady => _isReady;

		[SerializeField] Transform _directionPoint = default;
		// TODO when add new few cannons can be refactored to make config for CannonBlocks
		[Header("Properties")]
		[SerializeField] float _reloadTime = default;
		[SerializeField] float _speed = default;

		Vector2 _direction = default;
		bool _isReady = true;

		protected override void Start() {
			p_contactDetector.Setup(this);
			_direction = (_directionPoint.position - transform.position).normalized;
		}

		public void Fire(FireUnit fireUnit) {
			if (_isReady) {
				fireUnit.GetComponent<ViewDirectionController>().ChangeDirection(_direction);
				LeanTween.move(fireUnit.gameObject, transform.position + (Vector3)_direction * fireUnit.Range, fireUnit.Range)
					.setSpeed(_speed)
					.setOnComplete(fireUnit.Die);
				fireUnit.Use(gameObject);
				StartCoroutine(Reload());
			}
		}

		IEnumerator Reload() {
			_isReady = false;
			yield return new WaitForSeconds(_reloadTime);
			_isReady = true;
		}
	}
}
