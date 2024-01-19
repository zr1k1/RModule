using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GraphicKitsConfig", menuName = "AppConfigs/GraphicKitsConfigs/HeroGraphicKitsConfig", order = 1)]
public class HeroGraphicKitsConfig : GraphicKitsConfig {

	public string TryGetSkinPreviewAddress(string graphicKitConfigNameKey) {
		var graphicKit = p_graphicKitConfigs.Find(graphicKitConfig => graphicKitConfig.Key == graphicKitConfigNameKey);
		if (graphicKit != null) {
			return graphicKit.GetSkinPreviewAddress();
		}

		return "";
	}
}
