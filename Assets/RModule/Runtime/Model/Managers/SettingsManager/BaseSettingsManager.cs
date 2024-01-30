using System;
using System.Collections;
using UnityEngine;

public abstract class BaseSettingsManager<PurchasableGameItem, Placement, OptionaAppConfigValue, OptionalSettingType, OptionaDebugValue, OptionalCrossPlatformAppConfigValue>
	: SingletonMonoBehaviour<BaseSettingsManager<PurchasableGameItem, Placement, OptionaAppConfigValue, OptionalSettingType, OptionaDebugValue, OptionalCrossPlatformAppConfigValue>>

	where PurchasableGameItem : Enum
	where Placement : Enum
	where OptionalSettingType : Enum
	where OptionaDebugValue : Enum
	where OptionaAppConfigValue : Enum 
	where OptionalCrossPlatformAppConfigValue : Enum {

	// Accessors
	public AppConfig<PurchasableGameItem, Placement, OptionaAppConfigValue, OptionalCrossPlatformAppConfigValue> AppConfigData => _appConfigData;
	public BaseDebugConfig<OptionaDebugValue> DebugConfig => _debugConfig;

	// Outlets
	[SerializeField] protected AppConfig<PurchasableGameItem, Placement, OptionaAppConfigValue, OptionalCrossPlatformAppConfigValue> _appConfigData = default;
	[SerializeField] protected BaseDebugConfig<OptionaDebugValue> _debugConfig = default;
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

		UpdateNubmerOfStars();
		UpdateLastPlayedDay();
		UpdatePlayedDaysInARowCount();
		LateInitialize();

		Application.targetFrameRate = _appConfigData.TargetFps;

		SetReady();

		yield return null;
	}

	public override bool IsInitialized() {
		return _isReady;
	}

	void UpdateNubmerOfStars() {
		SetValue(CommonSetting.NumberOfStarts, GetValue<int>(CommonSetting.NumberOfStarts) + 1);
	}

	void UpdateLastPlayedDay() {
		SetValue(CommonSetting.LastPlayedDay, DateTime.Now);
	}

	void UpdatePlayedDaysInARowCount() {
		if ((GetValue<DateTime>(CommonSetting.LastPlayedDay) - DateTime.Now).Days <= 1) {
			SetValue(CommonSetting.PlayedDaysInARowCount, GetValue<int>(CommonSetting.PlayedDaysInARowCount) + 1);
		}
	}

	protected abstract void LateInitialize();

	void SetReady() {
		_isReady = true;
	}

	// To sets Action to Unity Events on inspector in SettingDatas
	public virtual void SetEnableSounds(bool enable) {
		_soundsEnabler.SetEnableSounds(enable);
	}

	public virtual void SetEnableMusic(bool enable) {
		_musicEnabler.SetEnableMusic(enable);
	}

	public virtual void SetEnableVibration(bool enable) {
		_setEnableVibration?.Invoke(enable);
	}

	// Others
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

	public virtual T1 GetValue<T1>(OptionalCrossPlatformAppConfigValue setting) {
		return ((IValueGetter<OptionalCrossPlatformAppConfigValue>)_optionalSettings).GetValue<T1>(setting);
	}
}
