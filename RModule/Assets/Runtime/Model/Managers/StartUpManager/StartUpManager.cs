using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpManager : SingletonMonoBehaviour<StartUpManager> {
	bool _isInitialized;

	public IEnumerator Initialize() {
		Debug.Log("StartupManager Initialize");

		//LeanTween.init(maxSimultaneousTweens);

		//foreach (var prefab in _managersPrefabs)
		//	Instantiate(prefab);

		//yield return LocalizationManager.Instance.Initialize();
		//SomeManagersDidReady?.Invoke(1);

		//yield return SettingsManager.Instance.Initialize();
		//SomeManagersDidReady?.Invoke(2);

		//yield return GameDataManager.Instance.Initialize();
		//SomeManagersDidReady?.Invoke(3);

		//yield return GraphicKitsManager.Instance.Initialize();
		//SomeManagersDidReady?.Invoke(4);

		//yield return IAPManager.Instance.InitializeAsync(GameDataManager.Instance.StoreProductsProvider).AsIEnumerator();

		//SomeManagersDidReady?.Invoke(5);

		//_appPolicyService = new AppPolicyService(new PlayerPrefsSaveService());
		//_appPolicyService.Initialize();

		//if (SceneManager.GetActiveScene().name == ScenesHelper.SceneType.SplashScene.ToString()) {
		//	if (!_appPolicyService.AppPolicyAccepted)
		//		yield return ShowPolicyPopup();
		//	//if (!AppPolicyManager.Instance.UserHasProvidedAge())
		//	//    yield return ShowAgePopup();
		//}

		//// For iOS we need to ask for app tracking consent with native sdk
		//yield return AppTrackingConsentManager.AskForAppTrackingConsent();
		//SomeManagersDidReady?.Invoke(6);

		////yield return IAPIsInitializedOrTimeOut();
		//SomeManagersDidReady?.Invoke(7);

		//yield return InitConsentDependentManagers();
		//SomeManagersDidReady?.Invoke(ManagersCount);

		//SoundsManager.Instance.PlayImproveMusic(SettingsManager.Instance.AppConfigData.MusicConfigs[0]);

		//_isInitialized = true;
		//Debug.Log("StartupManager Initialized");

		//AnalyticsHelper.TryLogFirstLaunch();

		//if (_sceneToOpen != ScenesHelper.SceneType.None)
		//	ScenesHelper.Open(_sceneToOpen);

		//todo remove
		yield return null;
	}

	public override bool IsInitialized() {
		return _isInitialized;
	}
	IEnumerator InitConsentDependentManagers() {
		//if (SettingsManager.Instance.AppConfigData.EnableFirebaseInitialization)
		//	yield return FirebaseManager.Instance.InitializeAsync().AsIEnumerator();

		//Debug.Log("Instantiate _consentDependentManagersPrefabs");
		//foreach (var prefab in _consentDependentManagersPrefabs) {
		//	Debug.Log($"Instantiate {prefab.name}");
		//	Instantiate(prefab);
		//}

		//AnalyticsHelper.Initialize(_appPolicyService);
		//Debug.Log($"AdsManager InitializeAdNetworks");
		//// if age 0 then appodeal test mode not work and not initializing
		//GameAdsManager.Init(_appPolicyService.HasTargetedAdsConsent);

		//if (SettingsManager.Instance.AppConfigData.EnableRemoteConfig)
		//	yield return EnableRemoteConfigOrTimeOut();

		//CheckForCanceledPurchasesAndEnableBackAdsIfCanceledPurchaseIsRemoveAdsProduct();

		//GameDataManager.Instance.RestoreNonConsumablesFromPlayerData();
		yield return null;
	}

	IEnumerator EnableRemoteConfigOrTimeOut() {
		//Debug.Log($"EnableRemoteConfigOrTimeOut");
		//bool remoteConfigActivationDidFinish = false;
		//bool remoteConfigActivationSuccess = false;
		//FirebaseManager.Instance.EnableRemoteConfig(Application.isEditor, (success) => {
		//	remoteConfigActivationDidFinish = true;
		//	remoteConfigActivationSuccess = success;
		//});

		//float beginTime = Time.time;
		//float timePassed = 0;
		//while (!remoteConfigActivationDidFinish) {
		//	timePassed = Time.time - beginTime;
		//	if (timePassed > _firebaseRemoteConfigActivationTimeOut)
		//		break;
			yield return null;
		//}

		//if (remoteConfigActivationSuccess)
		//	AnalyticsHelper.LogFirebaseRemoteConfigLoaded();
	}

	void CheckForCanceledPurchasesAndEnableBackAdsIfCanceledPurchaseIsRemoveAdsProduct() {
		//Debug.Log($"CheckForCanceledPurchasesAndEnableBackAdsIfCanceledPurchaseIsRemoveAdsProduct");
		////If user cancel purchase product we need to remove it from playerData
		////If user cancel purchase product with disable ads we need to check it and enable ads back
		//bool isHaveDisableAdsPurchasedProduct = false;
		//foreach (var product in GameDataManager.Instance.StoreProductsProvider.StoreProductsDatabase.AllStoreProducts) {
		//	if (!IAPManager.Instance.ProductIsPurchased(product.ProductId) && GameDataManager.Instance.PlayerData.ProductIsPurchased(product.ProductId))
		//		GameDataManager.Instance.PlayerData.RemoveProductKeyToPurchased(product.ProductId);
		//	if (product.IsRemoveAdsProduct && IAPManager.Instance.ProductIsPurchased(product.ProductId)) {
		//		isHaveDisableAdsPurchasedProduct = true;
		//	}
		//}
		//if (!isHaveDisableAdsPurchasedProduct) {
		//	GameDataManager.Instance.PlayerData.SetEnableAds(true);
		//	GameAdsManager.DisableAds(false);
		//}
	}

	// ---------------------------------------------------------------
	// Popups

	IEnumerator ShowPolicyPopup() {
		//var _appConfigData = SettingsManager.Instance.AppConfigData;
		//_popupsController.Create<CustomPrivacyPolicyPopupVC>().Setup(
		//	_appConfigData.AppPolicyLink,
		//	_appConfigData.AppTermsLink,
		//	_appPolicyService.AcceptAppPolicy).Show();

		//while (!_appPolicyService.AppPolicyAccepted) {
			yield return null;
		//}
	}

	IEnumerator ShowAgePopup() {
		//_popupsController.Create<AgePopupVC>().Setup(age => {
		//	_appPolicyService.SetUserAge(age);
		//	_appPolicyService.SetAdsConsent(true);
		//}).Show();

		//while (!_appPolicyService.UserHasProvidedAge) {
			yield return null;
		//}
	}
}
