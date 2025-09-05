using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectionController<T> where T : MonoBehaviour {
	public static GameObject FirstSelected => s_firstSelected;

	static GameObject s_firstSelected = default;
	static SelectionComponent s_firstSelectedSelectionComponent = default;
	static List<SelectionComponent> s_selectionComponents = new List<SelectionComponent>();
	static List<SelectionComponent> s_canInteractWithFirstSelectedSelectionComponents = new List<SelectionComponent>();
	static bool s_selectionIsEnabled = true;

	public static void AddSelectionComponent(SelectionComponent<T> selectionComponent) {
		if (!s_selectionComponents.Contains(selectionComponent))
			s_selectionComponents.Add(selectionComponent);
	}

	public static void RemoveSelectionComponent(SelectionComponent<T> selectionComponent) {
		if (s_selectionComponents.Contains(selectionComponent))
			s_selectionComponents.Remove(selectionComponent);
	}

	public static void Reset() {
		Debug.Log($"SelectionController : Reset");
		if (s_firstSelected != null) {
			s_firstSelectedSelectionComponent.Deselect();
			s_firstSelectedSelectionComponent = null;
		}
		s_firstSelected = null;

		foreach (var selectionComponent in s_canInteractWithFirstSelectedSelectionComponents) {
			selectionComponent.ShowSelectionView(false);
		}
		s_canInteractWithFirstSelectedSelectionComponents.Clear();
	}

	public static void TryResetSelection(SelectionComponent<T> selectionComponent) {
		if (selectionComponent == s_firstSelectedSelectionComponent) {
			Reset();
		}
	}

	public static void Select(SelectionComponent<T> selectionComponent) {
		if (!s_selectionIsEnabled)
			return;

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

	public static void SetEnableSelection(bool enableSelection) {
		s_selectionIsEnabled = enableSelection;
	}

	static void SetSelectionFirst(SelectionComponent<T> selectionComponent) {
		//Debug.Log($"SelectionController : SetSelectionFirst {selectionComponent.GameObject.name}");
		if (selectionComponent.DisableSelectionFirst)
			return;

		var levelElement = selectionComponent.GameObject;
		s_firstSelectedSelectionComponent = selectionComponent;
		selectionComponent.Select();
		s_firstSelected = levelElement;

		SelectInteractableSelectionComponentsWithFirstSelected();
	}

	public static void TryUpdateSelections() {
		if (s_firstSelected == null)
			return;

		foreach (var selectionComponent in s_canInteractWithFirstSelectedSelectionComponents) {
			selectionComponent.ShowSelectionView(false);
		}
		s_canInteractWithFirstSelectedSelectionComponents.Clear();
		SelectInteractableSelectionComponentsWithFirstSelected();
	}

	static void SelectInteractableSelectionComponentsWithFirstSelected() {
		foreach(var selectionComponent in s_selectionComponents) {
			var interactingWithOthersGameElements = selectionComponent.GameObject?.GetComponent<IInteractingWithOthersGameElements<T>>();
			if (interactingWithOthersGameElements != null) {
				if (interactingWithOthersGameElements.CanInteract(s_firstSelected.GetComponent<T>())) {
					selectionComponent.ShowSelectionView(true);
					s_canInteractWithFirstSelectedSelectionComponents.Add(selectionComponent);
				}
			}
		}
	}
}
