using UnityEngine;

public class SafeAreaHelper : MonoBehaviour {

	[SerializeField] bool _applyLeftSafeArea = true;
	[SerializeField] bool _applyTopSafeArea = true;
	[SerializeField] bool _applyRightSafeArea = true;
	[SerializeField] bool _applyBottomSafeArea = true;

	// --- Private vars ---
	RectTransform _panel;
	Rect _lastSafeArea = new Rect(0, 0, 0, 0);

	// ---------------------------------------------------------------
	// MonoBehaviour

	void Start() {
		_panel = GetComponent<RectTransform>();
		Refresh();
	}

	// ---------------------------------------------------------------
	// General Methods

	void Refresh() {
		Rect safeArea = GetSafeArea();

		if (safeArea != _lastSafeArea) {
			ApplySafeArea(safeArea);
		}
	}

	Rect GetSafeArea() {
		return Screen.safeArea;
	}

	void ApplySafeArea(Rect r) {
		_lastSafeArea = r;

		// Convert safe area rectangle from absolute pixels to normalised anchor coordinates
		Vector2 anchorMin = r.position;
		Vector2 anchorMax = r.position + r.size;
		anchorMin.x = _applyLeftSafeArea ? anchorMin.x / Screen.width : 0f;
		anchorMin.y = _applyBottomSafeArea ? anchorMin.y / Screen.height : 0f;
		anchorMax.x = _applyRightSafeArea ? anchorMax.x / Screen.width : 1f;
		anchorMax.y = _applyTopSafeArea ? anchorMax.y / Screen.height : 1f;

		_panel.anchorMin = anchorMin;
		_panel.anchorMax = anchorMax;

		//Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}", name, r.x, r.y, r.width, r.height, Screen.width, Screen.height);
	}
}
