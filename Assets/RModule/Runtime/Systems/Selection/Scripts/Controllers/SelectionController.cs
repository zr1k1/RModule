using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectionController<T> where T : MonoBehaviour {
	public static GameObject FirstSelected => s_firstSelected;

	static GameObject s_firstSelected = default;
	static SelectionComponent<T> s_firstSelectedSelectionComponent = default;

	public static void Reset() {
		Debug.Log($"SelectionController : Reset");
		if (s_firstSelected != null) {
			s_firstSelectedSelectionComponent.Deselect();
			s_firstSelectedSelectionComponent = null;
		}
		s_firstSelected = null;
	}

	public static void TryResetSelection(SelectionComponent<T> selectionComponent) {
		if (selectionComponent == s_firstSelectedSelectionComponent) {
			Reset();
		}
	}

	public static void Select(SelectionComponent<T> selectionComponent) {
		var levelElement = selectionComponent.GameObject;
		if (s_firstSelected == null) {
			if (levelElement.GetComponent<IInteractingWithOthersGameElements<T>>() != null) {
				SetSelectionFirst(selectionComponent);
			}
		} else {
			Debug.Log($"SelectionController : T {typeof(T).ToString()}");
			var interactingWithOthersGameElements = levelElement.GetComponent<IInteractingWithOthersGameElements<T>>();
			if (interactingWithOthersGameElements != null) {
				if (!interactingWithOthersGameElements.TryInteract(s_firstSelected.GetComponent<T>())) {
					Debug.Log($"SelectionController : TryInteract false");
					Reset();
					SetSelectionFirst(selectionComponent);
					return;
				}
			}
			Reset();
		}
	}

	static void SetSelectionFirst(SelectionComponent<T> selectionComponent) {
		Debug.Log($"SelectionController : SetSelectionFirst {selectionComponent.GameObject.name}");
		var levelElement = selectionComponent.GameObject;
		s_firstSelectedSelectionComponent = selectionComponent;
		selectionComponent.Select();
		s_firstSelected = levelElement;
	}
}
