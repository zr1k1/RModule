using System.Collections;
using UnityEngine;

#if USE_FIREBASE && USE_MTUNITYCORE
using Imported.Managers.MTFirebase;
using MTUnityCore.Runtime.Extensions;
#endif

public class FirebaseManagerInitializer : Initializer {
	bool _enableFirebaseInitialization;

	public FirebaseManagerInitializer(bool enableFirebaseInitialization) {
		_enableFirebaseInitialization = enableFirebaseInitialization;
	}

	public override IEnumerator Initialize() {
#if USE_FIREBASE && USE_MTUNITYCORE
		Debug.Log("FirebaseManagerInitializer : TryInitializeFirebaseManager");
		if(_enableFirebaseInitialization)
			yield return FirebaseManager.Instance.InitializeAsync().AsIEnumerator();
#endif
		yield return null;
	}
}
