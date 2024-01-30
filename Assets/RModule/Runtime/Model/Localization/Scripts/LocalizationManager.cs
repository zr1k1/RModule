using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class LocalizationManager : SingletonMonoBehaviour<LocalizationManager> {
	// Accessors
	public bool StringsLoaded => _stringsLoaded;
	public SystemLanguage CurrentLanguage => _currentLanguage;
	public SystemLanguage[] SupportedLanguages => _supportedLanguages;

	// Outlets
	[SerializeField] string _baseStringsFileName = "_strings";
	[Tooltip("Assets/Resources/...")]
	[SerializeField] string _localizationFolderName = "locData";
	[SerializeField] SystemLanguage[] _supportedLanguages = default;

	[Header("Debug only"), Space]
	[SerializeField] SystemLanguage _debugLanguage = default;
	[SerializeField] bool _useDebugLanguage = default;

	// Private vars
	Dictionary<string, object> _localizationDictionary = new ();
	SystemLanguage _currentLanguage;
	bool _stringsLoaded;
	bool _isLoadingStrings;
	string _savedLanguageId;

	// Const
	const SystemLanguage c_defaultLanguage = SystemLanguage.English;
	const string k_savedLanguage = "savedLanguage";

	public IEnumerator Initialize() {
		Debug.Log("LocalizationManager Initialize");
		_savedLanguageId = PlayerPrefs.GetString(k_savedLanguage, "");

		// For some reason addressable strings loading fails the first time it is called
		// So we'll wait here and keep retrying until they are loaded
		while (!_stringsLoaded) {
			TryAutoPickLanguage();
			yield return null;
		}

		yield return WaitForInitialized();
		Debug.Log("LocalizationManager Initialized");
	}

	public override bool IsInitialized() {
		return _stringsLoaded && StringsAreValid();
	}
	void TryAutoPickLanguage() {
		if (!_isLoadingStrings)
			AutopickLanguage();
	}

	void AutopickLanguage(Action<bool> callback = null) {
		var pickedLanguage = c_defaultLanguage;

		if (LanguageIsSupported(Application.systemLanguage))
			pickedLanguage = Application.systemLanguage;

		if (SystemLanguageHelper.CurrentOperationSystemLocaleExistInLanguagesListForConvertToRussianSystemLanguage())
			pickedLanguage = SystemLanguage.Russian;

		if (!string.IsNullOrEmpty(_savedLanguageId))
			pickedLanguage = LanguageForIdString(_savedLanguageId);

		if (_useDebugLanguage && Application.isEditor)
			pickedLanguage = _debugLanguage;

		SwitchLanguage(pickedLanguage, callback);
	}

	public bool LanguageIsSupported(SystemLanguage language) {
		return _supportedLanguages.Contains(language);
	}

	public void SwitchLanguage(SystemLanguage language, Action<bool> callback = null) {
		if (_currentLanguage != language) {
			_currentLanguage = language;

			SetSavedLanguage(_currentLanguage);
			LoadStringsForLanguage(_currentLanguage, callback);
		} else {
			if (_stringsLoaded) {
				callback?.Invoke(true);
			} else {
				LoadStringsForLanguage(_currentLanguage, callback);
			}
		}
	}

	public void LoadStringsForLanguage(SystemLanguage language, Action<bool> callback) {
		string languageId = LanguageIdStringForType(language);
		LoadStringsForLanguageId(languageId, (success) => {
			callback?.Invoke(success);
		});
	}

	public void LoadStringsForLanguageId(string languageId, Action<bool> callback) {
		if (_isLoadingStrings)
			// Silently return without any callback so we can call this method multiple times
			// from coroutine and receive only one callback from the initial call when it is completed
			return;

		if (string.IsNullOrEmpty(languageId)) {
			callback?.Invoke(false);
			return;
		}

		string fileName = languageId + _baseStringsFileName;
		string filePath = $"{_localizationFolderName}/{fileName}";

		_isLoadingStrings = true;
		_localizationDictionary.Clear();


		ResourceRequest rr = Resources.LoadAsync<TextAsset>(filePath);
		rr.completed += handle => {
			if (handle.isDone) {
				TextAsset textAsset = rr.asset as TextAsset;
				if (textAsset != null) {
					var jsonString = textAsset.text;
					var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
					if (!_localizationDictionary.TryAddRange(dict, out string key)) {
						_localizationDictionary = null;
						callback?.Invoke(false);
					} else {
						_stringsLoaded = true;
						callback?.Invoke(true);
					}
				} else {
					_localizationDictionary = null;
					callback?.Invoke(false);
				}
			} else {
				Debug.LogError("Failed to load localized text file");
				callback?.Invoke(false);
			}

			_isLoadingStrings = false;
		};


	}

	public string GetLocalizedValue(string key) {
		if (_localizationDictionary == null)
			return key;

		return _localizationDictionary.TryGetValue(key, out var locValue) ? locValue as string : key;
	}

	void SetSavedLanguage(SystemLanguage language) {
		string languageId = LanguageIdStringForType(language);
		if (_savedLanguageId != languageId) {
			_savedLanguageId = languageId;
			PlayerPrefs.SetString(k_savedLanguage, _savedLanguageId);
			PlayerPrefs.Save();
		}
	}

	public bool StringsAreValid() {
		return GetLocalizedValue("kStringsAreValid") == "1";
	}

	public string LanguageIdForCurrentLocale() {
		return LanguageIdStringForType(_currentLanguage);
	}

	public static string LanguageIdStringForType(SystemLanguage language) {
		string lang = "";

		switch (language) {
			case SystemLanguage.German:
				lang = "de";
				break;
			case SystemLanguage.English:
				lang = "en";
				break;
			case SystemLanguage.Spanish:
				lang = "es";
				break;
			case SystemLanguage.French:
				lang = "fr";
				break;
			case SystemLanguage.Italian:
				lang = "it";
				break;
			case SystemLanguage.Japanese:
				lang = "ja";
				break;
			case SystemLanguage.Korean:
				lang = "ko";
				break;
			case SystemLanguage.Dutch:
				lang = "nl";
				break;
			case SystemLanguage.Portuguese:
				lang = "pt";
				break;
			case SystemLanguage.Russian:
				lang = "ru";
				break;
			case SystemLanguage.Turkish:
				lang = "tr";
				break;

			default:
				lang = "en";
				break;
		}

		return lang;
	}

	public static SystemLanguage LanguageForIdString(string idString) {
		var langType = SystemLanguage.English;

		if (idString == "de")
			langType = SystemLanguage.German;
		else if (idString == "en")
			langType = SystemLanguage.English;
		else if (idString == "es")
			langType = SystemLanguage.Spanish;
		else if (idString == "fr")
			langType = SystemLanguage.French;
		else if (idString == "it")
			langType = SystemLanguage.Italian;
		else if (idString == "ja")
			langType = SystemLanguage.Japanese;
		else if (idString == "ko")
			langType = SystemLanguage.Korean;
		else if (idString == "nl")
			langType = SystemLanguage.Dutch;
		else if (idString == "pt")
			langType = SystemLanguage.Portuguese;
		else if (idString == "ru")
			langType = SystemLanguage.Russian;
		else if (idString == "tr")
			langType = SystemLanguage.Turkish;

		return langType;
	}
}
