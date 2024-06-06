using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour {
	// Accessors
	public int TotalTime => _totalTime;
	public bool IsStarted => _isStarted;

	// Private vars
	Action<int> _tickCallback;
	Action _endCallback;
	int _totalTime;

	// Settings vars
	bool _isLooping;
	bool _isStarted;
	DateTime _pauseDateTime;
	bool _destroyHimselfOnEnd;

	public static Timer Create(int secondsTime, Action endCallback = null) {
		GameObject timerGO = new GameObject("timer", typeof(Timer));
		Timer timer = timerGO.GetComponent<Timer>();
		timer.Setup(secondsTime, endCallback);

		return timer;
	}

	public void Setup(int secondsTime, Action endCallback) {
		_totalTime = secondsTime;
		_endCallback = endCallback;
	}

	// Actions
	public void StartTimer() {
		_isStarted = true;
		StartCoroutine(Wait());
	}

	public void StopTimer() {
		_isStarted = false;
		StopAllCoroutines();
	}

	public void ResetTimer() {
		StopTimer();
		if (_isLooping)
			StartTimer();
	}

	public void SetTickCallback(Action<int> tickCallback) {
		_tickCallback = tickCallback;
	}

	public Timer SetDestroyHimselfOnEnd() {
		_destroyHimselfOnEnd = true;

		return this;
	}

	public void OnApplicationPause(bool pause) {
		Debug.Log($"Timer : OnApplicationPause {pause}");
		if (_isStarted) {
			if (pause) {
				_pauseDateTime = DateTime.Now;
			} else {
				_totalTime = Mathf.Clamp(_totalTime - (int)(DateTime.Now - _pauseDateTime).TotalSeconds, 0, 9999999);
				Debug.Log($"Timer : _totalTime {_totalTime}");
			}
		}
	}

	public void Destroy() {
		Destroy(gameObject);
	}

	IEnumerator Wait() {
		while(_totalTime > 0) { 
			yield return new WaitForSeconds(1);
			_totalTime -= 1;
			_tickCallback?.Invoke(_totalTime);
		}
		End();
	}

	void End() {
		_endCallback?.Invoke();
		if (_destroyHimselfOnEnd)
			Destroy();
	}

	// Settings
	public void SetLoop(bool isLooping) {
		_isLooping = isLooping;
	}
}
