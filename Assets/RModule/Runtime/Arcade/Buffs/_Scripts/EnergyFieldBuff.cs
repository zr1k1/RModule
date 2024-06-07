namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using RModule.Runtime.DealDamageSystem;

	public class EnergyFieldBuff : Buff, IDamageAbsorber {
		// Outlets
		[SerializeField] protected int _value = default;

		public void AbsorbDamage(DamageData damageData) {
			if (damageData.damageConfig is ICannotBeAbsorbDamage)
				return;			

			var tempValue = Mathf.Clamp(_value - damageData.GetDamage(), 0, _value);
			damageData.ModifyDamage(-_value);
			_value = tempValue;

			if (_value == 0) {
				DidEnd?.Invoke(this);
			}
		}
	}
}
