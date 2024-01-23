using System;
using System.Collections.Generic;

namespace RModule.Runtime.SavedGames {
	public interface ISavedGamesService {
		void LoadAllGamesFromCloud(Action<List<string>> callback);
		void DeleteGame(string name, Action<bool> callback);
		void SaveGame(string name, string gameDataString, Action<bool> callback);
	}
}