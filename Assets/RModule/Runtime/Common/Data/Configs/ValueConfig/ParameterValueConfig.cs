using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class ParameterValueConfig<TClass, TValueType> : ValueConfig<TClass> where TClass : ParametrableValue<TValueType> {

	public override T GetValue<T>() {
		return (T)(object)_value.GetValue();
	}
}
