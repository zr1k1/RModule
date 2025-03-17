using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GradeButton : MonoBehaviour {
	// Events
	public UnityEvent DidTapped = default;

	public virtual void OnMouseUpAsButton() {
		DidTapped?.Invoke();
	}
}
