using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StoreIds {
	public enum StoreType { AppStore, GooglePlay, AppGallery, GalaxyStore, YandexGames }

	// --- Accessors ---
	public string AppStoreProductId => _appStoreProductId;
	public string GooglePlayProductId => _googlePlayProductId;
	public string AppGalleryProductId => _appGalleryProductId;
	public string GalaxyStoreProductId => _galaxyStoreProductId;
	public string YandexGamesStoreProductId => _yandexGamesStoreProductId;

	[SerializeField] string _appStoreProductId;
	[SerializeField] string _googlePlayProductId;
	[SerializeField] string _appGalleryProductId;
	[SerializeField] string _galaxyStoreProductId;
	[SerializeField] string _yandexGamesStoreProductId;
}
