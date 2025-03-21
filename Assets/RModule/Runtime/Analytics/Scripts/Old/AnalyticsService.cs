using System.Collections.Generic;
using RModule.Runtime.Services;
using UnityEngine;

public class AnalyticsService {
	readonly ISaveService _saveService;
	readonly List<IAnalyticsSender> _senders = new List<IAnalyticsSender>();

	public AnalyticsService(ISaveService saveService) {
		_saveService = saveService;
	}

	// ---------------------------------------------------------------
	// Initialization

	public void Initialize(List<IAnalyticsSender> analyticsSenders) {
		_senders.AddRange(analyticsSenders);
	}

	// --------------------------------------------------------------------------
	// Events

	public void SendEvent(string eventName, bool oneTime = false) {
		SendEvent(eventName, null, oneTime);
	}

	public void SendEvent(string eventName, Dictionary<string, string> parameters, bool oneTime = false) {
		if (eventName?.Length == 0) return;
		if (_senders.Count == 0) {
			Debug.LogError("Analytics service is not initialized. There are no senders.");
			return;
		}

		if (oneTime) {
			if (OneTimeEventAlreadySent(eventName)) {
				return;
			}
		}

		foreach (var sender in _senders) {
			if (parameters == null) {
				sender.SendEvent(eventName);
			} else {
				sender.SendEvent(eventName, parameters);
			}
		}

		if (oneTime) {
			_saveService.SetValue(eventName, true);
		}
	}

	public bool OneTimeEventAlreadySent(string eventName) {
		return _saveService.GetValue(eventName, false);
	}
}
