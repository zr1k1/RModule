using System;
using UnityEngine.Events;

[Serializable]
public class ChangeableValueData<T> : ValueData<T> {
	public UnityEvent<T> ValueDidChange = default;
}