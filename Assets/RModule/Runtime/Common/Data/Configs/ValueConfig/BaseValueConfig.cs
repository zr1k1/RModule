using System;
using UnityEngine;

[Serializable]
public abstract class BaseValueConfig : ScriptableObject { 
	public abstract T GetValue<T>();
}