using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemLanguageHelper {
    public enum SystemLanguage { Unknown, Abkhazian, Azerbaijani, Georgian, Armenian, Kazakh, Kirghiz, Tajik, Ukrainian, Belarussian }

	// Private vars
	public static bool CurrentOperationSystemLocaleExistInLanguagesListForConvertToRussianSystemLanguage() {
		List<SystemLanguage> languagesListForConvertToRussianSystemLanguage = new List<SystemLanguage>();
		languagesListForConvertToRussianSystemLanguage.Add(SystemLanguage.Kazakh);
		languagesListForConvertToRussianSystemLanguage.Add(SystemLanguage.Ukrainian);
		languagesListForConvertToRussianSystemLanguage.Add(SystemLanguage.Belarussian);
		//languagesListForConvertToRussianSystemLanguage.Add(SystemLanguage.Kirghiz);
		//languagesListForConvertToRussianSystemLanguage.Add(SystemLanguage.Tajik);
		//languagesListForConvertToRussianSystemLanguage.Add(SystemLanguage.Abkhazian);
		//languagesListForConvertToRussianSystemLanguage.Add(SystemLanguage.Azerbaijani);
		//languagesListForConvertToRussianSystemLanguage.Add(SystemLanguage.Georgian);
		//languagesListForConvertToRussianSystemLanguage.Add(SystemLanguage.Armenian);

		return languagesListForConvertToRussianSystemLanguage.Contains(ConvertSngIsoLanguageLettersToSystemLanguage());
	}

	public static bool CurrentSystemLanguageIsBelaru() {
		return System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "be";
	}

	static SystemLanguage ConvertSngIsoLanguageLettersToSystemLanguage() {
		string twoISOsymbols = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
		Debug.Log($"GetSystemLanguage twoISOsymbols {twoISOsymbols}");

		if (twoISOsymbols.Length != 2) {
            Debug.Log("Неверное количество символов ISO для определения языка по 2 символам");
            return SystemLanguage.Unknown;
        }

        if (twoISOsymbols == "ab")
            return SystemLanguage.Abkhazian;
        else if (twoISOsymbols == "az")
            return SystemLanguage.Azerbaijani;
        else if (twoISOsymbols == "ka")
            return SystemLanguage.Georgian;
        else if (twoISOsymbols == "hy")
            return SystemLanguage.Armenian;
        else if (twoISOsymbols == "kk")
            return SystemLanguage.Kazakh;
        else if (twoISOsymbols == "ky")
            return SystemLanguage.Kirghiz;
        else if (twoISOsymbols == "tg")
            return SystemLanguage.Tajik;
		else if (twoISOsymbols == "uk")
			return SystemLanguage.Ukrainian;
		else if (twoISOsymbols == "be")
			return SystemLanguage.Belarussian;
		else {
            Debug.Log("current language is ru or cant convert to ru");
            return SystemLanguage.Unknown;
        }
    }
}
