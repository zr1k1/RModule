using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RModule.Runtime.GraphicKitsSystem {

	[CreateAssetMenu(fileName = "GraphicKitsConfig", menuName = "AppConfigs/GraphicKitsConfigs/LocationsGraphicKitsConfig", order = 1)]
	public class LocationsGraphicKitsConfig : GraphicKitsConfig {

		// Outlets
		[Tooltip("Will repeat after end")]
		[SerializeField] protected List<int> _maxLevelNumberForApplyGraphicKitWithSameIndex = default;

		public T GetSpriteAddress<T>(string graphicKitConfigNameKey, string parameterKey) {
			GetGraphicKitConfigByNameKeyOrDefault(graphicKitConfigNameKey).TryGetParameter(parameterKey, out T parameter);
			return parameter;
		}

		public GraphicKitConfig GetGraphicKitByLevelNumber(int levelNumber) {
			var lastMaxLevelToApply = _maxLevelNumberForApplyGraphicKitWithSameIndex[_maxLevelNumberForApplyGraphicKitWithSameIndex.Count - 1];
			int modifiedLevelNumber = ((levelNumber - 1) % lastMaxLevelToApply) + 1;
			modifiedLevelNumber = Mathf.Clamp(modifiedLevelNumber, 1, lastMaxLevelToApply);
			int index = _maxLevelNumberForApplyGraphicKitWithSameIndex.IndexOf(_maxLevelNumberForApplyGraphicKitWithSameIndex.Find(maxLevelNumberToApply => modifiedLevelNumber <= maxLevelNumberToApply));
			if (index < p_graphicKitConfigs.Count) {
				return p_graphicKitConfigs[index];
			} else {
				Debug.LogError($"Check _maxlevelNumberForApplyGraphicKitWithSameIndex list elements count! Must be same as in the list p_graphicKitConfigs");
				return p_graphicKitConfigs[p_graphicKitConfigs.Count - 1];
			}
		}
	}
}