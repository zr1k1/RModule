using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RModule.Runtime.Extensions {
	public static class Vector3Extension {

		/// <summary>Calculates angle between 2 vectors. Angle increases clockwise.</summary>
		/// <param name="from"> Starting Vector with 0°, like Vector3.up</param>
		/// <param name="clockwise">Clockwise.</param>
		/// <param name="o">Starting position of vectors.</param>
		/// <returns>Angle between 0° and 360° from.</returns>
		public static float CalculateAngle(this Vector3 to, Vector3 from, bool clockwise = true) {
			return Quaternion.FromToRotation(clockwise ? to - from : from - to, from).eulerAngles.z;
		}
	}
}
