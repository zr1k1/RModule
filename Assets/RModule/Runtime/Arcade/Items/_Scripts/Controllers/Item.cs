namespace RModule.Runtime.Arcade {

	using UnityEngine;
	using UnityEngine.Events;
	using RModule.Runtime.Sounds;
	using RModule.Runtime.LeanTween;

	public class Item : LevelElement, ILevelPauseHandler {
		// Events
		public UnityEvent DidDestroyed = default;

		// Accessors
		public SpriteRenderer SpriteRenderer => p_spriteRenderer;
		public Rigidbody2D Rigidbody2D => p_rigidbody2D;
		public Vector2 StartPosition => p_startPosition;

		// Outlets
		[SerializeField] protected SpriteRenderer p_spriteRenderer = default;
		[SerializeField] protected SoundEffectPlayer p_sfx;
		[SerializeField] protected ItemAnimationComponent p_itemAnimationComponent = default;

		// Privats
		protected Collider2D p_collider2D;
		protected Rigidbody2D p_rigidbody2D;
		protected Vector2 p_startPosition;

		protected override void Awake() {
			base.Awake();
			if (p_spriteRenderer == null)
				p_spriteRenderer = GetComponent<SpriteRenderer>();
			p_collider2D = GetComponent<Collider2D>();
			p_rigidbody2D = GetComponent<Rigidbody2D>();
			p_sfx = GetComponent<SoundEffectPlayer>();
			p_startPosition = transform.position;
		}

		protected virtual void OnTriggerEnter2D(Collider2D collision) {
			var itemContactHandlers = collision.GetComponents<IItemContactHandler>();
			if (itemContactHandlers.Length > 0) {
				foreach (var itemContactHandler in itemContactHandlers)
					itemContactHandler.OnStartContactWithItem(this);
			}
		}

		protected virtual void OnTriggerExit2D(Collider2D collision) {
			var itemContactHandlers = collision.GetComponents<IItemContactHandler>();
			if (itemContactHandlers.Length > 0) {
				foreach (var itemContactHandler in itemContactHandlers)
					itemContactHandler.OnEndContactWithItem(this);
			}
		}

		protected virtual void OnCollisionEnter2D(Collision2D collision) {
			var itemContactHandlers = collision.gameObject.GetComponents<IItemContactHandler>();
			if (itemContactHandlers.Length > 0) {
				foreach (var itemContactHandler in itemContactHandlers)
					itemContactHandler.OnStartContactWithItem(this);
			}
		}

		protected virtual void OnCollisionExit2D(Collision2D collision) {
			var itemContactHandlers = collision.gameObject.GetComponents<IItemContactHandler>();
			if (itemContactHandlers.Length > 0) {
				foreach (var itemContactHandler in itemContactHandlers)
					itemContactHandler.OnEndContactWithItem(this);
			}
		}

		protected virtual void TryPlaySound() {
			if (p_sfx != null) {
				p_sfx.PlayEffect();
			} else {
				Debug.LogWarning($"{gameObject.name} : Sfx component is not exist");
			}
		}

		public virtual void Destroy() {
			DidDestroyed?.Invoke();
		}

		public virtual void OnLevelPause() {
			LeanTween.pause(gameObject);
		}

		public virtual void OnLevelResume() {
			LeanTween.resume(gameObject);
		}
	}
}