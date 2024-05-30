using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueComponent<T> : MonoBehaviour {
	public T DefaultValue => p_defaultValue;
	public T Value => p_value;

	[SerializeField] protected T p_defaultValue = default;
	[SerializeField] protected T p_value = default;

	public void SetValue(T value) {
		p_value = value;
	}
}
