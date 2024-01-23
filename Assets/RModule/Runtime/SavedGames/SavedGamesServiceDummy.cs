using System;
using System.Collections.Generic;
using UnityEngine;

namespace RModule.Runtime.SavedGames {
	public class SavedGamesServiceDummy : ISavedGamesService {
		
		// Private vars
		List<string> _dummyGames;
		
		// ---------------------------------------------------------------
		// ISavedGamesService

		public void LoadAllGamesFromCloud(Action<List<string>> callback) {
			var dummyGames = new List<string>();
			for (int i = 0; i < 5; i++) {
				const string gameDataString = "%7B%22deviceName%22%3A%22x86%5F64%20Simulator%22%2C%22timestamp%22%3A1596438523%2E0%2C%22locale%5Fprogress%22%3A%7B%22fr%22%3A%7B%22golden%5Fwords%22%3A%5B%5D%2C%22currentLevelNumber%22%3A1%2C%22currentPackNumber%22%3A1%2C%22passed%5Fpacks%22%3A%5B%5D%2C%22completed%5Flevels%22%3A%7B%7D%2C%22season%5Fgame%5Fprogress%22%3A%7B%7D%7D%2C%22ru%22%3A%7B%22season%5Fgame%5Fprogress%22%3A%7B%22curLvl%22%3A1%2C%22curId%22%3A%22sg%5Fsummer%22%2C%22passed%22%3A%7B%7D%2C%22option%22%3A2%2C%22daysLeftTemp%22%3A0%2C%22gw%22%3A%5B%5D%7D%2C%22completed%5Flevels%22%3A%7B%221%22%3A5%7D%2C%22passed%5Fpacks%22%3A%5B%5D%2C%22currentPackNumber%22%3A1%2C%22golden%5Fwords%22%3A%5B%5D%2C%22currentLevelNumber%22%3A6%7D%2C%22en%22%3A%7B%22golden%5Fwords%22%3A%5B%5D%2C%22currentLevelNumber%22%3A1%2C%22currentPackNumber%22%3A1%2C%22passed%5Fpacks%22%3A%5B%5D%2C%22completed%5Flevels%22%3A%7B%7D%2C%22season%5Fgame%5Fprogress%22%3A%7B%7D%7D%2C%22pt%22%3A%7B%22golden%5Fwords%22%3A%5B%5D%2C%22currentLevelNumber%22%3A1%2C%22currentPackNumber%22%3A1%2C%22passed%5Fpacks%22%3A%5B%5D%2C%22completed%5Flevels%22%3A%7B%7D%2C%22season%5Fgame%5Fprogress%22%3A%7B%7D%7D%7D%2C%22iapp%22%3Afalse%2C%22total%5Fscore%22%3A60%2C%22bc%22%3Atrue%2C%22purchased%5Fproducts%22%3A%7B%22theme%5Fsea%22%3A1%2C%22monster%5F1%22%3A1%2C%22monster%5F5%22%3A1%2C%22bg%5Fflowers%22%3A1%2C%22monster%5F0%22%3A1%7D%2C%22unique%5Fname%22%3A%22F6623T9972W1594974388%22%2C%22timeMode%5Fdata%22%3A%7B%7D%2C%22levelsWOHints%22%3A4%2C%22total%5Fcoins%22%3A26673%2C%22levelsWOMistakes%22%3A4%2C%22purchased%5Fpacks%22%3A%5B%5D%2C%22vocabularyWordsFound%22%3A0%2C%22total%5Fhints%22%3A31%7D";
				string decodedDataJsonString = Uri.UnescapeDataString(gameDataString);
				Debug.Log($"Decoded game string: {decodedDataJsonString}");
				dummyGames.Add(decodedDataJsonString);
			}

			_dummyGames = dummyGames;
			callback?.Invoke(dummyGames);
		}

		public void DeleteGame(string name, Action<bool> callback) {
			if (_dummyGames.Count > 0) {
				_dummyGames.RemoveAt(0);
				callback?.Invoke(true);
			} else {
				callback?.Invoke(false);
			}
		}
		
		public void SaveGame(string name, string gameDataString, Action<bool> callback) { }
	}
}