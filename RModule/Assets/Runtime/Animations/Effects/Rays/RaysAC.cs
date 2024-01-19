using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaysAC : BaseAC {
	// Outlets
	[SerializeField] RectTransform _rotateTransform = default;
	[SerializeField] RectTransform _alphaTransform = default;
	[Header("Properties")]
	[Range(1, 100)][SerializeField] float _rotateSpeed = default;
	[SerializeField] bool clockwise = default;
	[Range(1, 400)] [SerializeField] float _changeAlphaSpeed = default;
	[SerializeField] bool _playOnAwake = default;

	// Private vars
	float _aplhaTime;
	bool _switchAlpha;

	void Start() {
		_aplhaTime = 100f / _changeAlphaSpeed;
		if (_playOnAwake)
			Play();
	}

	protected override IEnumerator Animate() {
		Debug.Log("Animate rays");

		//LeanTween.rotateAroundLocal(_rotateTransform, Vector3.forward, clockwise ? -360 : 360, 100f / _rotateSpeed).setLoopClamp();
		LeanTween.rotateZ(_rotateTransform.gameObject, (clockwise ? 1 : -1) * 180, 100f / _rotateSpeed).setLoopClamp();

		StartCoroutine(ChangeAlpha());

		yield return null;
	}

	IEnumerator ChangeAlpha() {
		_switchAlpha = !_switchAlpha;
		float alpha = _switchAlpha ? Random.Range(0.5f, 1f) : Random.Range(0.1f, 0.5f);

		yield return LeanTween.alpha(_alphaTransform, alpha, _aplhaTime);
		yield return new WaitForSeconds(_aplhaTime);
		StartCoroutine(ChangeAlpha());
	}
}
