using System;
using System.Collections;
using UnityEngine;

[Serializable] public class LangStringDictionary : SerializableDictionary<SystemLanguage, string> { }
[Serializable] public class SettingsDictionary<T0, T1> : SerializableDictionary<T0, T1> { }

public class BaseSettingsManager<PurchasableGameItem, Placement, OptionaAppConfigValue, OptionalSettingType, OptionaDebugValue>
	: SingletonMonoBehaviour<BaseSettingsManager<PurchasableGameItem, Placement, OptionaAppConfigValue, OptionalSettingType, OptionaDebugValue>>

	where OptionalSettingType : Enum
	where OptionaDebugValue : Enum
	where OptionaAppConfigValue : Enum {

	// Accessors
	public AppConfig<PurchasableGameItem, Placement, OptionaAppConfigValue> AppConfigData => _appConfigData;
	public DebugConfig<OptionaDebugValue> DebugConfig => _debugConfig;

	// Outlets
	[SerializeField] protected AppConfig<PurchasableGameItem, Placement, OptionaAppConfigValue> _appConfigData = default;
	[SerializeField] protected DebugConfig<OptionaDebugValue> _debugConfig = default;
	[SerializeField] protected LangStringDictionary _databaseNames = new LangStringDictionary();

	// Settings
	[SerializeField] protected SettingsData<CommonSetting> _commonSettings = default;
	[SerializeField] protected SettingsData<OptionalSettingType> _optionalSettings = default;

	// Private vars
	protected ISoundsEnabler _soundsEnabler;
	protected IMusicEnabler _musicEnabler;
	protected Action<bool> _setEnableVibration;
	protected bool _isReady;

	//--------------------------------------------------------------------------
	// IInitializable

	public virtual IEnumerator Initialize(ISoundsPlayerService soundsPlayerService, Action<bool> setEnableVibration) {
		_soundsEnabler = soundsPlayerService;
		_musicEnabler = soundsPlayerService;
		_setEnableVibration = setEnableVibration;

		Application.targetFrameRate = _appConfigData.TargetFps;

		_isReady = true;
		yield return null;
	}

	public override bool IsInitialized() {
		return _isReady;
	}

	// To sets Action to Unity Events on inspector in SettingDatas
	public virtual void SetEnableSounds(bool enable) {
		_soundsEnabler.SetSoundEnabled(enable);
	}

	public virtual void SetEnableMusic(bool enable) {
		_musicEnabler.SetMusicEnabled(enable);
	}

	public virtual void SetEnableVibration(bool enable) {
		_setEnableVibration?.Invoke(enable);
	}

	// Others
	public string GetStoreLink() {
		return _appConfigData.GetStorelink();
	}

	public string GetSupportEmailString() {
		string aboutString = GetAboutDeviceString();
		aboutString = Uri.EscapeDataString(aboutString);
		string emailString = $"mailto:{_appConfigData.ContactEmail}?subject={aboutString}";
		return emailString;
	}

	public static string GetAboutDeviceString() {
		string osVersion = SystemInfo.operatingSystem;
		string deviceModel = SystemInfo.deviceModel;
		string gameTitle = Application.productName;
		string appVersion = Application.version;
		string aboutString = $"{gameTitle} ({appVersion}, {osVersion}, {deviceModel})";

		return aboutString;
	}

	public virtual void SetValue<T1>(CommonSetting setting, T1 value) {
		((IValueSetter<CommonSetting>)_commonSettings).SetValue(setting, value);
	}

	public virtual void SetValue<T1>(OptionalSettingType setting, T1 value) {
		((IValueSetter<OptionalSettingType>)_optionalSettings).SetValue(setting, value);
	}

	public virtual T1 GetValue<T1>(CommonSetting setting) {
		return ((IValueGetter<CommonSetting>)_commonSettings).GetValue<T1>(setting);
	}

	public virtual T1 GetValue<T1>(OptionalSettingType setting) {
		return ((IValueGetter<OptionalSettingType>)_optionalSettings).GetValue<T1>(setting);
	}
}
