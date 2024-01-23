using System;
using System.Collections.Generic;
using System.Linq;
//using RModule.Runtime.MiniJSON;
using Newtonsoft.Json;

public static class DictionaryExtensions {

	public static bool TryAddRange (this Dictionary<string, object> sourceDictionary, Dictionary<string, object> dictionary, out string conflictKey) {
		conflictKey = "";
		foreach (var item in dictionary) {
            if (sourceDictionary.ContainsKey(item.Key)) {
                conflictKey = item.Key;
                return false;
            } else
                sourceDictionary.Add(item.Key, item.Value);
        }

		return true;
    }

	public static byte[] GetSafeByteArrayValue(this Dictionary<string, object> dictionary, string key) {
		if (!dictionary.ContainsKey(key))
			return new byte[0];


		return ConvertHelper.ObjectToByteArray(dictionary[key]);
	}

	public static int GetSafeIntValue(this Dictionary<string, object> dictionary, string key, int defaultValue = 0) {
        if (!dictionary.ContainsKey(key))
            return defaultValue;
            
        return Convert.ToInt32(dictionary[key]);
	}

	public static bool GetSafeBoolValue(this Dictionary<string, object> dictionary, string key, bool defaultValue = false) {
        if (!dictionary.ContainsKey(key))
            return defaultValue;

        return Convert.ToBoolean(dictionary[key]);
	}

	public static long GetSafeLongValue(this Dictionary<string, object> dictionary, string key, long defaultValue = 0) {
        if (!dictionary.ContainsKey(key))
            return defaultValue;

		return Convert.ToInt64(dictionary[key]);
    }

	public static string GetSafeStringValue(this Dictionary<string, object> dictionary, string key, string defaultValue = "") {
        if (!dictionary.ContainsKey(key))
            return defaultValue;

        return Convert.ToString(dictionary[key]);
	}

	public static List<int> GetSafeIntList(this Dictionary<string, object> dictionary, string key) {
        List<int> listInt = new List<int>();
        if (!dictionary.ContainsKey(key))
            return listInt;

        foreach (var obj in (List<object>)dictionary[key]) 
			listInt.Add(Convert.ToInt32(obj));
		return listInt;
	}

	public static int[] GetSafeIntArray(this Dictionary<string, object> dictionary, string key) {
        if (!dictionary.ContainsKey(key))
            return new int[1]; 

        return ((object[])dictionary[key]).Select(s => (int)s).ToArray();
	}

	public static List<string> GetSafeStringList(this Dictionary<string, object> dictionary, string key) {
        List<string> result = new List<string>();
        if (!dictionary.ContainsKey(key))
            return result; 

		foreach (var obj in (List<object>)dictionary[key])
			result.Add(Convert.ToString(obj));

		return result;
	}

	public static List<bool> GetSafeBoolList(this Dictionary<string, object> dictionary, string key) {
        if (!dictionary.ContainsKey(key))
            return new List<bool>();

        return ((List<object>)dictionary[key]).Select(s => (bool)s).ToList();
	}

    public static T GetSafeEnumValue<T>(this Dictionary<string, object> dictionary, string key) {
        return (T)Enum.Parse(typeof(T), dictionary[key].ToString());
    }

    public static T GetSafeEnumValue<T>(this Dictionary<int, object> dictionary, int key) {
        return (T)Enum.Parse(typeof(T), dictionary[key].ToString());
    }

    public static Dictionary<string, List<bool>> GetSafeDictStringListBool(this Dictionary<string, object> dictionary, string key) {
		if (!dictionary.ContainsKey(key))
			return new Dictionary<string, List<bool>>();

		var dictStringListBool = dictionary[key];
		var dict = dictStringListBool.ToDictionaryString<object>();
		Dictionary<string, List<bool>> result = new Dictionary<string, List<bool>>();
		foreach (var keyValuePair in dict) {
			List<bool> listBool = ((List<object>)dict[keyValuePair.Key]).Select(s => (bool)s).ToList();
			result.Add(keyValuePair.Key, listBool);
		}
		return result;
	}

	public static Dictionary<string, List<string>> GetSafeDictStringListString(this Dictionary<string, object> dictionary, string key) {
		if (!dictionary.ContainsKey(key))
			return new Dictionary<string, List<string>>();
		var dictStringListStrings = dictionary[key];
		var dict = dictStringListStrings.ToDictionaryString<object>();
		Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
		foreach (var keyValuePair in dict) {
			List<string> listString = ((List<object>)dict[keyValuePair.Key]).Select(s => (string)s).ToList();
			result.Add(keyValuePair.Key, listString);
		}
		return result;
	}

    public static Dictionary<string, List<bool>> GetSafeStringListBoolDict(this Dictionary<string, object> dictionary, string key) {
		if (!dictionary.ContainsKey(key))
			return new Dictionary<string, List<bool>>();
		var dictStringListBool = dictionary[key];
        var dict = dictStringListBool.ToDictionaryString<object>();
        Dictionary<string, List<bool>> result = new Dictionary<string, List<bool>>();
        foreach (var keyValuePair in dict) {
            List<bool> listBool = ((List<object>)dict[keyValuePair.Key]).Select(s => (bool)s).ToList();
            result.Add(keyValuePair.Key, listBool);
        }
        return result;
	}

