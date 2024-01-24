using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class LevelProgressData {
	// Accessors
	public List<Vector2> PickedCoinsPositions => _pickedCoinsPositions;
	public int CheckPointIndex => _checkPointIndex;

	// Private vars	
	List<Vector2> _pickedCoinsPositions = new List<Vector2>();
	int _checkPointIndex = -1;

	// Classes

	public LevelProgressData(List<Vector2> pickedCoinsPositions, int checkPointIndex) {
		_pickedCoinsPositions.AddRange(pickedCoinsPositions);
		_checkPointIndex = checkPointIndex;
	}

	// Methods
	public bool TryAddPickedCoinPosition(Vector2 coinPosition) {
		if (!_pickedCoinsPositions.Contains(coinPosition)) {
			_pickedCoinsPositions.Add(coinPosition);
			return true;
		}
		return false;
	}

	public void SetCheckPointIndex(int checkPointIndex) {
		_checkPointIndex = checkPointIndex;
	}

	public void ResetCheckPoint() {
		_checkPointIndex = -1;
	}

	// Getters

	// JSON Generation
	public class JLevelProgressData {
		public List<Vector2> pickedCoinsPositions = new List<Vector2>();
		public int checkPointIndex = -1;
	}

	public string GenerateJsonAndEncodeData() {
		Debug.Log($"LevelProgressData : GenerateJsonAndEncodeData");
		string serializedDict = generateJsonDataString();

		//// Encode
		//var encodedBytes = Encoding.UTF8.GetBytes(serializedDict);
		//string encodedDataString = Convert.ToBase64String(encodedBytes);

		string encodedDataString = serializedDict;

		return encodedDataString;
	}

	public static LevelProgressData DecodeJsonAndGenerateGameData(string encodedDataString) {
		Debug.Log($"LevelProgressData : DecodeData {encodedDataString}");
		if (string.IsNullOrEmpty(encodedDataString))
			return null;

		//// Decode
		//var decodedBytes = Convert.FromBase64String(encodedDataString);
		//string decodedText = Encoding.UTF8.GetString(decodedBytes);

		string decodedText = encodedDataString;

		JLevelProgressData jData = JsonConvert.DeserializeObject<JLevelProgressData>(decodedText);
		LevelProgressData data = new LevelProgressData(jData.pickedCoinsPositions, jData.checkPointIndex);

		return data;
	}

	public static LevelProgressData CreateDefaultProgressData() {
		return new LevelProgressData(new List<Vector2>(), -1);
	}

	string generateJsonDataString() {
		JLevelProgressData jData = new JLevelProgressData();
		jData.pickedCoinsPositions = new List<Vector2>(_pickedCoinsPositions);
		jData.checkPointIndex = _checkPointIndex;

		return JsonConvert.SerializeObject(jData, Formatting.Indented, new JsonSerializerSettings {
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		});
	}
}
