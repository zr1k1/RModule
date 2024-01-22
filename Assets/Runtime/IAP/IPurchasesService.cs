using System.Collections.Generic;
using UnityEngine;

#if USE_IAP

namespace RModule.Runtime.IAP {
	public interface IPurchasesService {
		bool IsInitialized { get; }
		IStoreProductsProvider storeProductsProvider { get; }
		void Initialize(IStoreProductsProvider storeProductsProvider);
		void PurchaseProduct(string productId, IAPManager.PurchaseCompleteDelegate purchaseCallback);
		void RestorePurchases(IAPManager.RestorePurchasesCompleteDelegate restorePurchaseComplete);
		string PriceStringForProductId(string productId, SystemLanguage locale);
		string ProductCurrencyCode(string productId);
		decimal LocalizedPrice(string productId);
		bool ProductIsPurchased(string productId);
		void PrintAllProductsToConsole();
		void ClearProductPurchaseCallback();
		void ClearProductsRestoreCallback();
		List<string> GetActiveSubscriptionsProductIds();
		bool SubscriptionIsActiveForProductWithId(string productId);
	}
}

#endif