using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SpriteSetterByDistanceBetweenObjects : MonoBehaviour {
	// Events
	public UnityEvent<int> DidChangeStateIndex = default;

	// Parameters
	public float maxDistance = default;

	// Outlets
	[SerializeField] Transform _transform1 = default;
	[SerializeField] Transform _transform2 = default;
	[SerializeField] PercentOfMaxDistanceSprite _percentOfMaxDistanceSprite = default;

	// Privats
	float _distance;
	int _totalIndexToApplySprites;
	int _prevIndex;
	int _currentIndex;

	// Classes
	[Serializable] public class PercentOfMaxDistanceSprite : SerializableDictionary<float, ToApplySpriteParametersList> { }

	[Serializable]
	public class ToApplySpriteParametersList {
		public List<ToApplySpriteParameters> toApplySpriteParametersList = default; 
	}

	[Serializable]
	public class ToApplySpriteParameters {
		public Sprite sprite = default;
		public List<SpriteRenderer> spriteRenderers = default;

		public void ApplySprite() {
			foreach(var spriteRenderer in spriteRenderers)
				spriteRenderer.sprite = sprite;
		}
	}

	public void SetSprite(float percentKey, int indexOfToApplySpriteParametersList, Sprite sprite) {
		if (_percentOfMaxDistanceSprite.ContainsKey(percentKey)) {
			if(indexOfToApplySpriteParametersList < _percentOfMaxDistanceSprite[percentKey].toApplySpriteParametersList.Count) {
				_percentOfMaxDistanceSprite[percentKey].toApplySpriteParametersList[indexOfToApplySpriteParametersList].sprite = sprite;
			} else {
				Debug.LogError($"indexOfToApplySpriteParametersList {indexOfToApplySpriteParametersList} is not exist in toApplySpriteParametersList");
			}

		} else {
			Debug.LogError($"percent key {percentKey} is not exist in _percentOfMaxDistanceSprite dictionary");
		}
	}

	void Start() {
		CalculateParameters();
	}

	void Update() {
		CalculateParameters();
	}

	void CalculateParameters() {
		CalculateLength();
		SetSpriteByLength();
	}

	void CalculateLength() {
		_distance = Vector2.Distance(_transform1.position, _transform2.position);
	}

	void SetSpriteByLength() {
		var currentValue = _distance / maxDistance;

		_totalIndexToApplySprites = 0;
		for (int i = 0; i < _percentOfMaxDistanceSprite.Count; i++) {
			if (currentValue > _percentOfMaxDistanceSprite.Keys.ElementAt(i)) {
				_totalIndexToApplySprites = i;
			} else
				break;
		}

		var totalObjectsToApplyList = _percentOfMaxDistanceSprite.ElementAt(_totalIndexToApplySprites).Value;
		foreach (var objectToApply in totalObjectsToApplyList.toApplySpriteParametersList) {
			objectToApply.ApplySprite();
		}

		if(_currentIndex != _totalIndexToApplySprites) {
			_prevIndex = _currentIndex;
			_currentIndex = _totalIndexToApplySprites;
			DidChangeStateIndex?.Invoke(_currentIndex);
		}
	}
}
