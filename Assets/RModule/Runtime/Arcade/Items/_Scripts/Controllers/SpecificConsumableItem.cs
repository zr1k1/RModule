namespace RModule.Runtime.Arcade {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SpecificConsumableItem : ConsumableItem {
        public Sprite ActionIconSprite => _actionIconSprite;

        [SerializeField] Sprite _actionIconSprite = default;

        protected override void Start() {
            p_contactDetector.Setup(this);
        }

    }
}
