using System;
using System.Collections;
using RModule.Runtime.Arcade;
using RModule.Runtime.LeanTween;
using UnityEngine;

public interface ITeleport {
}

public class TeleportBlock : BaseBlock, ITeleport {

	// Delegates
	public delegate IEnumerator Animation(GameObject teleportedGo);

	public Animation AnimationIn;
	public Animation AnimationOut;

	// Accessors
	public TeleportBlock DestinationTeleport => _destinationTeleport;

	// Outlets
	[SerializeField] protected TeleportBlock _destinationTeleport = default;

	// Privats

	private void Start() {
		AnimationIn = DefaultAnimationTeleportIn;
		AnimationOut = DefaultAnimationTeleportOut;
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision) {
		StartCoroutine(TryTeleport(collision.gameObject));
	}

	protected virtual void OnCollisionEnter2D(Collision2D collision) {
		StartCoroutine(TryTeleport(collision.gameObject));
	}

	IEnumerator TryTeleport(GameObject go) {
		Debug.Log($"TryTeleport {go.name}");
		if (_destinationTeleport != null) {
			var iTeleportable = go.GetComponent<ITeleportable>();
			if (iTeleportable != null && iTeleportable.CanTeleport()) {
				yield return StartCoroutine(iTeleportable.OnStartTeleport(this));
				StartCoroutine(Teleport(go, iTeleportable.OnEndTeleport));
			}
		}
		yield return null;

	}

	protected virtual IEnumerator Teleport(GameObject go, Action finishCallback) {
		yield return AnimationIn(go);
		go.transform.position = _destinationTeleport.transform.position;
		yield return _destinationTeleport.AnimationOut(go);
		finishCallback?.Invoke();
	}

	public virtual IEnumerator DefaultAnimationTeleportIn(GameObject go) {
		LeanTween.scale(go, Vector3.zero, 0.5f);
		LeanTween.move(go, transform, 0.5f);
		yield return new WaitForSeconds(0.5f);
	}

	public virtual IEnumerator DefaultAnimationTeleportOut(GameObject go) {
		LeanTween.scale(go, Vector3.one, 0.5f);
		yield return new WaitForSeconds(0.5f);
	}
}
