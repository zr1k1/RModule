using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectInteractable<T> where T : MonoBehaviour {
	public bool TryInteract(T otherLevelElement);
	public bool CanInteract(T otherLevelElement);
}
