using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.LeanTween;

public class SpriteOffset : MonoBehaviour {

	public SpriteRenderer _renderer = default;
	public Vector2 offset = default;

	Vector2 _lastOffset = default;

	private void Update() {
		if (_lastOffset != offset) {
			_lastOffset = offset;
			_renderer.material.SetTextureOffset("_MainTex", offset);
		}
	}
}
