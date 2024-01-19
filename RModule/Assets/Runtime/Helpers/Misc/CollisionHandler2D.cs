using UnityEngine;
using UnityEngine.Events;

public class CollisionHandler2D : MonoBehaviour {
	public UnityEvent<Collision2D> CollisionDidEnter2D = default;
	public UnityEvent<Collision2D> CollisionDidStay2D = default;
	public UnityEvent<Collision2D> CollisionDidExit2D = default;
	public UnityEvent<Collider2D> TriggerDidEnter2D = default;
	public UnityEvent<Collider2D> TriggerDidStay2D = default;
	public UnityEvent<Collider2D> TriggerDidExit2D = default;

	private void OnCollisionEnter2D(Collision2D collision) {
		CollisionDidEnter2D?.Invoke(collision);
	}

	private void OnCollisionStay2D(Collision2D collision) {
		CollisionDidStay2D?.Invoke(collision);
	}

	private void OnCollisionExit2D(Collision2D collision) {
		CollisionDidExit2D?.Invoke(collision);
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		TriggerDidEnter2D?.Invoke(collider);
	}

	private void OnTriggerStay2D(Collider2D collider) {
		TriggerDidStay2D?.Invoke(collider);
	}

	private void OnTriggerExit2D(Collider2D collider) {
		TriggerDidExit2D?.Invoke(collider);
	}
}
