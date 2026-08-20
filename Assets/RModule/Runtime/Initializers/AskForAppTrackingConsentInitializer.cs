using System.Collections;
using UnityEngine;
#if USE_MTUNITYCORE
using MTUnityCore.Runtime.Plugins;
#endif

public class AskForAppTrackingConsentInitializer : Initializer {

	public override IEnumerator Initialize() {
		Debug.Log("AskForAppTrackingConsentInitializer : TryAskForAppTrackingConsent");

#if USE_MTUNITYCORE
		Debug.Log("AskForAppTrackingConsentInitializer : Use Native AskForAppTrackingConsent");
		yield return AppTrackingConsentManager.AskForAppTrackingConsent();
#endif
		yield return null;
	}
}
