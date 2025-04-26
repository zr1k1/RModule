namespace RModule.Runtime.Arcade {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using RModule.Runtime.Utils;
	using RModule.Runtime.Utils.Vectors;

	public class MoveRegulator : BaseBlock {
		// Events
		public UnityEvent<MoveRegulator> DirectionDidChange = default;

		// Accessors
		public Vector2 Direction => p_direction;

		// Outlets
		[SerializeField] ViewDirectionController _viewDirectionController = default;
		[SerializeField] Degrees90DirectionsCalculator _degrees90DirectionsCalculator = default;
		[SerializeField] float _switchDirectionDelay = default;
		[SerializeField] int _startDirectionIndex = default;

		// Privats
		protected List<Vector2> p_possibleDirections = new List<Vector2>();
		protected Vector2 p_direction;
		protected int p_directionIndex;
		protected bool p_switch;

		protected override void Start() {
			p_contactDetector.Setup(this);
			foreach(var direction in _degrees90DirectionsCalculator.DirectionVectors)
				p_possibleDirections.Add(direction.Value);
			p_direction = p_possibleDirections[_startDirectionIndex];
			p_directionIndex = _startDirectionIndex;

			p_switch = true;
		}

		private void Update() {
			if (p_switch) {
				p_switch = false;
				StartCoroutine(SwitchDirectionAndDelay());
			}
		}

		IEnumerator SwitchDirectionAndDelay() {
			p_directionIndex = Utils.GetLoopNextNumber(p_directionIndex, 0, p_possibleDirections.Count);
			p_direction = p_possibleDirections[p_directionIndex];
			_viewDirectionController.ChangeDirection(p_direction);
			DirectionDidChange?.Invoke(this);
			yield return new WaitForSeconds(_switchDirectionDelay);
			p_switch = true;
		}
	}
}
