using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class MotivationMessagesDictionary : SerializableDictionary<MotivationMessages.ShowType, MotivationMessages> { }

[CreateAssetMenu(fileName = "MotivationMessagesConfig", menuName = "RModule/Examples/AppConfigs/MotivationMessagesConfig", order = 1)]
public class MotivationMessagesConfig : ScriptableObject {
    // Accessors
    public MotivationMessagesDictionary MotivationMessagesDictionary => _motivationMessagesDictionary;

    // Outlets
    [SerializeField] string _changeableParameterKeyInMotivationMessage = "%P";
    [SerializeField] MotivationMessagesDictionary _motivationMessagesDictionary = default;
    [SerializeField] List<Sprite> _images = default;

    public void Reset() {
        foreach (var keyPair in _motivationMessagesDictionary)
            keyPair.Value.Reset();
    }

    public string GetRandomString(MotivationMessages.ShowType showType) {
        if (_motivationMessagesDictionary.Contains(showType))
            return _motivationMessagesDictionary[showType].GetRandomStringKey(_changeableParameterKeyInMotivationMessage);
        else
            return $"Type {showType} Not exist in dictionary";
    }

    public float GetViewDuration(MotivationMessages.ShowType showType) {
        if (_motivationMessagesDictionary.Contains(showType))
            return _motivationMessagesDictionary[showType].ViewDuration;
        else
            return 0f;
    }

    public bool ConditionForShowIsTrue(MotivationMessages.ShowType showType) {
       return _motivationMessagesDictionary[showType].ConditionForShowIsTrue(showType);
    }

    public bool TryGetRandomImg(out Sprite randomImg) {
        randomImg = null;
        if (_images.Count > 0) {
            return _images[UnityEngine.Random.Range(0, _images.Count)];
        }

        return false;

    }
}
