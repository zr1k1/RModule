using System;
using System.Collections.Generic;
using System.Linq;
using RModule.Runtime.Utils;
using UnityEngine;
using RModule.Runtime.Services;

public class AppMetricaSenderInputData : OldAnalyticsHelper.SenderInputData {
	public bool trackAdRevenue;
	public string apiKey;
	public int sessionTimeout;
	public bool enableLogs;
}

public class FirebaseSenderInputData : OldAnalyticsHelper.SenderInputData {
	public bool trackAdRevenue;
}

public static class OldAnalyticsHelper {

	public static class Event {
		internal const string fbRemoteConfigLoaded = "fb_remote_config_loaded";
		internal const string FIRST_LAUNCH = "FIRST_LAUNCH";
		internal const string LEVEL_START = "LEVEL_START";
		internal const string LEVEL_COMPLETE = "LEVEL_COMPLETE";
		internal const string SKIP_LEVEL = "SKIP_LEVEL";
		internal const string LIKE_LEVEL = "LIKE_LEVEL";
		internal const string PLAYER_SAW_PUZZLE_RULE = "PLAYER_SAW_PUZZLE_RULE";
		internal const string GAME_END_TIME_IN_DAYS = "GAME_END_TIME_IN_DAYS";
	}

	public static class Parameter {
		internal const string TOTAL_COMPLETE = "TOTAL_COMPLETE";
		internal const string TYPE = "TYPE";
		internal const string N = "N";// index In The List Of Logic Game Type
		internal const string DAYS = "DAYS";
	}

	//Privats
	static InputData s_inputData;
	static AnalyticsService s_analyticsService;

	// Consts
	const string k_first_launch = "k_first_launch";
	const string k_level_start = "k_level_start_";
	const string k_level_complete = "k_level_complete_";
	const string k_level_skip = "k_level_skip_";
	const string k_first_launch_time = "k_first_launch_time";
	const string k_end_game_time = "k_end_game_time";

	// Classes
	public class InputData {
		public int userAge;
		public bool isFirstLaunch;
		public List<SenderInputData> senderInputDatas = new List<SenderInputData>();

		public bool TryGetSenderInputData<TSenderInputData>(out TSenderInputData foundedSenderInputData) where TSenderInputData : SenderInputData {
			foundedSenderInputData = (TSenderInputData)senderInputDatas.Find(senderInputData => senderInputData is TSenderInputData);

			return foundedSenderInputData != null;
		}
	}

	public class SenderInputData {
	}

	// ---------------------------------------------------------------
	// Initialization

	public static void Initialize(InputData inputData) {
		s_inputData = inputData;
		s_analyticsService = new AnalyticsService(new PlayerPrefsSaveService());
		List<IAnalyticsSender> _senders = new List<IAnalyticsSender>();

		// Setup analytics senders
#if USE_FIREBASE
		if (SettingsManager.Instance.AppConfigData.Common.EnableFirebaseInitialization) {
			if (s_inputData.TryGetSenderInputData<FirebaseSenderInputData>(out var firebaseSenderInputData)) {
				_senders.Add(new FirebaseAnalyticsSender(s_inputData.userAge, firebaseSenderInputData.trackAdRevenue));
			} else {
				Debug.LogError($"FirebaseSenderInputData not exist in s_senderInputDatas list!");
			}
		}
#endif

#if USE_APPMETRICA
		if (s_inputData.TryGetSenderInputData<AppMetricaSenderInputData>(out var appMetricaSenderInputData)) {
			_senders.Add(AppMetricaSender.Create(appMetricaSenderInputData.apiKey
				, appMetricaSenderInputData.trackAdRevenue, s_inputData.isFirstLaunch
				, appMetricaSenderInputData.sessionTimeout, appMetricaSenderInputData.enableLogs)
			);
		} else {
			Debug.LogError($"AppMetricaSenderInputData not exist in s_senderInputDatas list!");
		}
#endif
		s_analyticsService.Initialize(_senders);
	}

	// ---------------------------------------------------------------
	// General Events

	public static void LogFirebaseRemoteConfigLoaded() {
		SendEvent(Event.fbRemoteConfigLoaded);
	}

	//public static void TryLogLikeLevel(LogicGameType logicGameType, int totalLevelsComplete) {
	//	if (PlayerPrefsHelper.GetBool($"{Event.LIKE_LEVEL}{totalLevelsComplete}", false))
	//		return;

	//	PlayerPrefsHelper.SetBool($"{Event.LIKE_LEVEL}{totalLevelsComplete}", true);

	//	var parameters = new Dictionary<string, string> {
	//		[Parameter.TOTAL_COMPLETE] = $"{totalLevelsComplete}",
	//		[Parameter.TYPE] = $"{logicGameType}"
	//	};

	//	Debug.Log($"AnalyticsHelper : LogLikeLevel {totalLevelsComplete}");

	//	SendEvent($"{Event.LIKE_LEVEL}", parameters);

