namespace RModule.Runtime.Vibration {

	using UnityEngine;

	public class VibrationAndroid : MonoBehaviour, IVibrator {
		//
		static AndroidJavaClass unityPlayer;
		static AndroidJavaObject currentActivity;
		static AndroidJavaObject vibrator;
		static long s_androidShortVibrationTime = 100;
		static AndroidJavaObject s_tickEffect;

		// Consts
		const int k_effectTick = 2; // VibrationEffect.EFFECT_TICK
		const int k_effectSupportYes = 1; // Vibrator.VIBRATION_EFFECT_SUPPORT_YES
		const int k_defaultAmplitude = -1; // VibrationEffect.DEFAULT_AMPLITUDE
		// ERM motors (cheap devices) need ~20ms to spin up, shorter pulses are not felt at all
		const long k_tickFallbackMs = 20;
		const int k_tickFallbackAmplitude = 100;

		void Start() {
#if UNITY_ANDROID && !UNITY_EDITOR
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        s_tickEffect = CreateTickEffect();
#endif
		}
		public void SetShortVibrationTime(long androidShortVibrationTime) {
			s_androidShortVibrationTime = androidShortVibrationTime;
		}

		static AndroidJavaObject CreateTickEffect() {
			try {
				int sdkInt = new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT");
				if (sdkInt < 26 || vibrator == null)
					return null;

				var vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
				// Predefined EFFECT_TICK only when the vibrator HAL really supports it,
				// otherwise the framework fallback is too weak/short and nothing is felt
				if (sdkInt >= 30 && vibrator.Call<int>("areAllEffectsSupported", new object[] { new int[] { k_effectTick } }) == k_effectSupportYes)
					return vibrationEffect.CallStatic<AndroidJavaObject>("createPredefined", k_effectTick);

				int amplitude = vibrator.Call<bool>("hasAmplitudeControl") ? k_tickFallbackAmplitude : k_defaultAmplitude;
				return vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", k_tickFallbackMs, amplitude);
			} catch (System.Exception e) {
				Debug.LogWarning($"VibrationAndroid: CreateTickEffect failed: {e.Message}");
				return null;
			}
		}

		void Vibrate(long milliseconds) {
			vibrator.Call("vibrate", milliseconds);
		}

		void Vibrate(long[] pattern, int repeat) {
			vibrator.Call("vibrate", pattern, repeat);
		}

		void IVibrator.VibrateShort() {
			Debug.Log("Vibrate");
			Vibrate(s_androidShortVibrationTime);
		}

		void IVibrator.VibrateTick() {
			if (vibrator == null)
				return;
			if (s_tickEffect != null)
				vibrator.Call("vibrate", s_tickEffect);
			else
				Vibrate(k_tickFallbackMs);
		}

		void IVibrator.VibrateCustom(long milliseconds) {
			Vibrate(milliseconds);
		}

		void IVibrator.VibratePattern(long[] pattern, int repeat) {
			Vibrate(pattern, repeat);
		}

		void IVibrator.Cancel() {
			vibrator.Call("cancel");
		}
	}

}