using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DateTimeExtension {
	public static string ConvertToStringHoursMinSec(this TimeSpan timeSpan, SystemLanguage language) {
		//string current = $"{timeSpan.Hours.ToString("00")}:{timeSpan.Minutes.ToString("00")}:{timeSpan.Seconds.ToString("00")}";

		int hours = timeSpan.Hours;
		int mins = timeSpan.Minutes;
		int secs = timeSpan.Seconds;
		string hoursFormattedStr = timeSpan.Hours.ToString("00");
		if (hours.ToString().Length == 1) {
			hoursFormattedStr = timeSpan.Hours.ToString("0");
		}
		string minsFormattedStr = timeSpan.Minutes.ToString("00");
		if (mins.ToString().Length == 1) {
			minsFormattedStr = timeSpan.Minutes.ToString("0");
		}
		string secsFormattedStr = timeSpan.Seconds.ToString("00");
		if (secs.ToString().Length == 1) {
			secsFormattedStr = timeSpan.Seconds.ToString("0");
		}
		string hoursUnitWord = " h";
		string minsUnitWord = " min";
		string secsUnitWord = " sec";

		if (language == SystemLanguage.Russian) {
			string hoursStr = hours.ToString();
			string minStr = mins.ToString();
			string secStr = secs.ToString();

			hoursUnitWord = " час";
			minsUnitWord = " мин";
			secsUnitWord = " сек";
		}
		string result = "";
		if (hours == 0) {
			if (mins != 0) {
				result = minsFormattedStr + minsUnitWord + " ";
			}
		} else {
			result = hoursFormattedStr + hoursUnitWord + " " + minsFormattedStr + minsUnitWord + " ";
		}

		result += secsFormattedStr + secsUnitWord;

		return result;
	}
}
