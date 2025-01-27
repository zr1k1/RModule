using System;
using UnityEngine;

[Serializable]
public class ValueConfig<T> : BaseValueConfig {
	public T DefaultValue => _value;

	[Header("Default value")]
	[SerializeField] protected T _value = default;

	public override T1 GetValue<T1>() {
		return (T1)(object)_value;
	}
}

public abstract class ParameterValueConfig<TClass, TValueType> : ValueConfig<TClass> where TClass : ParametrableValue<TValueType> {

	public override T GetValue<T>() {
		return (T)(object)_value.GetValue();
	}
}