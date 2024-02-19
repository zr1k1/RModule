namespace RModule.Runtime.Arcade {
	using UnityEngine;
	using RModule.Runtime.Arcade.Inventory;

	public class UnPickableItem : Item, IUseable {
		public virtual void Use(GameObject userGo) {
		}
	}

}