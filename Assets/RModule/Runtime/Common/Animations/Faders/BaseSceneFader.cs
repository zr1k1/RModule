using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSceneFader : MonoBehaviour, ISceneFader {
	protected enum FadeDirection { In, Out }

	public abstract void FadeOut(Action callback = null);
	public abstract void FadeIn(Action callback = null);
	public abstract IEnumerator FadeOut();
	public abstract IEnumerator FadeIn();
}
