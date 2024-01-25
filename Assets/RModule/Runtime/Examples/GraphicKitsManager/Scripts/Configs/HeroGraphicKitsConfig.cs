using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RModule.Runtime.GraphicKitsSystem {

	[CreateAssetMenu(fileName = "ExampleHeroGraphicKitsConfig", menuName = "RModule/Examples/AppConfigs/GraphicKitsConfigs/ExampleHeroGraphicKitsConfig", order = 1)]
	public class HeroGraphicKitsConfig : GraphicKitsConfig {

		[SerializeField] string _previewSpriteAddress = default;

		public string TryGetSkinPreviewAddress(string graphicKitConfigNameKey) {
			var graphicKit = p_graphicKitConfigs.Find(graphicKitConfig => graphicKitConfig.Key == graphicKitConfigNameKey);
			if (graphicKit != null) {
				graphicKit.TryGetValue(_previewSpriteAddress, out string address);

				return address;
			}

			return "";
		}
	}
}
