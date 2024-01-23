using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

//public interface IDataFileSaveLoader {
//	void SaveData();
//	void LoadData();
//}

public class Data<OptionalValuesNames> : IValueSetter<OptionalValuesNames>, IValueGetter<OptionalValuesNames> where OptionalValuesNames : Enum {
	public static Data<OptionalValuesNames> CreateDefaultData(PersistentDataConfig<OptionalValuesNames> dataConfig) {
		return new Data<OptionalValuesNames>(dataConfig);
	}

	public PersistentDataConfig<OptionalValuesNames> PersistentDataConfig => _persistentDataConfig;
	public Dictionary<int, object> Values => _values;

	// JSON Generation
	PersistentDataConfig<OptionalValuesNames> _persistentDataConfig;
	//JData _jData;
	Dictionary<int, object> _values;

	public class JData {
		public Dictionary<int, object> values;
	}

	public Data(PersistentDataConfig<OptionalValuesNames> persistentDataConfig) {
		_persistentDataConfig = persistentDataConfig;
		_values = _persistentDataConfig.GetAllValues();
	}

	public Data(Dictionary<int, object> values) {
		_values = values;
	}

	public string GenerateJsonAndEncodeData() {
		Debug.Log($"GenerateJsonAndEncodeData");
		string serializedDict = generateJsonDataString();

		//// Encode
		//var encodedBytes = Encoding.UTF8.GetBytes(serializedDict);
		//string encodedDataString = Convert.ToBase64String(encodedBytes);

		string encodedDataString = serializedDict;

		return encodedDataString;
	}

	public static Data<OptionalValuesNames> DecodeJsonAndGenerateGameData(string encodedDataString) {
		Debug.Log($"DecodeData {encodedDataString}");
		if (string.IsNullOrEmpty(encodedDataString))
			return null;

		//// Decode
		//var decodedBytes = Convert.FromBase64String(encodedDataString);
		//string decodedText = Encoding.UTF8.GetString(decodedBytes);

		string decodedText = encodedDataString;

		JData jData = JsonConvert.DeserializeObject<JData>(decodedText);
		Data<OptionalValuesNames> data = new (jData.values);

		return data;
	}

	string generateJsonDataString() {
		//JPlayerData jPlayerData = new JPlayerData();
		//jPlayerData.coins = _coins;
		//jPlayerData.disableAds = _disableAds;
		//jPlayerData.heroGraphicKitKey = _heroGraphicKitKey;
		//jPlayerData.heroGraphicKitKeysCollection = _heroGraphicKitKeysCollection;
		//jPlayerData.purchasedProducts = _purchasedProducts;
		JData jData = new JData();
		jData.values = _values;

		return JsonConvert.SerializeObject(jData, Formatting.Indented);
	}

	public void Reset() {
		FileInfo fileInfo = new FileInfo(_persistentDataConfig.GetPath());
		if (fileInfo.Exists)
			fileInfo.Delete();
	}

	public void SetValue<T1>(OptionalValuesNames enumType, T1 value) {
		int numberKey = Convert.ToInt32(enumType);
		if (!_values.ContainsKey(numberKey)) {
			//Debug.Log($"Key {numberKey} is not present on _values dictonary");
			_values.Add(numberKey, value);
			return;
		}
		_values[numberKey] = value;
	}

	public T1 GetValue<T1>(OptionalValuesNames enumType) {
		return (T1) _values[Convert.ToInt32(enumType)];
	}
}

[Serializable]
public class DataProvider<OptionalValuesNames, DataConfigClass>
	//where DataClass : Data<OptionalValuesNames>
	where DataConfigClass : PersistentDataConfig<OptionalValuesNames>
	where OptionalValuesNames : Enum {

	// Accessors
	public Data<OptionalValuesNames> Data => _data;

	// Private vars
	Data<OptionalValuesNames> _data;
	DataConfigClass _dataConfig;

	public DataProvider(DataConfigClass dataConfigClass) {
		_dataConfig = dataConfigClass;
	}

	// ---------------------------------------------------------------
	// Player Data Management

	public void SaveData() {
		Debug.Log("SavePlayerData");
		CreateDirectoryIfNotExists();
		File.WriteAllText(_dataConfig.GetPath(), _data.GenerateJsonAndEncodeData());
	}

	public void LoadData(int number = -1) {
		//_dataConfig = dataConfig;
		Debug.Log($"DataProvider : {_dataConfig.GetPath()}");
		_data = _dataConfig.DataIsNotExist() ? Data<OptionalValuesNames>.CreateDefaultData(_dataConfig) : Data<OptionalValuesNames>.DecodeJsonAndGenerateGameData(File.ReadAllText(_dataConfig.GetPath(number)));

		SaveData();
	}

	public void ChangeData(Data<OptionalValuesNames> data) {
		_data = data;
		SaveData();
	}

	void CreateDirectoryIfNotExists() {
		if (!Directory.Exists(_dataConfig.GetDirectoryPath()))
			Directory.CreateDirectory(_dataConfig.GetDirectoryPath());
	}

	//static string generateFullPath() {
	//	return generateDirectoryPath() + c_playerDataFileName;
	//}

	//static string generateDirectoryPath() {
	//	return Application.persistentDataPath + "/" + c_playerDataFolderName + "/";
	//}

	//bool dataIsNotExist() {
	//	return !File.Exists(generateFullPath());
	//}

	public void Reset() {
		FileInfo fileInfo = new FileInfo(_dataConfig.GetPath());
		if (fileInfo.Exists)
			fileInfo.Delete();
	}
}
