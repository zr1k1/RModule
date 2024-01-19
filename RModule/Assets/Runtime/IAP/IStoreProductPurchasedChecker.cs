namespace RModule.Runtime.IAP {
	public interface IStoreProductPurchasedChecker {
		bool ProductIsPurchased(string productId);
		bool ProductIsPurchased(IStoreProductData storeProductData);
	}
}
