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
		return Quaternion.FromToRotation(clockwise ? endPoint - startPoint : startPoint - endPoint, beginAngleFrom).eulerAngles.z;
	}

	/// <summary>Calculates angle between 2 vectors. Angle increases clockwise.</summary>
	/// <param name="startPoint"> Starting Vector with 0°, like Vector3.up</param>
	/// <param name="beginAngleFrom">Vector.up like default clock.</param>
	/// <param name="clockwise">Clockwise.</param>
	/// <returns>Angle between 0° and 360°.</returns>
	public static float CalculateAngle(Vector2 startPoint, Vector2 endPoint, Vector2 beginAngleFrom, bool clockwise = true) {			 
		return CalculateAngle(startPoint, endPoint, beginAngleFrom, clockwise);
	}
}
