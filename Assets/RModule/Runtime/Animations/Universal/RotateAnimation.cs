using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.LeanTween;

public class RotateAnimation : MonoBehaviour {
	// Outlets
	[SerializeField] GameObject _objToRotate = default;
	[SerializeField] float _rotateDuration = default;
	[Tooltip("Vector3.forward = z axis")]
	[SerializeField] Vector3 _axis = Vector3.forward;

	 void Start() {
		LeanTween.rotateAround(_objToRotate, _axis, 360, _rotateDuration).setLoopClamp();
	}
}