	public static Dictionary<int, List<int>> GetSafeDictIntListInt(this Dictionary<string, object> dictionary, string key) {
		if (!dictionary.ContainsKey(key))
			return new Dictionary<int, List<int>>();
		var dictStringListStrings = dictionary[key];
		var dict = dictStringListStrings.ToDictionaryString<object>();
		Dictionary<int, List<int>> result = new Dictionary<int, List<int>>();
		foreach (var keyValuePair in dict) {
			List<int> list = ((List<object>)dict[keyValuePair.Key]).Select(s => Convert.ToInt32(s)).ToList();
			result.Add(Convert.ToInt32(keyValuePair.Key), list);
		}
		return result;
	}

	public static Dictionary<int, List<string>> GetSafeDictIntListString(this Dictionary<string, object> dictionary, string key) {
		if (!dictionary.ContainsKey(key))
			return new Dictionary<int, List<string>>();
		var dictStringListStrings = dictionary[key];
		var dict = dictStringListStrings.ToDictionaryString<object>();
		Dictionary<int, List<string>> result = new Dictionary<int, List<string>>();
		foreach (var keyValuePair in dict) {
			List<string> listString = ((List<object>)dict[keyValuePair.Key]).Select(s => (string)s).ToList();
			result.Add(Convert.ToInt32(keyValuePair.Key), listString);
		}
		return result;
	}

	public static Dictionary<int, bool> GetSafeIntBoolDict(this Dictionary<string, object> dictionary, string key) {
		if (!dictionary.ContainsKey(key))
			return new Dictionary<int, bool>();
		var objT = dictionary[key];
		var dict = objT.ToDictionaryString<object>();
		Dictionary<int, bool> result = new Dictionary<int, bool>();
		foreach (var keyValuePair in dict)
			result.Add(Convert.ToInt32(keyValuePair.Key), Convert.ToBoolean(keyValuePair.Value));
		return result;
	}

	public static Dictionary<int, int> GetSafeIntIntDict(this Dictionary<string, object> dictionary, string key) {
		if(!dictionary.ContainsKey(key))
			return new Dictionary<int, int>();
		var objT = dictionary[key];
		var dict = objT.ToDictionaryString<object>();
		Dictionary<int, int> result = new Dictionary<int, int>();
		foreach (var keyValuePair in dict)
			result.Add(Convert.ToInt32(keyValuePair.Key), Convert.ToInt32(keyValuePair.Value));
		return result;
	}

	public static Dictionary<int, string> GetSafeIntStringDict(this Dictionary<string, object> dictionary, string key) {
		if (!dictionary.ContainsKey(key))
			return new Dictionary<int, string>();
		var objT = dictionary[key];
		var dict = objT.ToDictionaryString<object>();
		Dictionary<int, string> result = new Dictionary<int, string>();
		foreach (var keyValuePair in dict)
			result.Add(Convert.ToInt32(keyValuePair.Key), Convert.ToString(keyValuePair.Value));
		return result;
	}

	//public static Dictionary<int, T> GetSafeIntEnumDict<T>(this Dictionary<string, object> dictionary, string key) {
	//	if (!dictionary.ContainsKey(key))
	//		return new Dictionary<int, T>();
	//	var objT = dictionary[key];
	//	var dict = objT.ToDictionaryString<object>();
	//	Dictionary<int, T> result = new Dictionary<int, T>();
	//	foreach (var keyValuePair in dict) {
	//		T enumValue = dict.GetSafeEnumValue<T>(keyValuePair.Key);
	//		result.Add(Convert.ToInt32(keyValuePair.Key), enumValue);
	//	}
	//	return result;
	//}

	public static Dictionary<string, bool> GetSafeStringBoolDict(this Dictionary<string, object> dictionary, string key) {
		if (!dictionary.ContainsKey(key))
			return new Dictionary<string, bool>();
		var obj = dictionary[key];
		var dict = obj.ToDictionaryString<object>();
		Dictionary<string, bool> result = new Dictionary<string, bool>();
		foreach (var keyValuePair in dict) 
			result.Add(keyValuePair.Key, Convert.ToBoolean(keyValuePair.Value));
		return result;
	}

	public static Dictionary<string, int> GetSafeIntDictionary(this Dictionary<string, object> dictionary, string key) {
		var castedDictionary = new Dictionary<string, int>();

		if (!dictionary.ContainsKey(key)) {
			return castedDictionary;
		}

		var childDictionary = dictionary[key] as Dictionary<string, object>;
		if (childDictionary == null) {
			return castedDictionary;
		}

		foreach (var pair in childDictionary) {
			castedDictionary[pair.Key] = Convert.ToInt32(pair.Value);
		}

		return castedDictionary;
	}

	public static Dictionary<string, TValue> ToDictionaryString<TValue>(this object obj) {
		//var json = Json.Serialize(obj);
		//var dictionary = Json.Deserialize(json) as Dictionary<string, TValue>;
		var json = JsonConvert.SerializeObject(obj);
		var dictionary = JsonConvert.DeserializeObject(json) as Dictionary<string, TValue>;

		return dictionary;
	}

	public static Dictionary<int, TValue> ToDictionaryInt<TValue>(this object obj) {
		//var json = Json.Serialize(obj);
		//var dictionary = Json.Deserialize(json) as Dictionary<int, TValue>;
		var json = JsonConvert.SerializeObject(obj);
		var dictionary = JsonConvert.DeserializeObject(json) as Dictionary<int, TValue>;

		return dictionary;
	}
}
