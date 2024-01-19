namespace RModule.Runtime.SavedGames {
	public static class SavedGamesServiceProvider {

		public static ISavedGamesService CreateGameService() {
#if UNITY_EDITOR
			return new SavedGamesServiceDummy();
#else
			
	#if UNITY_ANDROID && USE_GOOGLE_SERVICES
	        return new SavedGamesServiceAndroid();
	#elif UNITY_ANDROID && USE_HUAWEI_SERVICES
	        return new SavedGamesServiceHuawei();
	#elif UNITY_IOS
			return new SavedGamesServiceIos();
	#else
			return new SavedGamesServiceDummy();
	#endif
#endif
		}
	}
}