using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChargesCounter : MonoBehaviour {
	// Events
	public UnityEvent AllChargesDidUsed = default;

	// Accessors
	public int ChargesCount => _chargesCount;

	// Outlets
	[SerializeField] int _chargesCount = default;

	public bool TryUseCharge() {
		if(_chargesCount > 0) {
			_chargesCount--;

			if(_chargesCount == 0)
				AllChargesDidUsed?.Invoke();

			return true;
		}

		return false;
	}
}
