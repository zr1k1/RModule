using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorsHelper {
		
	/// <summary>Calculates angle between 2 vectors. Angle increases clockwise.</summary>
	/// <param name="startPoint"> Starting Vector with 0°, like Vector3.up</param>
	/// <param name="beginAngleFrom">Vector.up like default clock.</param>
	/// <param name="clockwise">Clockwise.</param>
	/// <returns>Angle between 0° and 360°.</returns>
	public static float CalculateAngle(Vector3 startPoint, Vector3 endPoint, Vector3 beginAngleFrom, bool clockwise = true) {
		if(clockwise)
			return Quaternion.FromToRotation(endPoint - startPoint, beginAngleFrom).eulerAngles.z;
		else
			return 360 - Quaternion.FromToRotation(endPoint - startPoint, beginAngleFrom).eulerAngles.z;
	}
}
