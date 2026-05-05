using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseSceneFader : MonoBehaviour, ISceneFader {
	public UnityEvent DidStartFadeIn = default;
	public UnityEvent DidStartFadeOut = default;

	protected enum FadeDirection { In, Out }

	public abstract void FadeOut(Action callback = null);
	public abstract void FadeIn(Action callback = null);
	public abstract IEnumerator FadeOut();
	public abstract IEnumerator FadeIn();
}
