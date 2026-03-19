using UnityEngine;
using System.Collections;
using System;
using RModule.Runtime.LeanTween;

public class SlidingDoorsUIAnimation : MonoBehaviour {
    [Header("Doors")]
    [SerializeField] RectTransform _leftDoor;
    [SerializeField] RectTransform _rightDoor;

    [Header("Settings")]
    [SerializeField] float _openDistance = default;
    [SerializeField] float _speed = default;

    [Header("Debug fields")]
    [SerializeField] bool _testOpen;
    [SerializeField] bool _testClose;

    Vector2 _leftClosedPos;
    Vector2 _rightClosedPos;
    Coroutine _currentAnim;

    void Awake() {
        _leftClosedPos = _leftDoor.anchoredPosition;
        _rightClosedPos = _rightDoor.anchoredPosition;
    }

    void Update() {
        if (_testOpen) {
            _testOpen = false;
            Open(true, null);
        }
        if (_testClose) {
            _testClose = false;
            Close(true, null);
        }
    }

    public void Open(bool animate, Action finishCallback) {
        StartAnimation(true, animate, finishCallback);
    }

    public void Close(bool animate, Action finishCallback) {
        StartAnimation(false, animate, finishCallback);
    }

    void StartAnimation(bool open, bool animate, Action finishCallback) {
        if (_currentAnim != null)
            StopCoroutine(_currentAnim);

        _currentAnim = StartCoroutine(AnimateDoors(open, animate, finishCallback));
    }

    public IEnumerator AnimateDoors(bool open, bool animate, Action finishCallback) {
        Vector2 leftTarget = open
            ? _leftClosedPos + Vector2.left * _openDistance
            : _leftClosedPos;

        Vector2 rightTarget = open
            ? _rightClosedPos + Vector2.right * _openDistance
            : _rightClosedPos;

        bool inProgress = false;
        if (animate) {
            inProgress = true;

            LeanTween.moveLocalX(_leftDoor.gameObject, leftTarget.x, 1f / _speed);
            LeanTween.moveLocalX(_rightDoor.gameObject, rightTarget.x, 1f / _speed).setOnComplete(() => {
                inProgress = false;
            });
            while (inProgress) {
                yield return null;
            }
        }

        _leftDoor.anchoredPosition = leftTarget;
        _rightDoor.anchoredPosition = rightTarget;
        finishCallback?.Invoke();
    }
}
