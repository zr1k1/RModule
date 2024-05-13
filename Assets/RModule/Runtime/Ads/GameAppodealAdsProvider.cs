#if USE_RMODULE_ADS && USE_APPODEAL

using System;
using System.Collections.Generic;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;
using AppodealApi = AppodealAds.Unity.Api.Appodeal;
using MTUnityCore.Runtime.Managers.Ads;

#if UNITY_IOS
using UnityEngine.Advertisements;
#endif

public class GameAppodealAdsProvider : IAdsProvider, IBannerAdListener, IInterstitialAdListener, IRewardedVideoAdListener, IAppodealInitializationListener, IAdRevenueListener {

	public readonly struct Config {
		public string ApiKey { get; }
		public bool UserHasConsent { get; }
		public bool EnableTestMode { get; }

		public Config(string apiKey, bool userHasConsent, bool enableTestMode = false) {
			ApiKey = apiKey;
			UserHasConsent = userHasConsent;
			EnableTestMode = enableTestMode;
		}
	}

	// Private vars
	int _currentBannerHeight;
	IAdsListener _adsListener;
	readonly Config _config;

	Queue<string> _showingInterstitials = new Queue<string>();
	Queue<string> _showingRewarded = new Queue<string>();

	// ---------------------------------------------------------------
	// Constructor

	public GameAppodealAdsProvider(Config config) {
		_config = config;
	}

	// ---------------------------------------------------------------
	// IAdsProvider

	public void Initialize(IAdsListener adsListener) {
		_adsListener = adsListener;

		DisableUnityAdsConsentPopup();

		AppodealApi.setTesting(_config.EnableTestMode);
		AppodealApi.disableLocationPermissionCheck();
		AppodealApi.muteVideosIfCallsMuted(true);
		AppodealApi.setLogLevel(AppodealApi.LogLevel.None);

		AppodealApi.setAutoCache(AppodealApi.REWARDED_VIDEO, true);
		AppodealApi.setAutoCache(AppodealApi.INTERSTITIAL, true);
		AppodealApi.setAutoCache(AppodealApi.BANNER, true);

		AppodealApi.setRewardedVideoCallbacks(this);
		AppodealApi.setBannerCallbacks(this);
		AppodealApi.setInterstitialCallbacks(this);

		AppodealApi.setAdRevenueCallback(this);

		AppodealApi.initialize(_config.ApiKey, AppodealApi.REWARDED_VIDEO | AppodealApi.INTERSTITIAL | AppodealApi.BANNER, listener: this);
	}

	void DisableUnityAdsConsentPopup() {
		// To disable unity ads consent popup on ios we need to do this stuff
		// So for iOS builds we need to add Advertisements package to get access to the api
#if UNITY_IOS
		bool consentGiven = AppTrackingConsentManager.CurrentAppTrackingStatus ==
			                AppTrackingConsentManager.AppTrackingStatus.Authorized;
			
		var gdprMetaData = new MetaData("gdpr");
		gdprMetaData.Set("consent", consentGiven ? "true" : "false");
		Advertisement.SetMetaData(gdprMetaData);
#endif
	}

	public void UpdateAdTrackingConsent(bool hasConsent) {
		var ccpaConsent = hasConsent ? AppodealApi.CcpaUserConsent.OptIn : AppodealApi.CcpaUserConsent.OptOut;
		AppodealApi.updateCcpaConsent(ccpaConsent);

		var gdprConsent =
			hasConsent ? AppodealApi.GdprUserConsent.Personalized : AppodealApi.GdprUserConsent.NonPersonalized;
		AppodealApi.updateGdprConsent(gdprConsent);
	}

	public void PreloadAd(AdsManager.AdType adType, string placement = "") {
		AppodealApi.cache(AppodealAdTypeForAd(adType));
	}

	public bool AdIsReadyForShow(AdsManager.AdType adType, string placement) {
		return AppodealApi.canShow(AppodealAdTypeForAd(adType), placement);
	}

	public void ShowAd(AdsManager.AdType adType, string placement) {
		if (adType == AdsManager.AdType.Interstitial) {
			_showingInterstitials.Enqueue(placement);
		} else if (adType == AdsManager.AdType.Rewarded) {
			_showingRewarded.Enqueue(placement);
		}
		AppodealApi.show(AppodealAdTypeForAd(adType), placement);
	}

	public void HideBanner(string placement = "") {
		AppodealApi.hide(AppodealAdTypeForAd(AdsManager.AdType.BannerBottom));
	}

	public int CurrentBannerHeight() {
		return _currentBannerHeight;
	}

	// ---------------------------------------------------------------
	// Helpers

	void DisableUncertifiedAdNetworks() {
		var networksNames = new[] {
			"amazon_ads", "appnext", "avocarrot", "facebook",
			"flurry", "inmobi", "inner-active", "mailru", "mmedia",
			"mopub", "mobvista", "ogury", "openx", "pubnative", "smaato",
			"tapjoy", "yandex", "appodealx", "my_target", "mintegral",
			"appnexus", "mraid", "mraid_va", "nast", "vast", "vpaid"
		};

		foreach (string networksName in networksNames) {
			AppodealApi.disableNetwork(networksName);
		}
	}

