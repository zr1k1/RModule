using System;
using System.Threading.Tasks;

namespace RModule.Runtime.IAP {
	public delegate void PurchaseCompleteDelegate(string productId, bool success, PurchaseError purchaseError, object productObj = null);

	public class StoreProductsPurchaser {

		public delegate void PurchaseDidFinishDelegate(IStoreProductData productData, Result result);
		public delegate void IndicateLongProcessDelegate(bool longProcessActive);

		// Enums
		public enum Result { Success, StoreError, AdsError }

		// Events
		public event IndicateLongProcessDelegate IndicateLongProcess;

		// Private vars
		readonly StoreProductsProvider _productsProvider;
		readonly PlayerData _playerData;
		readonly IIAPService _iAPService;
		readonly IRewardedAdsPlacementsProvider _rewardedAdsPlacementsProvider;
		readonly IRewardedAdActionsProvider _rewardedAdActionsProvider;

		// ---------------------------------------------------------------
		// Constructor

		public StoreProductsPurchaser(StoreProductsProvider productsProvider, PlayerData playerData,
			IIAPService iAPService,
			IRewardedAdsPlacementsProvider rewardedAdsPlacementsProvider,
			IRewardedAdActionsProvider rewardedAdActionsProvider) {

			_productsProvider = productsProvider;
			_playerData = playerData;
			_iAPService = iAPService;
			_rewardedAdsPlacementsProvider = rewardedAdsPlacementsProvider;
			_rewardedAdActionsProvider = rewardedAdActionsProvider;
		}

		// ---------------------------------------------------------------
		// Purchasing

		public void PurchaseProduct(IStoreProductData productData, PurchaseDidFinishDelegate purchaseFinish) {
			switch (productData.PurchaseMethod) {
				case ProductPurchaseMethod.RealCurrency:
					PurchaseRealCurrencyProduct(productData, purchaseFinish);
					break;
				case ProductPurchaseMethod.InGameCurrency:
					PurchaseInGameCurrencyProduct(productData, purchaseFinish);
					break;
				case ProductPurchaseMethod.RewardedAd:
					PurchaseVideoAdProduct(productData, purchaseFinish);
					break;
				case ProductPurchaseMethod.Free:
					PurchaseRewardProduct(productData, purchaseFinish);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		void PurchaseInGameCurrencyProduct(IStoreProductData productData, PurchaseDidFinishDelegate purchaseFinish) {

		}

		async void PurchaseRealCurrencyProduct(IStoreProductData productData, PurchaseDidFinishDelegate purchaseFinish) {
			IndicateLongProcess?.Invoke(true);

			// Add delay here to allow delegate to show activity indicator, because on Android devices purchasing blocks the
			// main thread and indicator kinda stops in between appear animation
			await Task.Delay(500);

			_iAPService.PurchaseProduct(productData, (productId, success, purchaseError, productObj) => {
				IndicateLongProcess?.Invoke(false);
				if (success) {
					// No need to provide product here. It has been already provided from the IAPManager
					// Such a setup is needed to support automatic products restoration
					purchaseFinish?.Invoke(productData, Result.Success);
				} else {
					purchaseFinish?.Invoke(productData, Result.StoreError);
				}
			});
		}

		void PurchaseVideoAdProduct(IStoreProductData productData, PurchaseDidFinishDelegate purchaseFinish) {
			string placement = _rewardedAdsPlacementsProvider.GetRewardedPlacementByStoreProduct(productData);

			_rewardedAdActionsProvider.ShowRewardedAd(placement, success => {
				if (success) {
					_productsProvider.ProvideProduct(productData);
					purchaseFinish?.Invoke(productData, Result.Success);
				} else {
					purchaseFinish?.Invoke(productData, Result.AdsError);
				}
			});
		}

		void PurchaseRewardProduct(IStoreProductData productData, PurchaseDidFinishDelegate purchaseFinish) {
			_productsProvider.ProvideProduct(productData);
			purchaseFinish?.Invoke(productData, Result.Success);
		}
	}
}