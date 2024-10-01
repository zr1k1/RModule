using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Analytics;

public class ExampleFirstLaunchEvent : ExampleYourAppAnalyticEvent {
	public ExampleFirstLaunchEvent() {
		CreateSendEventCommand(ExampleAnalyticEvent.FirstLaunch).TrySend();
	}
}
