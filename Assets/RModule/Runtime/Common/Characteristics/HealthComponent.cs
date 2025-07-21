using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : FloatValueComponent {
	// Event
	public UnityEvent HealthDidLessThanZeroOrZero = default;

	bool _cancelSetupHealth;

	public override void SetValue(float value) {
		if (_cancelSetupHealth)
			return;

		base.SetValue(value);
		CheckValueIsLessThanZeroOrZeroAndCallEvent();
	}

	public override void ChangeValueByAmount(float value) {
		if (_cancelSetupHealth)
			return;

		base.ChangeValueByAmount(value);
		CheckValueIsLessThanZeroOrZeroAndCallEvent();
	}

	void CheckValueIsLessThanZeroOrZeroAndCallEvent() {
		if (p_value <= 0) {
			_cancelSetupHealth = true;
			HealthDidLessThanZeroOrZero?.Invoke();
		}
	}
}
