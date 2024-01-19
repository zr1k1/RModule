using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelPauseComponent : MonoBehaviour, ILevelPauseHandler {

	// Outlets
	[SerializeField] UnityEvent DidResumePauseNotFromLevel = default;
	[Tooltip("Add GameObjects using LeanTwean actions")]
	[SerializeField] List<GameObject> _tweansForPause = default;

	// Privats
	bool _isPausedNotFromLevel;
	float _pausedNotFromLevelStartTime;
	float _pausedFromLevelStartTime;
	float _pauseSeconds;
	IEnumerator _waitForResumeCo;
	Dictionary<int, bool> _pauseTweensIds = new Dictionary<int, bool>();

	public void SetPauseTweensIds(Dictionary<int, bool> pauseTweensIds) {
		_pauseTweensIds = pauseTweensIds;
	}

	public void PauseForTime(float seconds) {
		if (_waitForResumeCo != null)
			return;

		StartCoroutine(PauseAndWaitForResumeCo(seconds));
	}

	public void PauseForTime(float seconds, bool pauseOnlySettedTweensIdsList = false) {
		if (_waitForResumeCo != null)
			return;

		StartCoroutine(PauseAndWaitForResumeCo(seconds, pauseOnlySettedTweensIdsList));
	}

	IEnumerator PauseAndWaitForResumeCo(float seconds, bool pauseOnlySettedTweensIdsList = false) {
		_pauseSeconds = seconds;
		_pausedNotFromLevelStartTime = Time.time;
		PauseTweans(pauseOnlySettedTweensIdsList);

		_isPausedNotFromLevel = true;
		if (seconds > 0) {
			_waitForResumeCo = WaitForResumeCo(seconds);
			yield return _waitForResumeCo;
		}
		_waitForResumeCo = null;
		yield return null;
	}

	IEnumerator WaitForResumeCo(float seconds) {
		yield return new WaitForSeconds(seconds);

		ResumeTweans();
		if (_isPausedNotFromLevel)
			DidResumePauseNotFromLevel?.Invoke();
		_isPausedNotFromLevel = false;
	}

	void ILevelPauseHandler.OnLevelPause() {
		_pausedFromLevelStartTime = Time.time;
		if (_isPausedNotFromLevel) {
			if (_waitForResumeCo != null)
				StopCoroutine(_waitForResumeCo);
		} 
		PauseTweans();
	}

	void ILevelPauseHandler.OnLevelResume() {
		if (_isPausedNotFromLevel) {
			var totalTime = _pauseSeconds - (_pausedFromLevelStartTime - _pausedNotFromLevelStartTime);
			_waitForResumeCo = WaitForResumeCo(totalTime);
			StartCoroutine(_waitForResumeCo);
			if (_isPausedNotFromLevel) {
				ResumePausedNotFromLevelTweans();
			}
		} else {
			ResumeTweans();
		}
	}

	void PauseTweans(bool pauseOnlySettedTweensIdsList = false) {
		foreach (var tweanGo in _tweansForPause) {
			if (tweanGo != null) {
				if (pauseOnlySettedTweensIdsList) {
					foreach (var idPausePair in _pauseTweensIds) {
						if (idPausePair.Value)
							LeanTween.pause(idPausePair.Key);
					}
				} else {
					LeanTween.pause(tweanGo);
				}
			}
		}
	}

	void ResumeTweans(bool resumeOnlySettedTweensIdsList = false) {
		_waitForResumeCo = null;
		foreach (var tweanGo in _tweansForPause)
			if (tweanGo != null) {
				if (resumeOnlySettedTweensIdsList) {
					foreach (var idPausePair in _pauseTweensIds) {
						if (idPausePair.Value)
							LeanTween.resume(idPausePair.Key);
					}
				} else {
					LeanTween.resume(tweanGo);
				}
			}
	}

	void ResumePausedNotFromLevelTweans() {
		foreach (var tweanGo in _tweansForPause)
			if (tweanGo != null) {
				foreach (var idPausePair in _pauseTweensIds) { 
					if(!idPausePair.Value)
						LeanTween.resume(idPausePair.Key);
				}
			}
	}
}
