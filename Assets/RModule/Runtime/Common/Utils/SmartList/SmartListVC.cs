using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class SmartListVC : MonoBehaviour {
	// Enums
	enum Orientation { UpToDown, LeftToRight, RightToLeft, DownToUp }

	// Events
	public event UnityAction SetupDidFinished = default;

	// Outlets
	[SerializeField] RectTransform _elementsParent = default;
	[SerializeField] Orientation orientation = default;

	// Private vars
	Vector2 _elementSize;
	List<SmartListElement> _smartListElements = new List<SmartListElement>();

	public void InitializePrivateVars(Vector2 elementSize) {
		Assert.IsNotNull(_elementsParent, "_elementsParent not setted");
		_elementSize = elementSize;
	}

	public void ScrollTo(int index) {
		if (_smartListElements.Count == 0)
			return;
		RectTransform rt = _smartListElements[index].GetComponent<RectTransform>();
		RectTransform viewport = _elementsParent.parent.GetComponent<RectTransform>();

        float spacing = 0;
		float midIndex = ((float)_smartListElements.Count - 1f) / 2f;
		float indexFromMid = index - midIndex;
		int direction = orientation == Orientation.LeftToRight || orientation == Orientation.DownToUp ? -1 : 1;

		if (orientation == Orientation.LeftToRight) {
			float midPos = (float)_elementsParent.sizeDelta.x / 2f;
			HorizontalLayoutGroup horizontalLayoutGroup = _elementsParent.GetComponent<HorizontalLayoutGroup>();
			if (horizontalLayoutGroup)
				spacing = horizontalLayoutGroup.spacing;
			float boundValue = (_elementsParent.sizeDelta.x - viewport.rect.size.x) / 2f;
			float elementWidth = _elementSize.x + spacing;
			var pos = indexFromMid * elementWidth * direction;
			pos = Mathf.Clamp(pos, -boundValue, boundValue);
			_elementsParent.anchoredPosition = new Vector2(pos, _elementsParent.anchoredPosition.y);
		} else if(orientation == Orientation.UpToDown) {
			VerticalLayoutGroup verticalLayoutGroup = _elementsParent.GetComponent<VerticalLayoutGroup>();
            if (verticalLayoutGroup) 
				spacing = verticalLayoutGroup.spacing;
			float elementHeight = _elementSize.y + spacing;
			float boundValue = (_elementsParent.sizeDelta.y - viewport.rect.size.y) / 2f;
			var pos = indexFromMid * elementHeight * direction;
			pos = Mathf.Clamp(pos, -boundValue, boundValue);
			_elementsParent.anchoredPosition = new Vector2(_elementsParent.anchoredPosition.x, pos);
		}
	}

	public void Setup(Vector2 elementSize, int elementsCount, int beginIndex, Action<SmartListElement> visibleCallback, Action<SmartListElement> invisibleCallback) {
		InitializePrivateVars(elementSize);

		CreateEmptyElements(elementsCount, visibleCallback, invisibleCallback);
		StartCoroutine(WaitAndScrollTo(beginIndex));
	}

	public void AddListenersToFullyVisibleEvent(Action<SmartListElement> elementDidFullyVisible) {
		foreach(var smartListElement in _smartListElements)
			smartListElement.DidFullyVisible += elementDidFullyVisible.Invoke;

	}

	public void RemoveElement(int index) {
		Destroy(_smartListElements[index].gameObject);
		_smartListElements.RemoveAt(index);
	}

	public void RemoveElementById(int id) {
		var element = _smartListElements.Find(element => element.Id == id);
		if(element != null) {
			Destroy(element.gameObject);
			_smartListElements.Remove(element);
		}
	}

	void CreateEmptyElements(int count, Action<SmartListElement> visibleCallback, Action<SmartListElement> invisibleCallback) {
		for (int i = 0; i < count; i++)
			CreateEmptyElementWithImage(i, $"emptyElement_{i}", visibleCallback, invisibleCallback);
	}

	void CreateEmptyElementWithImage(int id, string elementName, Action<SmartListElement> visibleCallback, Action<SmartListElement> invisibleCallback) {
		GameObject emptyElement = new GameObject(elementName, typeof(RectTransform), typeof(SmartListElement));
		var smartListelement = emptyElement.GetComponent<SmartListElement>();
		smartListelement.Setup(id, visibleCallback, invisibleCallback);
		var rt = emptyElement.GetComponent<RectTransform>();
		rt.sizeDelta = _elementSize;
		rt.SetParent(_elementsParent, false);

		_smartListElements.Add(emptyElement.GetComponent<SmartListElement>());
	}

	IEnumerator WaitAndScrollTo(int index) {
		yield return new WaitForEndOfFrame();
		ScrollTo(index);
		SetupDidFinished?.Invoke();
	}
}
