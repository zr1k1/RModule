using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Возможные применения:
//Скейлит размер бекграйнда по ширине или высоте для уровня (targetSize) чтобы границы бг совпадали с границой уровня + поле обзора камеры

public class SpriteGameObjectResizerHelper {

	public static Vector3 ReSize(GameObject toResizeSpriteGameObject, Vector2 targetSize, Camera camera, bool _createPrimitiveImitationTarget = false) {
		var sprite = toResizeSpriteGameObject.GetComponent<SpriteRenderer>().sprite;
		var spriteSize = sprite.rect.size;

		var camHeight = 2 * camera.orthographicSize;
		var camWidth = camHeight * camera.aspect;
		var xAdditionalValue = 0f;
		var yAdditionalValue = 0f;
		if (targetSize.x < camWidth) {
			targetSize = new Vector2(camWidth, targetSize.y);
		}
		if (targetSize.y < camHeight) {
			targetSize = new Vector2(targetSize.x, camHeight);
		}
		var totalSizeWithCamFieldView = targetSize + new Vector2(xAdditionalValue, yAdditionalValue);
		var totalSizeAspect = totalSizeWithCamFieldView.x / totalSizeWithCamFieldView.y;
		var spriteAspect = spriteSize.x / spriteSize.y;
		float scale = spriteAspect > totalSizeAspect ? totalSizeWithCamFieldView.y * sprite.pixelsPerUnit / spriteSize.y
			: totalSizeWithCamFieldView.y * sprite.pixelsPerUnit / spriteSize.x;
		toResizeSpriteGameObject.transform.localScale = new Vector3(scale, scale, 1f);

		if (_createPrimitiveImitationTarget) {
			var imitateLevelSizeGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
			imitateLevelSizeGo.transform.localScale = new Vector3(targetSize.x, targetSize.y, 1f);
		}

		return new Vector3(scale, scale, 1f);
	}
}
