namespace RModule.Runtime.IAP {
	public interface IIAPService : IStoreProductPurchasedChecker {
		bool IsReady();
		void PurchaseProduct(IStoreProductData productData, PurchaseCompleteDelegate purchaseFinish);
		string GetPriceString(IStoreProductData storeProductData);
	}
}
