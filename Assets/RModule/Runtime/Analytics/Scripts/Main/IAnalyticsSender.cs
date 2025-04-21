using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RModule.Runtime.Analytics {
	public interface IAnalyticsSender {
		void SendEvent(string eventName);
		void SendEvent(string eventName, Dictionary<string, string> parameters);
	}
}
