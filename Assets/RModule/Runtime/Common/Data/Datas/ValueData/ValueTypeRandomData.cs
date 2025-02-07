using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public abstract class ValueTypeRandomData<T> : RandomData<T> {
	public T MinIncluded => _minIncluded;
	public T MaxIncluded => _maxIncluded;

	[SerializeField] protected T _minIncluded;
	[SerializeField] protected T _maxIncluded;
}
