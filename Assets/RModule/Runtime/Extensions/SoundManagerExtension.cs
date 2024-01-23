using System.Linq;
using RModule.Runtime.Sounds;
using UnityEngine;

public static class SoundManagerExtension {
	public static void PlaySoundEffect(this SoundsManager soundsManager, AudioClip clip, float volume, bool isImportantSound = true, bool discardSameSoundsInOneFrame = false) {
		var audioSources = soundsManager.GetComponents<AudioSource>().ToList().FindAll(audioSource => audioSource.clip == clip);

		if (discardSameSoundsInOneFrame && (audioSources.FindAll(audioSource => audioSource.isPlaying).Count > 0))
			return;

		soundsManager.PlaySoundEffect(clip, isImportantSound, discardSameSoundsInOneFrame);
		soundsManager.TrySetClipVolume(clip, volume);
	}

	public static void PlaySoundEffect(this SoundsManager soundsManager, SoundConfig soundConfig, bool isImportantSound = true, bool discardSameSoundsInOneFrame = false) {
		var clip = soundConfig.AudioClips[0];
		var audioSources = soundsManager.GetComponents<AudioSource>().ToList().FindAll(audioSource => audioSource.clip == clip);

		if (discardSameSoundsInOneFrame && (audioSources.FindAll(audioSource => audioSource.isPlaying).Count > 0)) {
			return;
		}

		soundsManager.PlaySoundEffect(clip, soundConfig.Volume, isImportantSound, discardSameSoundsInOneFrame);
		soundsManager.TrySetClipVolume(clip, soundConfig.Volume);
	}

	public static void PlayRandomSoundEffect(this SoundsManager soundsManager, SoundConfig improvedSoundConfig, bool isImportantSound = true, bool discardSameSoundsInOneFrame = false) {
		if (improvedSoundConfig.AudioClips == null)
			return;

		int randomIndex = Random.Range(0, improvedSoundConfig.AudioClips.Length);
		soundsManager.PlaySoundEffect(improvedSoundConfig.AudioClips[randomIndex], improvedSoundConfig.Volume, isImportantSound, discardSameSoundsInOneFrame);
		soundsManager.TrySetClipVolume(improvedSoundConfig.AudioClips[randomIndex], improvedSoundConfig.Volume);
	}

	public static void PlayImproveMusic(this SoundsManager soundsManager, SoundConfig soundConfig) {
		var clip = soundConfig.AudioClips[0];

		soundsManager.PlayMusic(clip);
		soundsManager.TrySetClipVolume(clip, soundConfig.Volume);
	}

	static void TrySetClipVolume(this SoundsManager soundsManager, AudioClip clip, float volume) {
		var audioSources = soundsManager.GetComponents<AudioSource>().ToList().FindAll(audioSource => audioSource.clip == clip);
		foreach (var audioSource in audioSources)
			audioSource.volume = volume;
	}
}
