using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.LeanTween;

public class ViewDirectionController : MonoBehaviour {
	public Transform target = default;

	// Privats
	public Vector2 faceDirection = Vector2.zero;
	public Vector2 direction = Vector2.zero;
	public RotateAnimation rotateAnimation = default;

	Vector2 _lastDirection = Vector2.zero;

	void Start() {
		_lastDirection = faceDirection;
	}

	void Update() {
		if (_lastDirection != direction)
			UpdateView();
	}

	void UpdateView() {
		var angle = Vector2.SignedAngle(_lastDirection, direction);
		_lastDirection = direction;
		if (rotateAnimation != null) {
			target = rotateAnimation.rotateData.objToRotate.transform;
			angle = target.localEulerAngles.z + angle;
			rotateAnimation.rotateData.angle = angle;
			rotateAnimation.RotateAroundZ();
		} else {
			angle = target.localEulerAngles.z + angle;
			target.localEulerAngles = new Vector3(0, 0, angle);
		}
	}

	public void ChangeDirection(Vector2 newDirection) {
		direction = newDirection;
	}
}
