namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class Coin : CollectedItem {
		//Accessors
		public int Value => _value;

		// Outlets
		[SerializeField] int _value = default;
	}

}