using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RModule.Runtime.GeometryHelper {
	//using GeometryHelper;

	public class DirectionCalculator<Directions> {
		//Accessors
		public SerializableDictionary<Directions, Vector2> DirectionVectors => _directionVectors;

		// Outlets
		[SerializeField] protected SerializableDictionary<Directions, Vector2> _directionVectors = default;
	}

	[Serializable]
	public class Degrees90DirectionsCalculator : DirectionCalculator<Degrees90DirectionsCalculator.Direction> {
		// TODO Make universal
		//begin from up
		public enum Direction { Up = 0, Right = 1, Down = 2, Left = 3, Unknown = 4 }

		public class Data {
			public Direction direction;
			public float angle;
		}

		//beginFrom = new Vector3(-1, 1)
		public static Data CalculateDirection(Vector3 startPoint, Vector3 endPoint, Vector3 beginAngleFrom) {
			var angle = Vectors.CalculateAngle(startPoint, endPoint, beginAngleFrom);
			int directionPartsCount = 4;
			float angleForDirection = 360f / (float)directionPartsCount;
			for (int i = 0; i < directionPartsCount; i++) {
				if ((angleForDirection * i <= angle) && angle < (angleForDirection * (i + 1))) {
					return new Data {
						direction = (Direction)i,
						angle = angle
					}; ;
				}
			}

			return new Data {
				direction = Direction.Unknown,
				angle = angle
			}; ;
		}
	}
}

