namespace RModule.Runtime.Arcade {
	public interface IDamageAbsorber : IBuffEffect {
		void AbsorbDamage(DamageData damageData);
	}

	public interface IBuffEffect {
	}
}
