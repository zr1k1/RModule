using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalizationTextDataConfig", menuName = "AppConfigs/Localization/LocalizationTextDataConfig")]
public class LocalizationTextDataConfig : ScriptableObject {

	// Accessors
	[SerializeField] protected string _key = default;
	[SerializeField] protected StoreTypeStringDictionary _storeTypeStringDictionary = default;

	public virtual string GetKey() {
		string key = _key;

		if (_storeTypeStringDictionary.Count > 0) {
#if USE_YG
			if (_storeTypeStringDictionary.ContainsKey(StoreIds.StoreType.YandexGames)) {
				key = _storeTypeStringDictionary[StoreIds.StoreType.YandexGames];
			}
#endif
		}

		return key;
	}
}
