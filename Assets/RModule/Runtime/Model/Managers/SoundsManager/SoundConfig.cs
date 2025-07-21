using UnityEngine;

namespace RModule.Runtime.Sounds {
	[CreateAssetMenu(fileName = "SoundConfig", menuName = "RModule/Sounds/SoundConfig")]
	public class SoundConfig : ScriptableObject {
		// Accessors
		public AudioClip[] AudioClips => _audioClips;
		public bool RandomizePitch => _randomizePitch;
		public float Volume => _volume;

		// Outlets
		[SerializeField] protected AudioClip[] _audioClips;
		[SerializeField] protected bool _randomizePitch;
		[Range(0f, 1f)]
		[SerializeField] protected float _volume = 1f;
	}
}