using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class AddressableGraphicSetter : MonoBehaviour {
	public enum GraphicCompoType { }
	// Outlets
	[Header("Set Address or AssetReference")]
	[SerializeField] string _address = default;
	[SerializeField] AssetReference _assetReference = default;
	[SerializeField] bool _changeSourceColorToCustomOnAwake = default;
	[SerializeField] Color _settedToComponentOnAwake = Color.white;

	// Private vars
	Image _image;
	SpriteRenderer _spriteRenderer;
	AddressableAssetProvider _graphicProvider;
	bool _beginCheckWhenSpriteComponentOnDisabled;
	bool _beginCheckWhenSpriteComponentOnEnabled;
	Color _sourceColor;

	private void Awake() {
		_image = GetComponent<Image>();
		_spriteRenderer = GetComponent<SpriteRenderer>();

		Assert.IsTrue((_image != null) || (_spriteRenderer != null), "Not setted component for Sprite");

		if (_image != null)
			_sourceColor = _changeSourceColorToCustomOnAwake ? _settedToComponentOnAwake : _image.color;
		else
			_sourceColor = _changeSourceColorToCustomOnAwake ? _settedToComponentOnAwake : _spriteRenderer.color;

		_beginCheckWhenSpriteComponentOnEnabled = true;
	}

	private void OnEnable() {
		if ((_image != null && _image.enabled) || (_spriteRenderer != null && _spriteRenderer.enabled))
			ProvideSprite();
	}

	private void OnDisable() {
		if ((_image != null && _image.enabled) || (_spriteRenderer != null && _spriteRenderer.enabled))
			Release();
	}

	private void Update() {
		if (_beginCheckWhenSpriteComponentOnEnabled) {
			if((_image != null && _image.enabled) || (_spriteRenderer != null && _spriteRenderer.enabled))
				TryProvide();
		} else if (_beginCheckWhenSpriteComponentOnDisabled) {
			if ((_image != null && !_image.enabled) || (_spriteRenderer != null && !_spriteRenderer.enabled))
				Release();
		}
	}

	void TryProvide() {
		Debug.Log("TryProvide");
		if ((!string.IsNullOrEmpty(_address) || _assetReference != null) && (_image != null || _spriteRenderer != null))
			if (_graphicProvider == null)
				ProvideSprite();
	}

	void ProvideSprite() {
		_graphicProvider = new AddressableAssetProvider();
		if(!string.IsNullOrEmpty(_address))
			_graphicProvider.LoadAsset<Sprite>(_address, SetSprite);
		else if(_assetReference != null) {
			_graphicProvider.LoadAsset<Sprite>(_assetReference, SetSprite);
		}
		_beginCheckWhenSpriteComponentOnEnabled = false;
		_beginCheckWhenSpriteComponentOnDisabled = true;

	}

	void SetSprite(Sprite sprite) {
		if (_image != null) {
			_image.sprite = sprite;
		} else {
			_spriteRenderer.sprite = sprite;
		}
		SetColor(_sourceColor);
	}

	void Release() {
		Debug.Log("Release");
		SetSprite(null);
		_beginCheckWhenSpriteComponentOnDisabled = false;
		_beginCheckWhenSpriteComponentOnEnabled = true;
		Destroy(_graphicProvider.AddressableDataTempGameObjectContainer);
		_graphicProvider = null;

		SetColor(new Color(_sourceColor.r, _sourceColor.g, _sourceColor.b, 0f));
	}

	void SetColor(Color color) {
		if (_image != null) {
			_image.color = color;
		} else {
			_spriteRenderer.color = color;
		}
	}
}
