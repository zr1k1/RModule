using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Data.Configs;

public class AppConfig<PurchasableGameItem, Placement, OptionaAppConfigValue> : ScriptableObject where OptionaAppConfigValue : Enum {
	// Accessors
	public AppEconomicsConfig<PurchasableGameItem> AppEconomicsData => _appEconomicsData;
	public AdPlacementsConfig<Placement> AdPlacementsConfig => _adPlacementsConfig;

	// Accessors
	public int TargetFps => _targetFPS;
	public bool EnableFirebaseInitialization => _enableFirebaseInitialization;
	public bool EnableRemoteConfig => _enableRemoteConfig;
	public int DragBeginTreeshold => _dragBeginTreeshold;
	public int FirstLevelToShowLikeGamePopup => _firstLevelToShowLikeGamePopup;
	public int LikeGamePopupLevelsThreeshold => _likeGamePopupLevelsThreshold;


	public string AppLinkIos => _platformDatas[Store.AppStore].StoreLink;
	public string AppLinkAndroid => _platformDatas[Store.GooglePlayStore].StoreLink;
	public string AppPolicyLink => _appPolicyLink;
	public string AppTermsLink => _appTermsLink;
	public string ContactEmail => _contactEmail;
	public string VkLink => _vkLink;
	public string FbLink => _fbLink;
	public string AppSiteLink => _appSiteLink;
	public string AppodealApiKey => Application.platform == RuntimePlatform.IPhonePlayer ? _appodealApiKeyIos : _appodealApiKeyAndroid;
	public string FlurryApiKey => Application.platform == RuntimePlatform.IPhonePlayer ? _flurryApiKeyIos : _flurryApiKeyAndroid;
	public string IOSGADApplicationIdentifier => _iosGADApplicationIdentifier;
	public string TrackingUsageDescription => _trackingUsageDescription;
	public bool EnableTestAdsMode => _enableTestAdsMode;

	// Outlets
	[Header("Misc app settings"), Space]
	[SerializeField] protected int _targetFPS = 60;
	[SerializeField] protected bool _enableFirebaseInitialization = default;
	[SerializeField] protected bool _enableRemoteConfig = default;
	[SerializeField] protected int _dragBeginTreeshold = default;
	[SerializeField] protected int _copyrightBeginYear = default;
	[SerializeField] protected int _firstLevelToShowLikeGamePopup = default;
	[SerializeField] protected int _likeGamePopupLevelsThreshold = default;
	[SerializeField] protected SerializableDictionary<Store, PlatformData> _platformDatas = default;

	[Header("Sounds"), Space]
	[SerializeField] protected SoundsConfig _soundsConfig = default;

	[Header("Economics"), Space]
	[SerializeField] protected AppEconomicsConfig<PurchasableGameItem> _appEconomicsData = default;

	[Header("Links"), Space]
	//[SerializeField] protected string _appLinkIOS = default;
	//[SerializeField] protected string _appLinkAndroid = default;
	[SerializeField] protected string _appPolicyLink = default;
	[SerializeField] protected string _appTermsLink = default;
	[SerializeField] protected string _contactEmail = default;
	[SerializeField] protected string _vkLink = default;
	[SerializeField] protected string _fbLink = default;
	[SerializeField] protected string _appSiteLink = default;

	[Header("API keys"), Space]
	[SerializeField] protected string _appodealApiKeyIos = default;
	[SerializeField] protected string _appodealApiKeyAndroid = default;
	[SerializeField] protected string _flurryApiKeyIos = default;
	[SerializeField] protected string _flurryApiKeyAndroid = default;
	[SerializeField] protected string _iosGADApplicationIdentifier = default;

	[Header("Plist descriptions"), Space]
	[SerializeField] protected string _trackingUsageDescription = default;

	[Header("Ads"), Space]
	[SerializeField] protected bool _enableTestAdsMode = default;
	[SerializeField] protected AdPlacementsConfig<Placement> _adPlacementsConfig = default;

	[Header("OptionaAppConfigValue"), Space]
	[SerializeField] protected SerializableDictionary<OptionaAppConfigValue, BaseValueConfig> _optionalValuesDict = default;

	//Classes
	[Serializable]
	public class PlatformData : IStoreLinkProvider, IAppodealAPIKeyLinkProvider {
		public string StoreLink => _storeLink;
		public string AppodealAPIKey => _appodealAPIKey;

		[SerializeField] protected string _storeLink = default;
		[SerializeField] protected string _appodealAPIKey = default;

	}

	// Interfaces
	public interface IStoreLinkProvider {
		public string StoreLink { get; }
	}
	public interface IAppodealAPIKeyLinkProvider {
		public string AppodealAPIKey { get; }
	}

	public string GetCopyRightString() {
		var copyrighString = $"© {_copyrightBeginYear}, Lunapp";
		if (_copyrightBeginYear < DateTime.Now.Year) {
			copyrighString = $"© {_copyrightBeginYear}-{DateTime.Now.Year}, Lunapp";
		}

		return copyrighString;
	}

	public string GetStorelink() {
		return Application.platform == RuntimePlatform.Android ?
			_platformDatas[Store.GooglePlayStore].StoreLink :
			_platformDatas[Store.AppStore].StoreLink;
	}

	// Outlets
	public virtual T1 GetValue<T1>(OptionaAppConfigValue valueType) {
		if (!_optionalValuesDict.ContainsKey(valueType)) {
			Debug.LogError($"Value {valueType} is not present on dictionary _optionalValuesDict");
			return default(T1);
		}
		var value = _optionalValuesDict[valueType].GetValue<object>();
		return (T1)value;
	}
}