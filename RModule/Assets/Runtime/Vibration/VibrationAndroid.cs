using UnityEngine;

public class VibrationAndroid : MonoBehaviour, IVibrator {
    //
    static AndroidJavaClass unityPlayer;
    static AndroidJavaObject currentActivity;
    static AndroidJavaObject vibrator;
    static long s_androidShortVibrationTime = 100;

    void Start() {
#if UNITY_ANDROID && !UNITY_EDITOR
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif
    }
    public void SetShortVibrationTime(long androidShortVibrationTime) {
        s_androidShortVibrationTime = androidShortVibrationTime;
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
