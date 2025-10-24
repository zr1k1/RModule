using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class RuWordEndingsDictionary : SerializableDictionary<RuWordEndingMaker.TypeOfEnding, string> { }
[Serializable] public class RuWordByTypeOfGrammaticCountEndingsDictionary : SerializableDictionary<RuWordEndingMaker.TypeOfGrammaticCount, string> { }

[Serializable]
public class RuWordEndingMaker {
	// Enums
	public enum TypeOfEnding { Ending1_21_31_etc = 0, Ending2_3_4_22_23_24_etc = 1, Ending5to20_25to30_etc = 2 }
	public enum TypeOfGrammaticCount { Once = 0, Plural = 1 }

	// Accessors
	public string WordWithoutEndingPart => _wordWithoutEndingPart;

	// Outlets
	[SerializeField] string _wordWithoutEndingPart = default;
	[Tooltip("Если надо полностью заменить, а не только окончание")]
	[SerializeField] bool _fullyReplace = default;
	[SerializeField] RuWordEndingsDictionary _endingPartsDict = default;
	[SerializeField] bool _useTypeOfGrammaticCount = default;
	[SerializeField] RuWordByTypeOfGrammaticCountEndingsDictionary _grammaticCountEndingPartsDict = default;

	// Privats
	static List<int> _numsForEnding2_3_4_22_23_24_etc = new List<int> { 2, 3, 4 };
	static List<int> _numsForEnding5to20_25to30_etc = new List<int> { 0, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };

	public string GetWordByCount(int count) {
		string word = _fullyReplace ? string.Empty : _wordWithoutEndingPart;
		if (_useTypeOfGrammaticCount) {

			return $"{word}{_grammaticCountEndingPartsDict[getTypeOfGrammaticCountByCount(count)]}";
		}

		return $"{word}{_endingPartsDict[getTypeOfEndingByCount(count)]}";
	}

	public static string GetWordWithEndingByCount(string wordWithoutEndingPart, Dictionary<TypeOfEnding, string> endingPartsDict, int count) {
		if (endingPartsDict.Count != 3)
			Debug.LogError($"Set 3 Values to dictionary endingPartsDict! (Example: for word Спичка you need set wordWithoutEndingPart = Спич and values 'ка' 'ки' 'ек')");

		return $"{wordWithoutEndingPart}{endingPartsDict[getTypeOfEndingByCount(count)]}";
	}

	public static void Test1() {
		Dictionary<TypeOfEnding, string> endingPartsDict = new Dictionary<TypeOfEnding, string> {
			[TypeOfEnding.Ending1_21_31_etc] = "ка",
			[TypeOfEnding.Ending2_3_4_22_23_24_etc] = "ки",
			[TypeOfEnding.Ending5to20_25to30_etc] = "ек"
		};
		for (int i = 0; i < 40; i++) {
			Debug.Log($"count = {i} : {GetWordWithEndingByCount("Спич", endingPartsDict, i) }");
		}
	}

	public static void Test2() {
		Dictionary<TypeOfEnding, string> endingPartsDict = new Dictionary<TypeOfEnding, string> {
			[TypeOfEnding.Ending1_21_31_etc] = "",
			[TypeOfEnding.Ending2_3_4_22_23_24_etc] = "а",
			[TypeOfEnding.Ending5to20_25to30_etc] = "ов"
		};
		for (int i = 0; i < 40; i++) {
			Debug.Log($"count = {i} : {GetWordWithEndingByCount("Квадрат", endingPartsDict, i) }");
		}
	}

	static TypeOfEnding getTypeOfEndingByCount(int count) {
		if (count == 1 || ((count % 10 == 1) && (count != 11))) {
			return TypeOfEnding.Ending1_21_31_etc;
		} else if (_numsForEnding2_3_4_22_23_24_etc.Contains(count % 10) && !_numsForEnding5to20_25to30_etc.Contains(count)) {
			return TypeOfEnding.Ending2_3_4_22_23_24_etc;
		} else if (_numsForEnding5to20_25to30_etc.Contains(count % 10) || _numsForEnding5to20_25to30_etc.Contains(count)) {
			return TypeOfEnding.Ending5to20_25to30_etc;
		}

		return TypeOfEnding.Ending1_21_31_etc;
	}

	static TypeOfGrammaticCount getTypeOfGrammaticCountByCount(int count) {
		if (count == 1) {
			return TypeOfGrammaticCount.Once;
		}

		return TypeOfGrammaticCount.Plural;
	}
}
