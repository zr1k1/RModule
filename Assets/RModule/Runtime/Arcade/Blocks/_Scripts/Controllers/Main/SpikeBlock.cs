namespace RModule.Runtime.Arcade {

	public class SpikeBlock : BaseBlock, IDangerousRadarObject, IHeroDamager {
		protected DamageDealerComponent _damageDealerComponent;

		protected override void Start() {
			_damageDealerComponent = GetComponent<DamageDealerComponent>();
			p_contactDetector.Setup(this);
		}
	}

}
