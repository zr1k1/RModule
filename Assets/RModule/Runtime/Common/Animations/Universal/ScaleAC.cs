using System.Collections;
using UnityEngine;
using RModule.Runtime.LeanTween;

public class ScaleAC : BaseAC {
    public AnimationData _animationData = default;

	protected override IEnumerator Animate() {
		float duration = _animationData.duration;
		if (_animationData.back) {
			duration *= 0.5f;
		}
		var from = _animationData.useFromByGo && _animationData.goToUse != null ? _animationData.goToUse.transform.localScale : _animationData.from;
		_animationData.goToUse.transform.localScale = from;
		var lt = LeanTween.scale(_animationData.goToUse, _animationData.to, duration)
			.setEase(_animationData.easeLeanTweenType)
			.setDelay(_animationData.startDelay);

		if (_animationData.useLoopsCount) {
			lt.setLoopType(_animationData.loopLeanTweenType);
			lt.setLoopCount(_animationData.loopsCount);
		}

		yield return new WaitForSeconds(duration);
		if (_animationData.back) {
			LeanTween.scale(_animationData.goToUse, from, duration)
				.setEase(_animationData.easeLeanTweenType);
			yield return new WaitForSeconds(duration);
		}

		yield return base.Animate();
	}
}
