using System;

namespace RModule.Runtime.IAP {
	[Serializable]
	public class StoreProductsProvider : IStoreProductsProvider {

		// Accessors
		public IProductsDatabase ProductsDatabase => _productsDatabase;

		// Privats
		IIAPService _iAPService;
		IProductsDatabase _productsDatabase;
		IPlayerPurchasedStoreProductSaveService _playerPurchasedStoreProductSaveService;
		IDisableAdsService _disableAdsService;

		public StoreProductsProvider(IIAPService iAPService,
			IProductsDatabase productsDatabase,
			IPlayerPurchasedStoreProductSaveService playerPurchasedStoreProductSaveService,
			IDisableAdsService disableAdsService) {

			_iAPService = iAPService;
			_productsDatabase = productsDatabase;
			_playerPurchasedStoreProductSaveService = playerPurchasedStoreProductSaveService;
			_disableAdsService = disableAdsService;
		}

		public void ProvideProduct(string productId) {
			var productData = _productsDatabase.GetProductWithId(productId);
			ProvideProduct(productData);
		}

		public void ProvideProduct(IStoreProductData productData) {
			if (productData == null)
				return;

			if (productData.ProductType == ProductType.NonConsumable) {
				_playerPurchasedStoreProductSaveService.AddProductKeyToPurchased(productData.ProductId);
			}

			TryProvideAdsDisabling(productData);
		}

		void TryProvideAdsDisabling(IStoreProductData productData) {
			if (productData.IsRemoveAdsProduct()) {
				_playerPurchasedStoreProductSaveService.SetEnableAds(false);
				_disableAdsService.DisableAds(true);
			}
		}

		// ---------------------------------------------------------------
		// General methods
		public bool ProductIsPurchased(string productId) {
			return ProductIsPurchased(_productsDatabase.GetProductWithId(productId));
		}

		public bool ProductIsPurchased(IStoreProductData productData) {
			if (productData.ProductType != ProductType.NonConsumable)
				return false;

			if (productData.PurchaseMethod == ProductPurchaseMethod.Free) {
				return _playerPurchasedStoreProductSaveService.ProductIsPurchased(productData.ProductId);
			}

			if (productData.PurchaseMethod == ProductPurchaseMethod.RealCurrency) {
				return _playerPurchasedStoreProductSaveService.ProductIsPurchased(productData.ProductId) ||
					   _iAPService.ProductIsPurchased(productData.ProductId);
			}

			return false;
		}

		public void RestoreNonConsumablesFromPlayerData(PlayerData playerData) {
			foreach (var iapProductData in _productsDatabase.AllIapProducts) {
				if (iapProductData.ProductType == ProductType.NonConsumable && playerData.ProductIsPurchased(iapProductData.ProductId)) {
					ProvideProduct(iapProductData as IStoreProductData);
				}
			}
		}
	}
}