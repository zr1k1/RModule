using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandlerUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IInputHandler {

    public Canvas Canvas => _canvas;
    public Camera UICamera => _camera;
    public static Vector2 PointerPosition { get; private set; }
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


    public void OnPointerClick(PointerEventData eventData) {
        PointerPosition = eventData.position;
        DidClick?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        PointerPosition = eventData.position;
        DidBeginDrag?.Invoke();
    }

    public void OnDrag(PointerEventData eventData) {
        PointerPosition = eventData.position;
        DidOnDrag?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData) {
        PointerPosition = eventData.position;
        DidEndDrag?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData) {
        PointerPosition = eventData.position;
        DidDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData) {
        PointerPosition = eventData.position;
        DidUp?.Invoke();
    }
}
