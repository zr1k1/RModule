using System;
using System.Collections.Generic;
using RModule.Runtime.Utils;
using UnityEngine;

public static class AnalyticsHelper {
	//public enum ReplayType { Pause, Finish, Die }
	//public enum TryGetSkinEventType { Coin, Rewarded, Skip, Show }

	//public static class Event {
	//	internal const string fbRemoteConfigLoaded = "fb_remote_config_loaded";
	//	internal const string FIRST_LAUNCH = "FIRST_LAUNCH";
	//	internal const string Lvl_start = "Lvl_start";
	//	internal const string Lvl_complete = "Lvl_complete";
	//	internal const string Lvl_replay_pause = "Lvl_replay_pause";
	//	internal const string Lvl_replay_finish = "Lvl_replay_finish";
	//	internal const string Lvl_replay_die = "Lvl_replay_die";
	//	internal const string skip_press = "skip_press";
	//	internal const string skip = "skip";
	//	internal const string skin_part_common = "skin_";
	//	internal const string skin_part_coin = "_coin";
	//	internal const string skin_part_rewarded = "_rewarded";
	//	internal const string skin_part_skip = "_skip";
	//	internal const string skin_part_show = "_show";
	//}

	//public static class Parameter {
	//	internal const string DAYS = "DAYS";
	//	internal const string level = "level";
	//	internal const string TRY = "try";
	//}

	//// Privats
	//static AnalyticsService s_analyticsService;

	//// Consts
	//const string k_first_launch = "k_first_launch";
	//const string k_level_start = "k_level_start_";
	//const string k_level_complete_tries = "k_level_complete_tries_";
	//const string k_first_launch_time = "k_first_launch_time";
	//const string k_number_of_skin_get_tries = "k_number_of_skin_get_tries";

	//// ---------------------------------------------------------------
	//// Initialization

	//public static void Initialize(AppPolicyService appPolicyService) {
	//	List<IAnalyticsSender> analyticsSenders = new List<IAnalyticsSender> {
	//		new AppMetricaSender()
	//	};
	//	if (SettingsManager.Instance.AppConfigData.EnableFirebaseInitialization)
	//		analyticsSenders.Add(new FirebaseAnalyticsSender(appPolicyService.UserAge, true));

	//	s_analyticsService = new AnalyticsService(new PlayerPrefsSaveService());
	//	s_analyticsService.Initialize(analyticsSenders);
	//}

	//// ---------------------------------------------------------------
	//// General Events

	//public static void LogFirebaseRemoteConfigLoaded() {
	//	SendEvent(Event.fbRemoteConfigLoaded);
	//}

	//public static void TryLogFirstLaunch() {
	//	if (!canLogEventOnlyOneTimeWithKey(k_first_launch))
	//		return;

	//	Debug.Log($"AnalyticsHelper : LogFirstLaunch");
	//	PlayerPrefsHelper.SetDateTime(k_first_launch_time, DateTime.Now);

	//	SendEvent($"{Event.FIRST_LAUNCH}");
	//}

	//public static void TryLogLevelStart(int levelNumber) {
	//	var prefsKey = $"{k_level_start}{levelNumber}";
	//	if (!canLogEventOnlyOneTimeWithKey(prefsKey))
	//		return;

	//	Debug.Log($"AnalyticsHelper : LogLevelStart {levelNumber}");

	//	var parameters = new Dictionary<string, string> {
	//		[Parameter.level] = $"{levelNumber}"
	//	};

	//	SendEvent($"{Event.Lvl_start}", parameters);
	//}

	//public static void TryLogLevelReplay(ReplayType replayType, int levelNumber) {
	//	Debug.Log($"AnalyticsHelper : TryLogLevelReplay {levelNumber}");

	//	var parameters = new Dictionary<string, string> {
	//		[Parameter.level] = $"{levelNumber}"
	//	};

	//	var eventName = $"{Event.Lvl_replay_pause}";
	//	if (replayType == ReplayType.Finish)
	//		eventName = $"{Event.Lvl_replay_finish}";
	//	else if (replayType == ReplayType.Die)
	//		eventName = $"{Event.Lvl_replay_die}";

	//	SendEvent(eventName, parameters);
	//}

	//public static void TryLogLevelCompleteTries(int levelNumber, int tries) {
	//	var prefsKey = $"{k_level_complete_tries}{levelNumber}";
	//	if (!canLogEventOnlyOneTimeWithKey(prefsKey))
	//		return;

	//	Debug.Log($"AnalyticsHelper : LogLevelCompleteTries {levelNumber} {tries}");

	//	var parameters = new Dictionary<string, string> {
	//		[Parameter.level] = $"{levelNumber}",
	//		[Parameter.TRY] = $"{tries}"
	//	};
	//	SendEvent($"{Event.Lvl_complete}", parameters);
	//}

	//public static void LogSkipPress(int levelNumber) {
	//	Debug.Log($"AnalyticsHelper : LogSkipPress {levelNumber}");

	//	var parameters = new Dictionary<string, string> {
	//		[Parameter.level] = $"{levelNumber}"
	//	};

	//	SendEvent($"{Event.skip_press}", parameters);
	//}

	//public static void LogSkip(int levelNumber) {
	//	Debug.Log($"AnalyticsHelper : LogSkip {levelNumber}");

	//	var parameters = new Dictionary<string, string> {
	//		[Parameter.level] = $"{levelNumber}"
	//	};

	//	SendEvent($"{Event.skip}", parameters);
	//}

	//public static void LogSkin(TryGetSkinEventType tryGetSkinEventType) {
		
	//	int numberOfSkinGetTries = PlayerPrefs.GetInt(k_number_of_skin_get_tries, 0);

	//	if (tryGetSkinEventType == TryGetSkinEventType.Show) {
	//		numberOfSkinGetTries++;
	//		PlayerPrefs.SetInt(k_number_of_skin_get_tries, numberOfSkinGetTries);
	//	}

	//	string combinedEventString = $"{Event.skin_part_common}{numberOfSkinGetTries}";
	//	if (tryGetSkinEventType == TryGetSkinEventType.Coin) {
	//		combinedEventString += Event.skin_part_coin;
	//	}else if (tryGetSkinEventType == TryGetSkinEventType.Rewarded) {
	//		combinedEventString += Event.skin_part_rewarded;
	//	} else if (tryGetSkinEventType == TryGetSkinEventType.Skip) {
	//		combinedEventString += Event.skin_part_skip;
	//	} else if (tryGetSkinEventType == TryGetSkinEventType.Show) {
	//		combinedEventString += Event.skin_part_show;
	//	}
	//	Debug.Log($"AnalyticsHelper : LogSkin {combinedEventString}");

	//	SendEvent(combinedEventString);
	//}

	//// Simplify understanding
	//static bool canLogEventOnlyOneTimeWithKey(string savedKeyInPlayerPrefs) {
	//	if (PlayerPrefsHelper.GetBool(savedKeyInPlayerPrefs))
	//		return false;
	//	PlayerPrefsHelper.SetBool(savedKeyInPlayerPrefs, true);

	//	return true;
	//}

	//// Sendings

	//static void SendEvent(string eventName) {
	//	s_analyticsService.SendEvent(eventName);
	//}

	//static void SendEvent(string eventName, Dictionary<string, string> parameters) {
	//	s_analyticsService.SendEvent(eventName, parameters);
	//}
}