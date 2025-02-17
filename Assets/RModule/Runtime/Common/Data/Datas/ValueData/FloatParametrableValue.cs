using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FloatParametrableValue : ParametrableValue<float>, IRandomValueGenerator<float>, IValueGetter<float> {
	[SerializeField] protected FloatRandomData _randomData = default;
	[SerializeField] protected bool _random = default;

	public virtual float GetRandomValue() {
		return ((IRandomValueGenerator<float>)_randomData).GetRandomValue();
	}

	public override float GetValue() {
		return _random ? GetRandomValue() : base.GetValue();
	}
}
