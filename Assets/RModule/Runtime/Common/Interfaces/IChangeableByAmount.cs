using UnityEngine;

public interface IChangeableByAmount<T> {
    void ChangeValueByAmount(T value);
}