	int AppodealAdTypeForAd(AdsManager.AdType adType) {
		switch (adType) {
			case AdsManager.AdType.BannerBottom:
				return AppodealApi.BANNER_BOTTOM;
			case AdsManager.AdType.Interstitial:
				return AppodealApi.INTERSTITIAL;
			case AdsManager.AdType.Rewarded:
				return AppodealApi.REWARDED_VIDEO;
			default:
				throw new ArgumentOutOfRangeException(nameof(adType), adType, null);
		}
	}

	// ---------------------------------------------------------------
	// IAppodealInitializationListener

	public void onInitializationFinished(List<string> errors) { }

	// ---------------------------------------------------------------
	// IBannerAdListener

	public void onBannerLoaded(int height, bool isPrecache) {
		_currentBannerHeight = height;
		_adsListener.OnAdDidFinishLoading(AdsManager.AdType.BannerBottom, true, "banner");
	}

	public void onBannerFailedToLoad() {
		_adsListener.OnAdDidFinishLoading(AdsManager.AdType.BannerBottom, false, "banner");
	}

	public void onBannerShown() {
		_adsListener.OnAdDidShow(AdsManager.AdType.BannerBottom, true, "banner");
	}

	public void onBannerShowFailed() { }

	public void onBannerClicked() {
		_adsListener.OnAdDidClick(AdsManager.AdType.BannerBottom, "banner");
	}

	public void onBannerExpired() { }

	// ---------------------------------------------------------------
	// IInterstitialAdListener

	public void onInterstitialLoaded(bool isPrecache) {
		_adsListener.OnAdDidFinishLoading(AdsManager.AdType.Interstitial, true, string.Empty);
	}

	public void onInterstitialFailedToLoad() {
		_adsListener.OnAdDidFinishLoading(AdsManager.AdType.Interstitial, false, string.Empty);
	}

	public void onInterstitialShowFailed() {
		string placement = _showingInterstitials.Count == 0 ? "" : _showingInterstitials.Dequeue();
		Debug.Log($"Failed to show interstitial ad. Placement: {placement}");
		_adsListener.OnAdDidClose(AdsManager.AdType.Interstitial, false, placement);
	}

	public void onInterstitialShown() {
		string placement = _showingInterstitials.Count == 0 ? "" : _showingInterstitials.Peek();
		_adsListener.OnAdDidShow(AdsManager.AdType.Interstitial, true, placement);
	}

	public void onInterstitialClosed() {
		string placement = _showingInterstitials.Count == 0 ? "" : _showingInterstitials.Dequeue();
		_adsListener.OnAdDidClose(AdsManager.AdType.Interstitial, false, placement);
	}

	public void onInterstitialClicked() {
		string placement = _showingInterstitials.Count == 0 ? "" : _showingInterstitials.Peek();
		_adsListener.OnAdDidClick(AdsManager.AdType.Interstitial, placement);
	}

	public void onInterstitialExpired() {
		Debug.Log("Interstitial ad expired");
	}

	// ---------------------------------------------------------------
	// IRewardedVideoAdListener

	public void onRewardedVideoLoaded(bool precache) {
		Debug.Log("Rewarded video finished loading");
		_adsListener.OnAdDidFinishLoading(AdsManager.AdType.Rewarded, true, string.Empty);
	}

	public void onRewardedVideoFailedToLoad() {
		Debug.Log("Rewarded video failed to load");
		_adsListener.OnAdDidFinishLoading(AdsManager.AdType.Rewarded, false, string.Empty);
	}

	public void onRewardedVideoShowFailed() {
		string placement = _showingRewarded.Count == 0 ? "" : _showingRewarded.Dequeue();
		Debug.Log($"Rewarded video show failed. Placement: {placement}");
		_adsListener.OnAdDidClose(AdsManager.AdType.Rewarded, false, placement);
	}

	public void onRewardedVideoShown() {
		string placement = _showingRewarded.Count == 0 ? "" : _showingRewarded.Peek();
		_adsListener.OnAdDidShow(AdsManager.AdType.Rewarded, true, placement);
	}

	public void onRewardedVideoFinished(double amount, string name) {
		Debug.Log("Rewarded video finished");
	}

	public void onRewardedVideoClosed(bool finished) {
		Debug.Log("Rewarded video closed");
		string placement = _showingRewarded.Count == 0 ? "" : _showingRewarded.Dequeue();
		_adsListener.OnAdDidClose(AdsManager.AdType.Rewarded, finished, placement);
	}

	public void onRewardedVideoExpired() {
		Debug.Log("Rewarded ad expired");
	}

	public void onRewardedVideoClicked() {
		string placement = _showingRewarded.Count == 0 ? "" : _showingRewarded.Peek();
		_adsListener.OnAdDidClick(AdsManager.AdType.Rewarded, placement);
	}

	// ---------------------------------------------------------------
	// IAdRevenueListener

	public void onAdRevenueReceived(AppodealAdRevenue ad) {
		var adRevenueData = new AdRevenueData(
			adType: ad.AdType,
			networkName: ad.NetworkName,
			adUnitName: ad.AdUnitName,
			demandSource: ad.DemandSource,
			placement: ad.Placement,
			revenue: ad.Revenue,
			currency: ad.Currency,
			revenuePrecision: ad.RevenuePrecision);

		_adsListener.OnAdRevenueDidReceive(adRevenueData);
	}
}

#endif
