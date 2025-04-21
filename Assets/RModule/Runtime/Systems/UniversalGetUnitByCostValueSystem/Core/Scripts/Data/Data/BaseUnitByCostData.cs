using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseUnitByCostDataConfig<TCost> : ScriptableObject where TCost : IComparable {
    [SerializeField] protected TCost _cost = default;

    public abstract bool TryGet(TCost valueToGet, out TCost remainder);

    public virtual TCost GetCost() {
        return _cost;

    }
}
