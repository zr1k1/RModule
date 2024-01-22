using UnityEngine;

namespace RModule.Runtime.Utils {
	public class FPSDisplay : MonoBehaviour {

		// --- Private vars ---
		float _deltaTime;

		// ---------------------------------------------------------------
		// GameObject lifecycle

		void Update() {
			_deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
		}

		void OnGUI() {
			int w = Screen.width, h = Screen.height;

			var style = new GUIStyle();

			var rect = new Rect(0, 0, w, h * 2 / 100);
			style.alignment = TextAnchor.UpperLeft;
			style.fontSize = h * 2 / 100;
			style.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 0.5f);
			float msec = _deltaTime * 1000.0f;
			float fps = 1.0f / _deltaTime;
			string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
			GUI.Label(rect, text, style);
		}
	}
}
