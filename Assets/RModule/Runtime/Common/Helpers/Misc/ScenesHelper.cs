using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesHelper {
	public static void Open<SceneTypeEnum>(SceneTypeEnum sceneType, bool fadeAnimation = true) {
		ScenesLoader<SceneTypeEnum>.Instance.Open(sceneType, fadeAnimation);
	}
	public static void OpenAsync<SceneTypeEnum>(SceneTypeEnum sceneType, bool fadeAnimation = true) {
		ScenesLoader<SceneTypeEnum>.Instance.OpenAsync(sceneType, fadeAnimation);
	}
	public static void OpenAsyncSingle<SceneTypeEnum>(SceneTypeEnum sceneType, bool fadeAnimation = true) {
		ScenesLoader<SceneTypeEnum>.Instance.OpenAsyncSingle(sceneType, fadeAnimation);
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
		var sceneFader = Object.FindFirstObjectByType<BaseSceneFader>();
		if (sceneFader == null || !fadeAnimation) {
			SceneManager.LoadScene(sceneType.ToString());
		} else {
			sceneFader.FadeOut(() => {
				SceneManager.LoadScene(sceneType.ToString());
			});
		}
	}
	public void OpenAsync(SceneTypeEnum sceneType, bool fadeAnimation = true) {
		s_currentScene = sceneType;
		var sceneFader = Object.FindFirstObjectByType<BaseSceneFader>();
		if (sceneFader == null || !fadeAnimation) {
			SceneManager.LoadSceneAsync(sceneType.ToString(), LoadSceneMode.Additive);
		} else {
			sceneFader.FadeOut(() => {
				SceneManager.LoadSceneAsync(sceneType.ToString(), LoadSceneMode.Additive);
			});
		}
	}
	public void OpenAsyncSingle(SceneTypeEnum sceneType, bool fadeAnimation = true) {
		s_currentScene = sceneType;
		var sceneFader = Object.FindFirstObjectByType<BaseSceneFader>();
		if (sceneFader == null || !fadeAnimation) {
			SceneManager.LoadSceneAsync(sceneType.ToString(), LoadSceneMode.Single);
		} else {
			sceneFader.FadeOut(() => {
				SceneManager.LoadSceneAsync(sceneType.ToString(), LoadSceneMode.Single);
			});
		}
	}
}

