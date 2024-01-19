using System;
using UnityEngine;

[Serializable]
public class LanguageConfig {

	// Accessors
	public SystemLanguage SystemLanguage => _systemLanguage;
	public Sprite Image => _icon;
	public string Label => _label;

	// Outlets
	[SerializeField] SystemLanguage _systemLanguage = default;
	[SerializeField] Sprite _icon = default;
	[SerializeField] string _label = default;
}
