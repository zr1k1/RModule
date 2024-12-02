using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	public static (int, Vector3) GetNearestPoint(this Vector3 targetPoint, List<Vector3> points) {
		(int, Vector3) result = (0, points[0]);
		float nearestDistance = Vector3.Distance(targetPoint, points[0]);
		for (int i = 0; i < points.Count; i++) {
			var distance = Vector3.Distance(targetPoint, points[i]);
			if (distance < nearestDistance) {
				nearestDistance = distance;
				result = (i, points[i]);
			}
		}

		return result;
	}

	public static bool PointIsInsidePolygon(List<Vector2> polygon, Vector2 testPoint) {
		bool result = false;
		int j = polygon.Count() - 1;
		for (int i = 0; i < polygon.Count(); i++) {
			if (polygon[i].y < testPoint.y && polygon[j].y >= testPoint.y || polygon[j].y < testPoint.y && polygon[i].y >= testPoint.y) {
				if (polygon[i].x + (testPoint.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < testPoint.x) {
					result = !result;
				}
			}
			j = i;
		}
		return result;
	}
}
