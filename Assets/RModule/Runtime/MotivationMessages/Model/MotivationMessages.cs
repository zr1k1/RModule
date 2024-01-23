using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MotivationMessages {
    // Enums
    // Add new enums by priority (for example 3 > 1)
    public enum ShowType { WhenLevelComplete = 0 }

    // Accessors
    public float ViewDuration => _viewDuration;

    // Outlets
    [SerializeField] float _viewDuration = default;
    [SerializeField] List<MotivationMessage> _motivationMessages = default;
    [SerializeField] bool lastShowedCanBeRepeated = default;

    // Private vars;
    ConditionMotivationMessage _conditionMotivation;
    int _lastIndexGettedMessage = 0;

    // Structs
    public struct Info {
        public Dictionary<MotivationMessage.IntegerParameterName, int> integerParameters;
    }

    public void Reset() {
        _conditionMotivation = null;
    }

    public string GetRandomStringKey(string changeableParameterKeyInMotivationMessage) {
        MotivationMessage motivationMessage;
        string message = "";
        if (_motivationMessages.Count > 1 || (_motivationMessages.Count > 0 && lastShowedCanBeRepeated)) {
            List<int> correctedIndexes = new List<int>();
            for (int i = 0; i < _motivationMessages.Count; i++) 
                correctedIndexes.Add(i);

            if (correctedIndexes.Contains(_lastIndexGettedMessage))
                correctedIndexes.Remove(_lastIndexGettedMessage);

            for (int i = 0; i < _motivationMessages.Count; i++) {
                motivationMessage = _motivationMessages[i];
                MotivationMessage.ChangeableParameterType changeableParameterType = motivationMessage.ParameterType;
                MotivationMessage.IntegerParameterName intParameterId = motivationMessage.IntParameterName;
            }

            if(correctedIndexes.Count > 0) {
                var correctedIndexesRndIndex = UnityEngine.Random.Range(0, correctedIndexes.Count);
                var messageRndIndex = correctedIndexes[correctedIndexesRndIndex];
                motivationMessage = _motivationMessages[messageRndIndex];
                message = LocalizedText.T(motivationMessage.Key);

                MotivationMessage.ChangeableParameterType changeableParameterType = motivationMessage.ParameterType;
                MotivationMessage.IntegerParameterName intParameterId = motivationMessage.IntParameterName;

                _lastIndexGettedMessage = messageRndIndex;
            }
        }

        return message;
    }

    public bool ConditionForShowIsTrue(ShowType showType) {
        if (_conditionMotivation == null) {
            if (showType == ShowType.WhenLevelComplete) {
                _conditionMotivation = new LevelCompleteConditionMotivationMessage();
            } else {
                Debug.LogError($"Not exist condition for {showType}");
                _conditionMotivation = new ConditionMotivationMessage();
                return false;
            }
        } 

        if (_conditionMotivation == null) 
            return false;
        else 
            return _conditionMotivation.IsTrue();
    }
}