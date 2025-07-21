using UnityEngine;

public interface IValueGetter<TValueType> {
    TValueType GetValue();
}