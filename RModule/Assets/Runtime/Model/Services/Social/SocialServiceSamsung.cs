#if PLATFORM_ANDROID && USE_SAMSUNG_SERVICES

using System;

namespace RModule.Runtime.Services.SocialServices {
	public class SocialServiceSamsung : ISocialService {

		public bool Authenticated => false;
		
		public void Setup() {
		}

		public void Authenticate(Action<bool> callback) {
			callback?.Invoke(false);
		}

		public void ShowLeaderboardUI() {
		}

		public void ShowAchievementsUI() {
		}

		public void ReportScore(long score, string leaderboardId) {
		}

		public void ReportProgress(string achievementId, double progress) {
		}
	}
}

#endif