using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Numerics;

[Serializable]
public class IntRandomData : ValueTypeRandomData<int> {
	[SerializeField] int _valueStep = 1;

	public override int GetRandomValue() {
		if (_maxIncluded == 0) {
			Debug.LogError($"IntRandomData : _maxIncluded is 0");
		}

		int rndValue = Random.Range(_minIncluded, _maxIncluded + 1);
		int value = Mathf.CeilToInt((float)rndValue / (float)_valueStep) * _valueStep;

		if (_maxIncluded >= _minIncluded) {
			return value;
		} else {
			Debug.LogError($"IntRandomData : max < min");

			return 0;
		}
	}
}
