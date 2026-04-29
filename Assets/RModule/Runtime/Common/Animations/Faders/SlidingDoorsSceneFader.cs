using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class SlidingDoorsSceneFader : BaseSceneFader {
    [SerializeField] SlidingDoorsUIAnimation _slidingDoorsUIAnimation = default;
    [SerializeField] GameObject _leftDoor = default;
    [SerializeField] GameObject _rightDoor = default;
    [SerializeField] Image _leftDoorIcon = default;
    [SerializeField] Image _rightDoorIcon = default;
    [SerializeField] bool _showUpAtStart = true;
    [SerializeField] ReferenceTypeRandomData<DoorsData> _doorsDatas = default;

    static int s_lastCloseIconIndex = -1;

    [Serializable]
    public class DoorsData {
        public Sprite leftSprite = default;
        public Sprite rightSprite = default;
    }

    void Start() {
        SetupIconByIndex(s_lastCloseIconIndex);
        SetActiveDoors(_showUpAtStart);
    }

    public void SetShowUpAtStart(bool showUpAtStart) {
        _showUpAtStart = showUpAtStart;
    }

    public void SetActiveDoors(bool active) {
        _leftDoor.SetActive(active);
        _rightDoor.SetActive(active);
    }

    public void SetStateWithoutAnimate(bool openState) {
        if (openState) {
            _slidingDoorsUIAnimation.Open(false, null);
        } else {
            _slidingDoorsUIAnimation.Close(false, null);
        }
    }

    void SetupRandomIcon() {
        var rndData = _doorsDatas.GetRandomValue();
        s_lastCloseIconIndex = _doorsDatas.Values.IndexOf(rndData);
        SetupIconByIndex(s_lastCloseIconIndex);
    }

    void SetupIconByIndex(int index) {
        if (index == -1) {
            SetupRandomIcon();

            return;
        }
        _leftDoorIcon.sprite = _doorsDatas.Values[s_lastCloseIconIndex].leftSprite;
        _rightDoorIcon.sprite = _doorsDatas.Values[s_lastCloseIconIndex].rightSprite;
    }

    void Open(Action callback) {
        SetupIconByIndex(s_lastCloseIconIndex);
        SetActiveDoors(true);
        _slidingDoorsUIAnimation.Open(true, callback);
    }

    void Close(Action callback) {
        SetupRandomIcon();
        SetActiveDoors(true);
        _slidingDoorsUIAnimation.Close(true, callback);
    }

    public override void FadeOut(Action callback = null) {
        Close(callback);
    }

    public override void FadeIn(Action callback = null) {
        Open(callback);
    }

    public override IEnumerator FadeOut() {
        SetupRandomIcon();
        SetActiveDoors(true);
        yield return _slidingDoorsUIAnimation.AnimateDoors(false, true, null);
    }

    public override IEnumerator FadeIn() {
        SetupIconByIndex(s_lastCloseIconIndex);
        SetActiveDoors(true);
        yield return _slidingDoorsUIAnimation.AnimateDoors(true, true, null);
    }
}
