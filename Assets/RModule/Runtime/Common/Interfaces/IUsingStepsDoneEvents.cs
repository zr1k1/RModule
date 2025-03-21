using System;

public interface IUsingStepsDoneEvents {
	public event Action AllStepsDidDone;
	public event Action<int> StepIndexDidDone;
}
