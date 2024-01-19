using System;
using System.IO;
using UnityEngine;

[Serializable]
public class GameProgressDataProvider {

	public GameProgressData ProgressData => _gameProgressData;

	// Private vars
	GameProgressData _gameProgressData = null;

	const string c_folderName = "GameProgressFile";
	const string c_commonFileName = "gamedata";
	const string c_fileFormat = ".json";

	public void SaveData() {
		//Debug.Log("SaveLevelProgressData");

		CreateDirectoryIfNotExists();
		File.WriteAllText(generateFullPath(), _gameProgressData.GenerateJsonAndEncodeData());
	}

	public GameProgressData LoadData() {
		if (File.Exists(generateFullPath()))
			_gameProgressData = GameProgressData.DecodeJsonAndGenerateGameData(File.ReadAllText(generateFullPath()));
		else
			_gameProgressData = GameProgressData.CreateDefaultGameProgressData();

		return _gameProgressData;
	}

	public void ResetProgress() {
		FileInfo fileInfo = new FileInfo(generateFullPath());
		if (fileInfo.Exists)
			fileInfo.Delete();
	}

	void CreateDirectoryIfNotExists() {
		if (!Directory.Exists(generateDirectoryPath()))
			Directory.CreateDirectory(generateDirectoryPath());
	}

	string generateFullPath() {
		return Path.Combine(generateDirectoryPath(), generateLevelProgressDataFileName());
	}

	string generateDirectoryPath() {
		return Path.Combine(Application.persistentDataPath, GameProgressData.FOLDER_NAME, c_folderName);
	}

	string generateLevelProgressDataFileName() {
		return $"{c_commonFileName}{c_fileFormat}";
	}
}
