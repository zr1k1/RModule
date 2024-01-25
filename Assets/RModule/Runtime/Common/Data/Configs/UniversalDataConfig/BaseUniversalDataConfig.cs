using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable] public class ValuesDictionary<T> : SerializableDictionary<string, T> { }

[CreateAssetMenu(fileName = "BaseUniversalDataConfig", menuName = "Helpers/UniversalDataConfigs/BaseUniversalDataConfig", order = 1)]
public class BaseUniversalDataConfig : ScriptableObject {
	// Accessors
	public string Key => _nameKey;

	// Outlets
	[SerializeField] protected string _nameKey = default;
	[SerializeField] protected ValuesDictionary<string> _stringValues = new ValuesDictionary<string>();
	[SerializeField] protected ValuesDictionary<bool> _boolValues = new ValuesDictionary<bool>();
	[SerializeField] protected ValuesDictionary<int> _intValues = new ValuesDictionary<int>();
	[SerializeField] protected ValuesDictionary<float> _floatValues = new ValuesDictionary<float>();
	[SerializeField] protected ValuesDictionary<Vector3> _vector3Values = new ValuesDictionary<Vector3>();
	[SerializeField] protected ValuesDictionary<Color> _colorValues = new ValuesDictionary<Color>();

	public virtual bool TryGetValue<T>(string valueKey, out T value) {
		value = default(T);

		if (_stringValues.Contains(valueKey) && _stringValues[valueKey] is T stringValue) {
			value = stringValue;
			return true;
		} else if (_boolValues.Contains(valueKey) && _boolValues[valueKey] is T boolValue) {
			value = boolValue;
			return true;
		} else if (_intValues.Contains(valueKey) && _intValues[valueKey] is T intValue) {
			value = intValue;
			return true;
		} else if (_floatValues.Contains(valueKey) && _floatValues[valueKey] is T floatValue) {
			value = floatValue;
			return true;
		} else if (_vector3Values.Contains(valueKey) && _vector3Values[valueKey] is T vector3Value) {
			value = vector3Value;
			return true;
		} else if (_colorValues.Contains(valueKey) && _colorValues[valueKey] is T colorValue) {
			value = colorValue;
			return true;
		} else {
			Debug.Log($"ValueKey {valueKey} is not present on dictionaries");
			return false;
		}
	}
}
