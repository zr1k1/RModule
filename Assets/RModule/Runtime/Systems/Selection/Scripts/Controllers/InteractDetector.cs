using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDetector<T, TInteractingElementsBaseClass> : IInteractingWithOthersGameElements<TInteractingElementsBaseClass>
	where T : TInteractingElementsBaseClass
	where TInteractingElementsBaseClass : MonoBehaviour {

	protected T _obj;
	//protected bool _interactInProgress;// TODO remake to optional
	ISelectInteractable<T> _detectedObject;

	public InteractDetector(T obj) {
		_obj = obj;
	}

	public virtual bool InteractionInProgress() {
		//return _interactInProgress;
		return false;
	}

	public virtual void SetInteractionInProgress(bool inProgress) {
		//_interactInProgress = inProgress;
	}

	public virtual bool TryInteract(TInteractingElementsBaseClass other) {
		Debug.Log($"InteractDetector : {other}");
		//if (other == _obj
		//	//|| _interactInProgress
		//	)
		//	return false;

		if (CanInteract(other)) {
			Debug.Log($"InteractDetector : CanInteract true {other}");
			if (_detectedObject.TryInteract(_obj)) {
				Debug.Log($"InteractDetector : TryInteract true {other}");
				return true;
			}
		}

		return false;
	}

	public virtual bool TryDetectObject(TInteractingElementsBaseClass other) {
		_detectedObject = other.GetComponent<ISelectInteractable<T>>();
		if (_detectedObject != null ) {
			return true;
		}

		return false;
	}

	public bool CanInteract(TInteractingElementsBaseClass other) {
		return other != _obj && TryDetectObject(other) && _detectedObject.CanInteract(_obj);
			//|| _interactInProgress;
	}
}