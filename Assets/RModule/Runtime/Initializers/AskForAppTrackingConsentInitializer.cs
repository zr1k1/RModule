using System.Collections;
using UnityEngine;
using MTUnityCore.Runtime.Plugins;

public class AskForAppTrackingConsentInitializer : Initializer {

	public override IEnumerator Initialize() {
		Debug.Log("AskForAppTrackingConsentInitializer : TryAskForAppTrackingConsent");

#if USE_MTUNITYCORE
	#if USE_APPODEAL
		Debug.Log("AskForAppTrackingConsentInitializer : Use Appodeal AskForAppTrackingConsent logic");
	#else
		Debug.Log("AskForAppTrackingConsentInitializer : Use Native AskForAppTrackingConsent");
		yield return AppTrackingConsentManager.AskForAppTrackingConsent();
	#endif
#endif
		yield return null;
	}
}
