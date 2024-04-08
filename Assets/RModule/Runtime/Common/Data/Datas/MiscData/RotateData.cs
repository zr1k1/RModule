using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RModule.Runtime.LeanTween;

[Serializable]
public class RotateData {
	public GameObject objToRotate = default;
	public float rotateDuration = default;
	[Tooltip("Vector3.forward = z axis")]
	public Vector3 axis = Vector3.forward;
	public bool clockwise = default;
	public bool playAtStart = true;
	public float angle;
	public LeanTweenType loopLeanTweenType = LeanTweenType.once;
}
