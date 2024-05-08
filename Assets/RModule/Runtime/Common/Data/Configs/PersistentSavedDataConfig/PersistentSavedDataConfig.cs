using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistentSavedDataConfig<OptionalValuesNames> : ScriptableObject where OptionalValuesNames : Enum {

	// Outlets
	[Tooltip("Example PlayerDataFolder or SomeFolder/PlayerDataFolder")]
	[SerializeField] string _folderPath = default; // Example PlayerDataFolder
	[Tooltip("Example player")]
	[SerializeField] string _fileName = default; // Example player
	[Tooltip("Example .json")]
	[SerializeField] string _fileFormat = default; // Example .json

	[SerializeField] SerializableDictionary<OptionalValuesNames, BaseValueConfig> _valuesDict = default;

	public bool DataIsExist() {
		return File.Exists(generateFullPath());
	}

	public string GetPath(int number = -1) {
		return generateFullPath();
	}

	public string GetDirectoryPath() {
		return $"{Application.persistentDataPath}/{_folderPath}/";
	}

	string generateFullPath(int number = -1) {
		var fileNumber = number >= 0 ? $"_{number}" : string.Empty;
		return $"{GetDirectoryPath()}{_fileName}{fileNumber}{_fileFormat}";
	}

	public Dictionary<int, object> GetAllValues() {
		var allValuesDictionary = new Dictionary<int, object>();
		foreach (var keyValuePair in _valuesDict) {
			var valueconfig = keyValuePair.Value ;
			Debug.Log($"valueconfig {valueconfig == null}");
			Debug.Log($"valueconfig {valueconfig.GetValue<object>()}");
			var value = valueconfig.GetValue<object>();
			allValuesDictionary.Add(Convert.ToInt32(keyValuePair.Key), value);
		}

		return allValuesDictionary;
	}
}
