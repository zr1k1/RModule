using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class ImageSplitter : EditorWindow {
	public enum EncodeType { PNG, JPG }
	// Private vars
	Texture2D _sourceTexture2D = default;
	int _splittedImageSize = default;
	List<SplittedImagesRow> _splittedImagesRows = new List<SplittedImagesRow>();
	string _resultImagesCommonName = "";
	int _beginNumerateRows = 0;
	int _beginNumerateColumns = 0;
	string _pathToSave = "Sprites/SplittedTextures/";
	EncodeType _resultImagesEncodeType;
	float space = 5;

	[MenuItem("RModule/Tools/Image/Image splitter window")]
	static void WindowInit() {
		EditorWindow window = GetWindow(typeof(ImageSplitter));
		window.Show();
	}

	private void OnGUI() {
		ShowLabelMenuName();
		ShowHelpLabel();
		SourceSpriteField();
		SplittedImagesSizeField();
		ResultImagesCommonNameField();
		BeginNumerateFromField();
		ButtonSplit();
		ButtonReset();
		ResultListOfSplittedImagesRowField();
		ShowFieldSavePath();
		ShowFieldEncodeType();
		ButtonCreateSritesInAssets();
	}

	void ShowLabelMenuName() {
		GUILayout.Label("Image Splitter");
		GUILayout.Space(space);
	}

	void ShowHelpLabel() {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.HelpBox("Check source image ImportSettings for real size and Read/Write enabled flag is True ", MessageType.Warning);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(space);
	}

	void SourceSpriteField() {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.HelpBox("Drag image for split here ", MessageType.Info);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		_sourceTexture2D = (Texture2D)EditorGUILayout.ObjectField("Source Texture2D", _sourceTexture2D, typeof(Texture2D), true);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(space);
	}

	void SplittedImagesSizeField() {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.HelpBox("Input pieces size, for example if source image 45x50, and splitted size = 40 result will be 4 images: 1-40x40 2-5x40 3-40x10 4-5x10", MessageType.Info);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		_splittedImageSize = EditorGUILayout.IntField("Splitted Images Size", _splittedImageSize);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(space);
	}

	void ResultImagesCommonNameField() {
		if (_resultImagesCommonName == "" && _sourceTexture2D != null)
			_resultImagesCommonName = _sourceTexture2D.name;
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.HelpBox("Result Images Common Name, for example if 'source' result will be 'source_piece_0_0', if field is empty name will be getting from source img name", MessageType.Info);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		_resultImagesCommonName = EditorGUILayout.TextField("Result Imgs Common Name", _resultImagesCommonName);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(space);
	}

	void BeginNumerateFromField() {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.HelpBox("Begin Numerate For Rwos/Columns, for example if rows '1' and colums '1' first piece will be 'source_piece_1_1'", MessageType.Info);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		_beginNumerateRows = EditorGUILayout.IntField("Begin Numerate For Rows", _beginNumerateRows);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		_beginNumerateColumns = EditorGUILayout.IntField("Begin Numerate For Columns", _beginNumerateColumns);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(space);
	}

	void ButtonSplit() {
		GUILayout.Space(space);
		if (GUILayout.Button("Split"))
			SplitImage();
	}

	void ButtonReset() {
		GUILayout.Space(2);
		if (GUILayout.Button("Reset Result"))
			ResetResult();
		GUILayout.Space(space);
	}

	void ResultListOfSplittedImagesRowField() {
		EditorGUILayout.BeginVertical();
		for (int i = 0; i < _splittedImagesRows.Count; i++) {
			EditorGUILayout.BeginHorizontal();
			for (int j = 0; j < _splittedImagesRows[i].sprites.Count; j++) {
				_splittedImagesRows[i].sprites[j] = (Texture2D)EditorGUILayout.ObjectField(_splittedImagesRows[i].sprites[j], typeof(Texture2D), true);
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();
		GUILayout.Space(space);
	}

	void SplitImage() {
		int width = _sourceTexture2D.width;
		int height = _sourceTexture2D.height;
		_sourceTexture2D.GetPixels();

		int rowsCount = Mathf.CeilToInt((float)height / (float)_splittedImageSize);
		int columnsCount = Mathf.CeilToInt((float)width / (float)_splittedImageSize);
		//Debug.Log("rowsCount = "+ rowsCount);
		var w = _splittedImageSize;
		var h = _splittedImageSize;
		int posX = 0;
		int posY = 0;
		for (int i = 0; i < rowsCount; i++) {
			SplittedImagesRow splittedImagesRow = new SplittedImagesRow { sprites = new List<Texture2D>() };
			var temp = Mathf.Clamp(_splittedImageSize * (rowsCount - i), 0, height) % _splittedImageSize;
			h = _splittedImageSize;
			if (i == rowsCount - 1) {
				h = height % _splittedImageSize;
				if (h == 0)
					h = _splittedImageSize;
			}

			_splittedImagesRows.Add(splittedImagesRow);
			for (int j = 0; j < columnsCount; j++) {
				temp = Mathf.Clamp(_splittedImageSize * (j + 1), 0, width) % _splittedImageSize;
				w = temp == 0 ? _splittedImageSize : temp;
				temp = Mathf.Clamp(_splittedImageSize * (rowsCount - i), 0, height) % _splittedImageSize;
				posX = _splittedImageSize * j;
				temp = height - _splittedImageSize * (i + 1);
				posY = Mathf.Clamp(temp, 0, _sourceTexture2D.height);
				//Debug.Log($"posX={posX}, posY={posY}, w={w}, h={h}");
				var pixs = _sourceTexture2D.GetPixels(posX, posY, w, h);
				Texture2D tex2d = new Texture2D(w, h, TextureFormat.RGBA32, true);
				tex2d.SetPixels(0, 0, w, h, pixs);
				tex2d.Apply();
				splittedImagesRow.sprites.Add(tex2d);
			}
		}
	}

	void ResetResult() {
		_splittedImagesRows.Clear();
	}

	void ShowFieldEncodeType() {
		_resultImagesEncodeType = (EncodeType)EditorGUILayout.EnumPopup("Result Images Encode Type", _resultImagesEncodeType);
		GUILayout.Space(space);
	}

	void ShowFieldSavePath() {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.HelpBox("Path To Save in Assets/", MessageType.Info);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		_pathToSave = EditorGUILayout.TextField("Path To Save Assets/", _pathToSave);
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(space);
	}

	void ButtonCreateSritesInAssets() {
		GUILayout.Space(space);
		if (GUILayout.Button("Create srites in assets"))
			CreateSpritesInAssets();
		GUILayout.Space(space);
	}

	void CreateSpritesInAssets() {
		string fileExt = "";
		if (_resultImagesEncodeType == EncodeType.PNG)
			fileExt = ".png";
		else if (_resultImagesEncodeType == EncodeType.JPG)
			fileExt = ".jpg";
		for (int i = 0; i < _splittedImagesRows.Count; i++) {
			for (int j = 0; j < _splittedImagesRows[i].sprites.Count; j++) {
				Sprite spr = Sprite.Create(_splittedImagesRows[i].sprites[j], new Rect(new Vector2(0, 0), new Vector2(_splittedImagesRows[i].sprites[j].width, _splittedImagesRows[i].sprites[j].height)), new Vector2(0.5f, 0.5f));
				spr.name = $"{_resultImagesCommonName}_piece_{i + _beginNumerateRows}_{j + _beginNumerateColumns}{fileExt}";
				SaveSpriteToEditorPath(spr, _pathToSave);
			}
		}
	}

	Sprite SaveSpriteToEditorPath(Sprite sp, string path) {
		path = Path.Combine(Application.dataPath, path);

		path = Path.Combine(path, $"{sp.name}");
		string dir = Path.GetDirectoryName(path);

		Directory.CreateDirectory(dir);
		//Debug.Log("path =" + path);
		Directory.CreateDirectory(dir);
		if (_resultImagesEncodeType == EncodeType.PNG)
			File.WriteAllBytes(path, sp.texture.EncodeToPNG());
		else if (_resultImagesEncodeType == EncodeType.JPG)
			File.WriteAllBytes(path, sp.texture.EncodeToJPG());
		//Debug.Log(_resultImagesEncodeType);
		AssetDatabase.Refresh();
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		return AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
	}
}

[Serializable]
public struct SplittedImagesRow {
	public List<Texture2D> sprites;
}
