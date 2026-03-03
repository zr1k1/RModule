using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class VerticalImageCombinerWindow : EditorWindow {
    private List<Texture2D> textures = new List<Texture2D>();
    private Vector2 scroll;

    [MenuItem("RModule/Tools/Image/Vertical Image Combiner")]
    public static void ShowWindow() {
        GetWindow<VerticalImageCombinerWindow>("Vertical Combiner");
    }

    private void OnGUI() {
        GUILayout.Label("Вертикальная склейка изображений", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("Добавить слот")) {
            textures.Add(null);
        }

        GUILayout.Space(10);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        for (int i = 0; i < textures.Count; i++) {
            EditorGUILayout.BeginHorizontal();
            textures[i] = (Texture2D)EditorGUILayout.ObjectField(
                "Texture " + (i + 1),
                textures[i],
                typeof(Texture2D),
                false);

            if (GUILayout.Button("X", GUILayout.Width(25))) {
                textures.RemoveAt(i);
                break;
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(15);

        if (GUILayout.Button("Склеить и сохранить")) {
            CombineAndSave();
        }
    }

    private void CombineAndSave() {
        if (textures.Count == 0) {
            EditorUtility.DisplayDialog("Ошибка", "Добавьте хотя бы одну текстуру.", "OK");
            return;
        }

        int width = textures[0].width;
        int totalHeight = 0;

        foreach (var tex in textures) {
            if (tex == null) {
                EditorUtility.DisplayDialog("Ошибка", "Есть пустые слоты.", "OK");
                return;
            }

            if (tex.width != width) {
                EditorUtility.DisplayDialog("Ошибка", "Все текстуры должны иметь одинаковую ширину.", "OK");
                return;
            }

            totalHeight += tex.height;
        }

        Texture2D combined = new Texture2D(width, totalHeight, TextureFormat.RGBA32, false);

        int currentY = 0;

        foreach (var tex in textures) {
            combined.SetPixels(0, currentY, tex.width, tex.height, tex.GetPixels());
            currentY += tex.height;
        }

        combined.Apply();

        string path = EditorUtility.SaveFilePanel(
            "Сохранить изображение",
            "",
            "CombinedTexture.png",
            "png");

        if (!string.IsNullOrEmpty(path)) {
            byte[] pngData = combined.EncodeToPNG();
            File.WriteAllBytes(path, pngData);
            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("Готово", "Изображение сохранено!", "OK");
        }
    }
}


