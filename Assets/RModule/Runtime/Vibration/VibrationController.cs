namespace RModule.Runtime.Vibration {

	using RModule.Runtime.Utils;
	using UnityEngine;

	public class VibrationController : MonoBehaviour {

		// Accessors
		public static bool IsInitialized => s_isInitialized;
		public static bool VibrationIsEnabled => PlayerPrefsHelper.GetBool(k_vibrationIsEnabled, true);

		// Private vars
		static VibrationAndroid s_vibrationAndroid;
		static VibrationIOS s_vibrationIOS;
		static IVibrator s_vibrator;
		static bool s_isInitialized;

		// Consts
		const string k_vibrationIsEnabled = "vibrationIsEnabled";

		public static void Initialize() {
			GameObject vibrationControllerGO = new GameObject("vibrationController",
				typeof(VibrationController), typeof(VibrationAndroid), typeof(VibrationIOS));

			s_vibrationAndroid = vibrationControllerGO.GetComponent<VibrationAndroid>();
			s_vibrationAndroid.enabled = false;
			s_vibrationIOS = vibrationControllerGO.GetComponent<VibrationIOS>();
			s_vibrationIOS.enabled = false;

			if (Application.platform == RuntimePlatform.Android) {
				s_vibrationAndroid.enabled = true;
				s_vibrator = s_vibrationAndroid;
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				s_vibrationIOS.enabled = true;
				s_vibrator = s_vibrationIOS;
			}

			s_isInitialized = true;
		}

		public static void SetEnable(bool enable, bool safeStateToPrefs) {
			if (safeStateToPrefs && enable != VibrationIsEnabled) {
				PlayerPrefsHelper.SetBool(k_vibrationIsEnabled, enable);
				PlayerPrefs.Save();
			}
		}

		public static void SetAndroidShortVibrationTime(long androidShortVibrationTime) {
			s_vibrationAndroid.SetShortVibrationTime(androidShortVibrationTime);
		}

		// Play vibrations
		public static void PlayShortVibration() {
			if (VibrationIsEnabled && s_isInitialized && s_vibrator != null)
				s_vibrator.VibrateShort();
		}
	}

}