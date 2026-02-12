namespace RModule.Runtime.Data.Configs {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System;

	//Dont forget to add "where OptionalValue : Enum" on child class
	public class BaseConfig<OptionalValueKey> : ScriptableObject, IValueGetterByEnum<OptionalValueKey> where OptionalValueKey : Enum {

		[Header("OptionaConfigValue"), Space]
		[SerializeField] protected SerializableDictionary<OptionalValueKey, BaseValueConfig> _valuesDict = default;

		public virtual T GetValue<T>(OptionalValueKey key) {

			if (!_valuesDict.ContainsKey(key)) {
				Debug.LogError($"Value {key} is not present on dictionary _optionalValuesDict in {name}");
				return default(T);
			}

			var value = _valuesDict[key].GetValue<object>();
			if (value is T typedValue)
				return typedValue;

			Debug.LogError($"Config key '{key}' has type {value?.GetType().Name}, but requested {typeof(T).Name}");

			return default;
		}
	}
}
