using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensions { 

	public static string ToHexString(this Color32 c) {
		return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", c.r, c.g, c.b, c.a);
	}

	public static string ToHexString(this Color color) {
		Color32 c = color;
		return c.ToHexString();
	}

	public static int ToHex(this Color32 c) {
		return (c.r << 24) | (c.g << 16) | (c.b << 8) | (c.a);
	}

	public static int ToHex(this Color color) {
		Color32 c = color;
		return c.ToHex();
	}

	public static bool CompareColor(this Color32 colorA, Color32 colorB) {
		return colorA.r == colorB.r && colorA.g == colorB.g && colorA.b == colorB.b && colorA.a == colorB.a;
	}
}
