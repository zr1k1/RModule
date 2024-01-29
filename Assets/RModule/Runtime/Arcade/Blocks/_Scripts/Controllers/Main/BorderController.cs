using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BorderController : MonoBehaviour {
	// Outlets
	[SerializeField] protected SpriteRenderer _spriteRenderer = default;
	[SerializeField] protected Transform _leftSide = default;
	[SerializeField] protected float _leftSideWidthInUnits = default;
	[SerializeField] protected Transform _rightSide = default;
	[SerializeField] protected float _rightSideWidthInUnits = default;

	// Privats
	protected SpriteRenderer _leftSideSpriteRenderer;
	protected SpriteRenderer _rightSideSpriteRenderer;

	protected virtual void Start() {
		UpdateView();
	}

	void Update() {
		if (Application.isEditor) {
			UpdateView();
		}
	}

	protected virtual void UpdateView() {
		var halfWidthOfMidPart = _spriteRenderer.size.x / 2f;
		_leftSide.localPosition = new Vector3(-halfWidthOfMidPart - _leftSideWidthInUnits / 2f, 0f, 0f);
		_rightSide.localPosition = new Vector3(halfWidthOfMidPart + _rightSideWidthInUnits / 2f, 0f, 0f);

		_leftSideSpriteRenderer = _leftSide.GetComponent<SpriteRenderer>();
		_rightSideSpriteRenderer = _rightSide.GetComponent<SpriteRenderer>();

		_leftSideSpriteRenderer.sortingLayerName = _spriteRenderer.sortingLayerName;
		_leftSideSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder;
		_rightSideSpriteRenderer.sortingLayerName = _spriteRenderer.sortingLayerName;
		_rightSideSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder;
	}
}
