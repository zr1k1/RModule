namespace RModule.Runtime.Arcade {

	using RModule.Runtime.LeanTween;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class AutoDoorAnimationComponent : DoorAnimationComponent {
		// Enums
		public enum IconsIdleAnimationType { PingPongMove, PingPongScale }

		// Outlets
		[SerializeField] SpriteRendererHelper _spriteRendererHelper = default;
		[SerializeField] SpriteRenderer _spriteRendererFrom = default;
		[SerializeField] SpriteRenderer _spriteRendererTo = default;
		[SerializeField] List<SpriteRenderer> _iconsSpriteRenderers = default;
		[SerializeField] StateData _canBeOpenData = default;
		[SerializeField] StateData _closedData = default;

		// Icons
		[SerializeField] float _arrowsMoveDistance = default;
		[SerializeField] float _arrowsDuration = default;
		[SerializeField] float _crestsScaleFactor = default;

		// Line
		[SerializeField] GameObject _lineGo = default;
		[SerializeField] float _lineMoveDistance = default;
		[SerializeField] float _lineDuration = default;

		// Privats
		bool _skipAnimation;

		[Serializable]
		public class StateData {
			public Sprite spriteFrom = default;
			public Sprite spriteTo = default;
			public Sprite iconSprite = default;
			public List<GameObject> listOfObjectsToActivate = default;
		}

		void Start() {
			_lineGo.SetActive(false);
			StartPlayIconsAnimation(IconsIdleAnimationType.PingPongMove);
		}

		public void CloseAndSkipAnimation() {
			_skipAnimation = true;
			StartCoroutine(CloseAnimation());
		}

		void StartPlayIconsAnimation(IconsIdleAnimationType iconsIdleAnimationType) {
			foreach (var iconSprRend in _iconsSpriteRenderers) {
				iconSprRend.transform.localPosition = new Vector3(0, iconSprRend.transform.localPosition.y, iconSprRend.transform.localPosition.z);
				iconSprRend.transform.localScale = Vector3.one;
				if (iconsIdleAnimationType == IconsIdleAnimationType.PingPongMove)
					LeanTween.moveLocalX(iconSprRend.gameObject, _arrowsMoveDistance, _arrowsDuration).setLoopPingPong();
				else if (iconsIdleAnimationType == IconsIdleAnimationType.PingPongScale)
					LeanTween.scale(iconSprRend.gameObject, Vector3.one * _crestsScaleFactor, _arrowsDuration).setLoopPingPong();
			}
		}

		public override IEnumerator OpenAnimation() {
			_spriteRendererFrom.sprite = _canBeOpenData.spriteFrom;
			_spriteRendererTo.sprite = _canBeOpenData.spriteTo;
			_spriteRendererHelper.UpdateParameters();
			foreach (var iconSprRend in _iconsSpriteRenderers) {
				iconSprRend.sprite = _canBeOpenData.iconSprite;
			}
			LeanTween.cancel(_spriteRendererFrom.gameObject);
			LeanTween.cancel(_spriteRendererTo.gameObject);

			LeanTween.color(_spriteRendererFrom.gameObject, new Color(1, 1, 1, 0), p_alphaDuration * skipAnimationModifier());
			LeanTween.color(_spriteRendererTo.gameObject, new Color(1, 1, 1, 1), p_alphaDuration * skipAnimationModifier());
			foreach (var iconSprRend in _iconsSpriteRenderers) {
				LeanTween.color(iconSprRend.gameObject, new Color(1, 1, 1, 0), p_alphaDuration * skipAnimationModifier());
			}

			_lineGo.SetActive(true);
			LeanTween.moveLocalY(_lineGo, _lineGo.transform.localPosition.y + _lineMoveDistance, _lineDuration * skipAnimationModifier());
			yield return new WaitForSeconds(p_alphaDuration * skipAnimationModifier());
			LeanTween.color(_spriteRendererTo.gameObject, new Color(1, 1, 1, 0), p_alphaDuration * skipAnimationModifier());
			yield return new WaitForSeconds(p_alphaDuration * skipAnimationModifier());
			_lineGo.SetActive(false);
		}

		public override IEnumerator CloseAnimation() {
			_spriteRendererFrom.sprite = _closedData.spriteFrom;
			_spriteRendererTo.sprite = _closedData.spriteTo;
			_spriteRendererHelper.UpdateParameters();
			foreach (var iconSprRend in _iconsSpriteRenderers) {
				iconSprRend.sprite = _closedData.iconSprite;
			}
			LeanTween.cancel(_spriteRendererFrom.gameObject);
			LeanTween.cancel(_spriteRendererTo.gameObject);

			LeanTween.color(_spriteRendererFrom.gameObject, new Color(1, 1, 1, 1), p_alphaDuration * skipAnimationModifier());
			yield return new WaitForSeconds(p_alphaDuration * skipAnimationModifier());
			LeanTween.color(_spriteRendererFrom.gameObject, new Color(1, 1, 1, 0), p_alphaDuration * skipAnimationModifier());
			LeanTween.color(_spriteRendererTo.gameObject, new Color(1, 1, 1, 1), p_alphaDuration * skipAnimationModifier());
			foreach (var iconSprRend in _iconsSpriteRenderers) {
				LeanTween.cancel(iconSprRend.gameObject);
				LeanTween.color(iconSprRend.gameObject, new Color(1, 1, 1, 1), p_alphaDuration * skipAnimationModifier());
			}
			StartPlayIconsAnimation(IconsIdleAnimationType.PingPongScale);
			yield return new WaitForSeconds(p_alphaDuration * skipAnimationModifier());
		}

		float skipAnimationModifier() {
			return _skipAnimation ? 0f : 1f;
		}
	}
}
