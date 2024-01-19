namespace RModule.Runtime.IAP {
	public interface IStoreProductsProvider {
		void ProvideProduct(string productId);
		IProductsDatabase ProductsDatabase { get; }
		bool ProductIsPurchased(IStoreProductData storeProductData);
	}
}
