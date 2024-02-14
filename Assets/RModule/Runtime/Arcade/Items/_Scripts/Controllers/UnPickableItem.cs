using UnityEngine;

namespace RModule.Runtime.Arcade {

	public class UnPickableItem : Item, IUseable {
		public virtual void Use(GameObject userGo) {
		}
	}

}