using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ValueComponent<T> : MonoBehaviour{
	// Event
	public UnityEvent<T, GameObject> ValueDidChange = default;

	// Accessors
	public T DefaultValue => p_defaultValue;
	public T Value => p_value;

	// Outlets
	[SerializeField] protected T p_defaultValue = default;
	[SerializeField] protected T p_value = default;

	public virtual void SetValue(T value) {
		p_value = value;

		ValueDidChange?.Invoke(p_value, gameObject);
	}
}
