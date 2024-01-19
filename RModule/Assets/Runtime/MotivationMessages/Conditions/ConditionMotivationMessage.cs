using UnityEngine;

public class ConditionMotivationMessage {
    [SerializeField] protected bool _showCountIsInfinity;

    public virtual bool IsTrue() {
        return true;
    }
}
