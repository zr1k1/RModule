using System;
using UnityEngine;

#if UNITY_ANDROID && USE_GOOGLE_SERVICES
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

namespace RModule.Runtime.Services.SocialServices {
	public class SocialServiceStandard : ISocialService {
		
		// ---------------------------------------------------------------
		// Constructor

		public SocialServiceStandard() {
		}
		
		void SetupGooglePlayConfiguration() {
#if UNITY_ANDROID && USE_GOOGLE_SERVICES
			var config = new PlayGamesClientConfiguration.Builder()
				.EnableSavedGames()
				.Build();
			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
#endif
		}
		
		// ---------------------------------------------------------------
		// ISocialService

		public bool Authenticated => Social.localUser.authenticated;

		public void Setup() {
			SetupGooglePlayConfiguration();
		}
		
		public void Authenticate(Action<bool> callback) {
			Social.localUser.Authenticate(callback);
		}

		public void ShowLeaderboardUI() {
			Social.ShowLeaderboardUI();
		}

		public void ShowAchievementsUI() {
			Social.ShowAchievementsUI();
		}

		public void ReportScore(long score, string leaderboardId) {
			Social.ReportScore(score, leaderboardId,
				success => {
					Debug.Log(
						$"Reported score for leaderboard {leaderboardId}. Value: {score.ToString()}. Success: {(success ? "true" : "false")}");
				});
		}

		public void ReportProgress(string achievementId, double progress) {
			Social.ReportProgress(achievementId, progress,
				success => {
					Debug.Log(
						$"Reported progress on {achievementId}. Value: {progress.ToString()}. Success: {(success ? "true" : "false")}");
				});
		}
	}
}