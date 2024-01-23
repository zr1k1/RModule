using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RModule.Runtime.Utils {

	public static class Utils {

		public static int GetLoopPrevNumber(int currentNumber, int beginNumber, int count) {
			int maxNumber = beginNumber + count;
			if (currentNumber < beginNumber || currentNumber > maxNumber) {
				Debug.Log($"currentNumber {currentNumber}");
				Debug.Log($"beginNumber {beginNumber}");
				Debug.Log($"maxNumber {maxNumber}");
				Debug.LogError("Wrong current number, he will be = from beginNumber to beginNumber + count - 1");
			}
			currentNumber = Mathf.Clamp(currentNumber, beginNumber, beginNumber + count);

			return (count + currentNumber - 1 - beginNumber) % count + beginNumber;
		}

		public static int GetLoopNextNumber(int currentNumber, int beginNumber, int count) {
			int maxNumber = beginNumber + count;
			if (currentNumber < beginNumber || currentNumber > maxNumber) {
				Debug.Log($"currentNumber {currentNumber}");
				Debug.Log($"beginNumber {beginNumber}");
				Debug.Log($"maxNumber {maxNumber}");
				Debug.LogError("Wrong current number, he will be = from beginNumber to beginNumber + count - 1");
			}
			currentNumber = Mathf.Clamp(currentNumber, beginNumber, beginNumber + count);

			return ((currentNumber - beginNumber + 1) % (count)) + beginNumber;
		}

		public static string FormatJson(string json) {
			var parsedJson = JsonConvert.DeserializeObject(json);

			return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
		}

		public static string JsonPrettify(string json) {
			using (var stringReader = new StringReader(json))
			using (var stringWriter = new StringWriter()) {
				var jsonReader = new JsonTextReader(stringReader);
				var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
				jsonWriter.WriteToken(jsonReader);
				return stringWriter.ToString();
			}
		}

		public static bool TryGetCurrentTime(out DateTime currentDay) {
			currentDay = DateTime.Now;
			if (Application.internetReachability == NetworkReachability.NotReachable)
				return false;
			Debug.Log($"{Application.internetReachability}");
			var client = new TcpClient("time.nist.gov", 13);
			using (var streamReader = new StreamReader(client.GetStream())) {
				var response = streamReader.ReadToEnd();
				if (response.Length < 25)
					return false;
				var utcDateTimeString = response.Substring(7, 17);
				currentDay = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
			}

			return true;
		}

		// Camera

		public static Vector2 ConvertWorldPosFromCameraToOtherCameraOnDifferentPlace(Vector2 position, Camera from, Camera to) {
			return to.ViewportToWorldPoint(from.WorldToViewportPoint(position));
		}

		// Points Pathes
		public static Vector3[] GeneratePathToLeanTwean(List<Vector3> pathPoints) {

			List<Vector3> generatePathToLeanTwean = new List<Vector3>();
			for (int i = 0; i < pathPoints.Count - 1; i++) {
				Vector2 controlPoint1 = pathPoints[i];
				Vector2 controlPoint2 = (Vector2)pathPoints[i + 1] - controlPoint1;
				controlPoint2 = controlPoint1 + Vector2.Distance(pathPoints[i + 1], controlPoint1) * controlPoint2.normalized / 2f;//middle between two points
				Vector2 endPoint1 = pathPoints[i + 1];
				Vector2 endPoint2 = pathPoints[i + 1];
				generatePathToLeanTwean.Add(controlPoint1);
				generatePathToLeanTwean.Add(controlPoint2);
				generatePathToLeanTwean.Add(endPoint1);
				generatePathToLeanTwean.Add(endPoint2);
			}

			return generatePathToLeanTwean.ToArray();
		}
	}
}
