namespace RModule.Runtime.Arcade {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using RModule.Runtime.LeanTween;
	using RModule.Runtime.Utils;

	public class SpinningBladeBlock : BaseBlock, IDamagable {

		// Outlets
		[SerializeField] protected Transform _container = default;
		[SerializeField] protected Transform _pointsToMoveParent = default;
		[SerializeField] protected float _moveDuration = default;
		[SerializeField] protected GameObject _spriteGo = default;
		[SerializeField] protected Rigidbody2D _rigidbody2D = default;
		[SerializeField] protected Collider2D _collider2D = default;
		[SerializeField] protected float _rotateDuration = default;
		[SerializeField] protected LeanTweenType _moveLoopLeanTweenType = default;
		[SerializeField] protected bool _setOrientToPath = default;
		[SerializeField] protected bool _notMove = default;

		// Privats
		protected List<Vector3> _pointsToMove = new List<Vector3>();
		protected LTDescr _moveTween;
		protected LTDescr _rotationTween;

		// Interfaces
		public interface ISpinningBladeBlockDestroyer { }

		protected void Start() {
			foreach (Transform pointTr in _pointsToMoveParent)
				_pointsToMove.Add(pointTr.position);

			var path = Utils.GeneratePathToLeanTwean(_pointsToMove);
			if (!_notMove)
				_moveTween = LeanTween.move(_container.gameObject, path, _moveDuration).setOrientToPath2d(_setOrientToPath).setLoopType(_moveLoopLeanTweenType);
			_rotationTween = LeanTween.rotateAround(_spriteGo, Vector3.forward, 360, _rotateDuration).setLoopClamp();
		}

		public override void Die() {
			base.Die();
			if (!_notMove)
				_moveTween.pause();
			_rotationTween.pause();
			_rigidbody2D.constraints = RigidbodyConstraints2D.None;
			_collider2D.enabled = false;
			Destroy(gameObject, 5);
		}

		public void Pause(float seconds) {
			Dictionary<int, bool> pauseTweensIds = new Dictionary<int, bool> {
				[_moveTween.id] = true,
				[_rotationTween.id] = false
			};
			p_levelPauseComponent.SetPauseTweensIds(pauseTweensIds);
			p_levelPauseComponent.PauseForTime(seconds, true);
		}

		public virtual bool TryTakeDmg(DamageData damageData) {
			if (damageData.damageSourceGameObject.GetComponent<ISpinningBladeBlockDestroyer>() != null) {
				Die();
				return true;
			}

			return false;
		}
	}
}