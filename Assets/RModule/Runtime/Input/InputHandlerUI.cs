using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputHandlerUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IInputHandler {

    public Canvas Canvas => _canvas;
    public Camera UICamera => _camera;

    // Events
    public event Action DidClick = default;
    public event Action DidBeginDrag = default;
    public event Action DidOnDrag = default;
    public event Action DidEndDrag = default;
    public event Action DidDown = default;
    public event Action DidUp = default;

    // Outlets
    [SerializeField] Canvas _canvas = default;
    [SerializeField] Camera _camera = default;

	// Private vars

	public void OnPointerClick(PointerEventData eventData) {
        DidClick?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        DidBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData) {
        DidOnDrag?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData) {
        DidEndDrag?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData) {
        DidDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData) {
        DidUp?.Invoke();
    }
}
