namespace RModule.Runtime.Vibration {

#if UNITY_IOS && !UNITY_EDITOR
using System.Collections;
using System.Runtime.InteropServices;
#endif
	using UnityEngine;

	public class VibrationIOS : MonoBehaviour, IVibrator {
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport ( "__Internal" )]
    private static extern bool _HasVibrator ();

    [DllImport ( "__Internal" )]
    private static extern void _Vibrate ();

    [DllImport ( "__Internal" )]
    private static extern void _VibratePop ();

    [DllImport ( "__Internal" )]
    private static extern void _VibratePeek ();

    [DllImport ( "__Internal" )]
    private static extern void _VibrateNope ();

    [DllImport ( "__Internal" )]
    private static extern void _VibrateSelection ();
#endif

		public static void VibratePop() {
#if UNITY_IOS && !UNITY_EDITOR
        _VibratePop ();
#endif
		}
		///<summary>
		/// Small peek vibration
		///</summary>
		public static void VibratePeek() {
#if UNITY_IOS && !UNITY_EDITOR
        _VibratePeek ();
#endif
		}

		public static void VibrateNope() {
#if UNITY_IOS && !UNITY_EDITOR
        _VibrateNope ();
#endif
		}

		///<summary>
		/// Lightest selection tick (UISelectionFeedbackGenerator)
		///</summary>
		public static void VibrateSelection() {
#if UNITY_IOS && !UNITY_EDITOR
        _VibrateSelection ();
#endif
		}

		public static bool HasVibrator() {
#if UNITY_IOS && !UNITY_EDITOR
        return _HasVibrator ();
#endif
			return false;
		}

		public static void Vibrate() {
#if !UNITY_WEBGL
			Handheld.Vibrate();
#endif
		}

		void IVibrator.VibrateShort() {
			VibratePeek();
		}

		void IVibrator.VibrateTick() {
			VibrateSelection();
		}

		void IVibrator.VibrateCustom(long milliseconds) {
			throw new System.NotImplementedException();
		}

		void IVibrator.VibratePattern(long[] pattern, int repeat) {
			throw new System.NotImplementedException();
		}

		void IVibrator.Cancel() {
			throw new System.NotImplementedException();
		}
	}

}