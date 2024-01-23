using System;
using UnityEngine;

namespace RModule.Runtime.Sounds {
	public class SoundEffectPlayer : MonoBehaviour {

		// Outlets
		[SerializeField] protected SoundConfig _soundEffectConfig = default;
		[SerializeField] protected bool _randomizePitch = false;
		[SerializeField] protected bool _playOnStart = false;
		[SerializeField] protected bool _isImportantSound = true;
		[SerializeField] protected bool _discardSameSoundsInOneFrame = false;

		void Start() {
			if (_playOnStart) {
				PlayEffect();
			}
		}

		public void PlayEffect() {
			if (_soundEffectConfig == null) {
				Debug.Log("Missing sound effect config file");
				return;
			}

			if (_soundEffectConfig.AudioClips == null || _soundEffectConfig.AudioClips.Length <= 0)
				return;

			if (_soundEffectConfig.AudioClips.Length > 1) {
				SoundsManager.Instance.PlayRandomSoundEffect(_soundEffectConfig.AudioClips, _isImportantSound, _discardSameSoundsInOneFrame);
			} else {
				SoundsManager.Instance.PlaySoundEffect(_soundEffectConfig.AudioClips[0], _isImportantSound, _discardSameSoundsInOneFrame);
			}
		}
	}
}
