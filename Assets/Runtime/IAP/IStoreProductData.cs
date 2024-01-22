namespace RModule.Runtime.IAP {
	public interface IStoreProductData {
		string ProductId { get; }
		ProductType ProductType { get; }
		ProductPurchaseMethod PurchaseMethod { get; }
		bool IsVisible();
		bool HideWhenPurchased();
		bool IsRemoveAdsProduct();
	}
}
