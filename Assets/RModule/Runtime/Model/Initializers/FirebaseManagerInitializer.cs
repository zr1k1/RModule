using System.Collections;
using UnityEngine;
#if USE_FIREBASE
using Imported.Managers.MTFirebase;
using MTUnityCore.Runtime.Extensions;
#endif

public class FirebaseManagerInitializer : Initializer {

	public override IEnumerator Initialize() {
#if USE_FIREBASE
		Debug.Log("FirebaseManagerInitializer : TryInitializeFirebaseManager");
		if (SettingsManager.Instance.AppConfigData.Common.EnableFirebaseInitialization)
			yield return FirebaseManager.Instance.InitializeAsync().AsIEnumerator();
#endif
		yield return null;
	}
}
