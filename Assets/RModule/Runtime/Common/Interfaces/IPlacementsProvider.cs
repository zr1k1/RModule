using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPlacementsProvider<PlacementEnum> where PlacementEnum : Enum {
    public string GetPlacement(PlacementEnum placementType);
}
