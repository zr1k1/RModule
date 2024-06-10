namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class Box : PickedAtHandItem, IPickable, IDoingDamageToObject<SpinningBladeBlock> {

		public override void Destroy() {
			base.Destroy();

			enabled = false;
			p_collider2D.enabled = false;
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			Destroy(gameObject, 5);
		}
	}

}
