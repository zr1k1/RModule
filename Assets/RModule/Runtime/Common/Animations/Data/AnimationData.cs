using UnityEngine;
using System;
using RModule.Runtime.LeanTween;

[Serializable]
public class AnimationData {
    public bool useFromByGo = default;
    public GameObject goToUse = default;
    public Vector3 from = default;
    public Vector3 to = default;
    public bool back = default;
    public float startDelay = default;
    public float duration = default;
    public LeanTweenType easeLeanTweenType = default;
    public LeanTweenType loopLeanTweenType = default;
    public bool useLoopsCount = default;
    public int loopsCount = default;
    public float delayBetweenLoops = default;
    public bool destroyOnFiinish = default;
}
