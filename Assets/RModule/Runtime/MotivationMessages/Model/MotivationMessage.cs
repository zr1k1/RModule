using System;
using UnityEngine;

[Serializable]
public class MotivationMessage {
    // Enums 
    public enum ChangeableParameterType { None = 0, Integer = 1 }
    public enum IntegerParameterName {
        None = 0
    }

    // Accessors 
    public string Key => _key;
    public ChangeableParameterType ParameterType => _changeableParameterType;
    public IntegerParameterName IntParameterName => _integerParametrId;

    // Private vars
    [SerializeField] string _key = default;
    [SerializeField] ChangeableParameterType _changeableParameterType = default;
    [SerializeField] IntegerParameterName _integerParametrId = default;
}
