namespace RModule.Runtime.Arcade {

	using UnityEngine;
	using UnityEngine.Events;

	public class BaseBlock : LevelElement, ISortable {
		// Events
		public UnityEvent<BaseBlock> WillDestroyed = default;

		// Outlets
		[SerializeField] protected SpriteRenderer spriteRenderer = default;

		// Privats
		protected Collider2D p_collider2D;
		protected Rigidbody2D p_rigidbody2D;
		protected ContactDetector p_contactDetector;

		protected override void Awake() {
			base.Awake();
			if (spriteRenderer == null)
				spriteRenderer = GetComponent<SpriteRenderer>();

			p_collider2D = GetComponent<Collider2D>();
			p_rigidbody2D = GetComponent<Rigidbody2D>();
			p_contactDetector = gameObject.AddComponent<ContactDetector>();
			p_contactDetector.DidStartContact.AddListener(OnStartContact);
			p_contactDetector.DidEndContact.AddListener(OnEndContact);
		}

		protected virtual void Start() {
			Debug.LogError($"{transform.parent.parent.name} : Override Start method and use _contactDetector.Setup(this)");
		}

		public virtual void Die() {
			WillDestroyed?.Invoke(this);
		}

		public int GetSortingOrder() {
			return spriteRenderer.sortingOrder;
		}

		public virtual void OnStartContact(GameObject userGo) {
		}

		public virtual void OnEndContact(GameObject userGo) {
		}
	}
}
