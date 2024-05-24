using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ViewDirectionController : MonoBehaviour {

	public Transform target = default;
	public Transform faceDirectionPoint = default;

	// Privats
	public Vector2 faceDirection = Vector2.zero;
	public Vector2 direction = Vector2.zero;
	public RotateAnimation rotateAnimation = default;

	Action _directionChangedCallback = null;

	Vector2 _lastDirection = Vector2.zero;
	float _prevAngle;

	void Start() {
		_lastDirection = faceDirection;
	}

	void Update() {
		if (_lastDirection != direction)
			UpdateView();
	}

	void UpdateView() {
		if(faceDirectionPoint != null)
			_lastDirection = (faceDirectionPoint.position - target.position).normalized;

		var angle = Vector2.SignedAngle(_lastDirection, direction);
		_lastDirection = direction;
		if (rotateAnimation != null) {
			target = rotateAnimation.rotateData.objToRotate.transform;
			angle = target.localEulerAngles.z + angle;
			rotateAnimation.rotateData.angle = angle;
			rotateAnimation.RotateAroundZ(_directionChangedCallback);
		} else {
			angle = target.localEulerAngles.z + angle;
			target.localEulerAngles = new Vector3(0, 0, angle);
		}
	}

	public void ChangeDirection(Vector2 newDirection, Action directionChangedCallback = null) {
		direction = newDirection;
		_directionChangedCallback = directionChangedCallback;
	}
}
