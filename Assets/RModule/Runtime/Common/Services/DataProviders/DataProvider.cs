using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class Data<OptionalValuesNames> : IValueSetter<OptionalValuesNames>, IValueGetterByEnum<OptionalValuesNames> where OptionalValuesNames : Enum {
	// Delegates
	public delegate string ConvertIntKeyToEnum(int intKey);

	public static Data<OptionalValuesNames> CreateDefaultData(PersistentSavedDataConfig<OptionalValuesNames> dataConfig) {
		return new Data<OptionalValuesNames>(dataConfig);
	}

	public PersistentSavedDataConfig<OptionalValuesNames> PersistentDataConfig => _persistentDataConfig;
	public Dictionary<int, object> Values => _values;

	// JSON Generation
	PersistentSavedDataConfig<OptionalValuesNames> _persistentDataConfig;
	Dictionary<int, object> _values = new Dictionary<int, object>();

	public class JData {
		public Dictionary<int, object> values;
	}

	public Data(PersistentSavedDataConfig<OptionalValuesNames> persistentDataConfig) {
		_persistentDataConfig = persistentDataConfig;
		var readOnly = _persistentDataConfig.GetAllValues();
		foreach (var keypair in readOnly) {
			_values.Add(keypair.Key, keypair.Value);
		}
	}

	public Data(Dictionary<int, object> values) {
		_values = values;

		//foreach (var keyValue in _values) {
		//	if (keyValue.Value is IList list) {
		//		foreach (var pos in list) {
		//			Debug.Log($"TEST : pos {pos}");
		//		}
		//	}
		//}
	}

	public string GenerateJsonAndEncodeData() {
		string serializedDict = generateJsonDataString();

		//// Encode
		//var encodedBytes = Encoding.UTF8.GetBytes(serializedDict);
		//string encodedDataString = Convert.ToBase64String(encodedBytes);

		string encodedDataString = serializedDict;

		return encodedDataString;
	}

	public static Data<OptionalValuesNames> DecodeJsonAndGenerateGameData(string encodedDataString) {
		if (string.IsNullOrEmpty(encodedDataString))
			return null;

		//// Decode
		//var decodedBytes = Convert.FromBase64String(encodedDataString);
		//string decodedText = Encoding.UTF8.GetString(decodedBytes);

		string decodedText = encodedDataString;

		JData jData = JsonConvert.DeserializeObject<JData>(decodedText);
		Data<OptionalValuesNames> data = new(jData.values);

		return data;
	}

	string generateJsonDataString() {
		JData jData = new JData();
		jData.values = _values;

		return JsonConvert.SerializeObject(jData, Formatting.Indented
			, new JsonSerializerSettings() {
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				FloatParseHandling = FloatParseHandling.Decimal
			}
			);
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

		if(value is not IConvertible) {
			Debug.LogError($"Check setted value is IConvertible and setted value type ({value.GetType()}) is correct!");
			return;
		}
		var convertedObj = Convert.ChangeType(value, _values[numberKey].GetType());

		if (convertedObj != null)
			_values[numberKey] = convertedObj;
		else {
			Debug.LogError($"The type of value ({value.GetType()}) to set differs from the one that is present in the dictionary.");
		}
	}

	public T1 GetValue<T1>(OptionalValuesNames enumType) {
		// for avoid convertions and reference types problems
		var serializedObject = JsonConvert.SerializeObject(_values[Convert.ToInt32(enumType)], Formatting.Indented
			, new JsonSerializerSettings() {
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			}
		);

		var deserializedObject = JsonConvert.DeserializeObject<T1>(serializedObject);

		return deserializedObject;
	}

	public void PrintDataValues(ConvertIntKeyToEnum dConvertIntKeyToEnum) {
		string printString = $"Data {this.GetType().ToString()} : \n";
		foreach (var keyPair in _values) {
			printString += $"KeyValuePair {dConvertIntKeyToEnum(keyPair.Key)} : {keyPair.Value} \n";
		}
		Debug.Log($"{printString}");
	}
}

[Serializable]
public class DataProvider<OptionalValuesNames, DataConfigClass>
	where DataConfigClass : PersistentSavedDataConfig<OptionalValuesNames>
	where OptionalValuesNames : Enum {

	// Accessors
	public Data<OptionalValuesNames> Data => _data;

	// Private vars
	Data<OptionalValuesNames> _data;
	DataConfigClass _dataConfig;

	int _id = -1;

	public DataProvider(DataConfigClass dataConfigClass) {
		_dataConfig = dataConfigClass;
	}

	public void SaveData() {
		Debug.Log("DataProvider : Save");
		CreateDirectoryIfNotExists();
		File.WriteAllText(_dataConfig.GetPath(_id), _data.GenerateJsonAndEncodeData());
	}

	public void LoadData(int id = -1) {
		_id = id;
		Debug.Log($"DataProvider : Load {_dataConfig.GetPath()}");
		if (_dataConfig.DataIsExist(_id)) {
			_data = Data<OptionalValuesNames>.DecodeJsonAndGenerateGameData(File.ReadAllText(_dataConfig.GetPath(_id)));
			var currentConfigValues = _dataConfig.GetAllValues();
			// Remove from loaded _data not existed keys in current config
			var keyToRemove = _data.Values.Keys.ToList().FindAll(key => !currentConfigValues.ContainsKey(key));
			foreach (var key in keyToRemove) {
				_data.Values.Remove(key);
			}
			// Add key values from current config not existed in _data.Values 
			foreach (var keyPair in currentConfigValues) {
				if (!_data.Values.ContainsKey(keyPair.Key))
					_data.Values.Add(keyPair.Key, keyPair.Value);
			}
		} else {
			_data = Data<OptionalValuesNames>.CreateDefaultData(_dataConfig);
		}

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

	public static void Reset(DataConfigClass dataConfigClass) {
		FileInfo fileInfo = new FileInfo(dataConfigClass.GetPath());
		if (fileInfo.Exists)
			fileInfo.Delete();
	}
}
