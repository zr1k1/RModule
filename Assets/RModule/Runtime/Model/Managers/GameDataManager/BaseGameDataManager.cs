﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BaseGameDataManager<PlayerDataValuesNames> : SingletonMonoBehaviour<BaseGameDataManager<PlayerDataValuesNames>>, IInitializable
	where PlayerDataValuesNames : Enum {
	// Accessors
	public static Data<PlayerDataValuesNames> PlayerData => Instance._playerDataProvider.Data;

	// Outlets 
	[SerializeField] protected PersistentSavedDataConfig<PlayerDataValuesNames> _playerConfig = default;

	// Private vars
	protected DataProvider<PlayerDataValuesNames, PersistentSavedDataConfig<PlayerDataValuesNames>> _playerDataProvider;
	protected bool _dataPreparingFinished = false;

	// Init
	public virtual IEnumerator Initialize() {
    	_dataPreparingFinished = false;

		PrepareData();

		_dataPreparingFinished = true;
		yield return WaitForInitialized();

		Debug.Log("GameDataManager Initialized");
		DebugActions();
	}

	public override bool IsInitialized() {
		return _dataPreparingFinished;
	}

	void OnDisable() {
		_playerDataProvider.SaveData();
	}

	// ---------------------------------------------------------------
	// General Methods

	protected virtual void PrepareData() {
		Debug.Log($"GameDataManager : PrepareData");
		_playerDataProvider = new DataProvider<PlayerDataValuesNames, PersistentSavedDataConfig<PlayerDataValuesNames>>(_playerConfig);
		_playerDataProvider.LoadData();
	}

	public void SavePlayerData() {
		_playerDataProvider.SaveData();
	}

	// Debug Actions
	public virtual void DebugActions() {
		if (Application.isEditor) {
		}
	}
}
