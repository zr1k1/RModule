using System;
using System.Collections;

public interface ISceneFader {
	public void FadeOut(Action callback = null);
	public void FadeIn(Action callback = null);
	public IEnumerator FadeOut();
	public IEnumerator FadeIn();
}
