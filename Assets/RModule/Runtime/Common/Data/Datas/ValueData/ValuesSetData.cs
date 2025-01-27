using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ValuesSetData<SomeValueType> : IValueGetterByEnum<SomeValueType> where SomeValueType : Enum {
	// Outlets
	// After add new settingsDictionary you need to add condition to TryFindDictionaryByValueType, GetAllValues method
	[SerializeField] protected BoolValuesData<SomeValueType> _boolValues = default;
	[SerializeField] protected IntValuesData<SomeValueType> _intValues = default;
	[SerializeField] protected StringValuesData<SomeValueType> _stringValues = default;
	[SerializeField] protected FloatValuesData<SomeValueType> _floatValues = default;
	[SerializeField] protected ListValuesData<SomeValueType, string> _listValuesData = default;
	[SerializeField] protected DictionaryValuesData<SomeValueType, string, int> _stringIntDictionaryValuesData = default;

	bool TryFindDictionaryByValueType<T1>(SomeValueType valueType, out SerializableDictionary<SomeValueType, ValueData<T1>> foundedDictionary) {
		object dictionaryObject = null;
		if (_boolValues.ContainsKey(valueType)) {
			dictionaryObject = _boolValues[valueType];
		} else if (_intValues.ContainsKey(valueType)) {
			dictionaryObject = _intValues[valueType];
		} else if (_stringValues.ContainsKey(valueType)) {
			dictionaryObject = _stringValues[valueType];
		} else if (_floatValues.ContainsKey(valueType)) {
			dictionaryObject = _floatValues[valueType];
		} else if (_listValuesData.ContainsKey(valueType)) {
			dictionaryObject = _listValuesData[valueType];
		} else if (_stringIntDictionaryValuesData.ContainsKey(valueType)) {
			dictionaryObject = _stringIntDictionaryValuesData[valueType];
		}
		// Example for add new:
		//else if (_someSettingsWithDefaultValue.ContainsKey(valueType)) {
		//	dictionaryObject = _someSettingsWithDefaultValue[valueType];
		//}

		foundedDictionary = (SettingsDictionary<SomeValueType, ValueData<T1>>)dictionaryObject;

		if (foundedDictionary == null) {
			Debug.LogError($"Cannot find setting {valueType} on dictionaries!");
		}
		return foundedDictionary != null;
	}

	public Dictionary<int, object> GetAllValues() {
		var allValuesDictionary = new Dictionary<int, object>();
		AddDictionaryToAllValuesDictionary(ref allValuesDictionary, _boolValues);
		AddDictionaryToAllValuesDictionary(ref allValuesDictionary, _intValues);
		AddDictionaryToAllValuesDictionary(ref allValuesDictionary, _stringValues);
		AddDictionaryToAllValuesDictionary(ref allValuesDictionary, _floatValues);
		AddDictionaryToAllValuesDictionary(ref allValuesDictionary, _listValuesData);
		AddDictionaryToAllValuesDictionary(ref allValuesDictionary, _stringIntDictionaryValuesData);

		return allValuesDictionary;
	}

	public virtual T1 GetValue<T1>(SomeValueType valueType) {
		if (!TryFindDictionaryByValueType<T1>(valueType, out var foundedDictionary)) {
			Debug.LogError($"Cannot get valueType {valueType} value");
			return default(T1);
		}

		return foundedDictionary[valueType].GetValue();
	}

	void AddDictionaryToAllValuesDictionary<T>(ref Dictionary<int, object> allValuesDictionary, ValuesData<SomeValueType, ValueData<T>, T> toAddDictionary) {
		foreach (var keyPair in toAddDictionary) {
			allValuesDictionary.Add(Convert.ToInt32(keyPair.Key), keyPair.Value.GetValue());
		}
	}
}
