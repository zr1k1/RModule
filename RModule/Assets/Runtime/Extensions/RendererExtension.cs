using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RendererExtension {
	/// <summary>
	/// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
	/// </summary>
	/// <returns>The amount of bounding box corners that are visible from the Camera.</returns>
	/// <param name="rectTransform">Rect transform.</param>
	/// <param name="camera">Camera.</param>
	private static int CountCornersVisibleFrom(this RectTransform rectTransform, Camera camera) {
		Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
		Vector3[] objectCorners = new Vector3[4];
		rectTransform.GetWorldCorners(objectCorners);

		int visibleCorners = 0;
		Vector3 tempScreenSpaceCorner; // Cached
		for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
		{
			tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); // Transform world space position of corner to screen space
			if (screenBounds.Contains(tempScreenSpaceCorner)) // If the corner is inside the screen
			{
				visibleCorners++;
			}
		}
		return visibleCorners;
	}

	private static int CountCornersVisibleFrom(this Transform transform, Vector2 objSize,Camera camera) {
		Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
		Vector3[] objectCorners = new Vector3[4];
		objectCorners[0] = new Vector3(transform.position.x - objSize.x / 2f, transform.position.y - objSize.y / 2f);
		objectCorners[1] = new Vector3(transform.position.x - objSize.x / 2f, transform.position.y + objSize.y / 2f);
		objectCorners[2] = new Vector3(transform.position.x + objSize.x / 2f, transform.position.y + objSize.y / 2f);
		objectCorners[3] = new Vector3(transform.position.x + objSize.x / 2f, transform.position.y - objSize.y / 2f);

		int visibleCorners = 0;
		Vector3 tempScreenSpaceCorner; // Cached
		for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
		{
			tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); // Transform world space position of corner to screen space
			if (screenBounds.Contains(tempScreenSpaceCorner)) // If the corner is inside the screen
			{
				visibleCorners++;
			}
		}
		return visibleCorners;
	}

	/// <summary>
	/// Determines if this RectTransform is fully visible from the specified camera.
	/// Works by checking if each bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
	/// </summary>
	/// <returns><c>true</c> if is fully visible from the specified camera; otherwise, <c>false</c>.</returns>
	/// <param name="rectTransform">Rect transform.</param>
	/// <param name="camera">Camera.</param>
	public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera) {
		return CountCornersVisibleFrom(rectTransform, camera) == 4; // True if all 4 corners are visible
	}

	public static bool IsFullyVisibleFrom(this Transform transform, Vector2 objSize, Camera camera) {
		return CountCornersVisibleFrom(transform, objSize, camera) == 4; // True if all 4 corners are visible
	}

	public static bool IsFullyInvisibleFrom(this RectTransform rectTransform, Camera camera) {
		return CountCornersVisibleFrom(rectTransform, camera) == 0; // True if all 4 corners are visible
	}

	public static bool IsFullyInvisibleFrom(this Transform transform, Vector2 objSize, Camera camera) {
		return CountCornersVisibleFrom(transform, objSize, camera) == 0; // True if all 4 corners are visible
	}

	/// <summary>
	/// Determines if this RectTransform is at least partially visible from the specified camera.
	/// Works by checking if any bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
	/// </summary>
	/// <returns><c>true</c> if is at least partially visible from the specified camera; otherwise, <c>false</c>.</returns>
	/// <param name="rectTransform">Rect transform.</param>
	/// <param name="camera">Camera.</param>
	public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera) {
		return CountCornersVisibleFrom(rectTransform,camera) > 0; // True if any corners are visible
	}

	public static bool IsVisibleFrom(this Transform transform, Vector2 objSize, Camera camera) {
		return CountCornersVisibleFrom(transform, objSize, camera) > 0; // True if any corners are visible
	}
}
