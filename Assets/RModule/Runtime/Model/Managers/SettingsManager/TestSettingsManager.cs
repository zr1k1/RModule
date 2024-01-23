using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Sounds;
//using RModule.Runtime.ge

public class TestSettingsManager : MonoBehaviour {
	private IEnumerator Start() {
		yield return SoundsManager.Instance.Initialize();
		yield return ExampleSettingsManager.Instance.Initialize(SoundsManager.Instance, (enable) => {
			VibrationController.SetEnable(enable, true);
		});
		Debug.Log($"TestSettingsManager : CommonSetting.SoundEffectsVolume {ExampleSettingsManager.Instance.GetValue<bool>(CommonSetting.SoundEffectsVolume)}");
		Debug.Log($"TestSettingsManager : CommonSetting.Ages {ExampleSettingsManager.Instance.GetValue<int>(CommonSetting.Ages)}");

		//todo test save stop play and get
	}
}
