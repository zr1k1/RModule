using System;
using System.Collections;
using RModule.Runtime.Arcade;
using RModule.Runtime.LeanTween;
using UnityEngine;
using UnityEngine.Events;

public class TeleportBlock : BaseBlock {
	//Events
	public UnityEvent<TeleportBlock, GameObject> DidStartTeleportIn = default;
	public UnityEvent<TeleportBlock, GameObject> DidStartTeleportOut = default;

	// Delegates
	public delegate IEnumerator Animation(GameObject teleportedGo);

	public Animation AnimationIn;
	public Animation AnimationOut;

	// Accessors
	public TeleportBlock DestinationTeleport => _destinationTeleport;

	// Outlets
	[SerializeField] protected TeleportBlock _destinationTeleport = default;

	// Privats

	protected override void Start() {
		p_contactDetector.Setup(this);
		AnimationIn = DefaultAnimationTeleportIn;
		AnimationOut = DefaultAnimationTeleportOut;
	}

	public void Teleport(GameObject go) {
		StartCoroutine(TryTeleport(go));
	}

	IEnumerator TryTeleport(GameObject go) {
		if (_destinationTeleport != null) {
			var iTeleportable = go.GetComponent<ITeleportable>();
			if (iTeleportable != null && iTeleportable.CanTeleport()) {
				iTeleportable.OnStartTeleport(this);
				StartCoroutine(Teleport(go, iTeleportable.OnEndTeleport));
			}
		}
		yield return null;

	}

	protected virtual IEnumerator Teleport(GameObject go, Action finishCallback) {
		DidStartTeleportIn?.Invoke(this, go);
		yield return AnimationIn(go);
		go.transform.position = _destinationTeleport.transform.position;
		DidStartTeleportOut?.Invoke(this, go);
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
