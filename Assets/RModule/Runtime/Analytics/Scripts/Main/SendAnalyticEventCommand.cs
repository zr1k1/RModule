using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RModule.Runtime.Analytics {
	public class SendAnalyticEventCommand<EventNameEnum, ParameterNameOfAnalyticEventEnum>
		where EventNameEnum : Enum where ParameterNameOfAnalyticEventEnum : Enum {

		// Accessors
		public string Name => _name;
		public string PrefsKey => _prefsKey;
		public bool IsOneTimeSend => _isOneTimeSend;
		public Dictionary<string, string> Parameters => _parameters;

		// Privats
		protected string _name;
		protected string _prefsKey;
		protected bool _isOneTimeSend;

		Dictionary<string, string> _parameters = new Dictionary<string, string>();

		public SendAnalyticEventCommand(EventNameEnum eventNameEnum) {
			if (!Analytics<EventNameEnum, ParameterNameOfAnalyticEventEnum>.TryGetEventData(eventNameEnum, out var analyticEventData)) {
				return;
			}

			_name = analyticEventData.Name;
			_prefsKey = analyticEventData.PrefsKey;
			_isOneTimeSend = analyticEventData.IsOneTimeSend;

			if (string.IsNullOrEmpty(_name))
				Debug.LogError($"Event name {Name} : Set name for event in analytic event data!");
			if (string.IsNullOrEmpty(_prefsKey))
				Debug.LogError($"Event name {Name} : Set prefsKey for event in analytic event data!");

			foreach (var parameterToUse in analyticEventData.ParametersToUse) {
				if (_parameters.ContainsKey(parameterToUse.ToString())) {
					Debug.LogError($"Event name {Name} : Parameter {parameterToUse} is already in dictionary! Remove duplicates in analytics config analyticEventData name = {analyticEventData.Name}");
				}
				_parameters.Add(parameterToUse.ToString(), string.Empty);
			}
		}

		public virtual bool TrySend() {
			if (IsCanSend()) {
				SaveSendedState();
				Analytics<EventNameEnum, ParameterNameOfAnalyticEventEnum>.Send(this);
				return true;
			}

			return false;
		}

		public SendAnalyticEventCommand<EventNameEnum, ParameterNameOfAnalyticEventEnum> AddStringToName(string str) {
			_name += str;

			return this;
		}

		public SendAnalyticEventCommand<EventNameEnum, ParameterNameOfAnalyticEventEnum> AddStringToPrefsKey(string str) {
			_prefsKey += str;

			return this;
		}

		public SendAnalyticEventCommand<EventNameEnum, ParameterNameOfAnalyticEventEnum> SetParameterValue(ParameterNameOfAnalyticEventEnum parameter, string value) {
			string key = parameter.ToString();
			if (_parameters.ContainsKey(key)) {
				_parameters[key] = value;
			} else {
				Debug.LogError($"Event name {Name} : Parameter {parameter} is not present on dictionary in config");
			}

			return this;
		}

		public virtual bool IsCanSend() {
			foreach (var parameter in _parameters) {
				if (string.IsNullOrEmpty(parameter.Value)) {
					Debug.LogError($"Event name {Name} : Value for Parameter {parameter} is not setupped! Use SetParameterValue for it!");
					return false;
				}
			}

			if (_isOneTimeSend && alreadySendedEarlier())
				return false;

			return true;
		}

		protected bool alreadySendedEarlier() {
			if (PlayerPrefs.GetInt(_prefsKey, 0) == 1) {
				Debug.Log($"Event name {Name} : already sended earlier");
				return true;
			}

			return false;
		}

		protected void SaveSendedState() {
			PlayerPrefs.SetInt(_prefsKey, 1);
		}
	}
}