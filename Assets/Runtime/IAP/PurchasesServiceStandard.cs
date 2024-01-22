#if USE_IAP

using System.Collections.Generic;
using RModule.Runtime.Utils;
using UnityEngine;
using UnityEngine.Purchasing;

namespace RModule.Runtime.IAP {
	public class PurchasesServiceStandard : IPurchasesService, IStoreListener {
		// Private vars
		bool _isInitialized;
		IStoreController _storeController;
		IExtensionProvider _extensionProvider;
		IAPManager.PurchaseCompleteDelegate _purchaseCallback;
		IAPManager.RestorePurchasesCompleteDelegate _restorePurchaseCompleteCallback;
		IProductProvider _productProvider;
		Dictionary<string, string> _introductoryInfoDict = null;

		// ---------------------------------------------------------------
		// IPurchasesService

		public bool IsInitialized => _isInitialized;

		public IProductProvider ProductProvider => _productProvider;

		public void Initialize(IProductProvider productProvider) {
			_productProvider = productProvider;

			var configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			foreach (var productData in _productProvider.ProductsDatabase.AllIapProducts) {
				Debug.Log("Will add iap product: " + productData.ProductId);

				configurationBuilder.AddProduct(productData.ProductId, productData.ProductType, new IDs {
					{productData.GetStoreProductId(StoreIds.StoreType.AppStore), AppleAppStore.Name},
					{productData.GetStoreProductId(StoreIds.StoreType.GooglePlay), GooglePlay.Name}
				});
			}

			UnityPurchasing.Initialize(this, configurationBuilder);
		}

		public void PurchaseProduct(string productId, IAPManager.PurchaseCompleteDelegate purchaseCallback) {
			var product = _storeController.products.WithID(productId);
			if (product == null) {
				purchaseCallback?.Invoke(productId, false, new IAPManager.PurchaseError(IAPManager.PurchaseError.ErrorType.Other, ""));
				return;
			}

			_purchaseCallback = purchaseCallback;
			_storeController.InitiatePurchase(productId);
		}

