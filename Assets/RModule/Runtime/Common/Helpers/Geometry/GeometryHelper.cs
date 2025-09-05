using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RModule.Runtime.GeometryHelper {
    public static class Lines {
        /// Get Intersection point
        /// </summary>
        /// <param name="a1">a1 is line1 start</param>
        /// <param name="a2">a2 is line1 end</param>
        /// <param name="b1">b1 is line2 start</param>
        /// <param name="b2">b2 is line2 end</param>
        /// <returns></returns>
        public static bool LinesСrossed(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersectionPoint) {
            intersectionPoint = Vector2.zero;
            Vector2 b = a2 - a1;
            Vector2 d = b2 - b1;
            var bDotDPerp = b.x * d.y - b.y * d.x;

            // if b dot d == 0, it means the lines are parallel so have infinite intersection points
            if (bDotDPerp == 0)
                return false;

            Vector2 c = b1 - a1;
            var t = (c.x * d.y - c.y * d.x) / bDotDPerp;
            if (t < 0 || t > 1) {
                return false;
            }

            var u = (c.x * b.y - c.y * b.x) / bDotDPerp;
            if (u < 0 || u > 1) {
                return false;
            }

            intersectionPoint = a1 + t * b;

            return true;
        }

    }
}

namespace GeometryHelper {
    public static class Paths {
        public static float GetPathLength(List<Vector3> pathPoints) {
            float length = 0f;

            for (int i = 0; i < pathPoints.Count - 1; i++)
                length += Vector3.Distance(pathPoints[i], pathPoints[i + 1]);

            return length;
        }
    }
}