using UnityEngine;

public class ValueConfig<T> : BaseValueConfig{
	[SerializeField] protected T _value = default;

	public override T1 GetValue<T1>() {
		return (T1)(object)_value;
	}
}