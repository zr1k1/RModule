using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dummyGradeConfig", menuName = "GradesSystem/Configs/dummyGradeConfig", order = 1)]
public class GradeConfig : ScriptableObject {
	// Accessors
	public float Price => _price;

	// Outlets
	[SerializeField] protected float _price = default;
}
