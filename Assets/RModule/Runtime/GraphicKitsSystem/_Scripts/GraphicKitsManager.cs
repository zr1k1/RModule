using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace RModule.Runtime.GraphicKitsSystem {

	public class GraphicKitsManager : SingletonMonoBehaviour<GraphicKitsManager>, ILocationGraphicKitsManager {
		// Accessors
		public string CurrentHeroSkinKey => _currentHeroSkinKey;
		public GraphicKitConfig CurrentHeroGraphicKitConfig => _currentHeroGraphicKitConfig;

		// Outlets 
		[SerializeField] HeroGraphicKitsConfig _heroGraphicKitsConfig = default;
		[SerializeField] LocationsGraphicKitsConfig _locationsGraphicKitsConfig = default;
		[SerializeField] string _currentHeroSkinKey = default;
		[SerializeField] string _currentLocationKey = default;

		[Header("Debug")]
		[SerializeField] bool _useDebugKeys = default;
		[SerializeField] string _debugHeroSkinKey = default;
		[SerializeField] string _debugLocationKey = default;

		// Private vars
		IGraphicKitDataProvider _graphicKitDataProvider;
		GraphicKitConfig _currentHeroGraphicKitConfig;
		GraphicKitConfig _currentLocationGraphicKitConfig;
		List<ILocationGraphicKitsElement> _locationGraphicKitsElements = new List<ILocationGraphicKitsElement>();
		List<Sprite> _loadedHeroSkinSprites = new List<Sprite>();

		bool _initializeFinished = false;

		public interface IGraphicKitDataProvider : IValueSaver<string> {
			string GetHeroGraphicKitKey();
			List<string> GetHeroGraphicKitKeysCollection();
		}

		// Init
		public IEnumerator Initialize(IGraphicKitDataProvider graphicKitDataProvider) {
			//_playerData = playerData;
			_graphicKitDataProvider = graphicKitDataProvider;
			SetupSkin(_useDebugKeys && Application.isEditor ? _debugHeroSkinKey : _graphicKitDataProvider.GetHeroGraphicKitKey());
			SetupLocation(_useDebugKeys && Application.isEditor ? _debugLocationKey : _locationsGraphicKitsConfig.GraphicKitConfigs[0].Key);

			_initializeFinished = true;

			yield return null;
			Debug.Log("GraphicKitsManager : Initialized");
		}

		public override bool IsInitialized() {
			return _initializeFinished;
		}

		void SetupSkin(string heroSkinKey) {
			_currentHeroSkinKey = heroSkinKey;
			_currentHeroGraphicKitConfig = _heroGraphicKitsConfig.GetGraphicKitConfigByNameKeyOrDefault(_currentHeroSkinKey);
		}

		void SetupLocation(string locationKey) {
			_currentLocationKey = locationKey;
			_currentLocationGraphicKitConfig = _locationsGraphicKitsConfig.GetGraphicKitConfigByNameKeyOrDefault(_currentLocationKey);
		}

		public void ChangeSaveHeroSkin(string heroSkinKey) {
			SetupSkin(heroSkinKey);

			//GameDataManager.Instance.ChangeSaveHeroSkin(heroSkinKey);
			_graphicKitDataProvider.TrySave(heroSkinKey);
		}

		public void ChangeToNextAndSaveHeroSkin() {
			ChangeSaveHeroSkin(_heroGraphicKitsConfig.GetNextGraphicKitConfigByNameKeyOrDefault(_currentHeroSkinKey).Key);
		}

		public void TryUpdateLocationGraphicKit(int levelNumber) {
			Debug.Log($"GraphicKitsManager : TryUpdateLocationGraphicKit");
			var graphicKitDataConfig = _locationsGraphicKitsConfig.GetGraphicKitByLevelNumber(levelNumber);
			string locationKey = graphicKitDataConfig.Key;
			if (locationKey != _currentLocationKey) {
				SetupLocation(locationKey);
				UpdateElementsView();
			}
		}

		// Gettees
		public bool TryGetRandomHeroGraphicKitKeyNotInCollection(out GraphicKitConfig randomHeroSkinKeyNotInCollection) {
			randomHeroSkinKeyNotInCollection = _currentHeroGraphicKitConfig;

			var allHeroSkinKeysNotInCollection = _heroGraphicKitsConfig.GraphicKitConfigs.FindAll(
				graphicKitConfig => !_graphicKitDataProvider.GetHeroGraphicKitKeysCollection().Contains(graphicKitConfig.Key));

			if (allHeroSkinKeysNotInCollection.Count > 0) {
				randomHeroSkinKeyNotInCollection = allHeroSkinKeysNotInCollection[Random.Range(0, allHeroSkinKeysNotInCollection.Count)];

				return true;
			}

			return false;
		}

		public bool TryGetSkinPreviewSprite(string graphicConfigKey, Action<Sprite> spriteLoadedCallback) {
			if (_heroGraphicKitsConfig.GetGraphicKitConfigByNameKeyOrDefault(graphicConfigKey).TryGetParameter("previewSpriteAddress", out string address)) {
				new AddressableAssetProvider().LoadAsset(address, spriteLoadedCallback);
				return true;
			}

			return false;
		}

		//ILocationGraphicKitsManager
		public void AddLocationElement(ILocationGraphicKitsElement locationGraphicKitsElement) {
			if (!_locationGraphicKitsElements.Contains(locationGraphicKitsElement))
				_locationGraphicKitsElements.Add(locationGraphicKitsElement);
		}

		public void RemoveLocationElement(ILocationGraphicKitsElement locationGraphicKitsElement) {
			if (_locationGraphicKitsElements.Contains(locationGraphicKitsElement))
				_locationGraphicKitsElements.Remove(locationGraphicKitsElement);
		}

		public void UpdateElementsView() {
			foreach (var element in _locationGraphicKitsElements)
				element.UpdateView();
		}

		public string GetLocationSpriteAddress(string key) {
			return _locationsGraphicKitsConfig.GetSpriteAddress<string>(_currentLocationKey, key);
		}

		public Color GetLocationColor(string key) {
			return _locationsGraphicKitsConfig.GetSpriteAddress<Color>(_currentLocationKey, key);
		}
	}
}