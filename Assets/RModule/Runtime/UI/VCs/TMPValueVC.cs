using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPValueVC<T> : ValueVC<T> {

	[Tooltip("For example string like [Score: YOURVALUE]. _symbolBeforeValue = ' ' ")]
	[SerializeField] protected string _symbolBeforeValue = default;

	protected TextMeshProUGUI _valueLabel;

	public override void UpdateValue(T value) {
		base.UpdateValue(value);
		if (_valueLabel == null)
			_valueLabel = GetComponent<TextMeshProUGUI>();
	}
}
