using System;
using UnityEngine;

namespace RModule.Runtime.IAP {
	[Serializable]
	public class StoreProductData : IAPProductData {
		public enum Category { RemoveAds = 0 }

		// Accessors
		public Category ProductCategory => _category;
		public PurchaseMethod ProductPurchaseMethod => _purchaseMethod;
		public bool IsRemoveAdsProduct => isRemoveAdsProduct;
		public Sprite TitleImage => _titleImage;
		public string TitleImageAddress => _titleImageAddress;
		public int Quantity => _quantity;
		public bool HideWhenPurchased => _hideWhenPurchased;
		public bool IsVisible => _isActive && _isVisible;
		public bool ShowProfitablyMark => _showProfitablyMark;

		// Outlets
		[SerializeField] Category _category = Category.RemoveAds;
		[SerializeField] PurchaseMethod _purchaseMethod = PurchaseMethod.RealCurrency;
		[SerializeField] bool isRemoveAdsProduct = default;
		[SerializeField] Sprite _titleImage = default;
		[SerializeField] int _quantity = default;
		[SerializeField] string _titleImageAddress = default;
		[SerializeField] bool _hideWhenPurchased = default;
		[SerializeField] bool _isVisible = true;
		[SerializeField] bool _isActive = true;
		[SerializeField] bool _showProfitablyMark = default;

		// Private vars

		// Const
		public const string DisableAds = "disable_ads";


		// ---------------------------------------------------------------
		// Helpers

#if USE_IAP
		public string GetPriceString() {
			if (_purchaseMethod == PurchaseMethod.RealCurrency) {
				return IAPManager.Instance.PriceStringForProductId(ProductId, LocalizationManager.Instance.CurrentLanguage);
			}

			return "";
		}
#endif
	}
}
