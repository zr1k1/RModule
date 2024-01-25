namespace RModule.Runtime.Camera {

	using UnityEngine;
	using UnityEngine.UI;
	using System;

	public interface ICameraController {
		BaseCameraController Prepare(CanvasScaler _canvasScaler, Vector2 sizeOfCentralizedObject, Vector2 positionOfCentralizedObject);
		void Setup(Action setupFinishCallback = null);
	}
}