using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Analytics;

public class ExampleLevelCompleteEvent : ExampleYourAppAnalyticEvent {

	public ExampleLevelCompleteEvent(int levelNumber, int tries) {
		CreateSendEventCommand(ExampleAnalyticEvent.LevelComplete)
			.SetParameterValue(ExampleParameterAnalyticEvent.N, levelNumber.ToString())
			.SetParameterValue(ExampleParameterAnalyticEvent.Tries, tries.ToString())
			.AddStringToName($"_{levelNumber}")
			.AddStringToPrefsKey($"_{levelNumber}")
			.TrySend();
	}
}