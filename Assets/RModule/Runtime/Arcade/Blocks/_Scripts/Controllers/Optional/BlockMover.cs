namespace RModule.Runtime.Arcade {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;
	using UnityEngine.Events;
	using RModule.Runtime.LeanTween;

	public class BlockMover : MonoBehaviour, IItemContactHandler, IInfluenceMovingObjects, IMoveable {
		// Enums
		public enum PointsType { Vectors, Transforms }

		// Events
		public UnityEvent<BlockMover> MovementDidEnd = default;
		public UnityEvent<BlockMover> MovementToNextPointDidCantChangeNow = default;
		public UnityEvent<BlockMover> MovementToPrevPointDidCantChangeNow = default;

		// Accessors
		public List<Vector3> PointsToMove => _pointsToMove;
		public Transform TransformToMove => _transformToMove;
		public bool NextPointExist { private set; get; }
		public bool PrevPointExist { private set; get; }

		// Outlets
		[SerializeField] Transform _transformToMove = default;
		[SerializeField] PointsType pointsType = default;
		[SerializeField] List<Vector3> _pointsToMove = default;
		[SerializeField] List<Transform> _pointsToMoveTransforms = default;
		[SerializeField] float _moveDuration = default;
		[SerializeField] LeanTweenType _moveLoopLeanTweenType = default;
		[SerializeField] bool _setOrientToPath = default;
		[SerializeField] bool _moveAtStart = true;

		// Privats
		LTDescr _moveLTDescr;
		List<IMoveBlockContactHandler> _stoppers = new List<IMoveBlockContactHandler>();

		List<Vector3> _simplePath = new List<Vector3>();
		int _currentPointToMoveIndex = 0;
		bool _moveInProgress;

		// Interfaces
		public interface IMoveBlockContactHandler {
			void OnStartMoveBlockContact(BlockMover blockMover);
			void OnEndMoveBlockContact(BlockMover blockMover);
		}

		private void OnTriggerEnter2D(Collider2D collision) {
			var iMoveBlockContactHandler = collision.gameObject.GetComponent<IMoveBlockContactHandler>();
			if(iMoveBlockContactHandler != null) {
				iMoveBlockContactHandler.OnStartMoveBlockContact(this);
				AddStopperPauseMove(iMoveBlockContactHandler);
			}
		}

		private void OnTriggerExit2D(Collider2D collision) {
			var iMoveBlockContactHandler = collision.gameObject.GetComponent<IMoveBlockContactHandler>();
			if (iMoveBlockContactHandler != null) {
				iMoveBlockContactHandler.OnEndMoveBlockContact(this);
				RemoveStopperAndTryResumeMove(iMoveBlockContactHandler);
			}
		}

		private void OnCollisionEnter2D(Collision2D collision) {
			var iMoveBlockContactHandler = collision.gameObject.GetComponent<IMoveBlockContactHandler>();
			if (iMoveBlockContactHandler != null) {
				iMoveBlockContactHandler.OnStartMoveBlockContact(this);
				AddStopperPauseMove(iMoveBlockContactHandler);
			}
		}

		private void OnCollisionExit2D(Collision2D collision) {
			var iMoveBlockContactHandler = collision.gameObject.GetComponent<IMoveBlockContactHandler>();
			if (iMoveBlockContactHandler != null) {
				iMoveBlockContactHandler.OnEndMoveBlockContact(this);
				RemoveStopperAndTryResumeMove(iMoveBlockContactHandler);
			}
		}

		void Start() {
			if (_pointsToMove.Count < 2 && _pointsToMoveTransforms.Count < 2) {
				Debug.LogWarning($"For move block needed 4 or more points");
				return;
			}
			_simplePath.AddRange(pointsType == PointsType.Vectors ? _pointsToMove : _pointsToMoveTransforms.Select(tr => tr.position));

			List<Vector3> totalFoursPointsToMove = new List<Vector3>();
			if (pointsType == PointsType.Vectors) {
				for (int i = 0; i < _pointsToMove.Count - 1; i++) {
					Vector2 pointToMove = _pointsToMove[i];
					Vector2 nextPointToMove = _pointsToMove[i + 1];
					totalFoursPointsToMove.AddRange(createLeanTweenFourPointsToMoveForPathFromOnePointToMove(pointToMove, nextPointToMove));
				}
			} else if (pointsType == PointsType.Transforms) {
				for (int i = 0; i < _pointsToMoveTransforms.Count - 1; i++) {
					Vector2 pointToMove = _pointsToMoveTransforms[i].position;
					Vector2 nextPointToMove = _pointsToMoveTransforms[i + 1].position;
					totalFoursPointsToMove.AddRange(createLeanTweenFourPointsToMoveForPathFromOnePointToMove(pointToMove, nextPointToMove));
				}
			}
			_pointsToMove = totalFoursPointsToMove;

			var path = _pointsToMove.ToArray();
			if (_moveAtStart)
				_moveLTDescr = LeanTween.move(_transformToMove.gameObject, path, _moveDuration).setOrientToPath2d(_setOrientToPath).setLoopType(_moveLoopLeanTweenType);
		}

		List<Vector3> createLeanTweenFourPointsToMoveForPathFromOnePointToMove(Vector2 pointToMove, Vector2 nextPointToMove) {
			List<Vector3> foursPointsToMove = new List<Vector3>();
			Vector2 controlPoint1 = pointToMove;
			Vector2 controlPoint2 = nextPointToMove - controlPoint1;
			controlPoint2 = controlPoint1 + Vector2.Distance(nextPointToMove, controlPoint1) * controlPoint2.normalized / 2f;//middle between two points
			Vector2 endPoint1 = nextPointToMove;
			Vector2 endPoint2 = nextPointToMove;
			foursPointsToMove.Add(controlPoint1);
			foursPointsToMove.Add(controlPoint2);
			foursPointsToMove.Add(endPoint1);
			foursPointsToMove.Add(endPoint2);

			return foursPointsToMove;
		}

		void IItemContactHandler.OnStartContactWithItem(Item item) {
			var iMoveBlockContactHandler = item.GetComponent<IMoveBlockContactHandler>();
			if (iMoveBlockContactHandler != null) {
				AddStopperPauseMove(iMoveBlockContactHandler);
			}
		}

		void IItemContactHandler.OnEndContactWithItem(Item item) {
			var iMoveBlockContactHandler = item.GetComponent<IMoveBlockContactHandler>();
			if (iMoveBlockContactHandler != null) {
				RemoveStopperAndTryResumeMove(iMoveBlockContactHandler);
			}
		}

		void AddStopperPauseMove(IMoveBlockContactHandler moveBlockContactHandler) {
			_stoppers.Add(moveBlockContactHandler);
			_moveLTDescr.pause();
		}

		void RemoveStopperAndTryResumeMove(IMoveBlockContactHandler moveBlockContactHandler) {
			_stoppers.Remove(moveBlockContactHandler);
			if (_stoppers.Count == 0)
				_moveLTDescr.resume();
		}

		public void TryMoveNextPoint() {
			int nextPointIndex = _currentPointToMoveIndex + 1;
			if (!_moveInProgress && nextPointIndex < _simplePath.Count) {
				_moveLTDescr = LeanTween.move(_transformToMove.gameObject, _simplePath[nextPointIndex], _moveDuration).setOnComplete(OnMoveEnd);
				_moveInProgress = true;
				_currentPointToMoveIndex = nextPointIndex;
			} else {
				MovementToNextPointDidCantChangeNow?.Invoke(this);
			}
		}

		public void TryMovePrevPoint() {
			int prevPointIndex = _currentPointToMoveIndex - 1;
			if (!_moveInProgress && prevPointIndex >= 0) {
				_moveLTDescr = LeanTween.move(_transformToMove.gameObject, _simplePath[prevPointIndex], _moveDuration).setOnComplete(OnMoveEnd);
				_moveInProgress = true;
				_currentPointToMoveIndex = prevPointIndex;
			} else {
				MovementToPrevPointDidCantChangeNow?.Invoke(this);
			}
		}

		void OnMoveEnd() {
			_moveInProgress = false;
			NextPointExist = _currentPointToMoveIndex + 1 < _simplePath.Count;
			PrevPointExist = _currentPointToMoveIndex - 1 >= 0;

			MovementDidEnd?.Invoke(this);
		}

		public Vector2 GetMoveEndPoint() {
			return Vector2.zero;
		}

		public bool TryGetMoveEndPoint(out Vector3 moveEndPoint) {
			throw new System.NotImplementedException();
		}

		public void StopMove() {
			throw new System.NotImplementedException();
		}

		public void PauseMove() {
			_moveLTDescr.pause();
		}

		public void ResumeMove() {
			throw new System.NotImplementedException();
		}
	}
}