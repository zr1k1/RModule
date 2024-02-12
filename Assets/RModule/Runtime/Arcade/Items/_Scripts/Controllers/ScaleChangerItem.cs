namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class ScaleChangerItem : ConsumableItem, IPickable {
		// Outlets
		[SerializeField] Sprite _lessThanOne = default;
		[SerializeField] Sprite _moreThanOne = default;
		[SerializeField] float _scaleModifier = default;

		public interface IScaleChangeable {
			void Scale(float scaleModifier);
		}

		protected override void Awake() {
			base.Awake();
			p_spriteRenderer.sprite = _scaleModifier < 1 ? _lessThanOne : _moreThanOne;
		}

		public override void Consume(GameObject consumer) {
			base.Consume(consumer);
			var iScaleChangeable = consumer.GetComponent<IScaleChangeable>();
			if (iScaleChangeable != null) {
				iScaleChangeable.Scale(_scaleModifier);
				gameObject.SetActive(false);
			}
		}
	}
}
