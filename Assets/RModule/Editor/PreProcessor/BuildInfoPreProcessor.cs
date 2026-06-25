using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildInfoPreProcessor : ScriptableObject, IPreprocessBuildWithReport {
    public int callbackOrder => 0;
    [SerializeField] IntValueConfig _buildNumberInValueConfig = default;

    public void OnPreprocessBuild(BuildReport report) {

        if (_buildNumberInValueConfig == null) {
            Debug.LogError("_buildNumberInValueConfig asset not found");
            return;
        }

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

        Debug.Log($"Build={_buildNumberInValueConfig.DefaultValue}");
    }
}
