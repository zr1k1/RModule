using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RModule.Runtime.LeanTween;

public class MotivationMessagesVC : MonoBehaviour {
    // Outlets
    [SerializeField] TextMeshProUGUI _messageLabel = default;
    [SerializeField] Image _image = default; 
    [SerializeField] MotivationMessagesConfig _motivationMessagesConfig = default;
    [SerializeField] RectTransform[] _rectTransformsForChangeAlpha = default;
    [Header("Animation properties")]
    [SerializeField] float _animTime = default;

    // Private vars
    IEnumerator _showMessageCo;
    void Start() {
        _motivationMessagesConfig.Reset();
        StartCoroutine(ChangeAlpha(0f, false));
    }

    public void TryShowMessage() {
        string message = "";
        float viewDuration = 0;
        for (int i = 0; i < _motivationMessagesConfig.MotivationMessagesDictionary.Count; i++)
            if (_motivationMessagesConfig.ConditionForShowIsTrue((MotivationMessages.ShowType)i)) {
                message = _motivationMessagesConfig.GetRandomString((MotivationMessages.ShowType)i);
                viewDuration = _motivationMessagesConfig.GetViewDuration((MotivationMessages.ShowType)i);
            }

        if (!string.IsNullOrEmpty(message)) {
            if (_showMessageCo != null)
                StopCoroutine(_showMessageCo);
            _showMessageCo = ShowAndDeactiveAfterDelay(message, viewDuration);
            StartCoroutine(_showMessageCo);
            _image.sprite = _motivationMessagesConfig.GetRandomImg();
        }
    }

    IEnumerator ShowAndDeactiveAfterDelay(string message, float viewDuration) {
        _messageLabel.text = message;
        yield return ChangeAlpha(1f, true);
        yield return new WaitForSeconds(viewDuration - 2 * _animTime);
        yield return ChangeAlpha(0f, true);
    }

    IEnumerator ChangeAlpha(float alpha, bool animate) {
        float time = animate ? _animTime : 0f;
        foreach (var rt in _rectTransformsForChangeAlpha)
            LeanTween.alpha(rt, alpha, time);
        LeanTween.value(_messageLabel.gameObject.gameObject
            , (alpha) => { _messageLabel.color = new Color(_messageLabel.color.r, _messageLabel.color.g, _messageLabel.color.b, alpha); }
            , _messageLabel.color.a, alpha, time);
        yield return new WaitForSeconds(time);
    }
}