using UnityEngine;
public abstract class ValueVC<T> : MonoBehaviour {
	public T Value => _value;

	protected T _value;

	public virtual void UpdateValue(T value) {
		_value = value;
	}
}