		public void RestorePurchases(IAPManager.RestorePurchasesCompleteDelegate restorePurchaseComplete) {
			// Will invoke ProcessPurchase during the restoration process
			_restorePurchaseCompleteCallback = restorePurchaseComplete;

#if UNITY_ANDROID
			_extensionProvider.GetExtension<IGooglePlayStoreExtensions>().RestoreTransactions(result => {
				_restorePurchaseCompleteCallback?.Invoke(result);
			});
#else
			_extensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
				_restorePurchaseCompleteCallback?.Invoke(result);
			});
#endif
		}

		public string PriceStringForProductId(string productId, SystemLanguage locale) {
			if (!IsInitialized)
				return "";

			var product = _storeController.products.WithID(productId);
			if (product == null)
				return "0";

			return StringsHelper.GetFormattedPriceString(product.metadata.localizedPrice, product.metadata.isoCurrencyCode, locale);
		}

		public string ProductCurrencyCode(string productId) {
			if (!IsInitialized)
				return "";

			var product = _storeController.products.WithID(productId);
			if (product == null)
				return "";

			return product.metadata.isoCurrencyCode;
		}

		public decimal LocalizedPrice(string productId) {
			var product = _storeController.products.WithID(productId);
			if (product == null)
				return 0;

			return product.metadata.localizedPrice;
		}

		public bool ProductIsPurchased(string productId) {
			if (!IsInitialized)
				return false;

			return _storeController.products.WithID(productId).hasReceipt;
		}

		public void PrintAllProductsToConsole() {
			if (!IsInitialized)
				return;

			int i = 1;
			foreach (var productData in _productProvider.ProductsDatabase.AllIapProducts) {
				var product = _storeController.products.WithID(productData.ProductId);
				Debug.Log($"Product {i.ToString()}: {product.metadata.localizedTitle} ({product.metadata.localizedPriceString})");
				i++;
			}
		}

		public void ClearProductPurchaseCallback() {
			_purchaseCallback = null;
		}

		public void ClearProductsRestoreCallback() {
			_restorePurchaseCompleteCallback = null;
		}

		public List<string> GetActiveSubscriptionsProductIds() {
			var activeSubscriptionsIds = new List<string>();
			if (!IsInitialized)
				return activeSubscriptionsIds;

			foreach (var product in _storeController.products.all) {
				if (SubscriptionIsActiveForProduct(product)) {
					activeSubscriptionsIds.Add(product.definition.id);
				}
			}

			return activeSubscriptionsIds;
		}

		public bool SubscriptionIsActiveForProductWithId(string productId) {
			if (!IsInitialized)
				return false;

			var product = _storeController.products.WithID(productId);
			return product != null && SubscriptionIsActiveForProduct(product);
		}

		// ---------------------------------------------------------------
		// Helpers

		bool SubscriptionIsActiveForProduct(Product product) {
			if (product.receipt == null || product.definition.type != ProductType.Subscription)
				return false;

			if (!CheckIfProductIsAvailableForSubscriptionManager(product.receipt))
				return false;

			string introJson = (_introductoryInfoDict == null || !_introductoryInfoDict.ContainsKey(product.definition.storeSpecificId)) ? null : _introductoryInfoDict[product.definition.storeSpecificId];
			var subscriptionManager = new SubscriptionManager(product, introJson);
			var info = subscriptionManager.getSubscriptionInfo();
			return info.isSubscribed() == Result.True;
		}

		bool CheckIfProductIsAvailableForSubscriptionManager(string receipt) {
			var receiptWrapper = (Dictionary<string, object>)Json.Deserialize(receipt);
			if (!receiptWrapper.ContainsKey("Store") || !receiptWrapper.ContainsKey("Payload")) {
				Debug.Log("The product receipt does not contain enough information");
				return false;
			}

			string store = (string)receiptWrapper["Store"];
			string payload = (string)receiptWrapper["Payload"];

			if (payload == null)
				return false;

			switch (store) {
				case GooglePlay.Name: {
						var payloadWrapper = (Dictionary<string, object>)Json.Deserialize(payload);
						if (!payloadWrapper.ContainsKey("json")) {
							Debug.Log(
								"The product receipt does not contain enough information, the 'json' field is missing");
							return false;
						}

						// var originalJsonPayloadWrapper =
						// 	(Dictionary<string, object>) Json.Deserialize((string) payloadWrapper["json"]);
						// if (originalJsonPayloadWrapper == null ||
						//     !originalJsonPayloadWrapper.ContainsKey("developerPayload")) {
						// 	Debug.Log(
						// 		"The product receipt does not contain enough information, the 'developerPayload' field is missing");
						// 	return false;
						// }
						//
						// string developerPayloadJSON = (string) originalJsonPayloadWrapper["developerPayload"];
						// var developerPayloadWrapper =
						// 	(Dictionary<string, object>) Json.Deserialize(developerPayloadJSON);
						// if (developerPayloadWrapper == null ||
						//     !developerPayloadWrapper.ContainsKey("is_free_trial") ||
						//     !developerPayloadWrapper.ContainsKey("has_introductory_price_trial")) {
						// 	Debug.Log(
						// 		"The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
						// 	return false;
						// }

						return true;
					}
				case AppleAppStore.Name:
				case AmazonApps.Name:
				case MacAppStore.Name: {
						return true;
					}
				default: {
						return false;
					}
			}
		}

		// ---------------------------------------------------------------
		// IStoreListener

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
			Debug.Log("IAP initialization succeed.");

			_storeController = controller;
			_extensionProvider = extensions;
			_isInitialized = true;

#if UNITY_IOS
			var appleExtensions = _extensionProvider.GetExtension<IAppleExtensions>();
			_introductoryInfoDict = appleExtensions.GetIntroductoryPriceDictionary();
#endif

			PrintAllProductsToConsole();
		}

		public void OnInitializeFailed(InitializationFailureReason error) {
			Debug.Log("IAP initialization failed. Error: " + error);
			_isInitialized = false;
		}

		public void OnInitializeFailed(InitializationFailureReason error, string? message) {
			Debug.Log("IAP initialization failed. Error: " + error);
			_isInitialized = false;
		}

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e) {
			var product = e.purchasedProduct;

			_productProvider.ProvideProduct(product.definition.id);

			_purchaseCallback?.Invoke(product.definition.id, true, null, product);
			ClearProductPurchaseCallback();

			return PurchaseProcessingResult.Complete;
		}

		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
			var errorType = failureReason == PurchaseFailureReason.UserCancelled
				? IAPManager.PurchaseError.ErrorType.UserCancelled
				: IAPManager.PurchaseError.ErrorType.Other;
			string message = failureReason.ToString();

			Debug.Log("Product purchase failed. Product: " + product.definition);
			Debug.Log("Reason: " + failureReason);
			_purchaseCallback?.Invoke(product.definition.id, false, new IAPManager.PurchaseError(errorType, message));
			ClearProductPurchaseCallback();
		}


	}
}

#endif
