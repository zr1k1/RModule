using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RModule.Runtime.LeanTween;

public static class TransformExtensions {
	public static List<Transform> GetChilds(this Transform parent) {
		List<Transform> ret = new List<Transform>();
		foreach (Transform child in parent)
			ret.Add(child);

		return ret;
	}

	public static void DisableRaycasts(this Transform source) {
		var image = source.GetComponent<Image>();
		var text = source.GetComponent<Text>();
		var textMeshProUGUI = source.GetComponent<TextMeshProUGUI>();
		if (image)
			image.raycastTarget = false;
		if (text)
			text.raycastTarget = false;
		if (textMeshProUGUI)
			textMeshProUGUI.raycastTarget = false;

		foreach (RectTransform child in source)
			DisableRaycasts(child);
	}

	public static void ChangeAllAlphas(this Transform source, float alpha, float time) {
		var image = source.GetComponent<Image>();
		var text = source.GetComponent<Text>();
		var textMeshProUGUI = source.GetComponent<TextMeshProUGUI>();
		if (image) {
			var color = image.color;
			var endColor = new Color(color.r, color.g, color.b, alpha);
			LeanTween.value(source.gameObject, (colorNew) => { image.color = colorNew; }, color, endColor, time);
		}
		if (text) {
			var color = text.color;
			var endColor = new Color(color.r, color.g, color.b, alpha);
			LeanTween.value(source.gameObject, (colorNew) => { text.color = colorNew; }, color, endColor, time);
		}
		if (textMeshProUGUI) {
			var color = textMeshProUGUI.color;
			var endColor = new Color(color.r, color.g, color.b, alpha);
			LeanTween.value(source.gameObject, (colorNew) => { textMeshProUGUI.color = colorNew; }, color, endColor, time);
		}

		foreach (RectTransform child in source)
			child.ChangeAllAlphas(alpha, time);
	}

	public static void SetAlphaToAllImgsAndTMPChilds(this RectTransform rectTransform, float alpha) {
		List<RectTransform> childs = new List<RectTransform>(rectTransform.GetComponentsInChildren<RectTransform>());
		foreach (var child in childs) {
			LeanTween.alpha(child.GetComponent<RectTransform>(), alpha, 0f);
			var tmp = child.GetComponent<TextMeshProUGUI>();
			if (tmp != null) {
				LeanTween.value(child.gameObject, (alpha) => { tmp.alpha = alpha; }, tmp.alpha, alpha, 0f);
			}
		}
	}

	// Возращает прямоугольник границ графики всех спрайт элементов являющихся дочерними source трансформы
	public static Rect CalculateSpriteGraphicsBoundsOnChilds(this Transform source, List<Transform> excludeTransforms = null, OffsetsData offsetsData = null) {
		var _spriteRenderers = source.GetComponentsInChildren<SpriteRenderer>().ToList();
		if (excludeTransforms != null)
			_spriteRenderers.RemoveAll(spriteRenderer => excludeTransforms.Contains(spriteRenderer.transform));

		var _allBounds = _spriteRenderers.Select(sprRend => sprRend.bounds).ToList();
		var minLeftX = _allBounds[0].min.x;
		var maxRightX = _allBounds[0].max.x;
		var minBottomY = _allBounds[0].min.y;
		var maxTopY = _allBounds[0].max.y;

		List<float> _mixXs = new List<float>();
		List<float> _maxXs = new List<float>();
		List<float> _minYs = new List<float>();
		List<float> _maxYs = new List<float>();
		foreach (var bound in _allBounds) {
			_mixXs.Add(bound.min.x);
			_maxXs.Add(bound.max.x);
			_minYs.Add(bound.min.y);
			_maxYs.Add(bound.max.y);
		}

		minLeftX = _mixXs.Min() - offsetsData.LeftOffset;
		maxRightX = _maxXs.Max() + offsetsData.RightOffset;
		minBottomY = _minYs.Min() - offsetsData.BottomOffset;
		maxTopY = _maxYs.Max() + offsetsData.TopOffset;

		float xSize = Mathf.Abs(maxRightX - minLeftX);
		float ySize = Mathf.Abs(maxTopY - minBottomY);
		var size = new Vector2(xSize, ySize);
		Vector2 diagonalVector = new Vector2(maxRightX, maxTopY) - new Vector2(minLeftX, minBottomY);
		float length = diagonalVector.magnitude;
		Vector2 centrPosition = new Vector2(minLeftX, minBottomY) + diagonalVector.normalized * length * 0.5f;

		return new Rect(centrPosition, size);
	}
}
