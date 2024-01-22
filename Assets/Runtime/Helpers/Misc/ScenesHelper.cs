using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScenesHelper {
	public static SceneType CurrentScene => s_currentScene;
	static SceneType s_currentScene;

	public enum SceneType {
		None = 0,
		SplashScene,
		CheatScene,
		MainMenuScene,
		GameScene,
		AllGamesScene,
		GamePlanScene,
		SettingsScene,
		GameCompleteScene,
		LevelCompleteScene
	};

	public static void Open(SceneType sceneType, bool fadeAnimation = true) {
		s_currentScene = sceneType;
		var sceneFader = Object.FindObjectOfType<SceneFader>();
		if (sceneFader == null || !fadeAnimation) {
			SceneManager.LoadScene(sceneType.ToString());
		} else {
			sceneFader.FadeOut(() => {
				SceneManager.LoadScene(sceneType.ToString());
			});
		}
	}
}
