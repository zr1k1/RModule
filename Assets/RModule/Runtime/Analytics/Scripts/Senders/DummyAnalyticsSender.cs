using UnityEngine;
using RModule.Runtime.Analytics;
using System.Collections.Generic;

public class DummyAnalyticsSender : IAnalyticsSender {
    public void SendEvent(string eventName) {
        Debug.Log($"DummyAnalyticsSender : SendEvent {eventName}");
    }

    public void SendEvent(string eventName, Dictionary<string, string> parameters) {
        Debug.Log($"DummyAnalyticsSender : SendEvent {eventName} parameters.Count {parameters.Count}");
    }
}
