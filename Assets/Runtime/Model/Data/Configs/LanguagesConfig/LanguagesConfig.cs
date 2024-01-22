using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LanguagesConfig", menuName = "RModule/Examples/AppConfigs/LanguagesConfig", order = 1)]
public class LanguagesConfig : ScriptableObject {

	// Outlets
	[SerializeField] List<LanguageConfig> _languageConfigs = default;

	public Sprite GetLanguageImage(SystemLanguage systemLanguage) {
		return GetLanguageConfig(systemLanguage).Image;
	}
	
	public string GetLanguageLabel(SystemLanguage systemLanguage) {
		return GetLanguageConfig(systemLanguage).Label;
	}

	LanguageConfig GetLanguageConfig(SystemLanguage systemLanguage) {
		return _languageConfigs.Find(languageConfig => languageConfig.SystemLanguage == systemLanguage);
	}
}


