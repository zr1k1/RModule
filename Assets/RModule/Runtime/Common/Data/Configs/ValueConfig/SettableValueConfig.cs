using System;
using UnityEngine;

[Serializable]
public class SettableValueConfig<T> : ValueConfig<T>, ISettableValue<T> {

	public void SetValue(T value) {
		_value = value;
	}
}
