using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable] public class ParametersDictionary<T> : SerializableDictionary<string, T> { }

[CreateAssetMenu(fileName = "BaseUniversalDataConfig", menuName = "Helpers/UniversalDataConfigs/BaseUniversalDataConfig", order = 1)]
public class BaseUniversalDataConfig : ScriptableObject {
	// Accessors
	public string Key => _nameKey;

	// Outlets
	[SerializeField] protected string _nameKey = default;
	[SerializeField] protected ParametersDictionary<string> _stringParams = new ParametersDictionary<string>();
	[SerializeField] protected ParametersDictionary<bool> _boolParams = new ParametersDictionary<bool>();
	[SerializeField] protected ParametersDictionary<int> _intParams = new ParametersDictionary<int>();
	[SerializeField] protected ParametersDictionary<float> _floatParams = new ParametersDictionary<float>();
	[SerializeField] protected ParametersDictionary<Vector3> _vector3Params = new ParametersDictionary<Vector3>();
	[SerializeField] protected ParametersDictionary<Color> _colorParams = new ParametersDictionary<Color>();

	public virtual bool TryGetParameter<T>(string parameterKey, out T value) {
		value = default(T);

		if (_stringParams.Contains(parameterKey) && _stringParams[parameterKey] is T stringParameter) {
			value = stringParameter;
			return true;
		} else if (_boolParams.Contains(parameterKey) && _boolParams[parameterKey] is T boolParameter) {
			value = boolParameter;
			return true;
		} else if (_intParams.Contains(parameterKey) && _intParams[parameterKey] is T intParameter) {
			value = intParameter;
			return true;
		} else if (_floatParams.Contains(parameterKey) && _floatParams[parameterKey] is T floatParameter) {
			value = floatParameter;
			return true;
		} else if (_vector3Params.Contains(parameterKey) && _vector3Params[parameterKey] is T vector3Parameter) {
			value = vector3Parameter;
			return true;
		} else if (_colorParams.Contains(parameterKey) && _colorParams[parameterKey] is T colorParameter) {
			value = colorParameter;
			return true;
		} else {
			Debug.Log($"ParameterKey {parameterKey} is not present on dictionaries");
			return false;
		}
	}
}
