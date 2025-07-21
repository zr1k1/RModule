using UnityEngine;

public interface IHealthable {
}

public interface IHealthable<T> {
    T Health { get; }
    void SetHealth(T value);
    void ChangeHealthByAmount(T amount);
}
