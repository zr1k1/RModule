using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace RModule.Runtime.GraphicKitsSystem {
	public abstract class BaseGraphicSetter<GraphicKitKey, GraphicKitValueType> : MonoBehaviour, IGraphicKitElement
		where GraphicKitKey : Enum {

		// Outlets
		[SerializeField] protected GraphicKitKey _graphicKitKey = default;
		[SerializeField] protected GraphicKitValueType _graphicKitValueType = default;
		[SerializeField] protected string _key = default;

		// Privats
		protected SpriteRenderer _spriteRenderer;
		protected Image _image;

		protected virtual void Awake() {
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_image = GetComponent<Image>();

			if (BaseGraphicKitsManager<GraphicKitKey, GraphicKitValueType>.InstanceCreatedAndInitialized())
				UpdateView();
		}

		protected virtual IEnumerator Start() {
			yield return BaseGraphicKitsManager<GraphicKitKey, GraphicKitValueType>.WaitForInstanceCreatedAndInitialized();

			BaseGraphicKitsManager<GraphicKitKey, GraphicKitValueType>.Instance?.AddElement(_graphicKitKey, this);

			UpdateView();
		}

		protected virtual void OnEnable() {
			if (BaseGraphicKitsManager<GraphicKitKey, GraphicKitValueType>.InstanceCreatedAndInitialized())
				BaseGraphicKitsManager<GraphicKitKey, GraphicKitValueType>.Instance?.AddElement(_graphicKitKey, this);
		}

		protected virtual void OnDisable() {
			if (BaseGraphicKitsManager<GraphicKitKey, GraphicKitValueType>.InstanceCreatedAndInitialized())
				BaseGraphicKitsManager<GraphicKitKey, GraphicKitValueType>.Instance?.RemoveElement(_graphicKitKey, this);
		}

		public void UpdateView() {
			if (!enabled)
				return;

			UpdateViewBody();
		}

		protected abstract void UpdateViewBody();
	}
}
