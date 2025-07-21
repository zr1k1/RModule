using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseCameraController : MonoBehaviour, ICameraController {
	// Enums
	public enum ViewDirection {
		Default = 0,
		TopView = 1,
	}

	// Accessors
	public Camera Camera => _gameCamera;
	public float TopMainOffsetHeightInUnits => _gameObjectCentralizer.TopMainOffsetHeightInUnits;
	public float BottomMainOffsetHeightInUnits => _gameObjectCentralizer.BottomMainOffsetHeightInUnits;
	public float LeftMainOffsetWidthInUnits => _gameObjectCentralizer.LeftMainOffsetWidthInUnits;
	public float RightMainOffsetWidthInUnits => _gameObjectCentralizer.GetRightMainOffsetWidthInUnits;

	// Outlets
	[SerializeField] protected ViewDirection _viewDirection = default;
	[SerializeField] protected RectTransform _safeAreaContainer = default;
	[SerializeField] protected Camera _gameCamera = default;

	[SerializeField] protected float _topMainOffsetInPixels = default;
	[SerializeField] protected float _bottomMainOffsetInPixels = default;
	[SerializeField] protected float _leftMainOffsetInPixels = default;
	[SerializeField] protected float _rightMainOffsetInPixels = default;

	[SerializeField] protected float _topAdditionalOffsetInUnits = default;
	[SerializeField] protected float _bottomAdditionalOffsetInUnits = default;
	[SerializeField] protected float _leftAdditionalOffsetInUnits = default;
	[SerializeField] protected float _rightAdditionalOffsetInUnits = default;

	// Privats
	protected GameObjectCentralizer _gameObjectCentralizer;
	protected GameObjectCentralizer.ResultData _resultData;
	protected CanvasScaler _canvasScaler;
	protected Vector2 _sizeOfCentralizedObject;
	protected Vector2 _positionOfCentralizedObject;
	protected Vector3 _cameraPosition;
	protected bool _prepared;

	protected virtual void Start() {
		if (_gameCamera == null)
			Debug.LogError("Set _gameCamera in inspector!");
	}

	public virtual void Setup(CanvasScaler canvasScaler, Vector2 sizeOfCentralizedObject, Vector2 positionOfCentralizedObject, Action setupFinishCallback = null) {
		Prepare(canvasScaler, sizeOfCentralizedObject, positionOfCentralizedObject);
		Setup(setupFinishCallback);
	}

	public virtual BaseCameraController Prepare(CanvasScaler canvasScaler, Vector2 sizeOfCentralizedObject, Vector2 positionOfCentralizedObject) {
		Debug.Log($"BaseCameraController : Prepare");
		Debug.Log($"BaseCameraController : sizeOfCentralizedObject {sizeOfCentralizedObject}");
		Debug.Log($"BaseCameraController : positionOfCentralizedObject {positionOfCentralizedObject}");

		_canvasScaler = canvasScaler;
		_sizeOfCentralizedObject = sizeOfCentralizedObject;
		_positionOfCentralizedObject = positionOfCentralizedObject;

		_prepared = true;

		return this;
	}

	public virtual void Setup(Action setupFinishCallback = null) {
		if (!_prepared) {
			Debug.LogError($"Сamera controller not prepared! Use Prepare method before Setup!");
			return;
		}

		_gameObjectCentralizer = new GameObjectCentralizer(_canvasScaler, _sizeOfCentralizedObject, _positionOfCentralizedObject, _gameCamera.orthographicSize)
		   .SetTopMainOffsetInPixels(_topMainOffsetInPixels)
		   .SetBottomMainOffsetInPixels(_bottomMainOffsetInPixels)
		   .SetLeftMainOffsetInPixels(_leftMainOffsetInPixels)
		   .SetRightMainOffsetInPixels(_rightMainOffsetInPixels)
		   .SetTopAdditionalOffsetUnits(_topAdditionalOffsetInUnits)
		   .SetBottomAdditionalOffsetUnits(_bottomAdditionalOffsetInUnits)
		   .SetLeftAdditionalOffsetUnits(_leftAdditionalOffsetInUnits)
		   .SetRightAdditionalOffsetUnits(_rightAdditionalOffsetInUnits);

		if (_safeAreaContainer != null)
			_gameObjectCentralizer.SetSafeAreaAnchoredMinMax(_safeAreaContainer.anchorMin, _safeAreaContainer.anchorMax);

		_resultData = _gameObjectCentralizer.Calculate();

		SetupTransformParamsByViewType();
		_gameCamera.orthographicSize = _resultData.OrthographicSizeCamera;

		setupFinishCallback?.Invoke();
	}

	void SetupTransformParamsByViewType() {
		_cameraPosition = new Vector3(_resultData.PositionCamera.x, _resultData.PositionCamera.y, _gameCamera.transform.position.z);
		var cameraRotation = Vector3.zero;

		if (_viewDirection == ViewDirection.TopView) {
			_cameraPosition = new Vector3(_resultData.PositionCamera.x, _gameCamera.transform.position.y, _resultData.PositionCamera.y);
			//_cameraPosition = new Vector3(_cameraPosition.x, _cameraPosition.z, _cameraPosition.y);
			cameraRotation = new Vector3(90, 0, 0);
		}

		_gameCamera.transform.position = _cameraPosition;
		_gameCamera.transform.localEulerAngles = cameraRotation;
	}

	public BaseCameraController SetSafeAreaContainer(RectTransform safeAreaContainer) {
		_safeAreaContainer = safeAreaContainer;

		return this;
	}

	public BaseCameraController SetTopMainOffsetInPixels(float topOffsetInPixels) {
		_topMainOffsetInPixels = topOffsetInPixels;

		return this;
	}

	public BaseCameraController SetBottomMainOffsetInPixels(float bottomOffsetInPixels) {
		_bottomMainOffsetInPixels = bottomOffsetInPixels;

		return this;
	}

	public BaseCameraController SetLeftMainOffsetInPixels(float leftOffsetInPixels) {
		_leftMainOffsetInPixels = leftOffsetInPixels;

		return this;
	}

	public BaseCameraController SetRightMainOffsetInPixels(float rightOffsetInPixels) {
		_rightMainOffsetInPixels = rightOffsetInPixels;

		return this;
	}

	public BaseCameraController SetTopAdditionalOffsetUnits(float topAdditionalOffset) {
		_topAdditionalOffsetInUnits = topAdditionalOffset;

		return this;
	}

	public BaseCameraController SetBottomAdditionalOffsetUnits(float bottomAdditionalOffset) {
		_bottomAdditionalOffsetInUnits = bottomAdditionalOffset;

		return this;
	}

	public BaseCameraController SetLeftAdditionalOffsetUnits(float leftAdditionalOffset) {
		_leftAdditionalOffsetInUnits = leftAdditionalOffset;

		return this;
	}

	public BaseCameraController SetRightAdditionalOffsetUnits(float rightAdditionalOffset) {
		_rightAdditionalOffsetInUnits = rightAdditionalOffset;

		return this;
	}
}
