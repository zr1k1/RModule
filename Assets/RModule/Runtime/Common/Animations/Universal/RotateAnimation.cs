using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.LeanTween;

public class RotateAnimation : MonoBehaviour {
	// Outlets
	public RotateData rotateData = default;

	void Start() {
		if (rotateData.playAtStart)
			Rotate();
	}

	public void Rotate() {
		LeanTween.rotateAround(rotateData.objToRotate, rotateData.axis, rotateData.clockwise ? -rotateData.angle : rotateData.angle, rotateData.rotateDuration)
			.setLoopType(rotateData.loopLeanTweenType);
	}

	public void RotateAroundZ() {
		LeanTween.rotateZ(rotateData.objToRotate, rotateData.angle, rotateData.rotateDuration);
	}
}
