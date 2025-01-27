using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public interface IParametersContainer {
	TValueType GetParameterValue<TParameterClass, TValueType>() where TParameterClass : BaseValueConfig;
}

[CreateAssetMenu(fileName = "ParametersContainerConfig", menuName = "RModule/Parameters/ParametersContainerConfig", order = 1)]
public class ParametersContainerConfig : ScriptableObject, IParametersContainer {
	[SerializeField] protected ParametersContainer _parametersContainer = default;

	public TValueType GetParameterValue<TParameterClass, TValueType>() where TParameterClass : BaseValueConfig {
		return ((IParametersContainer)_parametersContainer).GetParameterValue<TParameterClass, TValueType>();
	}
}

[Serializable]
public class ParametersContainer : IParametersContainer {
	[SerializeField] protected List<BaseValueConfig> _parameterValueList = default;

	public virtual TValueType GetParameterValue<TParameterClass, TValueType>() where TParameterClass : BaseValueConfig {
		var valueConfig = _parameterValueList.OfType<TParameterClass>().First();
		if (valueConfig == null) {
			Debug.LogError($"Parameter {typeof(TParameterClass)} is not present on _parameterValueList list!");
			return default(TValueType);
		}

		return valueConfig.GetValue<TValueType>();
	}
}

