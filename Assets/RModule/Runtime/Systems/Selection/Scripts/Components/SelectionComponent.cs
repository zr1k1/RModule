using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionComponent : MonoBehaviour, ISelectable {
	public bool IsSelected => _isSelected;
	public GameObject GameObject => _gameObject;

	[Tooltip("Setup game object with realized IInteractingWithOthersGameElements!")]
	[SerializeField] protected GameObject _gameObject = default;//todo change to t?
	[SerializeField] protected GameObject _selectionView = default;
	[SerializeField] protected bool _disableSelectionFirst = default;

	protected bool _isSelected;
	protected bool _selectionIsEnabled;

	public virtual void Deselect() {
		throw new System.NotImplementedException();
	}

	public virtual void Select() {
		throw new System.NotImplementedException();
	}

	public virtual void SetEnableSelection(bool enable) {
		_selectionIsEnabled = enable;
	}

	public virtual void ShowSelectionView(bool showHide) {
		if (_selectionView != null)
			_selectionView.SetActive(showHide);
	}

	public bool SelectionIsEnabled() {
		return _selectionIsEnabled;
	}
}

public class SelectionComponent<T> : SelectionComponent where T : MonoBehaviour {

	// Events
	public UnityEvent DidSelect = default;
	public UnityEvent DidDeselect = default;

	// Accessors
	//public GameObject GameObject => _gameObject;
	//public bool IsSelected => _isSelected;
	public bool DisableSelectionFirst => _disableSelectionFirst;

	//[Tooltip("Setup game object with realized IInteractingWithOthersGameElements!")]
	//[SerializeField] protected GameObject _gameObject = default;//todo change to t?
	////[SerializeField] protected GameObject _selectionView = default;
	//[SerializeField] protected bool _disableSelectionFirst = default;

	private void OnEnable() {
		SelectionController<T>.AddSelectionComponent(this);
	}

	private void OnDisable() {
		SelectionController<T>.RemoveSelectionComponent(this);
	}

	protected virtual void OnMouseUpAsButton() {
		TrySelect();
	}

	protected virtual bool TrySelect() {
		if (_gameObject == null) {
			Debug.LogError($"Setup game object with realized IInteractingWithOthersGameElements to _gameObject field in inspector!");
			return false;
		}

		if (!SelectCondition())
			return false;

		SelectionController<T>.Select(this);

		return true;
	}

	protected virtual bool SelectCondition() {
		return !_isSelected;
	}

	public override void SetEnableSelection(bool enable) {
		enabled = enable;
		GetComponent<Collider>().enabled = enable;
	}

	public override void Select() {
		if (!enabled || _isSelected)
			return;
		_isSelected = true;
		OnChangeSelectionState(true);
		DidSelect?.Invoke();
	}

	public override void ShowSelectionView(bool showHide) {
		if (_selectionView != null)
			_selectionView.SetActive(showHide);
	}

	public override void Deselect() {
		if (!_isSelected)
			return;
		_isSelected = false;
		OnChangeSelectionState(false);
		DidDeselect?.Invoke();
	}

	protected virtual void OnChangeSelectionState(bool enableSelection) {
		ShowSelectionView(enableSelection);
	}

	public virtual void Reset() {
		SelectionController<T>.TryResetSelection(this);
	}
}
