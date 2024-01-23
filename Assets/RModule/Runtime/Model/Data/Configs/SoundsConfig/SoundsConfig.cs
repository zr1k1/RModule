using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RModule.Runtime.Sounds;

[Serializable] public class SoundsDictionary : SerializableDictionary<string, SoundConfig> { }

public class SoundsConfig : MonoBehaviour {
	public List<SoundConfig> MusicConfigs => _musicConfigs;
	public SoundsDictionary SoundsConfigs => _soundsConfigs;

	[SerializeField] protected List<SoundConfig> _musicConfigs = default;
	[SerializeField] protected SoundsDictionary _soundsConfigs = default;
}
