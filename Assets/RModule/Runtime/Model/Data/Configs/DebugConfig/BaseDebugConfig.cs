using System;
using UnityEngine;

public class BaseDebugConfig<OptionalDebugValue> : ScriptableObject where OptionalDebugValue : Enum {
	//Accessors
	public bool DebugModeEnabled => _debugModeEnabled;

	//Outlets
	[SerializeField] protected bool _debugModeEnabled;

	[Header("OptionaConfigValue"), Space]
	[SerializeField] SerializableDictionary<OptionalDebugValue, BaseValueConfig> _optionalValuesDict = default;

	public virtual void EnableDebugMode() {
		_debugModeEnabled = true;
	}

	public virtual T1 GetValue<T1>(OptionalDebugValue valueType) {
		if (!_optionalValuesDict.ContainsKey(valueType)) {
			Debug.LogError($"Value {valueType} is not present on dictionary _optionalValuesDict");
			return default(T1);
		}
		var value = _optionalValuesDict[valueType].GetValue<object>();
		return (T1)value;
	}
}