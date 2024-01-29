namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class Star : CollectedItem, IStar {
		// Outlets
		[SerializeField] StarsConfig _starsConfig = default;
		[SerializeField] int _idFromConfig = default;

		protected override void Awake() {
			base.Awake();
			p_spriteRenderer.sprite = _starsConfig.GetIngameSpriteById(_idFromConfig);
		}

		public int GetID() {
			return _idFromConfig;
		}

		public GameObject GetGo() {
			return gameObject;
		}
	}
}
