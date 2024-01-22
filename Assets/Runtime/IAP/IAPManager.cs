#if USE_RMODULE_IAP

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RModule.Runtime.Utils;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

#if PLATFORM_ANDROID && USE_HUAWEI_SERVICES
using HmsPlugin;
#endif

namespace RModule.Runtime.IAP {

	public class IAPManager {
		// --- Singleton stuff ---
		public static IAPManager Instance => s_instance;
		static readonly IAPManager s_instance = new IAPManager();

		// Explicit static constructor to tell C# compiler
		// not to mark type as before field init
		static IAPManager() { }

		IAPManager() { }

		// Accessors
		public bool IsInitialized => _purchasesService != null && _purchasesService.IsInitialized;

		// Private vars
		IPurchasesService _purchasesService;
		string _storeCurrencyCode;

		public delegate void PurchaseCompleteDelegate(string productId, bool success, PurchaseError purchaseError, object productObj = null);
		public delegate void RestorePurchasesCompleteDelegate(bool success);

		

		// ---------------------------------------------------------------
		// Initialization

		public async Task InitializeAsync(IStoreProductsProvider productProvider) {
#if PLATFORM_ANDROID && USE_HUAWEI_SERVICES && !UNITY_EDITOR
		_purchasesService = new PurchasesServiceHuawei();
#elif PLATFORM_ANDROID && USE_SAMSUNG_SERVICES && !UNITY_EDITOR
		_purchasesService = new PurchasesServiceSamsung();
#else
			await InitializeUnityServicesAsync();
			_purchasesService = new PurchasesServiceStandard();
#endif

			await Task.Delay(millisecondsDelay: 1);
			_purchasesService.Initialize(productProvider);
		}

		static async Task InitializeUnityServicesAsync() {
			const string environment = "production";
			try {
				var options = new InitializationOptions().SetEnvironmentName(environment);

				await UnityServices.InitializeAsync(options);
			} catch (Exception exception) {
				// An error occurred during initialization.
			}
		}

		// ---------------------------------------------------------------
		// Purchasing

		public void PurchaseProduct(string productId, PurchaseCompleteDelegate purchaseCallback) {
			if (!IsInitialized) {
				purchaseCallback?.Invoke(productId, false, new PurchaseError(PurchaseError.ErrorType.IapNotInitialized, "IAP is not initialized"));
				return;
			}

			_purchasesService.PurchaseProduct(productId, purchaseCallback);
		}

		public void RestorePurchases(RestorePurchasesCompleteDelegate restorePurchaseComplete) {
			if (!IsInitialized) {
				restorePurchaseComplete?.Invoke(false);
				return;
			}

			_purchasesService.RestorePurchases(restorePurchaseComplete);
		}

		public void ClearProductPurchaseCallback() {
			if (!IsInitialized)
				return;

			_purchasesService.ClearProductPurchaseCallback();
		}

		public void ClearProductsRestoreCallback() {
			if (!IsInitialized)
				return;

			_purchasesService.ClearProductsRestoreCallback();
		}

		public List<string> GetActiveSubscriptionsProductIds() {
			if (!IsInitialized)
				return new List<string>();

			return _purchasesService.GetActiveSubscriptionsProductIds();
		}

		public bool SubscriptionIsActiveForProductWithId(string productId) {
			if (!IsInitialized)
				return false;

			return _purchasesService.SubscriptionIsActiveForProductWithId(productId);
		}

		public bool SubscriptionIsActiveForAnyProduct(List<string> productIds) {
			if (!IsInitialized)
				return false;

			var activeSubscriptionsProductsIds = GetActiveSubscriptionsProductIds();
			foreach (string activeProductId in activeSubscriptionsProductsIds) {
				if (productIds.Contains(activeProductId))
					return true;
			}

			return false;
		}

		// ---------------------------------------------------------------
		// Helpers

		void PrintAllProductsToConsole() {
			if (!IsInitialized)
				return;

			_purchasesService.PrintAllProductsToConsole();
		}

		public string PriceStringForProductId(string productId, SystemLanguage locale) {
			if (!IsInitialized)
				return "0";

			return _purchasesService.PriceStringForProductId(productId, locale);
		}

		public decimal LocalizedPriceForProductId(string productId) {
			if (!IsInitialized)
				return 0;

			return _purchasesService.LocalizedPrice(productId);
		}

		public bool ProductIsPurchased(string productId) {
			if (!IsInitialized)
				return false;

			return _purchasesService.ProductIsPurchased(productId);
		}

		public IAPProductData GetProductData(string productId) {
			if (!IsInitialized)
				return null;

			return _purchasesService.ProductProvider.ProductsDatabase.AllIapProducts.FirstOrDefault(product =>
				product.ProductId == productId);
		}

		public string GetStoreCurrencyCode() {
			if (!IsInitialized)
				return "";

			if (!string.IsNullOrWhiteSpace(_storeCurrencyCode))
				return _storeCurrencyCode;

			var anyProduct = _purchasesService.ProductProvider.ProductsDatabase.AllIapProducts.FirstOrDefault();
			_storeCurrencyCode = anyProduct != null ? _purchasesService.ProductCurrencyCode(anyProduct.ProductId) : "";
			return _storeCurrencyCode;
		}

		public string GetPriceString(IStoreProductData storeProductData) {
			if (storeProductData.PurchaseMethod == PurchaseMethod.RealCurrency) {
				return IAPManager.Instance.PriceStringForProductId(ProductId, LocalizationManager.Instance.CurrentLanguage);
			}

			return "";
		}
	}

}

#endif
