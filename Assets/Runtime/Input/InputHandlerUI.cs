using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputHandlerUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler {

    // Events
    public UnityEvent DidClick = default;
    public UnityEvent DidBeginDrag = default;
    public UnityEvent DidOnDrag = default;
    public UnityEvent DidEndDrag = default;
    public UnityEvent DidDown = default;
    public UnityEvent DidUp = default;

    // Private vars

    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log($"HandController : OnPointerClick");
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
