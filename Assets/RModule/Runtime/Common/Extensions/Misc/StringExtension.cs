using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StringExtension {
    public static Color ToColor(this string color) {
        if (color.StartsWith("#", StringComparison.InvariantCulture)) {
            color = color.Substring(1); // strip #
        }

        if (color.Length == 6) {
            color += "FF"; // add alpha if missing
        }

        var hex = Convert.ToUInt32(color, 16);
        var r = ((hex & 0xff000000) >> 0x18) / 255f;
        var g = ((hex & 0xff0000) >> 0x10) / 255f;
        var b = ((hex & 0xff00) >> 8) / 255f;
        var a = ((hex & 0xff)) / 255f;

        return new Color(r, g, b, a);
    }

    public static string ApplyColorToString(this string sourceString, Color color) {
        return $"<color=#{color.ToHexString()}>{sourceString}</color>";
    }

    public static string ApplyBoldToString(this string sourceString) {
        return $"<b>{sourceString}<b></b></b>";
    }

    public static int DamerauLevenshteinDistanceTo(this string @string, string targetString) {
        return DamerauLevenshteinDistance(@string, targetString);
    }

    public static int DamerauLevenshteinDistance(string string1, string string2) {
        if (string.IsNullOrEmpty(string1)) {
            if (!string.IsNullOrEmpty(string2))
                return string2.Length;
            return 0;
        }
        if (string.IsNullOrEmpty(string2)) {
            if (!string.IsNullOrEmpty(string1))
                return string1.Length;
            return 0;
        }

        int length1 = string1.Length;
        int length2 = string2.Length;
        int[,] d = new int[length1 + 1, length2 + 1];
        int cost, del, ins, sub;
        for (int i = 0; i <= d.GetUpperBound(0); i++)
            d[i, 0] = i;

        for (int i = 0; i <= d.GetUpperBound(1); i++)
            d[0, i] = i;

        for (int i = 1; i <= d.GetUpperBound(0); i++) {
            for (int j = 1; j <= d.GetUpperBound(1); j++) {
                if (string1[i - 1] == string2[j - 1])
                    cost = 0;
                else
                    cost = 1;
                del = d[i - 1, j] + 1;
                ins = d[i, j - 1] + 1;
                sub = d[i - 1, j - 1] + cost;
                d[i, j] = Math.Min(del, Math.Min(ins, sub));
                if (i > 1 && j > 1 && string1[i - 1] == string2[j - 2] && string1[i - 2] == string2[j - 1])
                    d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
            }
        }

        return d[d.GetUpperBound(0), d.GetUpperBound(1)];
    }

    public static string GetStringBetweenCharacters(string input, char charFrom, char charTo) {
        int posFrom = input.IndexOf(charFrom);
        if (posFrom != -1) //if found char
        {
            int posTo = input.IndexOf(charTo, posFrom + 1);
            if (posTo != -1) //if found char
            {
                return input.Substring(posFrom + 1, posTo - posFrom - 1);
            }
        }

        return string.Empty;
    }

    public static List<int> AllIndexesOf(this string str, string value) {
        if (String.IsNullOrEmpty(value))
            throw new ArgumentException("the string to find may not be empty", "value");
        List<int> indexes = new List<int>();
        for (int index = 0; ; index += value.Length) {
            index = str.IndexOf(value, index);
            if (index == -1)
                return indexes;
            indexes.Add(index);
        }
    }

    public static string ApplyColorToTextBetweenTwoCharacters(string source, char charFrom, char charTo, Color color) {
        if ((source.AllIndexesOf(charFrom.ToString()).Count != source.AllIndexesOf(charTo.ToString()).Count)
            || (source.AllIndexesOf(charFrom.ToString()).Count == 0 && source.AllIndexesOf(charTo.ToString()).Count == 0))
            return source;

        var subs = source.Split(charTo);
        var result = "";
        foreach (string sub in subs) {
            if (sub.Contains(charFrom)) {
                int indexStart = sub.IndexOf(charFrom);
                var str = sub.Remove(indexStart, 1);
                result += str.Remove(indexStart, str.Length - indexStart) + str.Substring(indexStart).ApplyColorToString(color);
            } else {
                result += sub;
            }
        }

        return result;
    }

    public static string TryReplaceNum(this string str, float num, string stringBeforeNum) {
        var foundedIndexes = str.AllIndexesOf(stringBeforeNum);
        if (foundedIndexes.Count > 0) {
            str = str.Remove(foundedIndexes[0]);
        }
        str += $"{stringBeforeNum}{num}";

        return str;
    }

    public static List<string> SplitToCharsList(this List<string> strings) {
        List<string> charsList = new();
        foreach (string str in strings)
            foreach (char c in str)
                charsList.Add(c.ToString());

        return charsList;
    }

    public static List<string> SplitToCharsList(this string str) {
        List<string> charsList = new();
        foreach (char c in str)
            charsList.Add(c.ToString());

        return charsList;
    }
}
