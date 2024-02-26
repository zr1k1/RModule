using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewDirectionController : MonoBehaviour {
	public Transform target = default;

	// Privats
	public Vector2 faceDirection = Vector2.zero;
	public Vector2 direction = Vector2.zero;

	Vector2 _lastDirection = Vector2.zero;

	void Update() {
		if(_lastDirection != direction)
			UpdateView();
	}

	void UpdateView() {
		var angle = VectorsHelper.CalculateAngle(new Vector3(0,0,0), direction, faceDirection, false);
		target.localEulerAngles = new Vector3(0, 0, angle);
		_lastDirection = direction;
	}

	public void ChangeDirection(Vector2 newDirection) {
		direction = newDirection;
	}
}
