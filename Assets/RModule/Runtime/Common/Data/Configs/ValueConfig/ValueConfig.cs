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