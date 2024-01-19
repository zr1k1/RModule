using System;

namespace RModule.Runtime.Services.SocialServices {
	public interface ISocialService {
		bool Authenticated { get; }
		void Setup();
		void Authenticate(Action<bool> callback);
		void ShowLeaderboardUI();
		void ShowAchievementsUI();
		void ReportScore(long score, string leaderboardId);
		void ReportProgress(string achievementId, double progress);
	}
}