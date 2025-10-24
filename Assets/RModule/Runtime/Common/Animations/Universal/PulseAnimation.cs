using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.LeanTween;

public class PulseAnimation : MonoBehaviour {
	// Outlets
	[SerializeField] float _pulseDuration = 1;
	[SerializeField] float _scaleTarget = 1.1f;
	[SerializeField] Animator _animator = default;
	[SerializeField] bool _disableAnimatorWhileIsPlaying = default;

	// Privats
	LTDescr _lTDescr;
	Vector2 _scale;
	float _totalTargetScale;

	private void Start() {
		_scale = transform.localScale;
		_totalTargetScale = _scale.x * _scaleTarget;
		if (_disableAnimatorWhileIsPlaying && _animator != null)
			_animator.enabled = false;

		var rt = GetComponent<RectTransform>();
		if (rt != null) {
			_lTDescr = LeanTween.scale(rt, new Vector3(_totalTargetScale, _totalTargetScale, _totalTargetScale), _pulseDuration).setLoopPingPong();
		} else {
			_lTDescr = LeanTween.scale(gameObject, new Vector3(_totalTargetScale, _totalTargetScale, _totalTargetScale), _pulseDuration).setLoopPingPong();
		}
	}

	public void Pause() {
		if (_disableAnimatorWhileIsPlaying && _animator != null)
			_animator.enabled = true;

		_lTDescr.pause();
	}

	public void Resume() {
		if (_disableAnimatorWhileIsPlaying && _animator != null)
			_animator.enabled = false;

		_lTDescr.resume();
	}
}
