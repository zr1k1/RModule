using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Analytics;

public class TestAnalyticsSC : MonoBehaviour, IAnalyticsSender {
	[SerializeField] BaseAnalyticsConfig<ExampleAnalyticEvent, ExampleParameterAnalyticEvent> _config = default;

	void Start() {
		List<IAnalyticsSender> senders = new List<IAnalyticsSender> {
			this
		};
		Analytics<ExampleAnalyticEvent, ExampleParameterAnalyticEvent>.Init(new Analytics<ExampleAnalyticEvent, ExampleParameterAnalyticEvent>.InputData(_config, senders, 0));

		new ExampleFirstLaunchEvent();
		new ExampleLevelCompleteEvent(1, 3);
	}

	public void SendEvent(string eventName) {
		Debug.Log($"TestAnalyticsSC : SendEvent {eventName}");
	}

	public void SendEvent(string eventName, Dictionary<string, string> parameters) {
		Debug.Log($"TestAnalyticsSC : SendEvent with params {eventName}");
	}
}
