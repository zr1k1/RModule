using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetterByDistanceBetweenObjects : MonoBehaviour {

	// Accessors
	public Color Color => _color;

	// Parameters
	public float distanceToMaxColor = default;

	// Outlets
	[SerializeField] List<SpriteRenderer> _spriteRenderers = default;
	[SerializeField] Transform _transform1 = default;
	[SerializeField] Transform _transform2 = default;
	[SerializeField] float _percentOfMaxDistanceToStartChangeColor = default;
	[SerializeField] Color _colorTo = default;

	// Privats
	float _distance;
	Color _color;

	void Start() {
		CalculateParameters();
	}

	void Update() {
		CalculateParameters();
	}

	void CalculateParameters() {
		CalculateLength();
		CalculateColorByLength();
	}

	void CalculateLength() {
		_distance = Vector2.Distance(_transform1.position, _transform2.position);
	}

	void CalculateColorByLength() {
		var currentValue = _distance / distanceToMaxColor;
		Color.RGBToHSV(_colorTo, out var h, out var s, out var v);
		var saturation = _distance >= _percentOfMaxDistanceToStartChangeColor * distanceToMaxColor ? currentValue * 1 : 0;
		_color = Color.HSVToRGB(h, saturation, v);
		foreach (var spriteRend in _spriteRenderers)
			spriteRend.color = _color;
	}
}
