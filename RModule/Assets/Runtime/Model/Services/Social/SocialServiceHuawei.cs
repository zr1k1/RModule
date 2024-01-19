#if PLATFORM_ANDROID && USE_HUAWEI_SERVICES

using System;
using HmsPlugin;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.Utils;
using RModule.Runtime.Managers.Misc;
using UnityEngine;

namespace RModule.Runtime.Services.SocialServices {
	public class SocialServiceHuawei : ISocialService {
		
		// Private vars
		bool _isLoggedIn;
		Action<bool> _logInCallback;
		
		// ---------------------------------------------------------------
		// Constructor

		public SocialServiceHuawei() {
			
		}

		// ---------------------------------------------------------------
		// ISocialService

		public bool Authenticated => _isLoggedIn;

		public void Setup() {
			//HuaweiServicesManager.Instance.AccountManager.OnSignInSuccess += OnLoginSuccess;
			//HuaweiServicesManager.Instance.AccountManager.OnSignInFailed += OnLoginFailure;
			
			// HMSGame manager will call sign in and then init all other managers and stuff
			HuaweiServicesManager.Instance.HuaweiGameManager.SignInSuccess += OnLoginSuccess;
			HuaweiServicesManager.Instance.HuaweiGameManager.SignInFailure += OnLoginFailure;
		}

		public void Authenticate(Action<bool> callback) {
			_logInCallback = callback;
			//HuaweiServicesManager.Instance.AccountManager.SignIn();
			HuaweiServicesManager.Instance.HuaweiGameManager.Init();
		}

		public void ShowLeaderboardUI() {
			HuaweiServicesManager.Instance.LeaderboardManager.ShowLeaderboards();
		}

		public void ShowAchievementsUI() {
			HuaweiServicesManager.Instance.AchievementsManager.ShowAchievements();
		}

		public void ReportScore(long score, string leaderboardId) {
			HuaweiServicesManager.Instance.LeaderboardManager.SubmitScore(leaderboardId, score);
		}

		public void ReportProgress(string achievementId, double progress) {
			HuaweiServicesManager.Instance.AchievementsManager.UnlockAchievement(achievementId);
		}
		
		// ---------------------------------------------------------------
		// Listeners

		public void OnLoginSuccess(AuthAccount authAccount) {
			Debug.Log($"Huawei log in success. authHuaweiId: {authAccount.DisplayName} - {authAccount.Uid}");
			_isLoggedIn = true;
			
			UnityMainThreadDispatcher.Instance().Enqueue(() => {
				_logInCallback?.Invoke(true);
				_logInCallback = null;
			});
		}

		public void OnLoginFailure(HMSException error) {
			Debug.Log($"Huawei log in error. Code: {error.ErrorCode}. Message: {error.Message}");
			_isLoggedIn = false;
			UnityMainThreadDispatcher.Instance().Enqueue(() => {
				_logInCallback?.Invoke(false);
				_logInCallback = null;
			});
		}
	}
}

#endif