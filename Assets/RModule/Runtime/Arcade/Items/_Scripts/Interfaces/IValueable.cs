namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public interface IValueable<T> {
		T GetValue();
	}
}