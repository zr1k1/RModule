using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnalyticsSender {
	void SendEvent(string eventName);
	void SendEvent(string eventName, Dictionary<string, string> parameters);
}
