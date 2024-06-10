using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatValueComponent : ValueComponent<float> {

	public virtual void SetValueByAmount(float value) {
		p_value += value;

		ValueDidChange?.Invoke(p_value, gameObject);
	}
}
