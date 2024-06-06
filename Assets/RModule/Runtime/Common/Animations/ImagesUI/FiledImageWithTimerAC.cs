using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RModule.Runtime.LeanTween;
using TMPro;

public class FiledImageWithTimerAC : BaseAC {
	// Outlets
	[SerializeField] Image _img = default;
	[SerializeField] TextMeshProUGUI _timerLabel = default;
	[SerializeField] float _fromFillAmount = 1f;
	[SerializeField] float _toFillAmount = 0f;

	// Private vars
	Timer _timer;

	public FiledImageWithTimerAC Setup(int timerSeconds, Action endCallback = null) {
		_timerLabel.text = timerSeconds.ToString();
		_timer = Timer.Create(timerSeconds);
		_timer.SetDestroyHimselfOnEnd();
		LeanTween.value(gameObject, (float)timerSeconds / (float)timerSeconds, (float)(timerSeconds - 1) / (float)timerSeconds, 1f).setOnUpdate(UpdateImageView);
		_timer.SetTickCallback((remainingTime) => {
			_timerLabel.text = remainingTime.ToString();
			LeanTween.value(gameObject, (float)remainingTime / (float)timerSeconds, (float)(remainingTime - 1) / (float)timerSeconds, 1f).setOnUpdate(UpdateImageView);
		});
		_timer.StartTimer();

		return (FiledImageWithTimerAC)base.SetupAnimation(() => {
			if (endCallback != null) {
				endCallback.Invoke();
			}
		});
	}

	void UpdateImageView(float value) {
		_img.fillAmount = value;
	}

	protected override IEnumerator Animate() {
		while (_timer.IsStarted)
			yield return null;

		yield return base.Animate();
	}

	public void Stop() {
		if (_timer != null) {
			StopAllCoroutines();
			LeanTween.cancel(gameObject);
			_timer.StopTimer();
			_timer.Destroy();
		}
	}
}
