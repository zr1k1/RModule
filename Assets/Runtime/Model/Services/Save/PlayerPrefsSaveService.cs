using System;
using RModule.Runtime.Utils;
using UnityEngine;

namespace RModule.Runtime.Services {
	public class PlayerPrefsSaveService : ISaveService {
		public void Save() => PlayerPrefs.Save();

		public void SetValue(string key, bool value) => PlayerPrefsHelper.SetBool(key, value);
		public bool GetValue(string key, bool defaultValue = false) => PlayerPrefsHelper.GetBool(key, defaultValue);

		public void SetValue(string key, int value) => PlayerPrefs.SetInt(key, value);
		public int GetValue(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);

		public void SetValue(string key, string value) => PlayerPrefs.SetString(key, value);
		public string GetValue(string key, string defaultValue = null) => PlayerPrefs.GetString(key, defaultValue);

		public void SetValue(string key, float value) => PlayerPrefs.SetFloat(key, value);
		public float GetValue(string key, float defaultValue = 0) => PlayerPrefs.GetFloat(key, defaultValue);

		public void SetValue(string key, DateTime value) => PlayerPrefsHelper.SetDateTime(key, value);
		public DateTime GetValue(string key, DateTime defaultValue = default) => PlayerPrefsHelper.GetDateTime(key, defaultValue);
	}
}
