using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class TextMeshProTextsUpdater : MonoBehaviour {
	public delegate string GetStringByTextMeshProIndex(int index);

	// Delegates
	public GetStringByTextMeshProIndex D_GetStringByTextMeshProIndex = default;

	// Outlets
	[SerializeField] SerializableDictionary<TextMeshPro, string> _textsToUpdate = default;
	[SerializeField] bool _getFromOutSourceByIndex = default;

	public void UpdateText() {
		if(_getFromOutSourceByIndex && D_GetStringByTextMeshProIndex == null) {
			Debug.LogError($"TextMeshProTextsUpdater : For set text from out source you need to set D_GetStringByTextMeshProIndex!");
		}

		for (int i = 0; i < _textsToUpdate.Count; i++) {
			_textsToUpdate.ElementAt(i).Key.text = _getFromOutSourceByIndex ? D_GetStringByTextMeshProIndex(i) : _textsToUpdate.ElementAt(i).Value;
		}
	}
}
