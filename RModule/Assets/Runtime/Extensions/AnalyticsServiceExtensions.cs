#if USE_MT_UNITY_CORE
using System.Collections.Generic;
using Imported.Analytics;

namespace RModule.Runtime.Extensions {
	public static class AnalyticsServiceExtensions {
		public static void SendEventWithLocaleSuffix(this AnalyticsService analyticsService, string eventName) {
			eventName += $"_{LocalizationManager.Instance.LanguageIdForCurrentLocale()}";
			analyticsService.SendEvent(eventName);
		}

		public static void SendEventWithLocaleSuffix(this AnalyticsService analyticsService, string eventName, Dictionary<string, string> parameters) {
			eventName += $"_{LocalizationManager.Instance.LanguageIdForCurrentLocale()}";
			analyticsService.SendEvent(eventName, parameters);
		}
	}
}
#endif
