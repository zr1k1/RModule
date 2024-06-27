namespace RModule.Runtime.Arcade {

	using UnityEngine;
	using UnityEngine.Events;
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
		[SerializeField] protected ItemAnimationComponent p_itemAnimationComponent = default;

		// Privats
		protected ContactDetector p_contactDetector;
		protected Collider2D p_collider2D;
		protected Rigidbody2D p_rigidbody2D;
		protected Vector2 p_startPosition;

		protected override void Awake() {
			base.Awake();
			if (p_spriteRenderer == null)
				p_spriteRenderer = GetComponent<SpriteRenderer>();
			p_collider2D = GetComponent<Collider2D>();
			p_rigidbody2D = GetComponent<Rigidbody2D>();
			p_startPosition = transform.position;
			p_contactDetector = gameObject.AddComponent<ContactDetector>();
		}

		protected virtual void Start() {
			Debug.LogError($"{transform.name} : Override Start method and use p_contactDetector.Setup(this);");
		}

		public virtual void Destroy() {
			Destroy(gameObject);
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