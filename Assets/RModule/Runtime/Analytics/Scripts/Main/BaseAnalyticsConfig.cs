using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RModule.Runtime.Analytics {

	[Serializable]
	public class AppMetricaData {
		public string ApiKey => _apiKey;
		public bool TrackAdRevenue => _trackAdRevenue;
		public int SessionTimeout => _sessionTimeout;
		public bool EnableLogs => _enableLogs;

		[SerializeField] protected string _apiKey;
		[SerializeField] protected bool _trackAdRevenue;
		[SerializeField] protected int _sessionTimeout;
		[SerializeField] protected bool _enableLogs;
	}

	public class BaseAnalyticsConfig : ScriptableObject {
		public AppMetricaData AppMetricaData => _appMetricaData;

		[SerializeField] protected AppMetricaData _appMetricaData;
	}

	public class BaseAnalyticsConfig<EventNameTypeEnum, ParameterNameAnalyticEvent> : BaseAnalyticsConfig
		where EventNameTypeEnum : Enum where ParameterNameAnalyticEvent : Enum {

		public SerializableDictionary<EventNameTypeEnum, BaseAnalyticEventDataConfig<ParameterNameAnalyticEvent>> EventDatas => _eventDatas;

		[SerializeField] protected SerializableDictionary<EventNameTypeEnum, BaseAnalyticEventDataConfig<ParameterNameAnalyticEvent>> _eventDatas = default;
	}
}
