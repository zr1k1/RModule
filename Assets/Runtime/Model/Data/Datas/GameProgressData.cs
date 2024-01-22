using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class GameProgressData {
	// Accessors
	public const string FOLDER_NAME = "GameData";
	public int LevelsPassed => _levelsPassed;

	// Private vars	
	int _levelsPassed = 0;
	Dictionary<int, int> _levelCompleteTries = new Dictionary<int, int>();

	public GameProgressData(int levelsPassed, Dictionary<int, int> levelCompleteTries) {
		_levelsPassed = levelsPassed;
		_levelCompleteTries = levelCompleteTries;
	}

	// Methods

	public void SetLevelsPassed(int levelsPassed) {
		Debug.Log($"levelsPassed = {levelsPassed}");
		_levelsPassed = levelsPassed;
	}

	public void IncreaseLevelCompleteTries(int levelNumber) {
		if (_levelCompleteTries.ContainsKey(levelNumber))
			_levelCompleteTries[levelNumber] = _levelCompleteTries[levelNumber] + 1;
	}

	public bool TryInitializeTriesCounting(int levelNumber) {
		if (!_levelCompleteTries.ContainsKey(levelNumber)) { 
			_levelCompleteTries.Add(levelNumber, 0);
			return true;
		}
		return false;
	}


	// Getters
	public int GetLevelCompleteTries(int levelNumber) {
		return _levelCompleteTries.ContainsKey(levelNumber) ? _levelCompleteTries[levelNumber] : 0;
	}

	// JSON Generation
	public class JGameProgressData {
		public int levelsPassed;
		public Dictionary<int, int> levelTries = new Dictionary<int, int>();
	}

	public string GenerateJsonAndEncodeData() {
		Debug.Log($"GenerateJsonAndEncodeData");
		string serializedDict = generateJsonDataString();

		//// Encode
		//var encodedBytes = Encoding.UTF8.GetBytes(serializedDict);
		//string encodedDataString = Convert.ToBase64String(encodedBytes);

		string encodedDataString = serializedDict;

		return encodedDataString;
	}

	public static GameProgressData DecodeJsonAndGenerateGameData(string encodedDataString) {
		Debug.Log($"DecodeData {encodedDataString}");
		if (string.IsNullOrEmpty(encodedDataString))
			return null;

		//// Decode
		//var decodedBytes = Convert.FromBase64String(encodedDataString);
		//string decodedText = Encoding.UTF8.GetString(decodedBytes);

		string decodedText = encodedDataString;

		JGameProgressData jGameProgressData = JsonConvert.DeserializeObject<JGameProgressData>(decodedText);
		GameProgressData gameProgressData = new GameProgressData(jGameProgressData.levelsPassed, jGameProgressData.levelTries);

		return gameProgressData;
	}

	public static GameProgressData CreateDefaultGameProgressData() {
		return new GameProgressData(0, new Dictionary<int, int>());
	}

	string generateJsonDataString() {
		JGameProgressData jGameProgressData = new JGameProgressData();
		jGameProgressData.levelsPassed = _levelsPassed;
		jGameProgressData.levelTries = _levelCompleteTries;

		return JsonConvert.SerializeObject(jGameProgressData, Formatting.Indented);
	}
}
