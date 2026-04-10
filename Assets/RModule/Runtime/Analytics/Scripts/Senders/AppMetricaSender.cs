
#if USE_APPMETRICA
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Imported.Analytics;
using MTUnityCore.Runtime.Managers.Ads;
using Io.AppMetrica;
using Newtonsoft.Json;
using RModule.Runtime.Analytics;
using MTUnityCore.Runtime.MiniJSON;

public class AppMetricaSender : IAnalyticsSender {
	readonly bool _trackAdRevenue;
	readonly string _apiKey;
	readonly bool _isFirstLaunch;
	readonly int _sessionTimeout;
	readonly bool _enableLogs;

	private AppMetricaSender(string apiKey, bool trackAdRevenue, bool isFirstLaunch, int sessionTimeout, bool enableLogs) {
		_trackAdRevenue = trackAdRevenue;
		_apiKey = apiKey;
		_isFirstLaunch = isFirstLaunch;
		_sessionTimeout = sessionTimeout;
		_enableLogs = enableLogs;
	}

	public static AppMetricaSender Create(string apiKey, bool trackAdRevenue, bool isFirstLaunch, int sessionTimeout = 10, bool enableLogs = false) {
		var sender = new AppMetricaSender(apiKey, trackAdRevenue, isFirstLaunch, sessionTimeout, enableLogs);
		sender.Activate();
		return sender;
	}

	void Activate() {
		if (_trackAdRevenue) {
			AdsManager.AdRevenueDidReceive += SendAdRevenueEvent;
		}

		AppMetrica.Activate(new AppMetricaConfig(_apiKey) {
			// copy settings from prefab
			CrashReporting = true, // prefab field 'Exceptions Reporting'
			SessionTimeout = _sessionTimeout, // prefab field 'Session Timeout Sec'
			LocationTracking = false, // prefab field 'Location Tracking'
			Logs = _enableLogs, // prefab field 'Logs'
			FirstActivationAsUpdate = !_isFirstLaunch, // prefab field 'Handle First Activation As Update'
			DataSendingEnabled = true, // prefab field 'Statistics Sending'
		});
	}

	public void Cleanup() {
		if (_trackAdRevenue) {
			AdsManager.AdRevenueDidReceive -= SendAdRevenueEvent;
		}
	}

	public void SendEvent(string eventName) {
		AppMetrica.ReportEvent(eventName);
	}

	public void SendEvent(string eventName, Dictionary<string, string> parameters) {
		string paramsJson = Json.Serialize(parameters);
		if (paramsJson != null) {
			AppMetrica.ReportEvent(eventName, paramsJson);
		} else {
			Debug.Log("Failed to serialize parameters for appMetrica");
		}
	}

	// ---------------------------------------------------------------
	// Ads

	public void SendAdRevenueEvent(AdRevenueData adRevenueData) {
		var adRevenue = new AdRevenue(adRevenueData.Revenue, adRevenueData.Currency);
		AppMetrica.ReportAdRevenue(adRevenue);
	}
}
#endif