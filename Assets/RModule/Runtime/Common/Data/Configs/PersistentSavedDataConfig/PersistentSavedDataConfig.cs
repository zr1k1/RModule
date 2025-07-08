using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;

public class PersistentSavedDataConfig<OptionalValuesNames> : ScriptableObject where OptionalValuesNames : Enum {
	// Outlets
	[Tooltip("Example PlayerDataFolder or SomeFolder/PlayerDataFolder")]
	[SerializeField] string _folderPath = "PlayerData"; // Example PlayerDataFolder
	[Tooltip("Example player")]
	[SerializeField] string _fileName = "player"; // Example player
	[Tooltip("Example .json")]
	[SerializeField] string _fileFormat = ".json"; // Example .json

	[SerializeField] SerializableDictionary<OptionalValuesNames, BaseValueConfig> _valuesDict = default;

	public bool DataIsExist(int id = -1) {
		return File.Exists(generateFullPath(id));
	}

	public string GetPath(int id = -1) {
		return generateFullPath(id);
	}

	public string GetDirectoryPath() {
		return $"{Application.persistentDataPath}/{_folderPath}/";
	}

	string generateFullPath(int id = -1) {
		var fileNumber = id >= 0 ? $"_{id}" : string.Empty;
		return $"{GetDirectoryPath()}{_fileName}{fileNumber}{_fileFormat}";
	}

	public ReadOnlyDictionary<int, object> GetAllValues() {
		var allValuesDictionary = new Dictionary<int, object>();
		foreach (var keyValuePair in _valuesDict) {
			var valueconfig = keyValuePair.Value ;
			var value =  valueconfig.GetValue<object>();
			allValuesDictionary.Add(Convert.ToInt32(keyValuePair.Key), value);
		}

		return new ReadOnlyDictionary<int, object>(allValuesDictionary);
	}

	public void DeleteData() {
		FileInfo fileInfo = new FileInfo(GetPath());
		if (fileInfo.Exists)
			fileInfo.Delete();
	}
}
