using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BaseGameDataManager<PlayerDataValuesNames> : SingletonMonoBehaviour<BaseGameDataManager<PlayerDataValuesNames>>, IInitializable
	where PlayerDataValuesNames : Enum {
	// Accessors
	public static Data<PlayerDataValuesNames> PlayerData => Instance._playerDataProvider.Data;
	//public StoreProductsProvider StoreProductsProvider => _storeProductsProvider;
	//public NotificationsService NotificationsService => _notificationsService;
	//public PlayerDataProvider PlayerDataProvider => _playerDataProvider;
	//public PlayerData PlayerData => _playerDataProvider.PlayerData;
	//public GameProgressDataProvider GameProgressDataProvider => _gameProgressDataProvider;
	//public GameProgressData GameProgressData => _gameProgressDataProvider.ProgressData;
	//public AppEconomicsData AppEconomicsData => _appEconomicsData;
	//public GameConfig GameConfig => _gameConfig;

	// Outlets 
	//[SerializeField] StoreProductsProvider _storeProductsProvider = new StoreProductsProvider();
	//[SerializeField] NotificationsService _notificationsService = new NotificationsService();
	[SerializeField] protected PersistentSavedDataConfig<PlayerDataValuesNames> _playerConfig = default;
	//[SerializeField] GameConfig _gameConfig = default;
	//[SerializeField] AppEconomicsData _appEconomicsData = default;

	// Private vars
	//PlayerDataProvider _playerDataProvider = new PlayerDataProvider();
	//GameProgressDataProvider _gameProgressDataProvider = new GameProgressDataProvider();
	protected DataProvider<PlayerDataValuesNames, PersistentSavedDataConfig<PlayerDataValuesNames>> _playerDataProvider;
	protected bool _dataPreparingFinished = false;

	// Init
	public virtual IEnumerator Initialize() {
    	_dataPreparingFinished = false;

		PrepareData();

		_dataPreparingFinished = true;
		yield return WaitForInitialized();
		//ExampleSettingsManager.Instance.GetSettingValue(ExampleSetting.SoundEffectsVolume);

		//_notificationsService.Initialize();
		//if (!AllLevelsIsComplete()) {
		//	_notificationsService.ScheduleNotifications(NotificationsConfigData.NotificationName.ComeBackToTheGame);
		//}

		Debug.Log("GameDataManager Initialized");
		DebugActions();
	}

	public override bool IsInitialized() {
		return _dataPreparingFinished;
	}

	void OnDisable() {
	}

	// ---------------------------------------------------------------
	// General Methods

	protected virtual void PrepareData() {
		Debug.Log($"GameDataManager : PrepareData");
		_playerDataProvider = new DataProvider<PlayerDataValuesNames, PersistentSavedDataConfig<PlayerDataValuesNames>>(_playerConfig);
		_playerDataProvider.LoadData();

		//_gameProgressDataProvider.LoadData();
	}

	// Resets
	public static void ResetProgress() {
		var dataPath = Path.Combine(Application.persistentDataPath, GameProgressData.FOLDER_NAME);
		Debug.Log($"ResetProgress FROM PATH = {dataPath}");
		DirectoryInfo di = new DirectoryInfo(dataPath);
		if (di.Exists) {
			Debug.Log($"ResetProgress progress data exist");
			di.Delete(true);
			//Instance?._gameProgressDataProvider.LoadData();
		}
	}

	//public static void ResetPlayerData() {
	//	PlayerDataProvider.Reset();
	//	if (Instance != null)
	//		Instance.LoadPlayerAndProgressData();
	//}

	//public static int GetLevelsCount() {
	//	return Instance._gameConfig.LevelsCount;
	//}

	// Debug Actions
	public virtual void DebugActions() {
		if (Application.isEditor) {
		}
	}

	//public void SetLevelsPassed(int levelsPassed) {
	//	_gameProgressDataProvider.ProgressData.SetLevelsPassed(levelsPassed);
	//	_gameProgressDataProvider.SaveData();
	//}

	//public void SetAllLevelsComplete() {
	//	_gameProgressDataProvider.ProgressData.SetLevelsPassed(GetLevelsCount());
	//	_gameProgressDataProvider.SaveData();
	//}

	//public void RestoreNonConsumablesFromPlayerData() {
	//	_storeProductsProvider.RestoreNonConsumablesFromPlayerData(PlayerData);
	//}
}
