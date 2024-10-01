using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AnalyticEventData<ParameterNameAnalyticEvent> {
	public string Name => _name;
	public string PrefsKey => _prefsKey;
	public bool IsOneTimeSend => _isOneTimeSend;
	public List<ParameterNameAnalyticEvent> ParametersToUse => _parametersToUse;

	// Outlets
	[SerializeField] string _name = default;
	[SerializeField] string _prefsKey = default;
	[SerializeField] bool _isOneTimeSend = default;
	[SerializeField] List<ParameterNameAnalyticEvent> _parametersToUse = default;
}
