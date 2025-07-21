using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatValueComponent : ValueComponent<float> {

    public override void ChangeValueByAmount(float value) {
        p_value += value;

        ValueDidChange?.Invoke(p_value, gameObject);
    }

}
