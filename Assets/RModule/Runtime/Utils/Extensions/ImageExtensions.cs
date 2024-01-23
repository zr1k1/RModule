using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ImageExtensions {
	public static void ResizeRectTransformWithTextureProportions(this Image image, Sprite sprite, float toSize) {
		var width = sprite.textureRect.width;
		var height = sprite.textureRect.height;

		if (height > width) {
			width = width / height * toSize;
			height = toSize;
		} else {
			height = height / width * toSize;
			width = toSize;
		}

		image.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
		image.sprite = sprite;
	}

	public static int NotTransparentPixelsCount(this Texture2D texture) {
		var pixels = texture.GetPixels();
		int count = 0;
		for(int i = 0; i < pixels.Length; i++)
			if(pixels[i].a > 0)
				count++;

		return count;
	}

	public static int GetColorCountWithoutTransparent(this Texture2D texture) {
		List<string> hexColors = new List<string>();
		var pixels = texture.GetPixels();
		for (int i = 0; i < pixels.Length; i++) {
			var hexColor = pixels[i].ToHexString();
			if (pixels[i].a == 1f && !hexColors.Contains(hexColor)) {
				hexColors.Add(hexColor);
			}
		}
		Debug.Log($"hexColors.Count = { hexColors.Count }");
		return hexColors.Count;
	}
}
