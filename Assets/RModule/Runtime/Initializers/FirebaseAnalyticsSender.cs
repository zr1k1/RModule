// USE_FIREBASE must be defined in the csc.rsp file
#if USE_FIREBASE && USE_MTUNITYCORE

using System;
using System.Collections.Generic;
using Imported.Managers.MTFirebase;
using MTUnityCore.Runtime.Extensions;
using MTUnityCore.Runtime.Managers.Ads;
using Firebase.Analytics;
using UnityEngine;

namespace RModule.Runtime.Analytics {

	public class FirebaseAnalyticsSender : IAnalyticsSender {

		readonly bool _trackAdRevenue;

		// ---------------------------------------------------------------
		// Constructor

		public FirebaseAnalyticsSender(int age, bool trackAdRevenue) {
			_trackAdRevenue = trackAdRevenue;

			if (!FirebaseManager.Instance.FirebaseIsReady) {
				Debug.LogError("FirebaseManager is not initialized. Need to initialize it before using any of the firebase analytics stuff.");
				return;
			}

			string allow = age > 13 ? "true" : "false";
			FirebaseAnalytics.SetUserProperty("ALLOW_AD_PERSONALIZATION_SIGNALS", allow);

			if (_trackAdRevenue) {
				AdsManager.AdRevenueDidReceive += SendAdRevenueEvent;
			}
		}

		public void Cleanup() {
			if (_trackAdRevenue) {
				AdsManager.AdRevenueDidReceive -= SendAdRevenueEvent;
			}
		}

		// ---------------------------------------------------------------
		// IAnalyticsSender

		public void SendEvent(string eventName) {
			if (!FirebaseManager.Instance.FirebaseIsReady)
				return;

			FirebaseAnalytics.LogEvent(eventName);
		}

		public void SendEvent(string eventName, Dictionary<string, string> parameters) {
			if (!FirebaseManager.Instance.FirebaseIsReady)
				return;

			var firParameters = new List<Parameter>();
			foreach (var parameter in parameters) {
				var firParameter = new Parameter(parameter.Key, parameter.Value);
				firParameters.Add(firParameter);
			}

			FirebaseAnalytics.LogEvent(eventName, firParameters.ToArray());
		}

		// ---------------------------------------------------------------
		// Ads

		public void SendAdRevenueEvent(AdRevenueData adRevenueData) {
			var parameters = new[]{
					new Parameter(FirebaseAnalytics.ParameterAdFormat, adRevenueData.AdType),
					new Parameter(FirebaseAnalytics.ParameterAdSource, adRevenueData.NetworkName),
					new Parameter(FirebaseAnalytics.ParameterAdUnitName, adRevenueData.AdUnitName),
					new Parameter(FirebaseAnalytics.ParameterCurrency, adRevenueData.Currency),
					new Parameter(FirebaseAnalytics.ParameterValue, adRevenueData.Revenue)
			};

			FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, parameters);
		}
	}

}

#endif
