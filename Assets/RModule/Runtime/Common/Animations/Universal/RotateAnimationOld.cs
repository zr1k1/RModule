using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.LeanTween;

public class RotateAnimationOld : MonoBehaviour {
	// Outlets
	[SerializeField] GameObject _objToRotate = default;
	[SerializeField] float _rotateDuration = default;
	[Tooltip("Vector3.forward = z axis")]
	[SerializeField] Vector3 _axis = Vector3.forward;
	[SerializeField] bool _clockwise = default;

	void Start() {
		LeanTween.rotateAround(_objToRotate, _axis, _clockwise ? -360 : 360, _rotateDuration).setLoopClamp();
	}
}
