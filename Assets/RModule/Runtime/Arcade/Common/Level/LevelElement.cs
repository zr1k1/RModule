namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class LevelElement : MonoBehaviour {

		// Outlets
		[SerializeField] protected LevelPauseComponent p_levelPauseComponent = default;

		protected virtual void Awake() {
			p_levelPauseComponent = GetComponent<LevelPauseComponent>();
		}
	}
}
