using UnityEngine;

namespace RModule.Runtime.GraphicKitsSystem {

	[CreateAssetMenu(fileName = "GraphicKitConfig", menuName = "Helpers/UniversalDataConfigs/GraphicKitConfig", order = 1)]
	public class GraphicKitConfig : BaseUniversalDataConfig {
		public string GetSkinPreviewAddress() {
			TryGetParameter("previewSpriteAddress", out string address);

			return address;
		}
	}
}
