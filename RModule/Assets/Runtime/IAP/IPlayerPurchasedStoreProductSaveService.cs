namespace RModule.Runtime.IAP {
	public interface IPlayerPurchasedStoreProductSaveService : IStoreProductPurchasedChecker {
		void AddProductKeyToPurchased(string productId);
		void SetEnableAds(bool enable);
	}
}
