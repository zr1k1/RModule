using System.Collections.Generic;

namespace RModule.Runtime.IAP {
	public interface IProductsDatabase {
		IEnumerable<IAPProductData> AllIapProducts { get; }
		IStoreProductData GetProductWithId(string productId);
	}
}
