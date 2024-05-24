using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RModule.Runtime.LeanTween;

public class RotateAnimation : MonoBehaviour {
	// Outlets
	public RotateData rotateData = default;

	LTDescr _descr;

	void Start() {
		if (rotateData.playAtStart)
			Rotate();
	}

	public void Rotate() {
		_descr = LeanTween.rotateAround(rotateData.objToRotate, rotateData.axis, rotateData.clockwise ? -rotateData.angle : rotateData.angle, rotateData.rotateDuration)
			.setLoopType(rotateData.loopLeanTweenType);
	}

	public void RotateAroundZ(Action directionChangedCallback = null) {
		_descr = LeanTween.rotateZ(rotateData.objToRotate, rotateData.angle, rotateData.rotateDuration);
		if(directionChangedCallback != null) {
			_descr.setOnComplete(directionChangedCallback);
		}
	}
}
