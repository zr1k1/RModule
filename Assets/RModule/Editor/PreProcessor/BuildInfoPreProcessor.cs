using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildInfoPreProcessor : ScriptableObject, IPreprocessBuildWithReport, IPostprocessBuildWithReport {

    public int callbackOrder => 0;

    [SerializeField] IntValueConfig _buildNumberInValueConfig = default;

    const string PreviousValueKey = "BuildInfoPreProcessor.PreviousBuildNumber";

    public void OnPreprocessBuild(BuildReport report) {
        if (_buildNumberInValueConfig == null) {
            Debug.LogError("_buildNumberInValueConfig asset not found");
            return;
        }

        // Сохраняем исходное значение
        EditorPrefs.SetInt(PreviousValueKey, _buildNumberInValueConfig.DefaultValue);

        int buildNumber = 0;

        switch (EditorUserBuildSettings.activeBuildTarget) {
            case BuildTarget.Android:
                buildNumber = PlayerSettings.Android.bundleVersionCode;
                break;

            case BuildTarget.iOS:
                buildNumber = int.Parse(PlayerSettings.iOS.buildNumber);
                break;
        }

        _buildNumberInValueConfig.SetValueOnlyInEditorMode(buildNumber);

        EditorUtility.SetDirty(_buildNumberInValueConfig);
        AssetDatabase.SaveAssets();
    }

    public void OnPostprocessBuild(BuildReport report) {
        if (_buildNumberInValueConfig == null)
            return;

        if (!EditorPrefs.HasKey(PreviousValueKey))
            return;

        int previousValue = EditorPrefs.GetInt(PreviousValueKey);

        _buildNumberInValueConfig.SetValueOnlyInEditorMode(previousValue);

        EditorUtility.SetDirty(_buildNumberInValueConfig);
        AssetDatabase.SaveAssets();

        EditorPrefs.DeleteKey(PreviousValueKey);

        Debug.Log($"Build finished. Restored value={previousValue}");
    }
}