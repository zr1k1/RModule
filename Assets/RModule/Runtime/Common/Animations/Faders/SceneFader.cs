using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : BaseSceneFader {

	[SerializeField] Image fadeOutUIImage;
	[SerializeField] float fadeSpeed = 0.8f;
	[SerializeField] bool enableFadeIn = true;
	[Range(0f, 1f)] [SerializeField] float initialAlpha = 0f;
	[SerializeField] bool waitForSpriteInGraphicComponentIsNotNull;

	//enum FadeDirection { In, Out }

	// ---------------------------------------------------------------
	// GameObject Lifecycle

	void OnEnable() {
		if (enableFadeIn) {
			FadeIn();
		} else {
			var color = fadeOutUIImage.color;
			color.a = initialAlpha;
			fadeOutUIImage.color = color;
			foreach (Transform child in fadeOutUIImage.transform) {
				Image img = child.GetComponent<Image>();
				if(img != null) {
					img.color = color;
				}
			}
		}
	}

	// ---------------------------------------------------------------
	// General Methods

	public override void FadeOut(Action callback = null) {
		StartCoroutine(FadeCo(FadeDirection.Out, callback));
	}

	public override void FadeIn(Action callback = null) {
		StartCoroutine(FadeCo(FadeDirection.In, callback));
	}

	public override IEnumerator FadeOut() {
		 yield return StartCoroutine(FadeCo(FadeDirection.Out));
	}

	public override IEnumerator FadeIn() {
		yield return StartCoroutine(FadeCo(FadeDirection.In));
	}

	// ---------------------------------------------------------------
	// Helpers

	IEnumerator FadeCo(FadeDirection fadeDirection, Action callback = null) {
		yield return Fade(fadeDirection);

		callback?.Invoke();
	}

	IEnumerator Fade(FadeDirection fadeDirection) {
		float alpha = fadeDirection == FadeDirection.In ? 1f : 0;
		float fadeEndValue = fadeDirection == FadeDirection.In ? 0 : 1f;
		if (fadeDirection == FadeDirection.In) {
			while (alpha >= fadeEndValue) {
				SetColorImage(ref alpha, fadeDirection);
				yield return null;
			}
			fadeOutUIImage.enabled = false;
		} else {
			fadeOutUIImage.enabled = true;
			if (waitForSpriteInGraphicComponentIsNotNull)
				while (fadeOutUIImage.sprite == null)
					yield return null;
			while (alpha < fadeEndValue) {
				SetColorImage(ref alpha, fadeDirection);
				yield return null;
			}
		}
	}

	void SetColorImage(ref float alpha, FadeDirection fadeDirection) {
		alpha += Time.deltaTime * (1.0f / fadeSpeed) * (fadeDirection == FadeDirection.In ? -1f : 1f);
		fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
		foreach (Transform child in fadeOutUIImage.transform) {
			Image img = child.GetComponent<Image>();
			if (img != null) {
				img.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
			}
		}
	}
}