namespace RModule.Runtime.Camera {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class FollowObjectCameraController : MonoBehaviour {
		// Outlets
		[SerializeField] Camera _camera = default;
		[SerializeField] GameObject _followGo = default;
		[SerializeField] Rect _fieldView = default;

		[Header("Debug")]
		[SerializeField] bool _instantiateImitationFieldView = default;

		// Privats
		float _camHeight;
		float _camWidth;
		float _minLeftX;
		float _maxRightX;
		float _minBottomY;
		float _maxTopY;

		bool _isFollow = true;

		public void StopFollow() {
			_isFollow = false;
		}

		public FollowObjectCameraController SetFollowGo(GameObject followGo) {
			_followGo = followGo;
			return this;
		}

		public FollowObjectCameraController SetFieldView(Rect viewRect, bool increaseFieldViewByCameraSize = false) {
			_fieldView = viewRect;

			_camHeight = 2 * _camera.orthographicSize;
			_camWidth = _camHeight * _camera.aspect;

			_minLeftX = viewRect.position.x - viewRect.size.x / 2f + (increaseFieldViewByCameraSize ? _camWidth / 2f : 0f);
			_maxRightX = viewRect.position.x + viewRect.size.x / 2f - (increaseFieldViewByCameraSize ? _camWidth / 2f : 0f);
			_minBottomY = viewRect.position.y - viewRect.size.y / 2f + (increaseFieldViewByCameraSize ? _camHeight / 2f : 0f);
			_maxTopY = viewRect.position.y + viewRect.size.y / 2f - (increaseFieldViewByCameraSize ? _camHeight / 2f : 0f);

			float xPos = _maxRightX > _minLeftX ? Mathf.Abs(_maxRightX - _minLeftX) : 0;
			float yPos = _maxTopY > _minBottomY ? Mathf.Abs(_maxTopY - _minBottomY) : 0;


			if (_instantiateImitationFieldView) {
				var imitationFieldViewGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
				imitationFieldViewGo.transform.position = viewRect.position;
				imitationFieldViewGo.transform.localScale = viewRect.size;
				imitationFieldViewGo.name = "ImitationFieldView";

				var imitationCameraMoveBoundsGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
				imitationCameraMoveBoundsGo.transform.position = viewRect.position;
				imitationCameraMoveBoundsGo.transform.localScale = new Vector3(xPos, yPos, 1f);
				imitationCameraMoveBoundsGo.name = "ImitationCameraMoveBounds";
			}

			return this;
		}

		void Update() {
			if (_followGo == null || _fieldView == null || !_isFollow)
				return;

			float xPos = 0;
			float yPos = 0;
			if (_fieldView.size.x > _camWidth) {
				xPos = Mathf.Clamp(_followGo.transform.position.x, _minLeftX, _maxRightX);
			}
			if (_fieldView.size.y > _camHeight) {
				yPos = Mathf.Clamp(_followGo.transform.position.y, _minBottomY, _maxTopY);
			}

			transform.position = new Vector3(xPos, yPos, _camera.transform.position.z);
		}

		// Statics

		public static float ConvertPixelsToUnits(CanvasScaler canvasScaler, float cameraOrthographicSize, float pixels) {
			return pixels / GetCanvasSizeInPixels(canvasScaler).y * cameraOrthographicSize * 2;
		}

		public static float ConvertUnitsToPixels(CanvasScaler canvasScaler, float cameraOrthographicSize, float units) {
			return units * GetCanvasSizeInPixels(canvasScaler).y / (cameraOrthographicSize * 2);
		}

		public static Vector2 GetCanvasSizeInPixels(CanvasScaler canvasScaler) {
			float screenWidth = Screen.width;
			float screenHeight = Screen.height;
			var scaleFactor = GetScaleFactor(canvasScaler);

			var realCanvasWidth = screenWidth / scaleFactor;
			var realCanvasHeight = screenHeight / scaleFactor;

			return new Vector2(realCanvasWidth, realCanvasHeight);
		}

		public static float GetScaleFactor(CanvasScaler canvasScaler) {
			float screenWidth = Screen.width;
			float screenHeight = Screen.height;
			var scaleFactor = 0f;
			if (canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight) {
				float kLogBase = 2;
				float logWidth = Mathf.Log(screenWidth / canvasScaler.referenceResolution.x, kLogBase);
				float logHeight = Mathf.Log(screenHeight / canvasScaler.referenceResolution.y, kLogBase);
				float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, canvasScaler.matchWidthOrHeight);
				scaleFactor = Mathf.Pow(kLogBase, logWeightedAverage);
			} else if (canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.Expand) {
				scaleFactor = Mathf.Min(screenWidth / canvasScaler.referenceResolution.x, screenHeight / canvasScaler.referenceResolution.y);
			} else if (canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.Shrink) {
				scaleFactor = Mathf.Max(screenWidth / canvasScaler.referenceResolution.x, screenHeight / canvasScaler.referenceResolution.y);
			}

			return scaleFactor;
		}
	}
}