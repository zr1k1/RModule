using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] public class StarIdStarDataDictionary : SerializableDictionary<int, StarsConfig.Data> { }

[CreateAssetMenu(fileName = "StarsConfig", menuName = "AppConfig/StarsConfig", order = 2)]
public class StarsConfig : ScriptableObject {
	// Outlets
	[SerializeField] StarIdStarDataDictionary _starIdSpriteDictionary = default;
	[SerializeField] Sprite _dummySprite = default;

	// Classes
	[Serializable]
	public class Data {
		public Sprite disabledSprite;
		public Sprite uiEnabledSprite;
		public Sprite ingameSprite;
	}

	public Sprite GetDisabledSpriteById(int id) {
		if (_starIdSpriteDictionary.ContainsKey(id)) {
			return _starIdSpriteDictionary[id].disabledSprite;
		} else {
			Debug.LogError($"id={id} not present on _starIdSpriteDictionary");
			return _dummySprite;
		}
	}

	public Sprite GetUISpriteById(int id) {
		if (_starIdSpriteDictionary.ContainsKey(id)) {
			return _starIdSpriteDictionary[id].uiEnabledSprite;
		} else {
			Debug.LogError($"id={id} not present on _starIdSpriteDictionary");
			return _dummySprite;
		}
	}

	public Sprite GetIngameSpriteById(int id) {
		if (_starIdSpriteDictionary.ContainsKey(id)) {
			return _starIdSpriteDictionary[id].ingameSprite;
		} else {
			Debug.LogError($"id={id} not present on _starIdSpriteDictionary");
			return _dummySprite;
		}
	}
}
