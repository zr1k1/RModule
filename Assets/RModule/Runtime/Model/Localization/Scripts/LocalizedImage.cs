using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LocalizedImage : MonoBehaviour {

	// Outlets
	[SerializeField] LangSpriteDictionary _localizedImages = default;
	[SerializeField] bool _useAddressables;
	[SerializeField] bool _useResources = default;
	[SerializeField] string _imageBaseName = default;
	[SerializeField] List<Image> _alsoApplyImages = default;

	// Private vars
	Image _image;
	Sprite _localizedSprite;

	IEnumerator Start() {
		yield return LocalizationManager.WaitForInstanceCreatedAndInitialized();

		_image = GetComponent<Image>();
		if (_image == null) {
			Debug.LogError($"GameObject is not have Image component!");
			yield break;
		}

		if (_localizedImages != null && _localizedImages.Count > 0) {
			var currentLanguage = LocalizationManager.Instance.CurrentLanguage;
			_localizedImages.TryGetValue(currentLanguage, out var localizedSprite);
			SetSprite(localizedSprite);
		}

		if (_useAddressables) {
			var currentLanguage = LocalizationManager.Instance.CurrentLanguage;
			string fullAssetName = $"{_imageBaseName}{LocalizationManager.LanguageIdStringForType(currentLanguage)}";
			Addressables.LoadAssetAsync<Sprite>(fullAssetName).Completed += handle => {
				if (handle.Status == AsyncOperationStatus.Succeeded) {
					SetSprite(handle.Result);
				}
			};
		} else if (_useResources) {
			var currentLanguage = LocalizationManager.Instance.CurrentLanguage;
			string fullAssetName = $"{_imageBaseName}{LocalizationManager.LanguageIdStringForType(currentLanguage)}";
			var sprite = Resources.Load<Sprite>(fullAssetName);
			SetSprite(sprite);
		}
	}

	void SetSprite(Sprite sprite) {
		if (_image == null) {
			Debug.LogError($"GameObject is not have Image component!");
			return;
		}
		_image.enabled = true;
		_localizedSprite = sprite;
		_image.sprite = _localizedSprite;
		AlsoApplyToImages(sprite);
	}

	void AlsoApplyToImages(Sprite sprite) {
		if (_localizedSprite != null)
			foreach (var img in _alsoApplyImages) {
				img.enabled = true;
				img.sprite = sprite;
			}
		else {
			Debug.LogError($"LocalizedImage : _localizedSprite is null!");
		}
	}

	void OnDestroy() {
		if (_useAddressables && _localizedSprite != null) {
			Addressables.Release(_localizedSprite);
		}
	}
}
