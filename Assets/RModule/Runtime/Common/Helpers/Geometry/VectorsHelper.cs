using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RModule.Runtime.GeometryHelper {

    public static class Vectors {

        /// <summary>Calculates angle between 2 vectors. Angle increases clockwise.</summary>
        /// <param name="startPoint"> Starting Vector with 0°, like Vector3.up</param>
        /// <param name="beginAngleFrom">Vector.up like default clock.</param>
        /// <param name="clockwise">Clockwise.</param>
        /// <returns>Angle between 0° and 360°.</returns>
        public static float CalculateAngle(Vector3 startPoint, Vector3 endPoint, Vector3 beginAngleFrom, bool clockwise = true) {
            if (clockwise)
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
    }

    public static class Points {
        public static bool PointIsInsidePolygon(Vector2 pointToCheck, List<Vector2> polygon) {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++) {
                if (polygon[i].y < pointToCheck.y && polygon[j].y >= pointToCheck.y || polygon[j].y < pointToCheck.y && polygon[i].y >= pointToCheck.y) {
                    if (polygon[i].x + (pointToCheck.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < pointToCheck.x) {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public static bool PointIsInsideBounds(Vector2 pointToCheck, Bounds bounds) {
            return PointIsInsidePolygon(pointToCheck, ConvertBoundsToPointsList(bounds));
        }

        public static List<Vector2> ConvertBoundsToPointsList(Bounds bounds) {
            List<Vector2> pointsList = new();

            pointsList.Add((Vector2)bounds.min + new Vector2(0, 0));
            pointsList.Add((Vector2)bounds.min + new Vector2(0, bounds.size.y));
            pointsList.Add((Vector2)bounds.min + new Vector2(bounds.size.x, bounds.size.y));
            pointsList.Add((Vector2)bounds.min + new Vector2(bounds.size.x, 0));

            return pointsList;
        }

        public static bool PointIsInsideCircle(Vector2 pointToCheck, Vector2 circleCenterPoint, float circleRadius) {
            float dx = Mathf.Abs(pointToCheck.x - circleCenterPoint.x);
            if (dx > circleRadius)
                return false;

            float dy = Mathf.Abs(pointToCheck.y - circleCenterPoint.y);
            if (dy > circleRadius)
                return false;
            if (dx + dy <= circleRadius)
                return true;

            return dx * dx + dy * dy <= circleRadius * circleRadius;
        }
    }
}