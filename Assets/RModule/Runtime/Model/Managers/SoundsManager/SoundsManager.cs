using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using RModule.Runtime.Utils;


namespace RModule.Runtime.Sounds {
	public class SoundsManager : SingletonMonoBehaviour<SoundsManager>, ISoundsPlayerService {
		// --- Public accessors ---
		public bool SoundEnabled => _soundEnabled;
		public bool MusicEnabled => _musicEnabled;
		public bool MusicIsMuted => GetMusicSource().mute;

		// Audio players components.
		[SerializeField] protected List<AudioSource> _effectsSources = new List<AudioSource>();
		[SerializeField] protected AudioSource _musicSource = default;
		[SerializeField] protected int _maxSoundSources = 10;
		[SerializeField] protected float _identicalSoundsDiscardPeriod = 0.1f;

		// --- Private vars ---
		protected bool _soundEnabled = true;
		protected bool _musicEnabled = true;
		protected readonly Dictionary<int, bool> _currentRestrictedSounds = new Dictionary<int, bool>();
		protected float _soundsDiscardPeriodCount;
		protected AudioClip _lastPlayedClip;

		const string k_soundEnabled = "MT_soundEnabled";
		const string k_musicEnabled = "MT_musicEnabled";

		public IEnumerator Initialize() {
			yield return null;
		}

		public override bool IsInitialized() {
			return true;
		}

		// ---------------------------------------------------------------
		// GameObject lifecycle

		void Start() {
			_soundEnabled = PlayerPrefsHelper.GetBool(k_soundEnabled, true);
			_musicEnabled = PlayerPrefsHelper.GetBool(k_musicEnabled, true);
		}

		void LateUpdate() {
			_soundsDiscardPeriodCount += Time.deltaTime;
			if (_soundsDiscardPeriodCount >= _identicalSoundsDiscardPeriod) {
				_soundsDiscardPeriodCount = 0f;

				_currentRestrictedSounds.Clear();
			}
		}

		// ---------------------------------------------------------------
		// General Methods

		public virtual void OnSoundsStateChanged(bool soundEnabled) {
			_soundEnabled = soundEnabled;
			PlayerPrefs.SetInt(k_soundEnabled, soundEnabled ? 1 : 0);
			PlayerPrefs.Save();
		}

		public virtual void OnMusicStateChanged(bool musicEnabled) {
			_musicEnabled = musicEnabled;
			PlayerPrefs.SetInt(k_musicEnabled, musicEnabled ? 1 : 0);
			PlayerPrefs.Save();

			if (_musicEnabled) {
				ResumeMusic();
			} else {
				PauseMusic();
			}
		}

		// ---------------------------------------------------------------
		// Sound effects

		public void PlaySoundEffect(SoundConfig soundConfig) {
			if (soundConfig == null || soundConfig.AudioClips == null || soundConfig.AudioClips.Length == 0)
				return;

			if (soundConfig.AudioClips.Length == 1) {
				PlaySoundEffect(soundConfig.AudioClips[0], soundConfig.RandomizePitch);
			} else {
				PlayRandomSoundEffect(soundConfig.AudioClips, soundConfig.RandomizePitch);
			}
		}

		public void PlaySoundEffect(AudioClip clip, bool isImportantSound = true, bool discardSameSoundsInOneFrame = false) {
			if (!_soundEnabled || clip == null)
				return;

			if (discardSameSoundsInOneFrame) {
				if (_currentRestrictedSounds.ContainsKey(clip.GetInstanceID())) {
					return;
				}
				_currentRestrictedSounds[clip.GetInstanceID()] = true;
			}

			var effectsSource = GetSoundEffectsSource(isImportantSound);
			if (effectsSource == null)
				return;

			effectsSource.pitch = 1f;

			effectsSource.clip = clip;
			effectsSource.Play();
		}

		public void PlayRandomSoundEffect(AudioClip[] clips, bool isImportantSound = true, bool discardSameSoundsInOneFrame = false) {
			if (clips == null || !_soundEnabled)
				return;

			int randomIndex = Random.Range(0, clips.Length);
			PlaySoundEffect(clips[randomIndex], isImportantSound, discardSameSoundsInOneFrame);
		}

		// ---------------------------------------------------------------
		// Bg music

		public void PlayMusic(SoundConfig soundConfig) {
			if (soundConfig == null || soundConfig.AudioClips == null || soundConfig.AudioClips.Length == 0)
				return;

			PlayMusic(soundConfig.AudioClips[0]);
		}

		public void PlayMusic(AudioClip clip) {
			var musicSource = GetMusicSource();
			if (_musicEnabled && musicSource.isPlaying && musicSource.clip == clip)
				return;

			StopMusic();
			musicSource.clip = clip;

			if (!_musicEnabled || clip == null)
				return;

			musicSource.Play();
			_lastPlayedClip = clip;
		}

		public void StopMusic(SoundConfig soundConfig) {
			if (soundConfig == null || soundConfig.AudioClips == null || soundConfig.AudioClips.Length == 0)
				return;

			var musicSource = GetMusicSource();
			if (musicSource.isPlaying && musicSource.clip == soundConfig.AudioClips[0]) {
				musicSource.Stop();
			}
		}

		public void StopMusic() {
			var musicSource = GetMusicSource();
			if (musicSource.isPlaying) {
				musicSource.Stop();
			}
		}

		public void ResumeMusic() {
			if (_musicEnabled && GetMusicSource().clip != null) {
				GetMusicSource().Play();
			}
		}

		public void PauseMusic() {
			if (GetMusicSource().clip != null) {
				GetMusicSource().Pause();
			}
		}

		public void PlayRandomMusic(AudioClip[] clips, bool excludeLastPlayedClip = true) {
			if (clips == null)
				return;

			var filteredClips = clips;
			if (excludeLastPlayedClip && _lastPlayedClip != null) {
				filteredClips = clips.Where(clip => clip != _lastPlayedClip).ToArray();
			}

			int randomIndex = Random.Range(0, filteredClips.Length);
			PlayMusic(filteredClips[randomIndex]);
		}

		public void MuteMusic(bool mute) {
			var musicSource = GetMusicSource();
			musicSource.mute = mute;
		}

		// ---------------------------------------------------------------
		// Helpers

		AudioSource GetSoundEffectsSource(bool isImportantSound) {
			foreach (var source in _effectsSources) {
				if (!source.isPlaying) {
					return source;
				}
			}

			if (_effectsSources.Count >= _maxSoundSources) {
				return isImportantSound ? _effectsSources[0] : null;
			}

			var soundSource = gameObject.AddComponent<AudioSource>();
			_effectsSources.Add(soundSource);
			return soundSource;
		}

		AudioSource GetMusicSource() {
			return _musicSource;
		}
	}
}
