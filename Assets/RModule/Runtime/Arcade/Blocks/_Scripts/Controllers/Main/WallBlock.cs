namespace RModule.Runtime.Arcade {

	public class WallBlock : BaseBlock {

		protected override void Start() {
			p_contactDetector.Setup(this);
		}
	}
}
