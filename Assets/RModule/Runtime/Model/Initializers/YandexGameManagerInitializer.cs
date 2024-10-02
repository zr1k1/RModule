using System.Collections;
using UnityEngine;

public class YandexGameManagerInitializer : Initializer {
	public override IEnumerator Initialize() {
#if USE_YG
		Debug.Log("YandexGameManagerInitializer : InitializeYandexGameManager");
		yield return YandexGameManager.WaitForInitialized();
#endif
		yield return null;
	}
}
