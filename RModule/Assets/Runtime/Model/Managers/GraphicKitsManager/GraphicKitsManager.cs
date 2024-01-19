using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

public class GraphicKitsManager : SingletonMonoBehaviour<GraphicKitsManager> {//, ILocationGraphicKitsManager {
	//// Accessors
	//public string CurrentHeroSkinKey => _currentHeroSkinKey;
	//public GraphicKitDataConfig CurrentHeroGraphicKitConfig => _currentHeroGraphicKitConfig;

	//// Outlets 
	//[SerializeField] HeroGraphicKitsConfig _heroGraphicKitsConfig = default;
	//[SerializeField] LocationsGraphicKitsConfig _locationsGraphicKitsConfig = default;
	//[SerializeField] string _currentHeroSkinKey = default;
	//[SerializeField] string _currentLocationKey = default;

	//[Header("Debug")]
	//[SerializeField] bool _useDebugKeys = default;
	//[SerializeField] string _debugHeroSkinKey = default;
	//[SerializeField] string _debugLocationKey = default;

	//// Private vars
	//PlayerData _playerData;
	//GraphicKitDataConfig _currentHeroGraphicKitConfig;
	//GraphicKitDataConfig _currentLocationGraphicKitConfig;
	//List<ILocationGraphicKitsElement> _locationGraphicKitsElements = new List<ILocationGraphicKitsElement>();
	//List<Sprite> _loadedHeroSkinSprites = new List<Sprite>();

	//bool _initializeFinished = false;

	//// Init
	//public override IEnumerator Initialize() {
	//	_playerData = GameDataManager.Instance.PlayerData;
	//	SetupSkin(_useDebugKeys && Application.isEditor ? _debugHeroSkinKey : _playerData.HeroGraphicKitKey);
	//	SetupLocation(_useDebugKeys && Application.isEditor ? _debugLocationKey : _locationsGraphicKitsConfig.GraphicKitConfigs[0].Key);

	//	_initializeFinished = true;

	//	yield return null;
	//	Debug.Log("GraphicKitsManager : Initialized");
	//}

	//public override bool IsInitialized() {
	//	return _initializeFinished;
	//}

	//void SetupSkin(string heroSkinKey) {
	//	_currentHeroSkinKey = heroSkinKey;
	//	_currentHeroGraphicKitConfig =
	//		_heroGraphicKitsConfig.GetGraphicKitConfigByNameKeyOrDefault(_currentHeroSkinKey);
	//}

	//void SetupLocation(string locationKey) {
	//	_currentLocationKey = locationKey;
	//	_currentLocationGraphicKitConfig =
	//		_locationsGraphicKitsConfig.GetGraphicKitConfigByNameKeyOrDefault(_currentLocationKey);
	//}

	//public void ChangeSaveHeroSkin(string heroSkinKey) {
	//	SetupSkin(heroSkinKey);

	//	GameDataManager.Instance.ChangeSaveHeroSkin(heroSkinKey);
	//}

	//public void ChangeToNextAndSaveHeroSkin() {
	//	ChangeSaveHeroSkin(_heroGraphicKitsConfig.GetNextGraphicKitConfigByNameKeyOrDefault(_currentHeroSkinKey).Key);
	//}

	//public void TryUpdateLocationGraphicKit(int levelNumber) {
	//	Debug.Log($"GraphicKitsManager : TryUpdateLocationGraphicKit");
	//	var graphicKitDataConfig = _locationsGraphicKitsConfig.GetGraphicKitByLevelNumber(levelNumber);
	//	string locationKey = graphicKitDataConfig.Key;
	//	if (locationKey != _currentLocationKey) {
	//		SetupLocation(locationKey);
	//		UpdateElementsView();
	//	}
	//}

	//// Gettees
	//public bool TryGetRandomHeroGraphicKitKeyNotInCollection(out GraphicKitDataConfig randomHeroSkinKeyNotInCollection) {
	//	randomHeroSkinKeyNotInCollection = _currentHeroGraphicKitConfig;

	//	var allHeroSkinKeysNotInCollection = _heroGraphicKitsConfig.GraphicKitConfigs.FindAll(
	//		graphicKitConfig => !_playerData.HeroGraphicKitKeysCollection.Contains(graphicKitConfig.Key));

	//	if (allHeroSkinKeysNotInCollection.Count > 0) {
	//		randomHeroSkinKeyNotInCollection = allHeroSkinKeysNotInCollection[Random.Range(0, allHeroSkinKeysNotInCollection.Count)];

	//		return true;
	//	}

	//	return false;
	//}

	//public bool TryGetSkinPreviewSprite(string graphicConfigKey, Action<Sprite> spriteLoadedCallback) {
	//	if (_heroGraphicKitsConfig.GetGraphicKitConfigByNameKeyOrDefault(graphicConfigKey).TryGetParameter("previewSpriteAddress", out string address)) {
	//		new AddressableAssetProvider().LoadAsset(address, spriteLoadedCallback);
	//		return true;
	//	}

	//	return false;
	//}
	//public bool TryGetAllNotInCollectionSkin(Action<List<Sprite>> spritesLoadedCallback) {
	//	var notInCollectionConfigs = _heroGraphicKitsConfig.GraphicKitConfigs.FindAll(config => !_playerData.HeroGraphicKitKeysCollection.Contains(config.Key)).ToList();
	//	if (notInCollectionConfigs.Count == 0)
	//		return false;

	//	_loadedHeroSkinSprites.Clear();
	//	foreach (var notInCollectionConfig in notInCollectionConfigs) {
	//		new AddressableAssetProvider().LoadAsset<Sprite>(
	//			notInCollectionConfig.GetSkinPreviewAddress()
	//			, (sprite) => { _loadedHeroSkinSprites.Add(sprite); });
	//	}
	//	StartCoroutine(WaitForAllSpritesLoaded(notInCollectionConfigs.Count, spritesLoadedCallback));

	//	return true;
	//}

	//IEnumerator WaitForAllSpritesLoaded(int notInCollectionConfigsCount, Action<List<Sprite>> spritesLoadedCallback) {
	//	while (_loadedHeroSkinSprites.Count < notInCollectionConfigsCount)
	//		yield return null;

	//	spritesLoadedCallback?.Invoke(_loadedHeroSkinSprites);
	//}

	////ILocationGraphicKitsManager
	//public void AddLocationElement(ILocationGraphicKitsElement locationGraphicKitsElement) {
	//	if(!_locationGraphicKitsElements.Contains(locationGraphicKitsElement))
	//		_locationGraphicKitsElements.Add(locationGraphicKitsElement);
	//}

	//public void RemoveLocationElement(ILocationGraphicKitsElement locationGraphicKitsElement) {
	//	if (_locationGraphicKitsElements.Contains(locationGraphicKitsElement))
	//		_locationGraphicKitsElements.Remove(locationGraphicKitsElement);
	//}

	//public void UpdateElementsView() {
	//	foreach (var element in _locationGraphicKitsElements)
	//		element.UpdateView();
	//}

	//public string GetLocationSpriteAddress(string key) {
	//	return _locationsGraphicKitsConfig.GetSpriteAddress<string>(_currentLocationKey, key);
	//}

	//public Color GetLocationColor(string key) {
	//	return _locationsGraphicKitsConfig.GetSpriteAddress<Color>(_currentLocationKey, key);
	//}
	public IEnumerator Initialize() {
		throw new NotImplementedException();
	}

	public override bool IsInitialized() {
		throw new NotImplementedException();
	}
}
