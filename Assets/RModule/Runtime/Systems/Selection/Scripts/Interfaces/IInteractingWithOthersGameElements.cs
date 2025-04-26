using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractingWithOthersGameElements<T> where T : MonoBehaviour {
	public bool TryInteract(T other);
	public bool CanInteract(T other);
	public bool InteractionInProgress();
	public void SetInteractionInProgress(bool inProgress);
}
