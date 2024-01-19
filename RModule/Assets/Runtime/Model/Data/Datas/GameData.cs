using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class GameData {
	// Accessors
	//public const string MINI_IMG_COMMON_ADDRESS = "img_game_";
	//public int Number => _number;
	//public int MaxScores => _maxScores;
	//public List<RoundData> RoundDatas => _roundDatas;

	//// Private vars
	//readonly int _number;
	//int _maxScores;
	//readonly List<RoundData> _roundDatas = new List<RoundData>();

	//// Classes
	//public class RoundData {
	//	// Accessors
	//	public int Number => _number;
 //       public List<QuestionData> QuestionDatas => _questionDatas;

	//	// Private vars
	//	readonly int _number;
	//	readonly List<QuestionData> _questionDatas = new List<QuestionData>();

	//	// Classes
	//	public class QuestionData {
	//		// Enums
	//		public enum DifficultType { Default = 0, Hard = 1}
	//		public enum Type { Answers4 = 0, Casino = 1, ManualEntry = 2, Sequence = 3, Pictures = 4 }

	//		// Accessors
	//		public int Id => _id;
	//		public int Number => _number;
 //           public Type QuestionType => _type;
	//		public DifficultType Difficult => _difficult;
	//		public string Question => _question;
 //           public string Answer1 => _answer1;
 //           public string Answer2 => _answer2;
 //           public string Answer3 => _answer3;
 //           public string Answer4 => _answer4;
 //           public string Explanation => _explanation;
 //           public string QuestionPicFileName => _questionPicFileName;
 //           public bool QuestionPicIsFullScreen => _questionPicIsFullScreen;
 //           public string AnswerPicFileName => _answerPicFileName;
 //           public bool AnswerPicIsFullScreen => _answerPicIsFullScreen;
	//		public Sprite QuestionImg => _questionImg;
	//		public Sprite AnswerImg => _answerImg;
	//		public List<Sprite> AdditionalImgs => _additionalImgs;

	//		// Private vars
	//		readonly int _id;
	//		readonly int _number;
	//		readonly Type _type;
	//		readonly DifficultType _difficult;
	//		readonly string _question;
	//		readonly string _answer1;
	//		readonly string _answer2;
	//		readonly string _answer3;
	//		readonly string _answer4;
	//		readonly string _explanation;
	//		readonly string _questionPicFileName;
	//		readonly bool _questionPicIsFullScreen;
	//		readonly string _answerPicFileName;
	//		readonly bool _answerPicIsFullScreen;
	//		Sprite _questionImg;
	//		Sprite _answerImg;
	//		List<Sprite> _additionalImgs = new List<Sprite>();

	//		static int s_idCounter = 1;

	//		public QuestionData(int number, Type type, DifficultType difficult, string question, string answer1, string answer2
	//			, string answer3, string answer4, string explanation, string questionPicFileName
	//			, bool questionPicIsFullScreen, string answerPicFileName, bool answerPicIsFullScreen) {

	//			_number = number;
	//			_type = type;
	//			_difficult = difficult;
	//			_question = question;
	//			_answer1 = answer1;
	//			_answer2 = answer2;
	//			_answer3 = answer3;
	//			_answer4 = answer4;
	//			_explanation = explanation;
	//			_questionPicFileName = questionPicFileName;
	//			_questionPicIsFullScreen = questionPicIsFullScreen;
	//			_answerPicFileName = answerPicFileName;
	//			_answerPicIsFullScreen = answerPicIsFullScreen;
	//			if (Application.isPlaying) {
	//				_id = s_idCounter;
	//				s_idCounter++;
	//			}
	//		}

	//		public void SetQuestionImg(Sprite questionImg) {
	//			_questionImg = questionImg;
	//		}

	//		public void SetAnswerImg(Sprite answerImg) {
	//			_answerImg = answerImg;
	//		}

	//		public void AddAdditionalImg(Sprite additionalImg) {
	//			_additionalImgs.Add(additionalImg);
	//		}

	//		public void DebugLog() {
	//			Debug.Log($"QuestionData _number = {_number}\n" +
	//				$"_type = {_type}\n" +
	//				$"_question = {_question}\n" +
	//				$"_answer1 = {_answer1}\n" +
	//				$"_answer2 = {_answer2}\n" +
	//				$"_answer3 = {_answer3}\n" +
	//				$"_answer4 = {_answer4}\n" +
	//				$"_explanation = {_explanation}\n" +
	//				$"_questionPicFileName = {_questionPicFileName}\n" +
	//				$"_questionPicIsFullScreen = {_questionPicIsFullScreen}\n" +
	//				$"_answerPicFileName = {_answerPicFileName}\n" +
	//				$"_answerPicIsFullScreen = {_answerPicIsFullScreen}\n"
	//				);
	//		}
	//	}

	//	public RoundData(int number, List<QuestionData> questionDataList) {
	//		_number = number;
	//		_questionDatas.AddRange(questionDataList);
	//	}

	//	public void DebugLog() {
	//		Debug.Log($"RoundData _number = {_number}\n");
 //           for (int i = 0; i < _questionDatas.Count; i++)
 //               _questionDatas[i].DebugLog();
	//	}

	//	public QuestionData GetQuestionByNumber(int number) {
	//		var questionData = _questionDatas.Find(questionData => questionData.Number == number);
	//		return questionData != null ? questionData : _questionDatas[0];

	//	}
	//}

	//public GameData(int number, List<RoundData> roundDatas) {
	//	_number = number;
	//	_roundDatas.AddRange(roundDatas);

	//	foreach(var roundData in _roundDatas) {
	//		int maxScoreByType = 1;
			
	//		if(roundData.QuestionDatas.Count > 0 && roundData.QuestionDatas[0].QuestionType == RoundData.QuestionData.Type.Casino) {
	//			maxScoreByType = 3;
	//		}
	//		_maxScores += maxScoreByType * roundData.QuestionDatas.Count;
	//	}
	//}

	//public void DebugLog() {
	//	Debug.Log($"GameData _number = {_number}\n _roundDatas.Count = {_roundDatas.Count}\n");
	//	for (int i = 0; i < _roundDatas.Count; i++)
	//		_roundDatas[i].DebugLog();
	//}

	//public int GetQuestionsCount() {
	//	int count = 0;
	//	foreach (var roundData in _roundDatas)
	//		count += roundData.QuestionDatas.Count;

	//	return count;
	//}

	//public RoundData GetRoundByNumber(int number) {
	//	var roundData = _roundDatas.Find(roundData => roundData.Number == number);
	//	return roundData != null ? roundData : _roundDatas[0];

	//}
	//// JSON Generation

	//public class JGameData {
	//	public int number;
	//	public JRoundDataArray jRoundDataArray;
	//}

	//public class JRoundDataArray {
	//	public JRoundData[] jRoundDatas;
	//}

	//public class JRoundData {
	//	public int number;
	//	public JQuestionDataArray jQuestionDataArray;
	//}

	//public class JQuestionDataArray {
	//	public JQuestionData[] jQuestionDatas;
	//}

	//public class JQuestionData {
	//	public int number;
	//	public RoundData.QuestionData.Type type;
	//	public RoundData.QuestionData.DifficultType difficult;
	//	public string question;
	//	public string answer1;
	//	public string answer2;
	//	public string answer3;
	//	public string answer4;
	//	public string explanation;
	//	public string questionPicFileName;
	//	public bool questionPicIsFullScreen;
	//	public string answerPicFileName;
	//	public bool answerPicIsFullScreen;
	//}

	//public string GenerateJsonAndEncodeData() {
	//	Debug.Log($"GenerateJsonAndEncodeData");
	//	string serializedDict = generateJsonDataString();

	//	//// Encode
	//	//var encodedBytes = Encoding.UTF8.GetBytes(serializedDict);
	//	//string encodedDataString = Convert.ToBase64String(encodedBytes);

	//	string encodedDataString = serializedDict;

	//	return encodedDataString;
	//}

	//public static GameData DecodeJsonAndGenerateGameData(string encodedDataString) {
	//	Debug.Log($"DecodeData {encodedDataString}");
	//	if (string.IsNullOrEmpty(encodedDataString))
	//		return null;

	//	//// Decode
	//	//var decodedBytes = Convert.FromBase64String(encodedDataString);
	//	//string decodedText = Encoding.UTF8.GetString(decodedBytes);

	//	string decodedText = encodedDataString;

	//	JGameData jGameData = JsonConvert.DeserializeObject<JGameData>(decodedText);
	//	//List<RoundData> roundDatas = new List<RoundData>();
	//	//var chars = SettingsManager.Instance.AppConfigData.CharsForColoringPartOfText;

	//	//for (int i = 0; i < jGameData.jRoundDataArray.jRoundDatas.Length; i++) {
	//	//	List<RoundData.QuestionData> questionDatas = new List<RoundData.QuestionData>();
	//	//	for (int j = 0; j < jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas.Length; j++) {
	//	//		questionDatas.Add(new RoundData.QuestionData(jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].number
	//	//			, jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].type
	//	//			, jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].difficult
	//	//			, StringExtension.ApplyColorToTextBetweenTwoCharacters(jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].question, chars[0][0], chars[1][0], SettingsManager.Instance.AppConfigData.TextSelectionColor)
	//	//			, jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].answer1
	//	//			, jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].answer2
	//	//			, jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].answer3
	//	//			, jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].answer4
	//	//			, StringExtension.ApplyColorToTextBetweenTwoCharacters(jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].explanation, chars[0][0], chars[1][0], SettingsManager.Instance.AppConfigData.TextSelectionColor)
	//	//			, jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].questionPicFileName
	//	//			, jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].questionPicIsFullScreen
	//	//			, jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].answerPicFileName
	//	//			, jGameData.jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j].answerPicIsFullScreen));
	//	//	}

	//	//	roundDatas.Add(new RoundData(jGameData.jRoundDataArray.jRoundDatas[i].number, questionDatas));
	//	//}
	//	//GameData gameData = new GameData(jGameData.number, roundDatas);

	//	return null;
	//}

	//public static string DummyJsonGameData() {
	//	JGameData jGameData = new JGameData();
	//	jGameData.number = 1;
	//	JRoundDataArray jRoundDataArray = new JRoundDataArray();
	//	jGameData.jRoundDataArray = jRoundDataArray;
	//	jRoundDataArray.jRoundDatas = new JRoundData[4];
	//	for (int i = 0; i < jRoundDataArray.jRoundDatas.Length; i++) {
	//		jRoundDataArray.jRoundDatas[i] = new JRoundData();
	//		jRoundDataArray.jRoundDatas[i].number = i;
	//		jRoundDataArray.jRoundDatas[i].jQuestionDataArray = new JQuestionDataArray();
	//		jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas = new JQuestionData[3];
	//		for (int j = 0; j < jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas.Length; j++) {
	//			jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j] = new JQuestionData {
	//				number = j,
	//				type = (RoundData.QuestionData.Type)(j % Enum.GetNames(typeof(RoundData.QuestionData.Type)).Length),
	//				difficult = (RoundData.QuestionData.DifficultType)(j % Enum.GetNames(typeof(RoundData.QuestionData.DifficultType)).Length),
	//				question = $"question_{j}",
	//				answer1 = $"answer1",
	//				answer2 = $"answer2",
	//				answer3 = $"answer3",
	//				answer4 = $"answer4",
	//				explanation = $"explanation",
	//				questionPicFileName = $"img_{j}",
	//				questionPicIsFullScreen = false,
	//				answerPicFileName = $"img_{j + 10}",
	//				answerPicIsFullScreen = true,
	//			};
	//		}
	//	}

	//	return JsonConvert.SerializeObject(jGameData, Formatting.Indented);
	//}

	//string generateJsonDataString() {
	//	JGameData jGameData = new JGameData();
	//	jGameData.number = _number;
	//	JRoundDataArray jRoundDataArray = new JRoundDataArray();
	//	jGameData.jRoundDataArray = jRoundDataArray;
	//	jRoundDataArray.jRoundDatas = new JRoundData[_roundDatas.Count];
	//	for (int i = 0; i < jRoundDataArray.jRoundDatas.Length; i++) {
	//		jRoundDataArray.jRoundDatas[i] = new JRoundData();
	//		jRoundDataArray.jRoundDatas[i].number = _roundDatas[i].Number;
	//		jRoundDataArray.jRoundDatas[i].jQuestionDataArray = new JQuestionDataArray();
	//		jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas = new JQuestionData[_roundDatas[i].QuestionDatas.Count];
	//		for (int j = 0; j < jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas.Length; j++) {
	//			jRoundDataArray.jRoundDatas[i].jQuestionDataArray.jQuestionDatas[j] = new JQuestionData {
	//				number = _roundDatas[i].QuestionDatas[j].Number,
	//				type = _roundDatas[i].QuestionDatas[j].QuestionType,
	//				difficult = _roundDatas[i].QuestionDatas[j].Difficult,
	//				question = _roundDatas[i].QuestionDatas[j].Question,
	//				answer1 = _roundDatas[i].QuestionDatas[j].Answer1,
	//				answer2 = _roundDatas[i].QuestionDatas[j].Answer2,
	//				answer3 = _roundDatas[i].QuestionDatas[j].Answer3,
	//				answer4 = _roundDatas[i].QuestionDatas[j].Answer4,
	//				explanation = _roundDatas[i].QuestionDatas[j].Explanation,
	//				questionPicFileName = _roundDatas[i].QuestionDatas[j].QuestionPicFileName,
	//				questionPicIsFullScreen = _roundDatas[i].QuestionDatas[j].QuestionPicIsFullScreen,
	//				answerPicFileName = _roundDatas[i].QuestionDatas[j].AnswerPicFileName,
	//				answerPicIsFullScreen = _roundDatas[i].QuestionDatas[j].AnswerPicIsFullScreen,
	//			};
	//		}
	//	}
	//	//jsonString = System.Text.RegularExpressions.Regex.Unescape(jsonString);

	//	return JsonConvert.SerializeObject(jGameData, Formatting.Indented);
	//}
}
