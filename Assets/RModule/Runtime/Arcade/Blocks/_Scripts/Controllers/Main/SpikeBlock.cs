namespace RModule.Runtime.Arcade {

	public class SpikeBlock : BaseBlock, IDangerousRadarObject {
		protected DamageDealerComponent _damageDealerComponent;

		protected override void Start() {
			_damageDealerComponent = GetComponent<DamageDealerComponent>();
			p_contactDetector.Setup(this);
		}
	}

}
