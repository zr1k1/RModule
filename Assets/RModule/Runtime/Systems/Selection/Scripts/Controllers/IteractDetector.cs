using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDetector<T, TInteractingElementsBaseClass> : IInteractingWithOthersGameElements<TInteractingElementsBaseClass>
	where T : TInteractingElementsBaseClass
	where TInteractingElementsBaseClass : MonoBehaviour {

	protected T _obj;
	protected bool _interactInProgress;

	public InteractDetector(T obj) {
		_obj = obj;
	}

	public virtual bool InteractionInProgress() {
		return _interactInProgress;
	}

	public virtual void SetInteractionInProgress(bool inProgress) {
		_interactInProgress = inProgress;
	}

	public virtual bool TryDetectObject(TInteractingElementsBaseClass other) {
		var detectedObject = other.GetComponent<ISelectInteractable<T>>();
		if (detectedObject != null) {
			if (detectedObject.TryInteract(_obj)) {
				return true;
			}
		}

		return false;
	}

	public virtual bool TryInteract(TInteractingElementsBaseClass other) {
		Debug.Log($"InteractDetector : {other} _interactInProgress {_interactInProgress}");
		if (_interactInProgress || other == _obj)
			return false;

		if (TryDetectObject(other))
			return true;

		return false;
	}
}