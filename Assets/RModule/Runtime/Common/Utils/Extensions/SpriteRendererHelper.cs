using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class SpriteRendererHelper : MonoBehaviour {
	// Outlets
	[SerializeField] List<ObjectToApplyParameters> _objectsToApplyParameters = default;

	// Privats
	SpriteRenderer _spriteRenderer;

	[Serializable]
	public class ObjectToApplyParameters {
		public SpriteRenderer spriteRenderer = default;
		public bool width = default;
		public bool height = default;
		public bool switchWidthAndHeight = default;
		public Vector2 widthHeightOffsets = default;
		public bool sortingLayer = default;
		public bool sortingLayerOrder = default;
		public Vector3 localPosition = default;
		public Vector3 localEulerAngles = default;
	}

	void Start() {
		UpdateParameters();
	}

	void Update() {
		if (Application.isEditor)
			UpdateParameters();
	}

	public void UpdateParameters() {
		if (_spriteRenderer == null)
			_spriteRenderer = GetComponent<SpriteRenderer>();

		foreach (var objectToApplyParameters in _objectsToApplyParameters) {
			if (objectToApplyParameters.spriteRenderer == null) {
				Debug.LogError("Sprite renderer not setup in inspector");
				continue;
			}

			TryApplySize(objectToApplyParameters);
			TryApplySortingLayer(objectToApplyParameters);
			TryApplySortingLayerOrder(objectToApplyParameters);
			ApplyLocalPosition(objectToApplyParameters);
			ApplyLocalEulerAngles(objectToApplyParameters);
		}
	}

	void TryApplySize(ObjectToApplyParameters objectToApplyParameters) {

		float width = objectToApplyParameters.width ? _spriteRenderer.size.x : objectToApplyParameters.spriteRenderer.size.x;
		float height = objectToApplyParameters.height ? _spriteRenderer.size.y : objectToApplyParameters.spriteRenderer.size.y; ;
		float widthOffset = objectToApplyParameters.width ? objectToApplyParameters.widthHeightOffsets.x : 0;
		float heightOffset = objectToApplyParameters.height ? objectToApplyParameters.widthHeightOffsets.y : 0;
		if (objectToApplyParameters.width && objectToApplyParameters.height && objectToApplyParameters.switchWidthAndHeight) {
			var tempWidth = width;
			width = height;
			height = tempWidth;
			tempWidth = widthOffset;
			widthOffset = heightOffset;
			heightOffset = tempWidth;
		}

		var totalSize = new Vector2(width, height);
		totalSize += new Vector2(widthOffset, heightOffset);

		objectToApplyParameters.spriteRenderer.size = totalSize;
	}

	void TryApplySortingLayer(ObjectToApplyParameters objectToApplyParameters) {
		if (objectToApplyParameters.sortingLayer) {
			objectToApplyParameters.spriteRenderer.sortingLayerName = _spriteRenderer.sortingLayerName;
		}
	}

	void TryApplySortingLayerOrder(ObjectToApplyParameters objectToApplyParameters) {
		if (objectToApplyParameters.sortingLayerOrder) {
			objectToApplyParameters.spriteRenderer.sortingOrder = _spriteRenderer.sortingOrder;
		}
	}

	void ApplyLocalPosition(ObjectToApplyParameters objectToApplyParameters) {
		objectToApplyParameters.spriteRenderer.transform.localPosition = objectToApplyParameters.localPosition;
	}

	void ApplyLocalEulerAngles(ObjectToApplyParameters objectToApplyParameters) {
		objectToApplyParameters.spriteRenderer.transform.localEulerAngles = objectToApplyParameters.localEulerAngles;
	}
}
