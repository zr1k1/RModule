namespace RModule.Runtime.Arcade {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class StopMoveItem : SpecificConsumableItem {

        protected override void Start() {
            p_contactDetector.Setup(this);
        }
    }
}
