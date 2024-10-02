using System.Collections;
#if USE_MTUNITY
using MTUnityCore.Runtime.Plugins;
#endif
using UnityEngine;

public class AskForAppTrackingConsentInitializer : Initializer {

	public override IEnumerator Initialize() {
		Debug.Log("AskForAppTrackingConsentInitializer : TryAskForAppTrackingConsent");
#if USE_APPODEAL
		Debug.Log("AskForAppTrackingConsentInitializer : Use Appodeal AskForAppTrackingConsent logic");
#elif USE_MTUNITY
		Debug.Log("AskForAppTrackingConsentInitializer : Use Native AskForAppTrackingConsent");
		yield return AppTrackingConsentManager.AskForAppTrackingConsent();
#else
		Debug.LogWarning("AskForAppTrackingConsentInitializer : NOT REALIZED");
#endif
		yield return null;
	}
}
