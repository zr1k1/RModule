namespace RModule.Runtime.Arcade {

	public class SpikeBlock : BaseBlock, IDangerousRadarObject, IHeroDamager {
		protected override void Start() {
			p_contactDetector.Setup(this);
		}
	}

}
