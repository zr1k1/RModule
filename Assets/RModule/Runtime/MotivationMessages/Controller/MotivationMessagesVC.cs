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
    [SerializeField] float _delayBeforeShow = default;
    [SerializeField] bool _autoHideAfterShowUp = true;

    // Private vars
    IEnumerator _showMessageCo;

    void Start() {
        _motivationMessagesConfig.Reset();
        StartCoroutine(ChangeAlpha(0f, false));
        _messageLabel.gameObject.SetActive(false);
    }

    public void TryShowMessage() {
        string key = "";
        float viewDuration = 0;
        for (int i = 0; i < _motivationMessagesConfig.MotivationMessagesDictionary.Count; i++)
            if (_motivationMessagesConfig.ConditionForShowIsTrue((MotivationMessages.ShowType)i)) {
                key = _motivationMessagesConfig.GetRandomString((MotivationMessages.ShowType)i);
                viewDuration = _motivationMessagesConfig.GetViewDuration((MotivationMessages.ShowType)i);
            }

        if (!string.IsNullOrEmpty(key)) {
            if (_showMessageCo != null)
                StopCoroutine(_showMessageCo);
            _showMessageCo = ShowAndDeactiveAfterDelay(key, viewDuration);
            StartCoroutine(_showMessageCo);
            if(_motivationMessagesConfig.TryGetRandomImg(out var randomImg)) {
                _image.sprite = randomImg;
            }
        }
    }

    IEnumerator ShowAndDeactiveAfterDelay(string key, float viewDuration) {
        yield return new WaitForSeconds(_delayBeforeShow);
        _messageLabel.text = LocalizedText.T(key);
        _messageLabel.gameObject.SetActive(true);
        yield return ChangeAlpha(1f, true);
        yield return new WaitForSeconds(viewDuration - 2 * _animTime);
		if (_autoHideAfterShowUp) {
            yield return ChangeAlpha(0f, true);
            _messageLabel.gameObject.SetActive(false);
        }
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