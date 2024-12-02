using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {
	public enum PunctuationMark {
		None = 0,
		TwoDots = 1
	}

	// Outlets
	[SerializeField] PunctuationMark _addToTextEnding = default;
	[Tooltip("Can be null. Use only for hard behaviour.")]
	[SerializeField] LocalizationTextDataConfig _localizationTextDataConfig = default;

	// Private vars
	bool _localized;

	// ---------------------------------------------------------------
	// GameObject lifecycle

	IEnumerator Start() {
		yield return LocalizationManager.WaitForInstanceCreatedAndInitialized();

		TryToLocalize();
	}

	void OnEnable() {
		// Sometimes objects can be set inactive before it can localize itself (because of the Coroutine start method)
		// So we'll try to localize it again here
		if (!LocalizationManager.InstanceCreatedAndInitialized())
			return;

		TryToLocalize();
	}

	// ---------------------------------------------------------------
	// General Methods

	public static string T(string key) {
		return LocalizationManager.Instance.GetLocalizedValue(key);
	}

	// ---------------------------------------------------------------
	// Helpers

	void TryToLocalize() {
		if (_localized)
			return;

		var text = GetComponent<Text>();
		if (text != null) {
			if(_localizationTextDataConfig != null) {
				text.text = _localizationTextDataConfig.GetKey();
			}
			text.text = GetLocalizedValue(text.text);
		}

		var tmProText = GetComponent<TextMeshProUGUI>();
		if (tmProText != null) {
			if (_localizationTextDataConfig != null) {
				tmProText.text = _localizationTextDataConfig.GetKey();
			}
			tmProText.text = GetLocalizedValue(tmProText.text);
		}

		var tmProTextWorld = GetComponent<TextMeshPro>();
		if (tmProTextWorld != null) {
			if (_localizationTextDataConfig != null) {
				tmProTextWorld.text = _localizationTextDataConfig.GetKey();
			}
			tmProTextWorld.text = GetLocalizedValue(tmProTextWorld.text);
		}

		_localized = true;
	}

	string GetLocalizedValue(string localizationKey) {
		return $"{ LocalizationManager.Instance.GetLocalizedValue(localizationKey)}{GetTextEnding()}";
	}

	string GetTextEnding() {
		if (_addToTextEnding == PunctuationMark.TwoDots)
			return ":";

		return string.Empty;
	}
}
