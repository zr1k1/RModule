using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseAC : MonoBehaviour {
	// Events
	public UnityEvent DidPlayCallback = default;
	public UnityEvent WillEndAfterDelayCallback = default;
	public UnityEvent DidEndCallback = default;

	// Public Accessors
	public bool IsPlaying => _isPlaying;

	// Outlets
	[SerializeField] bool playAtStart = default;
	[SerializeField] float delayBeforeDestroy = 0;
	[SerializeField] float delayBeforeCallEndEvent = 0;
	[SerializeField] bool notDestroyOnEnd = default;

	// Protected vars
	protected bool _isPlaying;

	// ---------------------------------------------------------------
	// Setup

	void Start() {
		if (playAtStart)
			Play();
	}

	public virtual BaseAC SetupAnimation(Action endCallback = null) {
		if (gameObject != null)
			gameObject.SetActive(false);
		if (endCallback != null)
			DidEndCallback.AddListener(endCallback.Invoke);

		return this;
	}

	public void RemoveDidEndListeners() {
		DidEndCallback?.RemoveAllListeners();
	}

	public void AddDidEndCallback(Action endCallback = null) {
		if (endCallback != null)
			DidEndCallback.AddListener(endCallback.Invoke);
	}

	public void Play() {
		DidPlayCallback?.Invoke();
		_isPlaying = true;
		if (gameObject != null)
			gameObject.SetActive(true);
		StartCoroutine(Animate());
	}

	protected virtual IEnumerator Animate() {
		delayBeforeDestroy = Mathf.Clamp(delayBeforeDestroy, delayBeforeCallEndEvent, 999);
		yield return new WaitForSeconds(delayBeforeCallEndEvent);
		DidEndCallback?.Invoke();
		_isPlaying = false;

		yield return new WaitForSeconds(delayBeforeDestroy - delayBeforeCallEndEvent);
		if (!notDestroyOnEnd && gameObject != null)
			Destroy(gameObject);
	}

	public virtual float GetTotalDuration() {
		return 0;
	}
}
