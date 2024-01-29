namespace RModule.Runtime.Arcade {

	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	public class AutoDoor : Door {
		// Enums
		public enum AutoCloseWhenTargetObjectsInDirection { Up, Right, Down, Left }

		// Outlets
		[SerializeField] AutoCloseWhenTargetObjectsInDirection _autoCloseWhenTargetObjectsInDirection = default;
		[SerializeField] float _additionalLengthAfterDoorPass = default;

		// Privats
		public List<Transform> _targetsToCheck = new List<Transform>();

		public void Update() {
			if (_targetsToCheck.Count == 0 || !_isOpen)
				return;

			if (_autoCloseWhenTargetObjectsInDirection == AutoCloseWhenTargetObjectsInDirection.Up) {
				var minY = _targetsToCheck.Select(objectToCheck => objectToCheck.transform.position.y).Min();
				if (minY > transform.position.y + _additionalLengthAfterDoorPass) {
					Close();
				}
			} else if (_autoCloseWhenTargetObjectsInDirection == AutoCloseWhenTargetObjectsInDirection.Right) {
				var minX = _targetsToCheck.Select(objectToCheck => objectToCheck.transform.position.x).Min();
				if (minX > transform.position.x + _additionalLengthAfterDoorPass) {
					Close();
				}
			} else if (_autoCloseWhenTargetObjectsInDirection == AutoCloseWhenTargetObjectsInDirection.Down) {
				var maxY = _targetsToCheck.Select(objectToCheck => objectToCheck.transform.position.y).Max();
				if (maxY < transform.position.y - _additionalLengthAfterDoorPass) {
					Close();
				}
			} else if (_autoCloseWhenTargetObjectsInDirection == AutoCloseWhenTargetObjectsInDirection.Left) {
				var maxX = _targetsToCheck.Select(objectToCheck => objectToCheck.transform.position.x).Max();
				if (maxX < transform.position.x - _additionalLengthAfterDoorPass) {
					Close();
				}
			}
		}

		public void SetTargetsToCheck(List<Transform> targetsToCheck) {
			_targetsToCheck.AddRange(targetsToCheck);
		}

		public void CloseAndSkipAnimation() {
			((AutoDoorAnimationComponent)p_itemAnimationComponent).CloseAndSkipAnimation();
		}
	}
}
