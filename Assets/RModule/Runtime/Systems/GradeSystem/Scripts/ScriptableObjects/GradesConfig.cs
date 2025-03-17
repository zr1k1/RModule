using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GradesConfig", menuName = "GradesSystem/Configs/GradesConfig", order = 1)]
public class GradesConfig : ScriptableObject {
	// Accessors
	public List<GradeConfig> GradesConfigs => _gradesConfigs;

	// Outlets
	[SerializeField] protected List<GradeConfig> _gradesConfigs = default;

}
