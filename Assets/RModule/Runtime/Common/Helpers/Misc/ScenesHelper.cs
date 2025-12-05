using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ScenesHelper {
	public static void Open<SceneTypeEnum>(SceneTypeEnum sceneType, bool fadeAnimation = true) {
		ScenesLoader<SceneTypeEnum>.Instance.Open(sceneType, fadeAnimation);
	}
	public static void OpenAsync<SceneTypeEnum>(SceneTypeEnum sceneType, bool fadeAnimation = true) {
		ScenesLoader<SceneTypeEnum>.Instance.OpenAsync(sceneType, fadeAnimation);
	}
}

public class ScenesLoader<SceneTypeEnum> {
	public static SceneTypeEnum CurrentScene => s_currentScene;
	static SceneTypeEnum s_currentScene;

	public static ScenesLoader<SceneTypeEnum> Instance => s_instance;
	static readonly ScenesLoader<SceneTypeEnum> s_instance = new ScenesLoader<SceneTypeEnum>();

	static ScenesLoader() { }

	ScenesLoader() {
	}

	public void Open(SceneTypeEnum sceneType, bool fadeAnimation = true) {
		s_currentScene = sceneType;
		var sceneFader = Object.FindObjectOfType<SceneFader>();
		if (sceneFader == null || !fadeAnimation) {
			SceneManager.LoadScene(sceneType.ToString());
		}
		else {
			sceneFader.FadeOut(() => {
				SceneManager.LoadScene(sceneType.ToString());
			});
		}
	}
	public void OpenAsync(SceneTypeEnum sceneType, bool fadeAnimation = true) {
		s_currentScene = sceneType;
		var sceneFader = Object.FindObjectOfType<SceneFader>();
		if (sceneFader == null || !fadeAnimation) {
			SceneManager.LoadSceneAsync(sceneType.ToString(), LoadSceneMode.Additive);
		}
		else {
			sceneFader.FadeOut(() => {
				SceneManager.LoadSceneAsync(sceneType.ToString(), LoadSceneMode.Additive);
			});
		}
	}
}

public enum ExampleSceneType {
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

//public static class ScenesHelper {
//	public static SceneType CurrentScene => s_currentScene;
//	static SceneType s_currentScene;


//	public static void Open(SceneType sceneType, bool fadeAnimation = true) {
//		s_currentScene = sceneType;
//		var sceneFader = Object.FindObjectOfType<SceneFader>();
//		if (sceneFader == null || !fadeAnimation) {
//			SceneManager.LoadScene(sceneType.ToString());
//		} else {
//			sceneFader.FadeOut(() => {
//				SceneManager.LoadScene(sceneType.ToString());
//			});
//		}
//	}
//}

