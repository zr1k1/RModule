using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseValueVC : MonoBehaviour {
	// Outlets
	[SerializeField] TextMeshProUGUI _countLabel = default;

	int _value;

	public void SetValue(int value) {
		_value += value;
		_countLabel.text = $"{_value}";
	}
}
