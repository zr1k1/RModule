using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.GraphicKitsSystem;

public class ExampleGraphicSetter : BaseGraphicSetter<ExampleGraphicKitKey, ExampleGraphicKitValueType> {
	protected override void UpdateViewBody() {

		if (_graphicKitValueType == ExampleGraphicKitValueType.SpriteAddress) {
			var address = ExampleGraphicKitsManager.Instance.GetGraphicKitValue<string>(_graphicKitKey, _key);
			new AddressableAssetProvider().LoadAsset<Sprite>(address, (sprite) => {
				if (_spriteRenderer != null) {
					_spriteRenderer.sprite = sprite;
				} else if (_image != null) {
					_image.sprite = sprite;
				} else {
					Debug.LogError("Component for set sprite  is not exist");
				}
			});
		} else if (_graphicKitValueType == ExampleGraphicKitValueType.Color) {
			_spriteRenderer.color = ExampleGraphicKitsManager.Instance.GetGraphicKitValue<Color>(_graphicKitKey, _key);
		}
	}
}
