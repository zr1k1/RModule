namespace RModule.Runtime.Arcade {

	using System.Collections.Generic;
	using UnityEngine;

	public class CheckPoint : UnPickableItem, IUseable {
		// Accessors
		public List<Transform> Points => _pointsTrs;

		// Outlets
		[SerializeField] List<Transform> _pointsTrs = default;

		// Privats
		bool _isActivated;

		protected override void Start() {
			p_spriteRenderer.enabled = false;
		}

		public override void Use(GameObject userGo) {
			base.Use(userGo);
			if (_isActivated)
				return;
			_isActivated = true;
			Debug.Log($"CheckPoint : Use");
		}
	}
}
