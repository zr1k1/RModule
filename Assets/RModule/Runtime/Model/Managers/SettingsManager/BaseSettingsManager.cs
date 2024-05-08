using System;
using System.Collections;
using UnityEngine;
using RModule.Runtime.Data.Configs;

public abstract class BaseSettingsManager<PurchasableGameItem, Placement, OptionalAppConfigSetting, OptionalSetting, OptionaDebugSetting, OptionalCrossPlatformAppConfigSetting>
	: SingletonMonoBehaviour<BaseSettingsManager<PurchasableGameItem, Placement, OptionalAppConfigSetting, OptionalSetting, OptionaDebugSetting, OptionalCrossPlatformAppConfigSetting>>
	, IPlacementsContainer<Placement>

	where PurchasableGameItem : Enum
	where Placement : Enum
	where OptionalSetting : Enum
	where OptionaDebugSetting : Enum
	where OptionalAppConfigSetting : Enum
	where OptionalCrossPlatformAppConfigSetting : Enum {

	// Accessors
	public AppConfig<PurchasableGameItem, Placement, OptionalAppConfigSetting, OptionalCrossPlatformAppConfigSetting> AppConfigData => _appConfigData;
	public BaseDebugConfig<OptionaDebugSetting> DebugConfig => _debugConfig;

	// Outlets
	[SerializeField] protected AppConfig<PurchasableGameItem, Placement, OptionalAppConfigSetting, OptionalCrossPlatformAppConfigSetting> _appConfigData = default;
	[SerializeField] protected BaseDebugConfig<OptionaDebugSetting> _debugConfig = default;

	// Settings
	[SerializeField] protected SettingsData<CommonSetting> _commonSettings = default;
	[SerializeField] protected SettingsData<OptionalSetting> _optionalSettings = default;

	// Private vars
	protected ISoundsStateHandler _soundsStateHandler;
	protected IMusicStateHandler _musicStateHandler;
	protected Action<bool> _setEnableVibration;
	protected bool _isReady;

	//--------------------------------------------------------------------------
	// IInitializable

	public virtual IEnumerator Initialize(ISoundsPlayerService soundsPlayerService, Action<bool> setEnableVibration) {
		_soundsStateHandler = soundsPlayerService;
		_musicStateHandler = soundsPlayerService;
		_setEnableVibration = setEnableVibration;

		_commonSettings.AddValueChangedListener(CommonSetting.SoundEnabled, (value) => { _soundsStateHandler.OnSoundsStateChanged((bool)value); });
		_commonSettings.AddValueChangedListener(CommonSetting.MusicEnabled, (value) => { _musicStateHandler.OnMusicStateChanged((bool)value); });
		_commonSettings.AddValueChangedListener(CommonSetting.VibrationEnabled, (value) => { _soundsStateHandler.OnSoundsStateChanged((bool)value); });

		UpdateNubmerOfStars();
		UpdateLastPlayedDay();
		UpdatePlayedDaysInARowCount();
		LateInitialize();

		Application.targetFrameRate = _appConfigData.TargetFps;
		_isReady = true;

		yield return null;
	}

	public override bool IsInitialized() {
		return _isReady;
	}

	void UpdateNubmerOfStars() {
		SetSetting(CommonSetting.NumberOfStarts, GetSetting<int>(CommonSetting.NumberOfStarts) + 1);
	}

	void UpdateLastPlayedDay() {
		SetSetting(CommonSetting.LastPlayedDay, DateTime.Now);
	}

	void UpdatePlayedDaysInARowCount() {
		if ((GetSetting<DateTime>(CommonSetting.LastPlayedDay) - DateTime.Now).Days <= 1) {
			SetSetting(CommonSetting.PlayedDaysInARowCount, GetSetting<int>(CommonSetting.PlayedDaysInARowCount) + 1);
		}
	}

	protected abstract void LateInitialize();

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

	// Set settings
	public virtual void SetSetting<T1>(CommonSetting setting, T1 value) {
		((IValueSetter<CommonSetting>)_commonSettings).SetValue(setting, value);
	}

	public virtual void SetSetting<T1>(OptionalSetting setting, T1 value) {
		((IValueSetter<OptionalSetting>)_optionalSettings).SetValue(setting, value);
	}

	public virtual T1 GetSetting<T1>(CommonSetting setting) {
		return ((IValueGetter<CommonSetting>)_commonSettings).GetValue<T1>(setting);
	}

	public virtual T1 GetSetting<T1>(OptionalSetting setting) {
		return ((IValueGetter<OptionalSetting>)_optionalSettings).GetValue<T1>(setting);
	}

	public virtual T1 GetSetting<T1>(OptionalCrossPlatformAppConfigSetting enumType) {
		return _appConfigData.GetValue<T1>(enumType);
	}

	public virtual T1 GetSetting<T1>(OptionalAppConfigSetting enumType) {
		return _appConfigData.GetValue<T1>(enumType);
	}

	public virtual T1 GetSetting<T1>(OptionaDebugSetting enumType) {
		return ((IValueGetter<OptionaDebugSetting>)_debugConfig).GetValue<T1>(enumType);
	}

	public string GetPlacement(Placement placementType) {
		return ((IPlacementsContainer<Placement>)_appConfigData).GetPlacement(placementType);
	}
}
