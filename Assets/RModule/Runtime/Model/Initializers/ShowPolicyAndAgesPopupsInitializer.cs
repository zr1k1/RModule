using System.Collections;
#if USE_MTUNITY
using MTUnityCore.Runtime.Popups;
using MTUnityCore.Runtime.Services;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowPolicyAndAgesPopupsInitializer : Initializer {

	public ShowPolicyAndAgesPopupsInitializer() {
	}

	public override IEnumerator Initialize() {
#if USE_MTUNITY && !USE_YG
		Debug.Log("ShowPolicyAndAgesPopupsInitializer : TryShowPolicyAndAgesPopups");
		_appPolicyService = new AppPolicyService(new PlayerPrefsSaveService());
		_appPolicyService.Initialize();

		if (SceneManager.GetActiveScene().name != "SplashScene")
			yield break;

		yield return TryShowPolicyPopup();
		yield return TryShowAgePopup();
#endif
		yield return null;
	}

#if USE_MTUNITY
	// Privats
	AppPolicyService _appPolicyService;
	PopupsController _popupsController;

	public ShowPolicyAndAgesPopupsInitializer(PopupsController popupsController) {
		_popupsController = popupsController;
	}



	IEnumerator TryShowPolicyPopup() {
		if (_appPolicyService.AppPolicyAccepted)
			yield break;

		var _appConfigData = SettingsManager.Instance.AppConfigData;
		_popupsController.Create<CustomPrivacyPolicyPopupVC>().Setup(
			_appConfigData.Common.AppPolicyLink,
			_appConfigData.Common.AppTermsLink,
			_appPolicyService.AcceptAppPolicy).Show();

		while (!_appPolicyService.AppPolicyAccepted) {
			yield return null;
		}
	}

	IEnumerator TryShowAgePopup() {
		if (_appPolicyService.UserHasProvidedAge)
			yield break;

		_popupsController.Create<AgePopupVC>().Setup(age => {
			_appPolicyService.SetUserAge(age);
			_appPolicyService.SetAdsConsent(true);
		}).Show();

		while (!_appPolicyService.UserHasProvidedAge) {
			yield return null;
		}
	}

#endif
}
