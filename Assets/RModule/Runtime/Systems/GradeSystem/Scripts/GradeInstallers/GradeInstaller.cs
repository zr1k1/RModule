using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GradeInstaller : MonoBehaviour{
	// Accessors
	public GradeConfig Config => _config;

	// Outlets
	[SerializeField] protected GradeButton _gradeButton = default;
	[SerializeField] protected List<GameObject> _installActivatedGOs = default;
	[SerializeField] protected List<GameObject> _deinstallActivatedGOs = default;
	[SerializeField] protected GradeConfig _config = default;

	public void Setup(Action<GradeInstaller> gradeButtonTappedCallback) {
		_gradeButton?.DidTapped.AddListener(()=> { gradeButtonTappedCallback.Invoke(this); });
	}

	public virtual void Install() {
		foreach (var go in _installActivatedGOs)
			go.SetActive(true);
	}

	public virtual void DeInstall() {
		foreach (var go in _deinstallActivatedGOs)
			go.SetActive(false);
	}
}
