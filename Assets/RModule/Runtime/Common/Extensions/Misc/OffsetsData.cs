using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetsData {
	public float LeftOffset { private set; get; }
	public float RightOffset { private set; get; }
	public float BottomOffset { private set; get; }
	public float TopOffset { private set; get; }

	public OffsetsData(float leftOffset, float rightOffset, float bottomOffset, float topOffset) {
		LeftOffset = leftOffset;
		RightOffset = rightOffset;
		BottomOffset = bottomOffset;
		TopOffset = topOffset;
	}
}
