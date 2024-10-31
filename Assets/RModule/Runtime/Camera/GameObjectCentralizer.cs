using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class GameObjectCentralizer {

	// Accessors
	public float TopMainOffsetHeightInUnits => ConvertPixelsToUnits(_canvasScaler, _resultData.OrthographicSizeCamera, _topTotalOffsetHeight);
	public float BottomMainOffsetHeightInUnits => ConvertPixelsToUnits(_canvasScaler, _resultData.OrthographicSizeCamera, _bottomTotalOffsetHeight);
	public float LeftMainOffsetWidthInUnits => ConvertPixelsToUnits(_canvasScaler, _resultData.OrthographicSizeCamera, _leftTotalOffsetWidth);
	public float GetRightMainOffsetWidthInUnits => ConvertPixelsToUnits(_canvasScaler, _resultData.OrthographicSizeCamera, _rightTotalOffsetWidth);

	// Privats
	protected CanvasScaler _canvasScaler;
	protected Vector2 _sizeObjToCentralize;
	protected Vector2 _canvasSize;

	protected float _topTotalOffsetHeight;
	protected float _bottomTotalOffsetHeight;
	protected float _leftTotalOffsetWidth;
	protected float _rightTotalOffsetWidth;

	protected float _topAdditionalOffset;
	protected float _bottomAdditionalOffset;
	protected float _leftAdditionalOffset;
	protected float _rightAdditionalOffset;

	protected float _topMainOffsetHeightInUnits;
	protected float _bottomMainOffsetHeightInUnits;
	protected float _leftMainOffsetWidthInUnits;
	protected float _rightMainOffsetWidthInUnits;

	protected float _coeffTopToolbarHeightToCanvasHeight;
	protected float _coeffBottomToolbarHeightToCanvasHeight;
	protected float _coeffLeftToolbarWidthToCanvasWidth;
	protected float _coeffRightToolbarWidthToCanvasWidth;

	protected Vector2 _safeAreaAnchoredMin = Vector2.zero;
	protected Vector2 _safeAreaAnchoredMax = Vector2.one;

	protected ResultData _resultData;

	// Classes
	[Serializable]
	public class ResultData {
		public Vector2 PositionCamera;
		public float OrthographicSizeCamera;
		public Vector2 FreeSpaceForGameObject;
	}

	public GameObjectCentralizer(CanvasScaler canvasScaler, Vector2 sizeObjToCentralize,
		Vector2 positionObjToCentralize, float cameraOrthographicSize) {

		_canvasScaler = canvasScaler;
		_sizeObjToCentralize = sizeObjToCentralize;
		_resultData = new ResultData {
			PositionCamera = positionObjToCentralize,
			OrthographicSizeCamera = cameraOrthographicSize
		};
	}

	public ResultData Calculate() {

		_canvasSize = GetCanvasSizeInPixels(_canvasScaler);

		ApplySafeAreaOffsetsToToolbarsSizes();
		CalculateFreeSpaceForGameObject();
		CalculateCoefficientsOfRatioOfSizeOfToolbarToSizeOfCanvas();
		CalculateOrthographicSize();
		UpdateOffsetsInUnits();
		CorrectCameraPositionByOffsets();

		return _resultData;
	}

	public GameObjectCentralizer SetTopMainOffsetInPixels(float topOffsetInPixels) {
		_topTotalOffsetHeight = topOffsetInPixels;

		return this;
	}

	public GameObjectCentralizer SetBottomMainOffsetInPixels(float bottomOffsetInPixels) {
		_bottomTotalOffsetHeight = bottomOffsetInPixels;

		return this;
	}

	public GameObjectCentralizer SetLeftMainOffsetInPixels(float leftOffsetInPixels) {
		_leftTotalOffsetWidth = leftOffsetInPixels;

		return this;
	}

	public GameObjectCentralizer SetRightMainOffsetInPixels(float rightOffsetInPixels) {
		_rightTotalOffsetWidth = rightOffsetInPixels;

		return this;
	}

	public GameObjectCentralizer SetTopAdditionalOffsetUnits(float topAdditionalOffset) {
		_topAdditionalOffset = topAdditionalOffset;

		return this;
	}

	public GameObjectCentralizer SetBottomAdditionalOffsetUnits(float bottomAdditionalOffset) {
		_bottomAdditionalOffset = bottomAdditionalOffset;

		return this;
	}

	public GameObjectCentralizer SetLeftAdditionalOffsetUnits(float leftAdditionalOffset) {
		_leftAdditionalOffset = leftAdditionalOffset;

		return this;
	}

	public GameObjectCentralizer SetRightAdditionalOffsetUnits(float rightAdditionalOffset) {
		_rightAdditionalOffset = rightAdditionalOffset;

		return this;
	}

	public GameObjectCentralizer SetSafeAreaAnchoredMinMax(Vector2 anchoredMin, Vector2 anchoredMax) {
		_safeAreaAnchoredMin = anchoredMin;
		_safeAreaAnchoredMax = anchoredMax;

		return this;
	}

	void ApplySafeAreaOffsetsToToolbarsSizes() {
		_topTotalOffsetHeight += (1f - _safeAreaAnchoredMax.y) * _canvasSize.y;
		_bottomTotalOffsetHeight += _safeAreaAnchoredMin.y * _canvasSize.y;
		_leftTotalOffsetWidth += _safeAreaAnchoredMin.x * _canvasSize.x;
		_rightTotalOffsetWidth += (1f - _safeAreaAnchoredMax.x) * _canvasSize.x;
	}

	void CalculateFreeSpaceForGameObject() {
		float freeHeightForGameObject = _canvasSize.y;
		float freeWidthForGameObject = _canvasSize.x;

		freeHeightForGameObject -= _topTotalOffsetHeight;
		freeHeightForGameObject -= _bottomTotalOffsetHeight;
		freeWidthForGameObject -= _leftTotalOffsetWidth;
		freeWidthForGameObject -= _rightTotalOffsetWidth;

		_resultData.FreeSpaceForGameObject = new Vector2(freeWidthForGameObject, freeHeightForGameObject);
	}

	void CalculateCoefficientsOfRatioOfSizeOfToolbarToSizeOfCanvas() {
		// Коэффициент отношения размера тулбара к размеру канваса 
		_coeffTopToolbarHeightToCanvasHeight = _topTotalOffsetHeight / _canvasSize.y;
		_coeffBottomToolbarHeightToCanvasHeight = _bottomTotalOffsetHeight / _canvasSize.y;
		_coeffLeftToolbarWidthToCanvasWidth = _leftTotalOffsetWidth / _canvasSize.x;
		_coeffRightToolbarWidthToCanvasWidth = _rightTotalOffsetWidth / _canvasSize.x;
	}

	void CalculateOrthographicSize() {
		float screenW = Screen.width;
		float screenH = Screen.height;
		float screenAspect = screenW / screenH;
		float objToCentralizeAspect = (_sizeObjToCentralize.x + _rightAdditionalOffset + _leftAdditionalOffset) / (_sizeObjToCentralize.y + _topAdditionalOffset + _bottomAdditionalOffset);
		float freeSpaceAspect = _resultData.FreeSpaceForGameObject.x / _resultData.FreeSpaceForGameObject.y;
		float totalScreenHeightInUnits;

		if (objToCentralizeAspect > freeSpaceAspect) {
			totalScreenHeightInUnits = (_sizeObjToCentralize.x + _rightAdditionalOffset + _leftAdditionalOffset) / ((1f - _coeffLeftToolbarWidthToCanvasWidth - _coeffRightToolbarWidthToCanvasWidth) * screenAspect);
		} else {
			totalScreenHeightInUnits = (_sizeObjToCentralize.y + _topAdditionalOffset + _bottomAdditionalOffset) / (1f - _coeffTopToolbarHeightToCanvasHeight - _coeffBottomToolbarHeightToCanvasHeight);
		}

		_resultData.OrthographicSizeCamera = totalScreenHeightInUnits / 2f;
	}

	void UpdateOffsetsInUnits() {
		_topMainOffsetHeightInUnits = ConvertPixelsToUnits(_canvasScaler, _resultData.OrthographicSizeCamera, _topTotalOffsetHeight);
		_bottomMainOffsetHeightInUnits = ConvertPixelsToUnits(_canvasScaler, _resultData.OrthographicSizeCamera, _bottomTotalOffsetHeight);
		_leftMainOffsetWidthInUnits = ConvertPixelsToUnits(_canvasScaler, _resultData.OrthographicSizeCamera, _leftTotalOffsetWidth);
		_rightMainOffsetWidthInUnits = ConvertPixelsToUnits(_canvasScaler, _resultData.OrthographicSizeCamera, _rightTotalOffsetWidth);
	}

	void CorrectCameraPositionByOffsets() {
		_resultData.PositionCamera += new Vector2((_rightMainOffsetWidthInUnits - _leftMainOffsetWidthInUnits) / 2f, (_topMainOffsetHeightInUnits - _bottomMainOffsetHeightInUnits) / 2f);
		_resultData.PositionCamera += new Vector2((_rightAdditionalOffset - _leftAdditionalOffset) / 2f, (_topAdditionalOffset - _bottomAdditionalOffset) / 2f);
	}

	// CanvasScaler Helpers
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
