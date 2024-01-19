using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LocalizedImage : MonoBehaviour {

	// Outlets
	[SerializeField] LangSpriteDictionary _localizedImages = default;
	[SerializeField] bool _useResources = default;
	[SerializeField] string _imageBaseName = default;

	IEnumerator Start() {
		yield return LocalizationManager.WaitForInstanceCreatedAndInitialized();

		var image = GetComponent<Image>();
		if (image == null)
			yield break;

		if (_localizedImages != null && _localizedImages.Count > 0) {
			var currentLanguage = LocalizationManager.Instance.CurrentLanguage;
			_localizedImages.TryGetValue(currentLanguage, out var localizedSprite);
			image.sprite = localizedSprite;
		}

		if (_useResources) {
			var currentLanguage = LocalizationManager.Instance.CurrentLanguage;
			string fullAssetName = $"{_imageBaseName}{LocalizationManager.LanguageIdStringForType(currentLanguage)}";
			var sprite = Resources.Load<Sprite>(fullAssetName);
			image.enabled = true;
			image.sprite = sprite;
		}
	}
}
