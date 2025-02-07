using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class IntParametrableValue : ParametrableValue<int>, IRandomValueGenerator<int>, IValueGetter<int> {
	[SerializeField] protected IntRandomData _randomData = default;
	[SerializeField] protected bool _random = default;

	public virtual int GetRandomValue() {
		return ((IRandomValueGenerator<int>)_randomData).GetRandomValue();
	}

	public override int GetValue() {
		return _random ? GetRandomValue() : base.GetValue();
	}
}

[Serializable]
public class StringParametrableValue : ParametrableValue<string>, IValueGetter<string> {
}

public class ParametrableValue<T> : ValueData<T> {
}
