using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettingsToOtherCamerasAppler : MonoBehaviour {

    [SerializeField] Camera _main = default;
    [SerializeField] List<Camera> _camerasToApply = default;

    public void UpdateSettingsFromMainCamera() {

		foreach (var camera in _camerasToApply) {
			camera.orthographicSize = _main.orthographicSize;
			camera.transform.position = _main.transform.position;
		}
	}
}
