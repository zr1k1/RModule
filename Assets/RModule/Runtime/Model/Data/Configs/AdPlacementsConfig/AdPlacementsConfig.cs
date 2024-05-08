using System;
using UnityEngine;

// Create your "CustomPlacementType" enum with placements for app
// Create a class and inherit from AdPlacementsConfig<CustomPlacementType>
namespace RModule.Runtime.Data.Configs {

	public interface IPlacementsContainer<PlacementEnum> {
		public string GetPlacement(PlacementEnum placementType);
	}

	public class AdPlacementsConfig<PlacementEnum> : BaseConfig<PlacementEnum>, IPlacementsContainer<PlacementEnum> where PlacementEnum : Enum {

		// ---------------------------------------------------------------
		// Accessors

		public string GetPlacement(PlacementEnum placementType) {
			if (!_valuesDict.Contains(placementType)) {
				Debug.LogError($"PlacementType {placementType} is not exist");
			} else if (string.IsNullOrEmpty(_valuesDict[placementType].GetValue<PlacementData>().Key)) {
				Debug.LogError($"Key for placementType {placementType} is empty");
			}

			return _valuesDict.ContainsKey(placementType) ? _valuesDict[placementType].GetValue<PlacementData>().Key : "";
		}

		public string GetRewardedPlacementByStoreProductId(string storeProductId) {
			foreach (var keyPair in _valuesDict)
				if (keyPair.Value.GetValue<PlacementData>().StoreProductId == storeProductId)
					return keyPair.Value.GetValue<PlacementData>().Key;

			Debug.LogWarning($"Key for store product id = {storeProductId} is not exist");

			return "";
		}
	}
}