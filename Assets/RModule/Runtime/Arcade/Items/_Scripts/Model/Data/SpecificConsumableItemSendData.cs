namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using System;

	[Serializable]
	public class SpecificConsumableItemSendData : ConsumableItemSendData {
		public Sprite actionIconSprite = default;
		public Action action = default;
	}
}