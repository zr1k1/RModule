using System;
using UnityEngine;

public class SmartListElement : MonoBehaviour {
	// Events
	public event Action<SmartListElement> DidFullyVisible = default;

	// Accessors
	public int Id => _id;
	public bool IsVisible => _isVisible;
	public bool WillRemove => _willRemove;

	// Private vars
	RectTransform myRectTransform;
	Action<SmartListElement> _visibleCallback;
	Action<SmartListElement> _invisibleCallback;
	bool _isVisible;
	bool _isFullyVisible;
	bool _willRemove;
	int _id;

	public void Setup(int id, Action<SmartListElement> visibleCallback, Action<SmartListElement> invisibleCallback) {
		_id = id;
		_visibleCallback = visibleCallback;
		_invisibleCallback = invisibleCallback;
	}

	public void SetWillRemove() {
		_willRemove = true;
	}

	private void Start() {
		myRectTransform = GetComponent<RectTransform>();
	}

	private void FixedUpdate() {
		if (_willRemove)
			return;
		bool isVisible = myRectTransform.IsVisibleFrom(Camera.main);
		if (_isVisible != isVisible) {
			_isVisible = isVisible;
			if (_isVisible)
				_visibleCallback?.Invoke(this);
			else
				_invisibleCallback?.Invoke(this);
		}
		bool isFullyVisible = myRectTransform.IsFullyVisibleFrom(Camera.main);
		if(_isFullyVisible != isFullyVisible) {
			_isFullyVisible = isFullyVisible;

			if (_isFullyVisible)
				DidFullyVisible?.Invoke(this);
		}
	}
}
