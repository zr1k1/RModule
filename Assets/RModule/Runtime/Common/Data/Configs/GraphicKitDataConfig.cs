using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GraphicKitDataConfig", menuName = "Helpers/UniversalDataConfigs/GraphicKitDataConfig", order = 1)]
public class GraphicKitDataConfig : BaseUniversalDataConfig {
	public string GetSkinPreviewAddress() {
		TryGetParameter("previewSpriteAddress", out string address);

		return address;
	}
}
