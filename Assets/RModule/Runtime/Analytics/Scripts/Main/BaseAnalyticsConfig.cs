using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RModule.Runtime.Analytics {
	public class BaseAnalyticsConfig<EventNameTypeEnum, ParameterNameAnalyticEvent> : ScriptableObject
		where EventNameTypeEnum : Enum where ParameterNameAnalyticEvent : Enum {

		public SerializableDictionary<EventNameTypeEnum, BaseAnalyticEventDataConfig<ParameterNameAnalyticEvent>> EventDatas => _eventDatas;

		[SerializeField] protected SerializableDictionary<EventNameTypeEnum, BaseAnalyticEventDataConfig<ParameterNameAnalyticEvent>> _eventDatas = default;
	}
}
