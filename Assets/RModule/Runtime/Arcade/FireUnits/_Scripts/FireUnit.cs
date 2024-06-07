namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using RModule.Runtime.DealDamageSystem;

	public class FireUnit : LevelElement, ISizeGetter, IUseable {
		// Events
		public UnityEvent<FireUnit> DidDestroyed = default;

		// Accessors
		public float Range => _range;

		// Outlets
		[SerializeField] protected DamageConfig p_damageConfig = default;
		// TODO when add new few fire units can be refactored to make config for FireUnits
		[SerializeField] protected float _range = default;
		[SerializeField] protected Vector3 _size = default;

		// Privats
		protected ContactDetector p_contactDetector;

		protected override void Awake() {
			base.Awake();
			p_contactDetector = gameObject.AddComponent<ContactDetector>();
			p_contactDetector.DidStartContact.AddListener(OnStartContact);
			p_contactDetector.DidEndContact.AddListener(OnEndContact);
		}

		protected virtual void Start() {
			Debug.LogError($"{transform.parent.parent.name} : Override Start method and use p_contactDetector.Setup(this);");
		}

		public virtual void OnStartContact(GameObject userGo) {
		}

		public virtual void OnEndContact(GameObject userGo) {
		}

		public Vector3 GetSize() {
			return _size;
		}

		public virtual void Use(GameObject userGo) {
		}

		public virtual void Die() {
			DidDestroyed?.Invoke(this);
			Destroy(gameObject);
		}
	}
}