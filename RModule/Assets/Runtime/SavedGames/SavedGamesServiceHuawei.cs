using System;
using System.Collections.Generic;

#if UNITY_ANDROID && USE_HUAWEI_SERVICES

namespace RModule.Runtime.SavedGames {
	public class SavedGamesServiceHuawei : ISavedGamesService {
		
		// ---------------------------------------------------------------
		// ISavedGamesService
		
		public void LoadAllGamesFromCloud(Action<List<string>> callback) {
			callback?.Invoke(new List<string>());
		}

		public void DeleteGame(string name, Action<bool> callback) {
			callback?.Invoke(false);
		}

		public void SaveGame(string name, string gameDataString, Action<bool> callback) {
			callback?.Invoke(false);
		}
	}
}

#endif