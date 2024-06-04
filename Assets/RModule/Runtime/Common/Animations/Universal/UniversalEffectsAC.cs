using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RModule.Runtime.LeanTween;

public class UniversalEffectsAC : BaseAC {	
	// Enums
	public enum ShowUpType { Scale0To1, Scale1To0, UIFromLeftScreenBound, Rotate, HorizontalScale0To1 }

	// Outlets
	[SerializeField] List<PropertiesOfObjectToShowUp> _propertiesOfObjectToShowUp = default;

	// Privats
	List<int> _skipStepsIndexes = new List<int>();

	// Classes
	[Serializable]
	public class PropertiesOfObjectToShowUp {
		[SerializeField] internal List<GameObject> gos = default;
		[SerializeField] internal bool isActiveAtStart = default;
		[SerializeField] internal List<Animator> animatorsToDeactive = default;
		[SerializeField] internal ShowUpType showUpType = default;
		[SerializeField] internal float speed = 1f;
		[SerializeField] internal bool skipWaitDuration = default;
		[SerializeField] internal float duration = default;
		[SerializeField] internal LeanTweenType leanTweenType = default;
		[SerializeField] internal UnityEvent action = default;
		[SerializeField] internal float waitAfterEnd = default;
		[SerializeField] internal UnityEvent didEndAction = default;

		internal void SetActiveAnimators(bool isActive) {
			foreach (var animator in animatorsToDeactive)
				if(animator)
					animator.enabled = isActive;
		}

		internal float GetWaitDurationAfterAnimationStart() {
			return skipWaitDuration ? 0 : duration;
		}
	}

	public override BaseAC SetupAnimation(Action endCallback = null) {
		base.SetupAnimation(endCallback);
		for (int i = 0; i < _propertiesOfObjectToShowUp.Count; i++) {
			if (_skipStepsIndexes.Contains(i))
				continue;

			foreach (var go in _propertiesOfObjectToShowUp[i].gos)
				go.SetActive(_propertiesOfObjectToShowUp[i].isActiveAtStart);
		}

		return this;
	}

	protected override IEnumerator Animate() {
		for (int i = 0; i < _propertiesOfObjectToShowUp.Count; i++) {
			if (_skipStepsIndexes.Contains(i))
				continue;

			_propertiesOfObjectToShowUp[i].SetActiveAnimators(false);

			if (_propertiesOfObjectToShowUp[i].showUpType == ShowUpType.Scale0To1) {
				foreach (var go in _propertiesOfObjectToShowUp[i].gos) {
					if (go != null) {
						go.SetActive(true);
						go.transform.localScale = Vector2.zero;
						LeanTween.scale(go, Vector2.one, _propertiesOfObjectToShowUp[i].duration).setEase(_propertiesOfObjectToShowUp[i].leanTweenType);
					}
				}
			} else if (_propertiesOfObjectToShowUp[i].showUpType == ShowUpType.Scale1To0) {
				foreach (var go in _propertiesOfObjectToShowUp[i].gos) {
					if (go != null) {
						go.SetActive(true);
						go.transform.localScale = Vector2.one;
						LeanTween.scale(go, Vector2.zero, _propertiesOfObjectToShowUp[i].duration).setEase(_propertiesOfObjectToShowUp[i].leanTweenType);
					}
				}
			} else if (_propertiesOfObjectToShowUp[i].showUpType == ShowUpType.UIFromLeftScreenBound) {
				foreach (var go in _propertiesOfObjectToShowUp[i].gos) {
					if (go != null) {
						go.SetActive(true);
						var rt = go.GetComponent<RectTransform>();
						if (rt != null) {
							rt.pivot = new Vector2(1, 0.5f);
							rt.anchoredPosition = Vector2.zero;
							var endPos = new Vector2(rt.sizeDelta.x, 0);
							LeanTween.move(rt, endPos, _propertiesOfObjectToShowUp[i].duration).setEase(_propertiesOfObjectToShowUp[i].leanTweenType);
						}
					}
				}
			} else if (_propertiesOfObjectToShowUp[i].showUpType == ShowUpType.Rotate) {
				foreach (var go in _propertiesOfObjectToShowUp[i].gos) {
					if (go != null) {
						go.SetActive(true);
						LeanTween.rotateAroundLocal(go, Vector3.forward, _propertiesOfObjectToShowUp[i].speed, _propertiesOfObjectToShowUp[i].duration).setEase(_propertiesOfObjectToShowUp[i].leanTweenType);
					}
				}
			} else if (_propertiesOfObjectToShowUp[i].showUpType == ShowUpType.HorizontalScale0To1) {
				foreach (var go in _propertiesOfObjectToShowUp[i].gos) {
					if (go != null) {
						go.SetActive(true);
						go.transform.localScale = new Vector3(0, 1f, 1f);
						LeanTween.scale(go, Vector2.one, _propertiesOfObjectToShowUp[i].duration).setEase(_propertiesOfObjectToShowUp[i].leanTweenType);
					}
				}
			}

			bool skipIfAllGOsIsNull = true;
			foreach (var go in _propertiesOfObjectToShowUp[i].gos) {
				if (go != null) {
					skipIfAllGOsIsNull = false;
					break;
				}
			}

			_propertiesOfObjectToShowUp[i].action?.Invoke();
			if (!skipIfAllGOsIsNull)
				yield return new WaitForSeconds(_propertiesOfObjectToShowUp[i].GetWaitDurationAfterAnimationStart());
			_propertiesOfObjectToShowUp[i].didEndAction?.Invoke();
			yield return new WaitForSeconds(_propertiesOfObjectToShowUp[i].waitAfterEnd);

			_propertiesOfObjectToShowUp[i].SetActiveAnimators(true);
		}

		yield return base.Animate();
	}

	// Actions
	public void RemovePropertieOfObjectToShowUp(int indexInListOfProperties) {
		if(indexInListOfProperties < _propertiesOfObjectToShowUp.Count) {
			_propertiesOfObjectToShowUp.RemoveAt(indexInListOfProperties);
		}
	}

	public void SetSkipAnimationStepsIndexes(List<int> skipIndexes) {
		_skipStepsIndexes = new List<int>(skipIndexes);
	}

	public void SetWaitAfterAndToAnimationStepByIndex(int stepIndex, float waitAfterEndDuration) {
		if (stepIndex >= _propertiesOfObjectToShowUp.Count)
			return;

		_propertiesOfObjectToShowUp[stepIndex].waitAfterEnd = waitAfterEndDuration;
	}
}
