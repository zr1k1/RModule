using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Analytics;

[CreateAssetMenu(fileName = "ExampleAnalyticsConfig", menuName = "RModule/Examples/AppConfigs/Analytics/ExampleAnalyticsConfig", order = 0)]
public class ExampleAnalyticsConfig : BaseAnalyticsConfig<ExampleAnalyticEvent, ExampleParameterAnalyticEvent> {
}
