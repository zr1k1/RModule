using System;
using System.Globalization;
using UnityEngine;

namespace RModule.Runtime.Utils {
	public static class PlayerPrefsHelper {

		public static void SetBool( string key, bool value) {
			PlayerPrefs.SetInt(key, value ? 1 : 0);
		}
		
		public static bool GetBool(string key, bool defaultValue = false) {
			return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
		}

		public static void SetDateTime(string key, DateTime dateTime) {
			PlayerPrefs.SetString(key, dateTime.ToString(CultureInfo.InvariantCulture));
		}

		public static DateTime GetDateTime(string key, DateTime defaultValue) {
			string timeString = PlayerPrefs.GetString(key);
			return string.IsNullOrEmpty(timeString) ? defaultValue : Convert.ToDateTime(timeString, CultureInfo.InvariantCulture);
		}
	}
}