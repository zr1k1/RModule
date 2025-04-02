using System;
using System.Collections;
using UnityEngine;
#if USE_FIREBASE && USE_MTUNITYCORE
using Imported.Managers.MTFirebase;
#endif

public class RemoteConfigInitializer : Initializer {

	// Privats
	float _firebaseRemoteConfigActivationTimeOut;
	Action<bool> _initializationFinishCallback;

	public RemoteConfigInitializer(float firebaseRemoteConfigActivationTimeOut, Action<bool> initializationFinishCallback) {
		_firebaseRemoteConfigActivationTimeOut = firebaseRemoteConfigActivationTimeOut;
		_initializationFinishCallback = initializationFinishCallback;
	}

	public override IEnumerator Initialize() {
#if USE_FIREBASE && USE_MTUNITYCORE
		Debug.Log("RemoteConfigInitializer : Try Initialize Firebase Remote Config");
		yield return EnableRemoteConfigOrTimeOut();
#endif
		yield return null;
	}

	//protected override void OnInitializationFinished() {
	//	OnInitializationFinishedCallback(_remoteConfigActivationSuccess);
	//}

	IEnumerator EnableRemoteConfigOrTimeOut() {
#if USE_FIREBASE && USE_MTUNITYCORE
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

		_initializationFinishCallback?.Invoke(remoteConfigActivationSuccess);
#endif
		yield return null;
	}
}
