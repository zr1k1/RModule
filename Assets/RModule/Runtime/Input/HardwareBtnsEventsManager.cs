using UnityEngine;
using UnityEngine.InputSystem;

public class HardwareBtnsEventsManager : SingletonMonoBehaviour<HardwareBtnsEventsManager> {

	public delegate void HardwareBackBtnTappedDelegate();
	public static event HardwareBackBtnTappedDelegate HardwareBackBtnTappedEvent;

	// ---------------------------------------------------------------
	// GameObject lifecycle

	void Update() {
		if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) {
			HardwareBackBtnTappedEvent?.Invoke();
		}
	}

	public override bool IsInitialized() {
		return Instance != null;
	}
}