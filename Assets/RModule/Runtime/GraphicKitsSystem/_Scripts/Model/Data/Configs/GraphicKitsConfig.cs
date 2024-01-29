using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.Utils;

namespace RModule.Runtime.GraphicKitsSystem {
	[CreateAssetMenu(fileName = "GraphicKitsConfig", menuName = "RModule/AppConfigs/GraphicKitsConfigs/GraphicKitsConfig", order = 1)]
	public class GraphicKitsConfig : ScriptableObject {
		// Accessors
		public List<GraphicKitConfig> GraphicKitConfigs => p_graphicKitConfigs;

		// Outlets
		[SerializeField] protected List<GraphicKitConfig> p_graphicKitConfigs = default;

		public GraphicKitConfig GetGraphicKitConfigByNameKeyOrDefault(string graphicKitConfigNameKey) {
			var graphicKit = p_graphicKitConfigs.Find(graphicKitConfig => graphicKitConfig.Key == graphicKitConfigNameKey);
			return graphicKit != null ? graphicKit : p_graphicKitConfigs[0];
		}

		public GraphicKitConfig GetNextGraphicKitConfigByNameKeyOrDefault(string graphicKitConfigNameKey) {
			var graphicKit = p_graphicKitConfigs.Find(graphicKitConfig => graphicKitConfig.Key == graphicKitConfigNameKey);
			graphicKit = graphicKit != null ? graphicKit : p_graphicKitConfigs[0];
			int index = p_graphicKitConfigs.IndexOf(graphicKit);
			var nextIndex = Utils.Utils.GetLoopNextNumber(index, 0, p_graphicKitConfigs.Count);

			return p_graphicKitConfigs[nextIndex];
		}

	}
}
