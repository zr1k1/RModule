using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public static class ListExtensions {

	public static void AddIfNotNull<T>(this List<T> list, T value) where T : class {
		if (value != null) 
			list.Add(value);
	}

	private static System.Random rnd = new System.Random();

	public static void Shuffle<T>(this IList<T> list) {
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = rnd.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static float WayLenght(this List<Vector3> way) {
		float wayLenght = 0f;

		if (way.Count < 2)
			return 0f;

		for (int i = 0; i < way.Count - 1; i++) {
			wayLenght += Vector3.Distance(way[i], way[i + 1]);
		}

		return wayLenght;
	}

	public static string SplitElements<T>(this List<T> list, string stringBetweenElements) {
		string str="";
		if (list.Count == 1) {
			str = list[0].ToString();
		}

		if (list.Count < 2) {
			return str;
		}

		for(int i = 0; i < list.Count-1; i++) {
			str += $"{list[i].ToString()}{stringBetweenElements}";
		}
		str += $"{list[^1].ToString()}";

		return str;
	}
}
