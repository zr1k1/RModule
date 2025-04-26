using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataConfig", menuName = "RModule/Systems/UniversalGetUnitByCostValueSystem/Levels/Main/LevelDataConfig", order = 1)]
public class LevelDataConfig : BaseUnitByCostDataConfig<float> {
	public int Number => _number;

	// Outlets
	[Header("Level")]
	[SerializeField] protected int _number = default;
	[Tooltip("Used only if level cost is have dependecies with other level data.Can be null. ")]
	[SerializeField] protected LevelDataConfig _otherLevelDataConfig = default;
	[SerializeField] protected bool _useOtherLevelDataForCalculateCost = default;

	public override bool TryGet(float valueToGet, out float remainder) {
		remainder = valueToGet;
		var cost = GetCost();
		if (valueToGet >= cost) {
			remainder = cost - valueToGet;

			return true;
		}

		return false;
	}
}
