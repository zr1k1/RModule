namespace RModule.Runtime.Arcade {

	public class WallBlock : BaseBlock, IStartContactDetector<CannonBlockFireUnit> {

		protected override void Start() {
			p_contactDetector.Setup(this);
		}

		public virtual void OnStartContact(CannonBlockFireUnit contactedObject) {
		}
	}
}
