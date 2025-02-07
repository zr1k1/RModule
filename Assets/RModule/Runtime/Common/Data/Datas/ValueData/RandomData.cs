using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomData<T> : IRandomValueGenerator<T> {

	public abstract T GetRandomValue();
}

public interface IRandomValueGenerator<T> {
	T GetRandomValue();
}
