using UnityEngine;
using System.IO;
using System;

public static class AndroidCoronaFilesSavePathGetter {
	const string c_coronaAdditionalPath = "app_data";

	//fileName example = "guessword.db"
	public static string GetPath() {
		string androidSavedFilesDirectoryPath = "";
#if UNITY_ANDROID && !UNITY_EDITOR
		androidSavedFilesDirectoryPath = GetAndroidFilesDirectoryPath();
#endif
		if (string.IsNullOrEmpty(androidSavedFilesDirectoryPath))
			return "";
		Debug.Log("androidFilesDirectoryPath= " + androidSavedFilesDirectoryPath);
		DirectoryInfo androidFilesDirectoryInfo = new DirectoryInfo(androidSavedFilesDirectoryPath);
		DirectoryInfo currentPackageDirectoryInfo = androidFilesDirectoryInfo.Parent;
		var currentPackagePath = currentPackageDirectoryInfo.FullName;
		Debug.Log("currentPackagePath = " + currentPackagePath);
		var coronaFilesSavePath = Path.Combine(currentPackagePath, c_coronaAdditionalPath);

		return coronaFilesSavePath;
	}

	static string GetAndroidFilesDirectoryPath() {
		string path = "";
#if UNITY_ANDROID && !UNITY_EDITOR
         try {
            using (var javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
               using (var javaObject = javaClass.GetStatic<AndroidJavaObject>("currentActivity")) {
                  path = javaObject.Call<AndroidJavaObject>("getFilesDir").Call<string>("getAbsolutePath");
               }
            }
         } catch (Exception e) {
            Debug.LogWarning(e.ToString());
            path = Application.persistentDataPath;
         }
#endif
		return path;
	}
}
