namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class FireUnit : LevelElement, ISizeGetter, IUseable {
		// Outlets
		[SerializeField] protected DamageConfig p_damageConfig = default;
		[SerializeField] protected Vector3 _size = default;

		// Privats
		protected ContactDetector p_contactDetector;

		protected override void Awake() {
			base.Awake();
			//if (spriteRenderer == null)
			//	spriteRenderer = GetComponent<SpriteRenderer>();

			//p_collider2D = GetComponent<Collider2D>();
			//p_rigidbody2D = GetComponent<Rigidbody2D>();
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
			Destroy(gameObject);
		}
	}
}