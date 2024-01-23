using System.Collections.Generic;

public static class ListExtensions {

	public static void AddIfNotNull<T>(this List<T> list, T value) where T : class {
		if (value != null) 
			list.Add(value); 
	}
}
