using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettingsToOtherCamerasAppler : MonoBehaviour {

    [SerializeField] Camera _main = default;
    [SerializeField] List<Camera> _camerasToApply = default;
    [SerializeField] bool _updateAtStart = false;

    bool _updateCameraView;

    void Start() {
        if (_updateAtStart)
            UpdateSettingsFromMainCamera();
    }

    public void UpdateSettingsFromMainCamera() {
        _updateCameraView = true;
    }

    private void LateUpdate() {
        if (_updateCameraView) {
            _updateCameraView = false;
            UpdateCameraView();
        }
    }

    void UpdateCameraView() {
        foreach (var camera in _camerasToApply) {
            camera.orthographicSize = _main.orthographicSize;
            camera.transform.position = _main.transform.position;
            camera.gameObject.SetActive(false);
            camera.gameObject.SetActive(true);
        }
    }
}
