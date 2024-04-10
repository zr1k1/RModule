namespace RModule.Runtime.Data.Configs {
	using System;
	using UnityEngine;

	public class BaseDebugConfig<OptionalDebugValue> : BaseConfig<OptionalDebugValue> where OptionalDebugValue : Enum {
		//Accessors
		public bool DebugModeEnabled => _debugModeEnabled;

		//Outlets
		[SerializeField] protected bool _debugModeEnabled;

		public virtual void EnableDebugMode() {
			_debugModeEnabled = true;
		}
	}
}