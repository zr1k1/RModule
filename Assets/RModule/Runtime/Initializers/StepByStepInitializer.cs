using System;
using System.Collections;
using System.Collections.Generic;

public class StepByStepInitializer : Initializer, IStepByStepInitializer {

	// Events
	public event Action AllStepsDidDone = default;
	public event Action<int> StepIndexDidDone = default;

	// Accessors
	public int StepsCount => _initializationSteps.Count;

	//Privats
	protected List<IEnumerator> _initializationSteps = new List<IEnumerator>();

	public StepByStepInitializer(params IEnumerator[] enumeratorSteps) {
		for (int i = 0; i < enumeratorSteps.Length; i++)
			_initializationSteps.Add(enumeratorSteps[i]);
	}

	public override IEnumerator Initialize() {
		for (int i = 0; i < _initializationSteps.Count; i++) {
			yield return _initializationSteps[i];
			StepIndexDidDone?.Invoke(i);
		}

		AllStepsDidDone?.Invoke();
	}
}
