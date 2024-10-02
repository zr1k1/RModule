using System.Collections;
using UnityEngine;
#if USE_FIREBASE
using Imported.Managers.MTFirebase;
#endif

public class RemoteConfigInitializer : Initializer {

	// Privats
	float _firebaseRemoteConfigActivationTimeOut;

	public RemoteConfigInitializer(float firebaseRemoteConfigActivationTimeOut) {
		_firebaseRemoteConfigActivationTimeOut = firebaseRemoteConfigActivationTimeOut;
	}

	public override IEnumerator Initialize() {
#if USE_FIREBASE
		Debug.Log("RemoteConfigInitializer : Try Initialize Firebase Remote Config");
		yield return EnableRemoteConfigOrTimeOut();
#endif
		yield return null;
	}

	IEnumerator EnableRemoteConfigOrTimeOut() {
#if USE_FIREBASE
		Debug.Log($"RemoteConfigInitializer : EnableRemoteConfigOrTimeOut");
		bool remoteConfigActivationDidFinish = false;
		bool remoteConfigActivationSuccess = false;

		FirebaseManager.Instance.EnableRemoteConfig(Application.isEditor, (success) => {
			remoteConfigActivationDidFinish = true;
			remoteConfigActivationSuccess = success;
		});

		float beginTime = Time.time;
		float timePassed = 0;
		while (!remoteConfigActivationDidFinish) {
			timePassed = Time.time - beginTime;
			if (timePassed > _firebaseRemoteConfigActivationTimeOut)
				break;
			yield return null;
		}

		if (remoteConfigActivationSuccess)
			AnalyticsHelper.LogFirebaseRemoteConfigLoaded();
#endif
		yield return null;
	}
}
