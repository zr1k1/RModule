using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.LeanTween;
using RModule.Runtime.Utils;

public class MoveGoAnimation : MonoBehaviour {
	// Accessors
	public float Duration => _duration;
	public GameObject Obj => _obj;

	// Outlets
	[SerializeField] GameObject _obj = default;
	[SerializeField] float _duration = default;
	[SerializeField] List<Vector3> _localPathPoints = default;
	[SerializeField] LeanTweenType _moveLoopLeanTweenType = LeanTweenType.once;
	[SerializeField] bool _playAtStart = default;

	// Privats
	LTDescr _tween;
	List<Vector3> _pathPoints;

	void Start() {
		if (_playAtStart)
			Play();
	}

	public void Play() {
		if (TrySetupPath())
			Move();
	}

	public void Stop() {
		LeanTween.cancel(gameObject);
	}

	bool TrySetupPath() {
		_pathPoints = new List<Vector3>();

		if (_localPathPoints.Count == 1) {
			_pathPoints.Add(_obj.transform.localPosition);
		}

		foreach (var localPathPoint in _localPathPoints) {
			_pathPoints.Add(localPathPoint);
		}

		if (_pathPoints.Count < 2) {
			Debug.LogError("Need minimum 2 point in _localPathPoints to work move animation");
			return false;
		}

		return true;
	}

	void Move() {
		_tween = LeanTween.moveLocal(_obj, Utils.GeneratePathToLeanTwean(_pathPoints), _duration).setLoopType(_moveLoopLeanTweenType);
	}
}
