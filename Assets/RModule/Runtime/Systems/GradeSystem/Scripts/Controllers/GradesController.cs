using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GradeEventArgs {
	public GradeInstaller GradeInstaller;
}

public interface IGradeable {
	public bool TryInstallNext();
	public bool TryInstallGradeByIndex(int gradeIndex);
}

public interface IGradeInstallersContainer {
	public bool TryGetCurrentGradeInstaller(out GradeInstaller currentGradeInstaller);
	public bool TryGetNextGradeInstaller(out GradeInstaller nextGradeInstaller);
}

public class GradesController : MonoBehaviour, IGradeable, IGradeInstallersContainer {
	// Events
	public UnityEvent<GradesController> DidUpgraded = default;
	public UnityEvent<GradesController, GradeEventArgs> GradeButtonDidTapped = default;

	// Outlets
	[SerializeField] List<GradeInstaller> _gradeInstallers = default;

	int _currentInstalledGradeIndex = 0;

	private void Start() {
		foreach(var gradeInstaller in _gradeInstallers) {
			gradeInstaller.Setup(OnGradeButtonTapped);
		}
	}

	protected virtual void OnEnable() {
		GradesManager.AddGradesController(this);
	}

	protected virtual void OnDisable() {
		GradesManager.RemoveGradesController(this);
	}

	void OnGradeButtonTapped(GradeInstaller gradeInstaller) {
		var args = new GradeEventArgs() {
			GradeInstaller = gradeInstaller
		};
		GradeButtonDidTapped?.Invoke(this, args);
	}

	public bool TryInstallGrade(GradeInstaller gradeInstaller) {
		if (_gradeInstallers.Contains(gradeInstaller)) {
			return TryInstallGradeByIndex(_gradeInstallers.IndexOf(gradeInstaller));
		}

		return false;
	}

	public bool TryInstallNext() {
		return TryInstallGradeByIndex(_currentInstalledGradeIndex + 1);
	}

	public bool TryInstallGradeByIndex(int gradeIndex) {
		if (gradeIndex < 0) {
			Debug.LogError($"GradesController : Grade index can be only positive!");
			return false;
		} else if (gradeIndex >= _gradeInstallers.Count) {
			Debug.LogError($"GradesController : Grade with index {_currentInstalledGradeIndex} is not exist!");
			return false;
		} else if(gradeIndex == _currentInstalledGradeIndex) {
			Debug.LogError($"GradesController : gradeIndex == _currentInstalledGradeIndex!");
			return false;
		}

		for(int i = _currentInstalledGradeIndex; i < gradeIndex; i++) {
			_gradeInstallers[i].DeInstall();
			if(i + 1 <= gradeIndex) {
				_gradeInstallers[i+1].Install();
			}
		}

		_currentInstalledGradeIndex = gradeIndex;				
		DidUpgraded?.Invoke(this);

		return true;
	}

	public bool TryGetCurrentGradeInstaller(out GradeInstaller currentGradeInstaller) {
		currentGradeInstaller = default(GradeInstaller);
		if (_currentInstalledGradeIndex >= 0 && _currentInstalledGradeIndex < _gradeInstallers.Count) {
			currentGradeInstaller = _gradeInstallers[_currentInstalledGradeIndex];
			return true;
		}

		return false;
	}

	public bool TryGetNextGradeInstaller(out GradeInstaller nextGradeInstaller) {
		Debug.LogError($"GradeButton : TryGetNextGradeInstaller");
		nextGradeInstaller = default(GradeInstaller);
		int nextGradeIndex = _currentInstalledGradeIndex + 1;
		if (nextGradeIndex < _gradeInstallers.Count) {
			nextGradeInstaller = _gradeInstallers[nextGradeIndex];
			return true;
		}

		return false;
	}
}
