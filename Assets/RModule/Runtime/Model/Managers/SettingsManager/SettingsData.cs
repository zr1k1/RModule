using System;
using RModule.Runtime.Services;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class SettingsData<OptionalSettingEnum> : IValueSetter<OptionalSettingEnum>, IValueGetter<OptionalSettingEnum> where OptionalSettingEnum : Enum {
	// Outlets
	[SerializeField] protected SerializableDictionary<OptionalSettingEnum, ValueConfigWithEvent> _settingsDict = default;

	// Privats
	protected PlayerPrefsSaveService _playerPrefsSaveService = new PlayerPrefsSaveService();

	// Classes
	[Serializable]
	public class ValueConfigWithEvent {
		public UnityEvent<object> ValueDidChange = default;

		public BaseValueConfig SettingConfig => _settingConfig;

		[SerializeField] BaseValueConfig _settingConfig = default;
	}

	// Consts
	const string c_save_key_prefix = "aleisa_rmodule_";

	public virtual void SetValue<T1>(OptionalSettingEnum setting, T1 value) {
		if (!_settingsDict.ContainsKey(setting)) {
			Debug.LogError($"Setting {setting} is not present on dictionary _settingsDict");
			return;
		}

		((IKeyValueSetter<string, T1>)_playerPrefsSaveService).SetValue($"{c_save_key_prefix}{setting}", value);

		_settingsDict[setting].ValueDidChange?.Invoke(value);
	}

	public virtual T1 GetValue<T1>(OptionalSettingEnum setting) {
		if (!_settingsDict.ContainsKey(setting)) {
			Debug.LogError($"Setting {setting} is not present on dictionary _settingsDict");
			return default(T1);
		}
		var value = _settingsDict[setting].SettingConfig.GetValue<object>();
		return (T1)value;
	}

	public void AddValueChangedListener(OptionalSettingEnum setting, UnityAction<object> action) {
		_settingsDict[setting].ValueDidChange.AddListener(action);
	}
}