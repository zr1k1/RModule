using System;
using UnityEngine;

[Serializable]
public class ValueData<T> : IValueGetter<T> {
	[SerializeField] protected T _value = default;

	public virtual T GetValue() {
		return _value;
	}

	public virtual void SetValue(T value) {
		Debug.LogError($"ValueData<T> : SetValue");
		_value = value;
		Debug.LogError($"ValueData<T> : SetValue {_value}");
	}
}