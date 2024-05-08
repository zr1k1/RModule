using UnityEngine;

public abstract class BaseValueConfig : ScriptableObject { 
	public abstract T GetValue<T>();
}