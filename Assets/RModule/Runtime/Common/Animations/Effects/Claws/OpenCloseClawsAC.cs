using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RModule.Runtime.LeanTween;

public class OpenCloseClawsAC : BaseAC {
	public enum OpenCloseType { OpenClose, OnlyOpen, OnlyClose }

	public OpenCloseType openCloseType = default;

	[SerializeField] List<ClawData> _clawDatas = default;
	[SerializeField] float _delayAfterStartOpening = default;
	[SerializeField] float _delayAfterStartClosing = default;
	[SerializeField] int _openCloseClawsCount = 1;
	[SerializeField] bool onlyOpen = default;
	[SerializeField] bool onlyClo = default;

	[Serializable]
	public class ClawData {
		public GameObject go = default;
		public float angle = default;
		public float duration = default;
		public LeanTweenType leanTweenType = default;
	}

	protected override IEnumerator Animate() {
		foreach (var clawData in _clawDatas) {
			LeanTween.rotateAroundLocal(clawData.go, Vector3.forward, clawData.angle, clawData.duration).setEase(clawData.leanTweenType);
		}
		yield return new WaitForSeconds(_delayAfterStartOpening);

		yield return base.Animate();
	}
}
