using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlacementData {
    public string Key = default;
    public bool IsRewarded = default;
    public string StoreProductId = default;
}

[CreateAssetMenu(fileName = "PlacementValueConfig", menuName = "RModule/Values/PlacementValueConfig", order = 1)]
public class PlacementValueConfig : ValueConfig<PlacementData> {
}
