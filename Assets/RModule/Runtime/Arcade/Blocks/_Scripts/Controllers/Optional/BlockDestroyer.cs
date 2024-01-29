using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockDestroyer : MonoBehaviour {
	// Outlets
	[SerializeField] SpriteRenderer _spriteRenderer = default;
	[SerializeField] GameObject _toDestroyGo = default;
	[SerializeField] float _statesCount = default;
	[SerializeField] float _stateDuration = default;

	// Privats
	bool _destroyInProgress;

	// IDestroyableBlock
	public void StartDestroyAnimation(Action finishCallback) {
		if (_destroyInProgress)
			return;

		_destroyInProgress = true;
		StartCoroutine(DelayAndDestroy(finishCallback));
	}

	IEnumerator DelayAndDestroy(Action finishCallback) {
		var alpha = _spriteRenderer.color.a;
		float alphaStep = alpha / _statesCount;
		for (int i = 0; i < _statesCount; i++) {
			_spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, alpha - alphaStep * i);
			yield return new WaitForSeconds(_stateDuration);
		}
		finishCallback?.Invoke();

		var pointEffector = GetComponent<PointEffector2D>();
		if (pointEffector != null) {
			pointEffector.forceMagnitude = 0f;
		}

		yield return new WaitForFixedUpdate();
		_toDestroyGo.SetActive(false);
		Destroy(_toDestroyGo, 1f);
	}
}
