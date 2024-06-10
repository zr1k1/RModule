namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using RModule.Runtime.DealDamageSystem;

	public class EnergyFieldBuff : Buff, IDamageAbsorber, IDoingDamageToObject<SpinningBladeBlock> {
		// Outlets
		[SerializeField] protected HealthComponent _healthComponent = default;

		public override Buff Setup(float timeInSeconds) {
			_healthComponent.HealthDidLessThanZeroOrZero.AddListener(Die);

			return base.Setup(timeInSeconds);
		}

		public void AbsorbDamage(DamageData damageData) {
			Debug.Log("EnergyFieldBuff : AbsorbDamage");
			if (damageData.damageConfig is ICannotBeAbsorbDamage)
				return;			

			var tempValue = Mathf.Clamp(_healthComponent.Value - damageData.GetDamage(), 0, _healthComponent.Value);
			damageData.ModifyDamage((int)-_healthComponent.Value);
			_healthComponent.SetValue(tempValue);
		}

		protected virtual void Die() {
			Debug.Log("EnergyFieldBuff : Die");
			DidEnd?.Invoke(this);			
		}
	}
}
