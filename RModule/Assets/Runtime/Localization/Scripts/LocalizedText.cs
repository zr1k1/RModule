using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {
	
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
			text.text = LocalizationManager.Instance.GetLocalizedValue(text.text);
		}

		var tmProText = GetComponent<TextMeshProUGUI>();
		if (tmProText != null) {
			tmProText.text = LocalizationManager.Instance.GetLocalizedValue(tmProText.text);
		}

		_localized = true;
	}
}
