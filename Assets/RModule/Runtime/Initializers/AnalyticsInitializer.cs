using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Services;

public class AnalyticsInitializer : Initializer {

	// Privats
	AppPolicyService _appPolicyService;

	public AnalyticsInitializer() {
	}

	public AnalyticsInitializer(AppPolicyService appPolicyService) {
		_appPolicyService = appPolicyService;
	}

	public override IEnumerator Initialize() {
#if !USE_YG
		Debug.Log("AnalyticsInitializer : Initialize Analytics");

		// Change to Rmodule Analytics
		//OldAnalyticsHelper.Initialize(new OldAnalyticsHelper.InputData {
		//	userAge = _appPolicyService.UserAge
		//	, isFirstLaunch = SettingsManager.Instance.NumberOfStarts == 1
		//	, senderInputDatas = new List<OldAnalyticsHelper.SenderInputData> {
		//		new FirebaseSenderInputData{
		//			trackAdRevenue = true },
		//		new AppMetricaSenderInputData{
		//			apiKey = SettingsManager.Instance.AppConfigData.Common.AppMetricaApiKey
		//			, enableLogs = false, sessionTimeout = 10, trackAdRevenue = true }
		//	}
		//});
#endif
		yield return null;
	}
}
