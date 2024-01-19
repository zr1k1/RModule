using System;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerDataProvider {
	// Accessors
	public PlayerData PlayerData => _playerData;

	// Private vars
	PlayerData _playerData;
	PlayerConfig _playerConfig;

	const string c_playerDataFolderName = "PlayerData";
	const string c_playerDataFileName = "player.json";

	// ---------------------------------------------------------------
	// Player Data Management
	
	public void SaveData() {
		Debug.Log("SavePlayerData");
		CreateDirectoryIfNotExists();
		File.WriteAllText(generateFullPath(), _playerData.GenerateJsonAndEncodeData());
	}

	public void LoadData(PlayerConfig playerConfig) {
		_playerConfig = playerConfig;
		_playerData = playerDataNotExist() ? PlayerData.CreateDefaultPlayerData(_playerConfig) : PlayerData.DecodeJsonAndGenerateGameData(File.ReadAllText(generateFullPath()));

		SaveData();
	}

	public void ChangeData(PlayerData newPlayerData) {
		_playerData = newPlayerData;
		SaveData();
	}

	void CreateDirectoryIfNotExists() {
		if (!Directory.Exists(generateDirectoryPath()))
			Directory.CreateDirectory(generateDirectoryPath());
	}
	
	static string generateFullPath() {
		return generateDirectoryPath() + c_playerDataFileName;
	}

	static string generateDirectoryPath() {
		return Application.persistentDataPath + "/" + c_playerDataFolderName + "/";
	}

	bool playerDataNotExist() {
		return !File.Exists(generateFullPath());
	}

	public static void Reset() {
		FileInfo fileInfo = new FileInfo(generateFullPath());
		if (fileInfo.Exists)
			fileInfo.Delete();
	}
}
