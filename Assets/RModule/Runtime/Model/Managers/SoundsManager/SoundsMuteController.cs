using UnityEngine;

public class SoundsMuteController : MonoBehaviour {

	// Private
	AudioSource[] audioSources;

	void Awake() {
		audioSources = GetComponents<AudioSource>();
	}

    void OnApplicationPause(bool pause) {
		if (pause) {
			foreach (var audioSource in audioSources)
				audioSource.mute = true;
		} else {
			foreach (var audioSource in audioSources)
				audioSource.mute = false;
		}
	}
}
