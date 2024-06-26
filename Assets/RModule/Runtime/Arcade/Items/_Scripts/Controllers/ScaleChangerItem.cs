namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class ScaleChangerItem : ConsumableItem, IPickable {
		//Acessors
		public float ScaleModifier => _scaleModifier;
		public Vector3 MoveGoToPointAfterTransformation => _transformationPoint.position;

		// Outlets
		[SerializeField] Sprite _lessThanOne = default;
		[SerializeField] Sprite _moreThanOne = default;
		[SerializeField] float _scaleModifier = default;
		[SerializeField] Transform _transformationPoint = default;

		protected override void Awake() {
			base.Awake();
			p_spriteRenderer.sprite = _scaleModifier < 1 ? _lessThanOne : _moreThanOne;
		}

		protected override void Start() {
			p_contactDetector.Setup(this);
		}

		public override void Consume(GameObject consumer) {
			base.Consume(consumer);
		}
	}
}
