using System;
using UnityEngine;

// Create your "CustomGameItem" enum with placements for app
// Create a class and inherit from AppEconomics<CustomGameItem>
// For additional conditions to get price value create SomeHardPriceConfig and inherit PriceConfig. drug in inspector
public class AppEconomicsConfig<PurchasableGameItem> : ScriptableObject {
	[Serializable] public class GameItemPriceDictionary : SerializableDictionary<PurchasableGameItem, PriceConfig> { }

	// Outlets
	[SerializeField] GameItemPriceDictionary _gameItemPriceDictionary = default;

	public virtual bool TryGetGameItemPrice(PurchasableGameItem item, out int price) {
		price = 0;
		if (!_gameItemPriceDictionary.ContainsKey(item)) {
			Debug.LogError($"Price for game item {item} is not exist");
			return false;
		}

		price = _gameItemPriceDictionary[item].GetValue();
		return true;
	}
}