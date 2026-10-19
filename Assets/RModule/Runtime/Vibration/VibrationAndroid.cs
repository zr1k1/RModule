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
		const long k_tickFallbackMs = 8;
		const int k_tickFallbackAmplitude = 30;

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
			int sdkInt = new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT");
			if (sdkInt < 26)
				return null;

			var vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
			if (sdkInt >= 29)
				return vibrationEffect.CallStatic<AndroidJavaObject>("createPredefined", k_effectTick);
			return vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", k_tickFallbackMs, k_tickFallbackAmplitude);
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