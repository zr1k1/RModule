﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Data.Configs;

public class AppConfig<PurchasableGameItem, Placement, OptionaAppConfigValue, CrossPlatformValue> : ScriptableObject, IPlacementsProvider<Placement>

	where PurchasableGameItem : Enum
	where Placement : Enum
	where OptionaAppConfigValue : Enum
	where CrossPlatformValue : Enum {

	// Accessors
	public AppEconomicsConfig<PurchasableGameItem> AppEconomicsData => _appEconomicsData;
	public AdPlacementsConfig<Placement> AdPlacementsConfig => _adPlacementsConfig;

	// Accessors
	public int TargetFps => _targetFPS;
	public bool EnableFirebaseInitialization => _enableFirebaseInitialization;
	public bool EnableRemoteConfig => _enableRemoteConfig;
	public int DragBeginTreeshold => _dragBeginTreeshold;

	public bool EnableTestAdsMode => _enableTestAdsMode;

	public string AppPolicyLink => _appPolicyLink;
	public string AppTermsLink => _appTermsLink;
	public string ContactEmail => _contactEmail;
	public string VkLink => _vkLink;
	public string FbLink => _fbLink;
	public string AppSiteLink => _appSiteLink;

	public string IOSGADApplicationIdentifier => _iosGADApplicationIdentifier;
	public string AppMetricaKey => _appMetricaKey;

	public string TrackingUsageDescription => _trackingUsageDescription;

	// Outlets
	[Header("Misc app settings"), Space]
	[SerializeField] protected int _targetFPS = 60;
	[SerializeField] protected bool _enableFirebaseInitialization = default;
	[SerializeField] protected bool _enableRemoteConfig = default;
	[SerializeField] protected int _dragBeginTreeshold = default;
	[SerializeField] protected int _copyrightBeginYear = default;

	[Header("Economics"), Space]
	[SerializeField] protected AppEconomicsConfig<PurchasableGameItem> _appEconomicsData = default;

	[Header("Ads"), Space]
	[SerializeField] protected bool _enableTestAdsMode = default;
	[SerializeField] protected AdPlacementsConfig<Placement> _adPlacementsConfig = default;

	[Header("Links"), Space]
	[SerializeField] protected string _appPolicyLink = default;
	[SerializeField] protected string _appTermsLink = default;
	[SerializeField] protected string _contactEmail = default;
	[SerializeField] protected string _vkLink = default;
	[SerializeField] protected string _fbLink = default;
	[SerializeField] protected string _appSiteLink = default;

	[Header("Keys"), Space]
	[SerializeField] protected string _iosGADApplicationIdentifier = default;
	[SerializeField] protected string _appMetricaKey = default;

	[Header("Plist descriptions"), Space]
	[SerializeField] protected string _trackingUsageDescription = default;

	[Header("OptionaCrossPlatformValuesDict"), Space]
	[SerializeField] protected SerializableDictionary<CrossPlatformValue, CrossPlatformValuesData> _crossPlatformValuesDict = default;

	[Header("OptionaAppConfigValue"), Space]
	[SerializeField] protected SerializableDictionary<OptionaAppConfigValue, BaseValueConfig> _optionalValuesDict = default;

	//Classes
	[Serializable]
	public class CrossPlatformValuesData {
		[SerializeField] internal protected SerializableDictionary<Store, string> values = default;
	}

	public string GetCopyRightString() {
		var copyrighString = $"© {_copyrightBeginYear}, Lunapp";
		if (_copyrightBeginYear < DateTime.Now.Year) {
			copyrighString = $"© {_copyrightBeginYear}-{DateTime.Now.Year}, Lunapp";
		}

		return copyrighString;
	}

	// Outlets
	public virtual T1 GetValue<T1>(OptionaAppConfigValue valueType) {
		if (!_optionalValuesDict.ContainsKey(valueType)) {
			Debug.LogError($"Value {valueType} is not present on dictionary _optionalValuesDict");
			return default(T1);
		}

		return _optionalValuesDict[valueType].GetValue<T1>();
	}

	public T1 GetValue<T1>(CrossPlatformValue key) {
		if (!_crossPlatformValuesDict.ContainsKey(key)) {
			Debug.LogError($"AppConfig : Value {key} is not present on dictionary _crossPlatformValuesDict");

			return default(T1);
		}

		if (Application.platform == RuntimePlatform.Android) {
			return (T1)(object)_crossPlatformValuesDict[key].values[Store.GooglePlayStore];
		} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return (T1)(object)_crossPlatformValuesDict[key].values[Store.AppStore];
		} else {
			return (T1)(object)_crossPlatformValuesDict[key].values[Store.GooglePlayStore];
		}
	}

	public string GetPlacement(Placement placementType) {
		return ((IPlacementsProvider<Placement>)_adPlacementsConfig).GetPlacement(placementType);
	}
}