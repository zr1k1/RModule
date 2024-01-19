#if UNITY_IOS

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RModule.Runtime.SavedGames {
	public class SavedGamesServiceIos : ISavedGamesService {

		List<SavedGameDataGameCenter> _savedGames = new List<SavedGameDataGameCenter>();
		
		// ---------------------------------------------------------------
		// Constructors

		public SavedGamesServiceIos() {
			GKNativeExtensions.GKInit(conflictDataArray => { Debug.Log("Conflict"); },
				modifiedData => { Debug.Log("Modified callback"); });
		}

		// ---------------------------------------------------------------
		// ISavedGamesService

		public void LoadAllGamesFromCloud(Action<List<string>> callback) {
			GKNativeExtensions.GKFetchSavedGames(async savedGames => {
				if (savedGames != null) {
					_savedGames = new List<SavedGameDataGameCenter>(savedGames);
				}

				var allGamesData = new List<string>();
				foreach (var savedGame in _savedGames) {
					Debug.Log($"Saved game: {savedGame.name}. Device: {savedGame.deviceName}");

					string dataString = await LoadGameAsync(savedGame.name);
					allGamesData.Add(dataString);
				}
				
				callback?.Invoke(allGamesData);
			});
		}
		
		public void DeleteGame(string name, Action<bool> callback) {
			GKNativeExtensions.GKDeleteGame(name, success => {
				callback?.Invoke(success);
			});
		}

		public void SaveGame(string name, string gameDataString, Action<bool> callback) {
			string escapedDataString = Uri.EscapeDataString(gameDataString);
			var dataBytes = System.Text.Encoding.UTF8.GetBytes(escapedDataString);
			
			GKNativeExtensions.GKSaveGame(dataBytes, name, center => {
				callback?.Invoke(center != null);
			});
		}
		
		// ---------------------------------------------------------------
		// Helpers

		void LoadGame(string name, Action<string> gameDataStringCallback) {
			var game = _savedGames.Find(savedGame => savedGame.name == name);
			if (game == null) {
				gameDataStringCallback?.Invoke("");
				return;
			}

			GKNativeExtensions.GKLoadGame(game, bytes => {
				string gameDataString = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
				Debug.Log($"Loaded game string: {gameDataString}");
				string decodedDataJsonString = Uri.UnescapeDataString(gameDataString);
				Debug.Log($"Decoded game string: {decodedDataJsonString}");
				gameDataStringCallback?.Invoke(decodedDataJsonString);
			});
		}
		
		Task<string> LoadGameAsync(string name) {
			var task = new TaskCompletionSource<string>();
			LoadGame(name, gameDataString => task.TrySetResult(gameDataString));
			return task.Task;
		}
	}
}

#endif