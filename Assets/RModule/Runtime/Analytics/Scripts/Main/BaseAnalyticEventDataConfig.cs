using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnalyticEventDataConfig<ParameterNameAnalyticEvent> : ScriptableObject {

	public AnalyticEventData<ParameterNameAnalyticEvent> AnalyticEventData => _analyticEventData;

	[SerializeField] protected AnalyticEventData<ParameterNameAnalyticEvent> _analyticEventData = default;
}
