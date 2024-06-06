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
		_timer.SetTickCallback((remainingTime) => {
			_timerLabel.text = remainingTime.ToString();
		});
		_timer.StartTimer();

		return (FiledImageWithTimerAC) base.SetupAnimation(() => {
			if (endCallback != null) {
				endCallback.Invoke();
			}
		});
	}

	protected override IEnumerator Animate() {
		LeanTween.value(gameObject, _fromFillAmount, _toFillAmount, _timer.TotalTime).setOnUpdate(UpdateImageView);
		yield return new WaitForSeconds(_timer.TotalTime);

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

	public void UpdateImageView(float value) {
		_img.fillAmount = value;
	}
}
