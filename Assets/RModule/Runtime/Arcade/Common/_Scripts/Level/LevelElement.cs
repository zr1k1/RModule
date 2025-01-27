namespace RModule.Runtime.Arcade {

	using UnityEngine;

	public class LevelElement : MonoBehaviour {

		// Outlets
		[Tooltip("If null will try GetComponent from this game object")]
		[SerializeField] protected LevelPauseComponent p_levelPauseComponent = default;

		protected virtual void Awake() {
			if(p_levelPauseComponent == null)
				p_levelPauseComponent = GetComponent<LevelPauseComponent>();
		}
	}
}
