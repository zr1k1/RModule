namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using RModule.Runtime.Utils.Vectors;
	using RModule.Runtime.Utils;
	using RModule.Runtime.LeanTween;

	// TODO Refactor duplicates
	public class StopObstacleObjMover : MonoBehaviour, ICustomRoundEnabler, ISizeSetter {

		public UnityEvent<Vector2> MoveDidBegan = default;
		public UnityEvent<StopObstacleObjMover> MoveDidChange = default;
		public UnityEvent MoveDidEnd = default;

		//Accessors
		public bool MoveInProgress => _moveInProgress;
		public Vector2 Direction => _direction;
		public Vector2 EndPoint => _endPoint;
		public List<Vector3> WayPoints => _wayPoints;
		public Vector2 CurrentWayPoint => _currentWayPoint;
		public Transform ObjTransform => _objTransform;

		// Outlets
		[SerializeField] Transform _objTransform = default;
		[SerializeField] Vector2 _objSize = default;
		[SerializeField] Degrees90DirectionsCalculator _degrees90DirectionsCalculator = default;
		[SerializeField] LayerMask _obstacleLayerMask = default;
		[SerializeField] float _movementSpeed = default;
		[SerializeField] bool _roundPositionTo0Point5 = true;
		[Header("Debug")]
		[SerializeField] bool _createMoveEndPoint = default;
		[SerializeField] GameObject _endMovePoint = default;

		// Privats
		Vector2 _direction = Vector2.right;
		Vector2 _endPoint;
		Vector2 _prevMovePoint;
		float _correctVectorLenght = 0f;
		bool _moveInProgress;
		List<Vector3> _wayPoints = new List<Vector3>();
		int _currentWayPointIndex;
		Vector3 _currentWayPoint;
		LTBezierPath _path;
		float _speedModifier = 1f;
		MoveData _currentMoveData;
		List<MoveData> _queueOfMoves = new List<MoveData>();

		// Classes
		public class MoveData {
			public LTDescr LTDescr;
			public Vector2 Direction;
		}

		public void SetObjToMoveTransfrm(Transform objTransform) {
			_objTransform = objTransform;
		}

		public void IncreaseSpeedModifier(float amount) {
			_speedModifier += amount;
		}

		public void MoveTo(Degrees90DirectionsCalculator.Direction direction, float correctVectorLenght = 0f) {
			_correctVectorLenght = correctVectorLenght;
			_direction = ConvertEnumDirectionToVector(direction);
			CalculateEndPointAndMove();
		}

		public void MoveTo(Vector2 direction, float correctVectorLenght = 0f) {
			OnEndPointDestination();
			_correctVectorLenght = correctVectorLenght;
			_direction = direction;
			CalculateEndPointAndMove();
		}

		public bool TryChangeEndPointAndMove(Vector2 endPoint, float correctVectorLenght = 0f) {
			_correctVectorLenght = correctVectorLenght;
			_endPoint = endPoint;
			_endPoint -= _direction * _correctVectorLenght;

			return TryMove();
		}

		public void MoveToPoint(Vector2 endPoint) {
			if (!_moveInProgress)
				return;

			OnEndPointDestination();

			_moveInProgress = true;
			var descr = LeanTween.move(_objTransform.gameObject, endPoint, 1f)
				.setOnComplete(OnEndPointDestination).setOnUpdateRatio(OnUpdateRation).setSpeed(GetSpeed());
			_currentMoveData = new MoveData {
				Direction = _direction,
				LTDescr = descr
			};
			_queueOfMoves.Add(_currentMoveData);
		}

		public void MoveToCurrentWayPointAndStop() {
			MoveToPoint(_currentWayPoint);
		}

		public void StopMove() {
			OnEndPointDestination();
		}

		public void CreateMoveAndAddQueueOfMoves(Vector3 startPoint, Vector3 direction) {
			if (tryCreateMoveLTDescr(startPoint, direction, out var moveDescr)) {
				moveDescr.pause();
				_queueOfMoves.Add(new MoveData {
					Direction = direction,
					LTDescr = moveDescr
				});
			}
		}

		bool tryCreateMoveLTDescr(Vector3 startPoint, Vector3 direction, out LTDescr moveDescr) {
			moveDescr = null;
			if (_roundPositionTo0Point5)
				startPoint = new Vector2(Mathf.FloorToInt(startPoint.x) + 0.5f, Mathf.FloorToInt(startPoint.y) + 0.5f);
			else {
				startPoint = new Vector2(Mathf.RoundToInt(startPoint.x), Mathf.RoundToInt(startPoint.y));
			}
			Vector3 endPoint = startPoint;
			List<Vector3> wayPoints = new List<Vector3>();
			var hit = Physics2D.Raycast(startPoint, direction, 100f, _obstacleLayerMask);
			if (hit.collider != null) {
				var reflectVector = Vector2.Reflect(direction, direction);
				endPoint = hit.point + new Vector2(reflectVector.x * _objSize.x, reflectVector.y * _objSize.y) * 0.5f;
			}
			int lenght = Mathf.RoundToInt(Vector3.Distance(startPoint, endPoint));

			if (lenght == 0) {
				return false;
			}

			wayPoints.Add(startPoint);

			for (int i = 0; i < lenght; i++) {
				wayPoints.Add((Vector3)startPoint + (Vector3)direction * (i + 1));
			}

			var points = Utils.GeneratePathToLeanTwean(wayPoints);
			var path = new LTBezierPath(points);

			moveDescr = LeanTween.move(_objTransform.gameObject, path, Vector2.Distance(startPoint, endPoint) / _movementSpeed)
			   .setOnComplete(OnEndPointDestination).setOnUpdateRatio(OnUpdateRation).setSpeed(GetSpeed()).pause();

			return true;
		}

		public void UpdateMove() {
			var tempWayPoints = new List<Vector3>(_wayPoints);
			_wayPoints.Clear();
			for (int i = 0; i < tempWayPoints.Count; i++) {
				if (i >= _currentWayPointIndex)
					_wayPoints.Add(tempWayPoints[i]);
			}
			TryMove();
		}

		float GetSpeed() {
			return _movementSpeed * _speedModifier;
		}

		void CalculateEndPointAndMove() {
			CalculateEndPoint(_objTransform.position, _direction);
			TryMove();
		}

		Vector2 ConvertEnumDirectionToVector(Degrees90DirectionsCalculator.Direction direction) {
			return _degrees90DirectionsCalculator.DirectionVectors[direction];
		}

		void CalculateEndPoint(Vector2 startPoint, Vector2 direction) {
			_endPoint = transform.position;
			var hit = Physics2D.Raycast(startPoint, direction, 100f, _obstacleLayerMask);
			if (hit.collider != null) {
				var reflectVector = Vector2.Reflect(direction, direction);
				_endPoint = hit.point + new Vector2(reflectVector.x * _objSize.x, reflectVector.y * _objSize.y) * 0.5f;
			}
			_endPoint -= direction * _correctVectorLenght;
			Debug.Log($"startPoint {startPoint} direction {direction} hit.point {hit.point} _endPoint {_endPoint}");

			if (_createMoveEndPoint) {
				_endMovePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				_endMovePoint.transform.position = _endPoint;
				_endMovePoint = hit.collider.gameObject;
			}
		}

		bool TryMove() {
			int lenght = Mathf.RoundToInt(Vector3.Distance(_objTransform.position, _endPoint));
			//Debug.Log($"PushBox : lenght {gameObject.name}{lenght}");
			if (lenght == 0) {
				return false;
			}

			_moveInProgress = true;
			_wayPoints.Clear();
			_wayPoints.Add(transform.position);
			Vector2 objStartPos = gameObject.transform.position;
			if (_roundPositionTo0Point5) {
				objStartPos = new Vector2(Mathf.FloorToInt(objStartPos.x) + 0.5f, Mathf.FloorToInt(objStartPos.y) + 0.5f);
			} else {
				objStartPos = new Vector2(Mathf.RoundToInt(objStartPos.x), Mathf.RoundToInt(objStartPos.y));
			}

			for (int i = 0; i < lenght; i++) {
				_wayPoints.Add((Vector3)objStartPos + (Vector3)_direction * (i + 1));
			}
			_currentWayPointIndex = 1;
			_currentWayPoint = _wayPoints[1];
			var points = Utils.GeneratePathToLeanTwean(_wayPoints);
			_path = new LTBezierPath(points);
			var descr = LeanTween.move(_objTransform.gameObject, _path, Vector2.Distance(_objTransform.position, _endPoint) / _movementSpeed)
			   .setOnComplete(OnEndPointDestination).setOnUpdateRatio(OnUpdateRation).setSpeed(GetSpeed());

			_currentMoveData = new MoveData {
				LTDescr = descr,
				Direction = _direction
			};

			MoveDidBegan?.Invoke(_direction);
			return true;
		}

		void OnUpdateRation(float val, float ratioPassed) {
			float ratioPerPoint = 1f / ((float)_wayPoints.Count - 1);
			_currentWayPointIndex = Mathf.FloorToInt(ratioPassed / ratioPerPoint);
			_currentWayPoint = _wayPoints[_currentWayPointIndex];
			//Debug.Log($"OnUpdateCurrentWayPoint {ratioPassed} {ratioPerPoint} {ratioPassed / ratioPerPoint}");
		}

		public void OnEndPointDestination() {
			if (_moveInProgress) {
				if (_queueOfMoves.Contains(_currentMoveData)) {
					_queueOfMoves.Remove(_currentMoveData);
					if (_queueOfMoves.Count > 0) {
						_currentWayPointIndex = 1;
						_currentWayPoint = _wayPoints[1];
						_direction = _queueOfMoves[0].Direction;
						_queueOfMoves[0].LTDescr.resume();
						MoveDidChange?.Invoke(this);
						return;
					}
				}

				MoveEndActions();
			}
		}

		void MoveEndActions() {
			_queueOfMoves.Clear();
			_speedModifier = 1f;
			_moveInProgress = false;
			LeanTween.cancel(_objTransform.gameObject);
			MoveDidEnd?.Invoke();
		}

		public void SetEnableRoundPositionTo0Point5(bool enable) {
			_roundPositionTo0Point5 = enable;
		}

		public void SetSize(Vector3 size) {
			_objSize = size;
		}
	}
}