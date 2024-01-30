using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Sounds;
using RModule.Runtime.Vibration;

public class TestSettingsManager : MonoBehaviour {
	private IEnumerator Start() {
		yield return SoundsManager.Instance.Initialize();
		yield return ExampleSettingsManager.Instance.Initialize(SoundsManager.Instance, (enable) => {
			VibrationController.SetEnable(enable, true);
		});
		Debug.Log($"TestSettingsManager : CommonSetting.SoundEffectsVolume {ExampleSettingsManager.Instance.GetSetting<bool>(CommonSetting.SoundEnabled)}");
		Debug.Log($"TestSettingsManager : CommonSetting.Ages {ExampleSettingsManager.Instance.GetSetting<int>(CommonSetting.Ages)}");

		//todo test save stop play and get
	}
}
