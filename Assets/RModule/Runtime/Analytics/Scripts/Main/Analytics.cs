using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RModule.Runtime.Analytics {

	public static class Analytics<EventNameEnum, ParameterNameOfAnalyticEventEnum>
		where EventNameEnum : Enum where ParameterNameOfAnalyticEventEnum : Enum {

		static List<IAnalyticsSender> s_analyticsSenders = new List<IAnalyticsSender>();
		static BaseAnalyticsConfig<EventNameEnum, ParameterNameOfAnalyticEventEnum> s_analyticsEventsConfig;

		public interface IAnalyticsInputDataContainer {
			public BaseAnalyticsConfig<EventNameEnum, ParameterNameOfAnalyticEventEnum> AnalyticsConfig { get; }
			public IAnalyticsSender AnalyticsSender { get; }
			public int Age { get; }
		}

		public class InputData {

			public BaseAnalyticsConfig<EventNameEnum, ParameterNameOfAnalyticEventEnum> AnalyticsConfig { get; private set; }
			public List<IAnalyticsSender> AnalyticsSenders { get; private set; }
			public int Age { get; private set; }

			public InputData(BaseAnalyticsConfig<EventNameEnum, ParameterNameOfAnalyticEventEnum> analyticsConfig, List<IAnalyticsSender> analyticsSenders, int age) {
				AnalyticsConfig = analyticsConfig;
				AnalyticsSenders = analyticsSenders;
				Age = age;
			}
		}

		public static void Init(InputData inputData) {
			s_analyticsSenders.AddRange(inputData.AnalyticsSenders);
			s_analyticsEventsConfig = inputData.AnalyticsConfig;
		}

		public static bool TryGetEventData(EventNameEnum eventNameEnum, out AnalyticEventData<ParameterNameOfAnalyticEventEnum> analyticEventData) {
			if (s_analyticsEventsConfig.EventDatas.ContainsKey(eventNameEnum)) {
				analyticEventData = s_analyticsEventsConfig.EventDatas[eventNameEnum].AnalyticEventData;
				return true;
			} else {
				Debug.LogError($"Event {eventNameEnum} is not present on BaseAnalyticsConfig dictionary!");
				analyticEventData = new AnalyticEventData<ParameterNameOfAnalyticEventEnum>();

				return false;
			}
		}

		public static void Send(SendAnalyticEventCommand<EventNameEnum, ParameterNameOfAnalyticEventEnum> sendAnalyticEventCommand) {
			foreach (var analyticsSender in s_analyticsSenders) {
				if (sendAnalyticEventCommand.Parameters.Count > 0)
					analyticsSender.SendEvent(sendAnalyticEventCommand.Name, sendAnalyticEventCommand.Parameters);
				else
					analyticsSender.SendEvent(sendAnalyticEventCommand.Name);
			}
		}
	}
}
