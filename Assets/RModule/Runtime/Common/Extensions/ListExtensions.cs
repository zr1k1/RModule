using System.Collections.Generic;
using System.Linq;
using System;

public static class ListExtensions {

	public static void AddIfNotNull<T>(this List<T> list, T value) where T : class {
		if (value != null) 
			list.Add(value);
	}

	private static Random rnd = new Random();

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
}
