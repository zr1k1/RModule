using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.LeanTween;
using RModule.Runtime.Utils;

public class MoveGoAnimation : MonoBehaviour {
	// Outlets
	[SerializeField] GameObject _obj = default;
	[SerializeField] float _duration = default;

	[Tooltip("Path need minimn 2 points to work")]
	[SerializeField] List<Vector3> _localPathPoints = new List<Vector3>(2);
	[SerializeField] LeanTweenType _moveLoopLeanTweenType = default;

	// Privats
	Vector3 _defaultPosition;
	LTDescr _tween;

	void Start() {

		if (_localPathPoints.Count < 2) {
			Debug.LogError("Need minimum 2 point to work move animation");
			return;
		}

		_defaultPosition = transform.position;
		var worldPathPoints = new List<Vector3>();
		foreach (var localPathPoint in _localPathPoints) {
			worldPathPoints.Add(_defaultPosition + localPathPoint);
		}
		_tween = LeanTween.move(_obj, Utils.GeneratePathToLeanTwean(worldPathPoints), _duration).setLoopType(_moveLoopLeanTweenType);
	}
}
