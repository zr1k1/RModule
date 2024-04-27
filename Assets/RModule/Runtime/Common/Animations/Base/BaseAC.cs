using System.Collections;
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

	public virtual BaseAC SetupAnimation(UnityAction endCallback = null) {
		gameObject.SetActive(false);
		if (endCallback != null)
			DidEndCallback.AddListener(endCallback);

		return this;
	}

	public void AddDidEndCallback(UnityAction endCallback) {
		if (endCallback != null)
			DidEndCallback.AddListener(endCallback);
	}

	public void Play() {
		DidPlayCallback?.Invoke();
		_isPlaying = true;
		gameObject.SetActive(true);
		StartCoroutine(Animate());
	}

	protected virtual IEnumerator Animate() {
		delayBeforeDestroy = Mathf.Clamp(delayBeforeDestroy, delayBeforeCallEndEvent + 0.5f, 999);
		yield return new WaitForSeconds(delayBeforeCallEndEvent);
		DidEndCallback?.Invoke();
		_isPlaying = false;

		yield return new WaitForSeconds(delayBeforeDestroy - delayBeforeCallEndEvent);
		if (!notDestroyOnEnd)
			Destroy(gameObject);
	}

	public virtual float GetTotalDuration() {
		return 0;
	}
}
