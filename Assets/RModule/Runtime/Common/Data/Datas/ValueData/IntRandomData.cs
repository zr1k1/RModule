using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class IntRandomData : ValueTypeRandomData<int> {
	public override int GetRandomValue() {
		if (_maxIncluded >= _minIncluded) {
			return Random.Range(_minIncluded, _maxIncluded + 1);
		} else {
			Debug.LogError($"IntRandomData : max < min");

			return 0;
		}
	}
}
