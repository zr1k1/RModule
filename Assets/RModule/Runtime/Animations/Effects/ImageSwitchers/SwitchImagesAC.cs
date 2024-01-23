using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using RModule.Runtime.LeanTween;

public class SwitchImagesAC : BaseAC {

	// Outlets
	[SerializeField] float _duration = default;
	[SerializeField] float _defaultSwitchTime = default;
	[SerializeField] Image _image = default;
	[SerializeField] AnimationCurve _animationCurve = default;

	// Privats
	public List<Sprite> _imagesToSwitch;
	float _switchTime;
	float _currentValue;
	int _lastIndex;
	IEnumerator _switchCo;

	public SwitchImagesAC Setup(Image image, List<Sprite> imagesToSwitch, Action finishCallback) {
		base.SetupAnimation(null);

		_image = image;
		_switchTime = _defaultSwitchTime;
		_imagesToSwitch = imagesToSwitch;
		_switchCo = Switch();

		DidEndCallback.AddListener(finishCallback.Invoke);
		return this;
	}

	protected override IEnumerator Animate() {
		if (_imagesToSwitch.Count > 1) {
			LeanTween.value(gameObject, (value) => {
				_currentValue = value;
			}, 1f, 0f, _duration).setEase(_animationCurve).setOnComplete(() => {
				StopCoroutine(_switchCo);
			});
			StartCoroutine(_switchCo);
			yield return new WaitForSeconds(_duration);
		} else if (_imagesToSwitch.Count == 0) {
			_image.sprite = _imagesToSwitch[0];
		}

		yield return base.Animate();
	}

	IEnumerator Switch() {
		var listImages = new List<Sprite>(_imagesToSwitch);
		listImages.RemoveAt(_lastIndex);
		var index = Random.Range(0, listImages.Count);
		_image.sprite = listImages[index];
		_lastIndex = _imagesToSwitch.IndexOf(_image.sprite);
		yield return new WaitForSeconds(_switchTime);
		_switchTime = _defaultSwitchTime * _currentValue;
		_switchCo = Switch();
		StartCoroutine(_switchCo);
	}
}
