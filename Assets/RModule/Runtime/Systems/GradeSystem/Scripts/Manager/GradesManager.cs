using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradesManager : SingletonMonoBehaviour<GradesManager> {
	//TODO grades popupsController

	// Privats
	List<GradesController> _gradeButtons = new List<GradesController>();

	public override bool IsInitialized() {
		return Instance.gameObject.activeSelf;
	}

	public static void AddGradesController(GradesController gradesController) {
		if (!Instance._gradeButtons.Contains(gradesController)) {
			Instance._gradeButtons.Add(gradesController);
			gradesController.GradeButtonDidTapped.AddListener(OnTryGradeSomeObject);
		}
	}

	public static void RemoveGradesController(GradesController gradesController) {
		if (Instance._gradeButtons.Contains(gradesController)) {
			Instance._gradeButtons.Remove(gradesController);
			gradesController.GradeButtonDidTapped.RemoveListener(OnTryGradeSomeObject);
		}
	}

	static void OnTryGradeSomeObject(GradesController sender, GradeEventArgs gradeEventArgs) {
		Debug.Log($"GradesManager : ShowGradePopup");
		//TODO make purchase logic with popups or something, now is free
		sender.TryInstallGrade(gradeEventArgs.GradeInstaller);
	}
}
