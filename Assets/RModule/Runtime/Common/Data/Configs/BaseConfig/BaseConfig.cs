namespace RModule.Runtime.Data.Configs {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System;

	//Dont forget to add "where OptionalValue : Enum" on child class
	public class BaseConfig<OptionalValue> : ScriptableObject where OptionalValue : Enum {

		[Header("OptionaConfigValue"), Space]
		[SerializeField] protected SerializableDictionary<OptionalValue, BaseValueConfig> _valuesDict = default;

		public virtual T GetValue<T>(OptionalValue valueType) {
			if (!_valuesDict.ContainsKey(valueType)) {
				Debug.LogError($"Value {valueType} is not present on dictionary _optionalValuesDict");
				return default(T);
			}
			var value = _valuesDict[valueType].GetValue<object>();
			return (T)value;
		}
	}
}
