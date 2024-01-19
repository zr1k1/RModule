using System;
using UnityEngine;

[Serializable]
public class ValueData<T> {
	public T Value => _value;

	[SerializeField] protected T _value = default;
}