using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerData {
	// Accessors
	public string DeviceName => _deviceName;
	public int Coins => _coins;
	public bool DisableAds => _disableAds;
	public string HeroGraphicKitKey => _heroGraphicKitKey;
	public Dictionary<string, int> PurchasedProducts => _purchasedProducts;
	public List<string> HeroGraphicKitKeysCollection => _heroGraphicKitKeysCollection;

	// Private vars
	string _deviceName;
	int _coins;
	bool _disableAds;
	string _heroGraphicKitKey;
	List<string> _heroGraphicKitKeysCollection;
	Dictionary<string, int> _purchasedProducts; // <"productKey" : qty>

	public PlayerData(int coins, bool disableAds, string heroGraphicKitKey,
		List<string> heroGraphicKitKeysCollection, Dictionary<string, int> purchasedProducts) {

		_deviceName = SystemInfo.deviceModel;
		_coins = coins;
		_disableAds = disableAds;
		_heroGraphicKitKey = heroGraphicKitKey;
		_heroGraphicKitKeysCollection = new List<string>(heroGraphicKitKeysCollection);
		_purchasedProducts = purchasedProducts;
	}

	public void ChangeCoins(int amount) {
		_coins += amount;
	}

	public void SetEnableAds(bool isEnable) {
		_disableAds = !isEnable;
	}

	public void SetHeroGraphicKitKey(string heroGraphicKitKey) {
		_heroGraphicKitKey = heroGraphicKitKey;
	}

	public void TryAddHeroGraphicKitKeyToCollection(string heroGraphicKitKey) {
		if (!_heroGraphicKitKeysCollection.Contains(heroGraphicKitKey)) {
			_heroGraphicKitKeysCollection.Add(heroGraphicKitKey);
		}
	}

	public void AddProductKeyToPurchased(string productKey) {
		if (_purchasedProducts.ContainsKey(productKey))
			_purchasedProducts[productKey] = 1;
		else
			_purchasedProducts.Add(productKey, 1);
	}

	public void RemoveProductKeyToPurchased(string productKey) {
		if (_purchasedProducts.ContainsKey(productKey))
			_purchasedProducts[productKey] = 0;
	}

	public bool ProductIsPurchased(string productKey) {
		int productQty = 0;
		if (_purchasedProducts.ContainsKey(productKey))
			_purchasedProducts.TryGetValue(productKey, out productQty);
		return productQty > 0;
	}

	public static PlayerData CreateDefaultPlayerData(PlayerConfig playerConfig) {
		return new PlayerData(0, false, "kDefault", new List<string> { "kDefault" }, new Dictionary<string, int>());
	}

	// JSON Generation

	public class JPlayerData {
		public int coins;
		public bool disableAds;
		public string heroGraphicKitKey;
		public List<string> heroGraphicKitKeysCollection = new List<string>();
		public Dictionary<string, int> purchasedProducts = new Dictionary<string, int>(); // <"productKey" : qty>
	}

	public string GenerateJsonAndEncodeData() {
		Debug.Log($"GenerateJsonAndEncodeData");
		string serializedDict = generateJsonDataString();

		//// Encode
		//var encodedBytes = Encoding.UTF8.GetBytes(serializedDict);
		//string encodedDataString = Convert.ToBase64String(encodedBytes);

		string encodedDataString = serializedDict;

		return encodedDataString;
	}

	public static PlayerData DecodeJsonAndGenerateGameData(string encodedDataString) {
		Debug.Log($"DecodeData {encodedDataString}");
		if (string.IsNullOrEmpty(encodedDataString))
			return null;

		//// Decode
		//var decodedBytes = Convert.FromBase64String(encodedDataString);
		//string decodedText = Encoding.UTF8.GetString(decodedBytes);

		string decodedText = encodedDataString;

		JPlayerData jPlayerData = JsonConvert.DeserializeObject<JPlayerData>(decodedText);
		PlayerData playerData = new PlayerData(jPlayerData.coins, jPlayerData.disableAds,
			jPlayerData.heroGraphicKitKey, jPlayerData.heroGraphicKitKeysCollection,
			jPlayerData.purchasedProducts);

		return playerData;
	}

	string generateJsonDataString() {
		JPlayerData jPlayerData = new JPlayerData();
		jPlayerData.coins = _coins;
		jPlayerData.disableAds = _disableAds;
		jPlayerData.heroGraphicKitKey = _heroGraphicKitKey;
		jPlayerData.heroGraphicKitKeysCollection = _heroGraphicKitKeysCollection;
		jPlayerData.purchasedProducts = _purchasedProducts;

		return JsonConvert.SerializeObject(jPlayerData, Formatting.Indented);
	}
}
