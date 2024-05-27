using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour {
	//Accessors
	public bool IsOn => _isOn;

	// Outlets
	[SerializeField] Image _image = default;
	[SerializeField] SpriteRenderer _spriteRenderer = default;
	[SerializeField] StateData _onStateData = default;
	[SerializeField] StateData _offStateData = default;

	//Privats
	bool _isOn;

	[Serializable]
	public class StateData {
		public Sprite sprite;
		public Color color;
	}

	public void SetOn(bool isOn) {
		_isOn = isOn;
		var usedStateData = _isOn ? _onStateData : _offStateData;
		if (_image != null) {
			_image.sprite = usedStateData.sprite;
			_image.color = usedStateData.color;
		} else if (_spriteRenderer != null) {
			_spriteRenderer.sprite = usedStateData.sprite;
			_spriteRenderer.color = usedStateData.color;
		}
	}
}
