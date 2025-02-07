using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class ReferenceTypeRandomData<T> : RandomData<T> {
	[SerializeField] protected List<T> _list = default;

	public override T GetRandomValue() {
		int rndIndex = Random.Range(0, _list.Count);

		return _list[rndIndex];
	}
}
