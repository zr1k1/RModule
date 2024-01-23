using UnityEngine;

[CreateAssetMenu(fileName = "ExamplePriceConfig", menuName = "RModule/Examples/AppConfigs/ExamplePriceConfig", order = 2)]
public class PriceConfig : ScriptableObject, IPriceValueProvider {

	[SerializeField] protected int _value = default;

	public virtual int GetValue() {
		return _value;
	}
}