	//}

	public static void TryLogFirstLaunch() {
		if (!canLogEventOnlyOneTimeWithKey(k_first_launch))
			return;

		Debug.Log($"AnalyticsHelper : LogFirstLaunch");
		PlayerPrefsHelper.SetDateTime(k_first_launch_time, DateTime.Now);

		SendEvent($"{Event.FIRST_LAUNCH}");
	}

	//public static void TryLogLevelStart(LevelData levelData, int totalLevelsComplete) {
	//	var prefsKey = $"{k_level_start}{totalLevelsComplete}";
	//	if (!canLogEventOnlyOneTimeWithKey(prefsKey))
	//		return;

	//	var parameters = new Dictionary<string, string> {
	//		[Parameter.TOTAL_COMPLETE] = $"{totalLevelsComplete}",
	//		[Parameter.TYPE] = $"{levelData.LogicGameType}",
	//		[Parameter.N] = $"{levelData.IndexInTheListOfThisTypeOfGame}"
	//	};

	//	Debug.Log($"AnalyticsHelper : LogLevelStart {totalLevelsComplete}");

	//	SendEvent($"{Event.LEVEL_START}", parameters);

	//}

	//public static void TryLogLevelComplete(LevelData levelData, int totalLevelsComplete) {
	//	var prefsKey = $"{k_level_complete}{totalLevelsComplete}";
	//	if (!canLogEventOnlyOneTimeWithKey(prefsKey))
	//		return;

	//	var parameters = new Dictionary<string, string> {
	//		[Parameter.TOTAL_COMPLETE] = $"{totalLevelsComplete}",
	//		[Parameter.TYPE] = $"{levelData.LogicGameType}",
	//		[Parameter.N] = $"{levelData.IndexInTheListOfThisTypeOfGame}"
	//	};

	//	Debug.Log($"AnalyticsHelper : LogLevelComplete {totalLevelsComplete}");

	//	SendEvent($"{Event.LEVEL_COMPLETE}", parameters);

	//}

	//public static void TryLogLevelSkip(LogicGameType logicGameType, int totalLevelsComplete) {
	//	var prefsKey = $"{k_level_skip}{totalLevelsComplete}";
	//	if (!canLogEventOnlyOneTimeWithKey(prefsKey))
	//		return;

	//	var parameters = new Dictionary<string, string> {
	//		[Parameter.TOTAL_COMPLETE] = $"{totalLevelsComplete}",
	//		[Parameter.TYPE] = $"{logicGameType}"
	//	};

	//	Debug.Log($"AnalyticsHelper : LogLevelSkip {totalLevelsComplete}");

	//	SendEvent($"{Event.SKIP_LEVEL}", parameters);

	//}

	//public static void LogPlayerSawPuzzleRule(LogicGameType logicGameType) {
	//	Debug.Log($"AnalyticsHelper : LogPlayerSawPuzzleRule {logicGameType}");
	//	var parameters = new Dictionary<string, string> {
	//		[Parameter.TYPE] = $"{logicGameType}"
	//	};

	//	SendEvent($"{Event.PLAYER_SAW_PUZZLE_RULE}", parameters);
	//}

	public static void TryLogGameEndTimeInDays(bool testInMinuts) {
		Debug.Log($"AnalyticsHelper : TryLogGameEndTimeInDays {DateTime.Now}");
		if (!canLogEventOnlyOneTimeWithKey(k_end_game_time))
			return;

		var remainingTime = DateTime.Now - PlayerPrefsHelper.GetDateTime(k_first_launch_time, DateTime.Now);
		var days = testInMinuts && Application.isEditor ? remainingTime.TotalMinutes : remainingTime.TotalDays;
		Debug.Log($"AnalyticsHelper : days {days}");

		var parameters = new Dictionary<string, string> {
			[Parameter.DAYS] = $"{days.ToString("0.0")}"
		};

		SendEvent($"{Event.GAME_END_TIME_IN_DAYS}", parameters);
	}

	// Simplify understanding
	static bool canLogEventOnlyOneTimeWithKey(string savedKeyInPlayerPrefs) {
		if (PlayerPrefsHelper.GetBool(savedKeyInPlayerPrefs))
			return false;
		PlayerPrefsHelper.SetBool(savedKeyInPlayerPrefs, true);

		return true;
	}

	// Sendings

	static void SendEvent(string eventName) {
		s_analyticsService?.SendEvent(eventName);
	}

	static void SendEvent(string eventName, Dictionary<string, string> parameters) {
		s_analyticsService?.SendEvent(eventName, parameters);
	}

	//static void SendEventWithLocaleSuffix(string eventName) {
	//	s_analyticsService?.SendEventWithLocaleSuffix(eventName);
	//}

	//static void SendEventWithLocaleSuffix(string eventName, Dictionary<string, string> parameters) {
	//	s_analyticsService?.SendEventWithLocaleSuffix(eventName, parameters);
	//}
}