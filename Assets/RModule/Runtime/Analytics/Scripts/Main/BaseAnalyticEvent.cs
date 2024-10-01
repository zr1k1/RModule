using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RModule.Runtime.Analytics {
	public class BaseAnalyticEvent<EventNameEnum, ParameterNameOfAnalyticEventEnum>
		where EventNameEnum : Enum where ParameterNameOfAnalyticEventEnum : Enum {

		protected SendAnalyticEventCommand<EventNameEnum, ParameterNameOfAnalyticEventEnum> CreateSendEventCommand(EventNameEnum eventNameEnum) {
			return new SendAnalyticEventCommand<EventNameEnum, ParameterNameOfAnalyticEventEnum>(eventNameEnum);
		}
	}
}
