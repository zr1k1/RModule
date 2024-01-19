using System;
using System.Collections.Generic;
using UnityEngine;

namespace RModule.Runtime.IAP {
	public class StoreProductsPresenter {

		// Accessors
		public List<IStoreProductData> Products => _products;

		// Private vars
		readonly IIAPService _iAPService;
		readonly List<IStoreProductData> _products = new List<IStoreProductData>();
		readonly IStoreProductsProvider _storeProductsProvider;
		readonly IRewardedAdActionsProvider _rewardedAdStateProvider;
		readonly IRewardedAdsPlacementsProvider _rewardedAdsPlacementsProvider;
		readonly IStatusIsAdEnabledProvider _statusIsAdEnabledProvider;
		readonly IIAPDebugSettingsProvider _iAPDebugSettingsProvider;

		// ---------------------------------------------------------------
		// Data update

		public StoreProductsPresenter(IIAPService iAPService,
			List<IStoreProductData> storeProductDatas,
			IStoreProductsProvider storeProductsProvider,
			IRewardedAdActionsProvider rewardedAdStateProvider,
			IRewardedAdsPlacementsProvider rewardedAdsPlacementsProvider,
			IStatusIsAdEnabledProvider statusIsAdEnabledProvider,
			IIAPDebugSettingsProvider iAPDebugSettingsProvider) {

			_iAPService = iAPService;
			_storeProductsProvider = storeProductsProvider;
			_rewardedAdStateProvider = rewardedAdStateProvider;
			_rewardedAdsPlacementsProvider = rewardedAdsPlacementsProvider;
			_statusIsAdEnabledProvider = statusIsAdEnabledProvider;
			_iAPDebugSettingsProvider = iAPDebugSettingsProvider;

			var products = storeProductDatas;
			products.RemoveAll(ShouldHideProduct);
			PrepareProductsList(products);
		}

		bool ShouldHideProduct(IStoreProductData productData) {

			//if (SettingsManager.Instance.DebugConfig.ShowAllStoreProducts)
			//	return false;

			// Filter out invisible
			if (!productData.IsVisible())
				return true;

			// Filter out not achieved products with PurchaseMethod.Reward
			//var productsProvider = GameDataManager.Instance.StoreProductsProvider;
			bool productPurchased = _storeProductsProvider.ProductIsPurchased(productData);
			if (productData.PurchaseMethod == ProductPurchaseMethod.Free &&
				!productPurchased) {
				return true;
			}

			// Filter out purchased products that can be bought only once
			if (productData.HideWhenPurchased() && productPurchased) {
				return true;
			}

			//// Filter out products with purchase method AdReward, when ad is not ready to show
			if (productData.PurchaseMethod == ProductPurchaseMethod.RewardedAd &&
				!_rewardedAdStateProvider.RewardedAdIsReadyForShow(_rewardedAdsPlacementsProvider.GetRewardedPlacementByStoreProduct(productData))) {
				Debug.Log("Rewarded ad is not ready. Will hide WatchAd product data");
				return true;
			}

			// Filter out real currency products when IAPManager in not ready (there is no internet connection)
			if (productData.PurchaseMethod == ProductPurchaseMethod.RealCurrency &&
				!_iAPService.IsReady()) {
				return true;
			}

			if (productData.IsRemoveAdsProduct() && !_statusIsAdEnabledProvider.AdsIsEnabled()) {
				return true;
			}

			return false;
		}

		void PrepareProductsList(List<IStoreProductData> products) {
			_products.Clear();
			_products.AddRange(products);
		}

		public bool HasRealCurrencyProducts() {
			foreach (var product in _products)
				if (product.PurchaseMethod == ProductPurchaseMethod.RealCurrency)
					return true;

			return false;
		}

		public IStoreProductData GetDisableAdsProduct() {
			return Products.Find(product => product.IsRemoveAdsProduct());
		}
	}
}