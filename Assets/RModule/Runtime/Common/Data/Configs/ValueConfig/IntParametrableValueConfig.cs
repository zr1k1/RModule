using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class IntRandomData : RandomData<int> {
	public override int GetRandomValue() {
		if (_maxIncluded >= _minIncluded) {
			return Random.Range(_minIncluded, _maxIncluded + 1);
		} else {
			Debug.LogError($"IntRandomData : max < min");

			return 0;
		}
	}
}

public abstract class RandomData<T> : IRandomValueGenerator<T> where T : IComparable {
	public T MinIncluded => _minIncluded;
	public T MaxIncluded => _maxIncluded;

	[SerializeField] protected T _minIncluded;
	[SerializeField] protected T _maxIncluded;

	public abstract T GetRandomValue();
}

public interface IRandomValueGenerator<T> {
	T GetRandomValue();
}

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
