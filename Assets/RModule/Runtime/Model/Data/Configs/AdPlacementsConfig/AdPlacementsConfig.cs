using System;
using UnityEngine;

// Create your "CustomPlacementType" enum with placements for app
// Create a class and inherit from AdPlacementsConfig<CustomPlacementType>
namespace RModule.Runtime.Data.Configs {

	public class AdPlacementsConfig<PlacementEnum> : ScriptableObject {
		[Serializable] public class PlacementStringDictionary : SerializableDictionary<PlacementEnum, Placement> { }

		// Outlets
		[SerializeField] protected PlacementStringDictionary _placements = default;

		// ---------------------------------------------------------------
		// Accessors

		public string GetPlacement(PlacementEnum placementType) {
			if (!_placements.Contains(placementType)) {
				Debug.LogError($"PlacementType {placementType} is not exist");
			} else if (string.IsNullOrEmpty(_placements[placementType].Key)) {
				Debug.LogError($"Key for placementType {placementType} is empty");
			}

			return _placements.ContainsKey(placementType) ? _placements[placementType].Key : "";
		}

		public string GetRewardedPlacementByStoreProductId(string storeProductId) {
			foreach (var placement in _placements)
				if (placement.Value.StoreProductId == storeProductId)
					return placement.Value.Key;

			Debug.LogWarning($"Key for store product id = {storeProductId} is not exist");

			return "";
		}
	}

	[Serializable]
	public class Placement {
		public string Key = default;
		public bool IsRewarded = default;
		public string StoreProductId = default;
	}
}