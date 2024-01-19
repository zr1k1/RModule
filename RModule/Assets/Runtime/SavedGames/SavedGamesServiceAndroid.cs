#if UNITY_ANDROID && USE_GOOGLE_SERVICES

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace RModule.Runtime.SavedGames {
	
	public class SavedGamesServiceAndroid : ISavedGamesService {

		// ---------------------------------------------------------------
		// ISavedGamesService

		public void LoadAllGamesFromCloud(Action<List<string>> callback) {
			var savedGameClient = PlayGamesPlatform.Instance.SavedGame;
			savedGameClient.FetchAllSavedGames(DataSource.ReadNetworkOnly, async (status, list) => {
				if (status == SavedGameRequestStatus.Success) {
					var allGamesData = new List<string>();
					foreach (var savedGame in list) {
						Debug.Log($"Saved game: {savedGame.Filename}. Description: {savedGame.Description}");

						string dataString = await LoadGameAsync(savedGame.Filename);
						allGamesData.Add(dataString);
					}
				
					callback?.Invoke(allGamesData);
					
				} else {
					callback?.Invoke(new List<string>());
				}
			});
		}

		public void DeleteGame(string name, Action<bool> callback) {
			var savedGameClient = PlayGamesPlatform.Instance.SavedGame;
			savedGameClient.OpenWithAutomaticConflictResolution(name, DataSource.ReadCacheOrNetwork,
				ConflictResolutionStrategy.UseLongestPlaytime, (status, metadata) => {
					if (status == SavedGameRequestStatus.Success) {
						var client = PlayGamesPlatform.Instance.SavedGame;
						client.Delete(metadata);
						callback?.Invoke(true);
					} else {
						callback?.Invoke(false);
					}
				});
		}
		
		public void SaveGame(string name, string gameDataString, Action<bool> callback) {
			string escapedDataString = Uri.EscapeDataString(gameDataString);
			var dataBytes = System.Text.Encoding.UTF8.GetBytes(escapedDataString);
			
			var savedGameClient = PlayGamesPlatform.Instance.SavedGame;
			savedGameClient.OpenWithAutomaticConflictResolution(name, DataSource.ReadCacheOrNetwork,
				ConflictResolutionStrategy.UseLongestPlaytime, (status, metadata) => {
					if (status == SavedGameRequestStatus.Success) {
						var deltaTimeSpan = DateTime.Now - metadata.LastModifiedTimestamp;
						var totalTimeSpan = deltaTimeSpan + metadata.TotalTimePlayed;
						SaveGame(metadata, dataBytes, totalTimeSpan, callback);
					} else {
						callback?.Invoke(false);
					}
				});
		}
		
		// ---------------------------------------------------------------
		// Helpers

		void LoadGame(string name, Action<string> gameDataStringCallback) {
			var savedGameClient = PlayGamesPlatform.Instance.SavedGame;
			savedGameClient.OpenWithAutomaticConflictResolution(name, DataSource.ReadNetworkOnly, 
				ConflictResolutionStrategy.UseMostRecentlySaved, (status, metadata) => {
					if (status == SavedGameRequestStatus.Success) {
						var client = PlayGamesPlatform.Instance.SavedGame;
						client.ReadBinaryData(metadata, (requestStatus, bytes) => {
							if (requestStatus == SavedGameRequestStatus.Success) {
								string gameDataString = System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
								Debug.Log($"Loaded game string: {gameDataString}");
								string decodedDataJsonString = Uri.UnescapeDataString(gameDataString);
								Debug.Log($"Decoded game string: {decodedDataJsonString}");
								gameDataStringCallback?.Invoke(decodedDataJsonString);
							} else {
								gameDataStringCallback?.Invoke("");
							}
						});
						
					} else {
						gameDataStringCallback?.Invoke("");
					}
				});
		}
		
		Task<string> LoadGameAsync(string name) {
			var task = new TaskCompletionSource<string>();
			LoadGame(name, gameDataString => task.TrySetResult(gameDataString));
			return task.Task;
		}
		
		void SaveGame (ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime, Action<bool> callback) {
			var savedGameClient = PlayGamesPlatform.Instance.SavedGame;

			var builder = new SavedGameMetadataUpdate.Builder();
			builder = builder
				.WithUpdatedPlayedTime(totalPlaytime)
				.WithUpdatedDescription("Saved game at " + DateTime.Now);
			var updatedMetadata = builder.Build();
			savedGameClient.CommitUpdate(game, updatedMetadata, savedData, (status, metadata) => {
				if (status == SavedGameRequestStatus.Success) {
					Debug.Log($"Saved game to cloud: {metadata.Filename}");
					callback?.Invoke(true);
				} else {
					callback?.Invoke(false);
				}
			});
		}
	}
}

#endif