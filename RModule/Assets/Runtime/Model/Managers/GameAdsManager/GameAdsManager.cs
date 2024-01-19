#if USE_RMODULE_ADS
using System;
using RModule.Runtime.Managers.Ads;

public class GameAdsManager {
	public static event Action RewardedAdDidLoad;
	public static event Action BannerAdDidLoad;
	public static event Action InterstitialAdDidShown;

	public static bool BannerIsAlreadyVisible { private set; get; }
	public static bool AdsDisabled => AdsManager.Instance.MandatoryAdsDisabled;

	static string s_bannerPlacement;

	public static void Init(bool hasTargetedAdsConsent, int age = 0) {
		s_bannerPlacement = SettingsManager.Instance.AppConfigData.AdPlacement.GetPlacement(AdPlacementsConfig.PlacementType.GameBanner);
		AdsManager.AdDidLoad += OnAdLoaded;
		AdsManager.RewardedAdDidLoad += () => { RewardedAdDidLoad?.Invoke(); };
		AdsManager.AdDidShow += OnAdShown;

		AdsManager.Instance.InitializeAdNetworks(new GameAppodealAdsProvider(
			new GameAppodealAdsProvider.Config(
				SettingsManager.Instance.AppConfigData.AppodealApiKey,
				hasTargetedAdsConsent,
				SettingsManager.Instance.AppConfigData.EnableTestAdsMode)));

		if (SettingsManager.Instance.DebugConfig.EnableImitationDisableAdsWithTempEnabldeAllAds)
			AdsManager.TempEnableAllAds = true;
	}

	static void OnAdLoaded(AdsManager.AdType adType, string unitIdOrPlacement) {
		if (adType == AdsManager.AdType.BannerBottom && AppConfigHelper.BannerAdEnabled()
			&& AdIsReadyForShow(AdsManager.AdType.BannerBottom, AdPlacementsConfig.PlacementType.GameBanner)) {

			BannerAdDidLoad?.Invoke();
		}
	}

	static void OnAdShown(AdsManager.AdType adType, string unitIdOrPlacement) {
		if (adType == AdsManager.AdType.Interstitial) {
			InterstitialAdDidShown.Invoke();
		}
	}

	public static bool TryShowBanner() {
		if (AppConfigHelper.BannerAdEnabled() && AdIsReadyForShow(AdsManager.AdType.BannerBottom, AdPlacementsConfig.PlacementType.GameBanner)) {
			ShowBanner();
			BannerIsAlreadyVisible = true;

			return true;
		}

		return false;
	}

	public static void TryShowInterstitial(AdPlacementsConfig.PlacementType placementType, Action<bool> finishCallback) {
		AdsManager.Instance.ShowAd(AdsManager.AdType.Interstitial, SettingsManager.Instance.AppConfigData.AdPlacement.GetPlacement(placementType), (success) => {
			finishCallback.Invoke(success);
		});
	}

	public static void TryShowRewarded(AdPlacementsConfig.PlacementType placementType, Action<bool> finishCallback) {
		var placement = SettingsManager.Instance.AppConfigData.AdPlacement.GetPlacement(placementType);
		AdsManager.Instance.ShowAd(AdsManager.AdType.Rewarded, placement, (success) => {
			finishCallback.Invoke(success);
			AdsManager.Instance.PreloadAd(AdsManager.AdType.Rewarded, placement);
		});
	}

	public static void TryPreloadAd(AdsManager.AdType adType, AdPlacementsConfig.PlacementType placementType) {
		if (!AdsManager.Instance.AdIsReadyForShow(adType, SettingsManager.Instance.AppConfigData.AdPlacement.GetPlacement(placementType))) {
			AdsManager.Instance.PreloadAd(adType, placementType.ToString());
		}
	}

	public static void HideBanner() {
		AdsManager.Instance.HideBanner();
		BannerIsAlreadyVisible = false;
	}

	public static bool AdIsReadyForShow(AdsManager.AdType adType, AdPlacementsConfig.PlacementType placementType) {
		return AdsManager.Instance.AdIsReadyForShow(adType, SettingsManager.Instance.AppConfigData.AdPlacement.GetPlacement(placementType));
	}

	public static void DisableAds(bool disable) {
		AdsManager.Instance.DisableAds(disable);
	}

	static void ShowBanner() {
		AdsManager.Instance.ShowAd(AdsManager.AdType.BannerBottom, s_bannerPlacement, null);
	}
}
#endif