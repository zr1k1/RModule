using System;
using System.IO;
using UnityEngine;

[Serializable]
public class LevelProgressDataProvider {
	// Accessors
	public LevelProgressData ProgressData => _progressData;

	// Private vars
	int _levelId;
	LevelProgressData _progressData = null;

	const string c_folderName = "LevelProgressFiles";
	const string c_commonFileName = "leveldata_";
	const string c_fileFormat = ".json";

	public void SaveData() {
		CreateDirectoryIfNotExists();
		File.WriteAllText(generateFullPath(), _progressData.GenerateJsonAndEncodeData());
	}

	public LevelProgressData LoadData(int levelId) {
		_levelId = levelId;

		if (File.Exists(generateFullPath()))
			_progressData = LevelProgressData.DecodeJsonAndGenerateGameData(File.ReadAllText(generateFullPath()));
		else
			_progressData = LevelProgressData.CreateDefaultProgressData();

		return _progressData;
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
		return $"{c_commonFileName}{_levelId}{c_fileFormat}";
	}
}
