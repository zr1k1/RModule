using System;
using UnityEngine;

namespace RModule.Runtime.IAP {
	[Serializable]
	public class IAPProductData {

		[SerializeField] string _productId;
		[SerializeField] StoreIds _storeIds;
		[SerializeField] ProductType _productType;
		[SerializeField] Sprite _storeImage;
		[SerializeField] string _title;

		// --- Accessors ---
		public string ProductId => _productId;
		public ProductType ProductType => _productType;
		public Sprite StoreImage => _storeImage;
		public string Title => _title;

		// ---------------------------------------------------------------
		// General methods

		public string GetStoreProductId(StoreIds.StoreType storeType) {
			var storeProductId = "";
			switch (storeType) {
				case StoreIds.StoreType.AppStore:
					storeProductId = _storeIds.AppStoreProductId;
					break;
				case StoreIds.StoreType.GooglePlay:
					storeProductId = _storeIds.GooglePlayProductId;
					break;
				case StoreIds.StoreType.AppGallery:
					storeProductId = _storeIds.AppGalleryProductId;
					break;
				case StoreIds.StoreType.GalaxyStore:
					storeProductId = _storeIds.GalaxyStoreProductId;
					break;
			}

			return storeProductId;
		}
	}

	[Serializable]
	public class StoreIds {
		public enum StoreType { AppStore, GooglePlay, AppGallery, GalaxyStore }

		[SerializeField] string _appStoreProductId;
		[SerializeField] string _googlePlayProductId;
		[SerializeField] string _appGalleryProductId;
		[SerializeField] string _galaxyStoreProductId;

		// --- Accessors ---
		public string AppStoreProductId => _appStoreProductId;
		public string GooglePlayProductId => _googlePlayProductId;
		public string AppGalleryProductId => _appGalleryProductId;
		public string GalaxyStoreProductId => _galaxyStoreProductId;
	}
}