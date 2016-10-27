using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class CsvUtils
{
    private static List<string> MatchList = new List<string>();

    public static string[,] Deserialize(string csvText)
    {
        char[] separator = new char[] { "\n"[0] };
        string[] strArray = csvText.Split(separator);
        int a = 0;
        for (int i = 0; i < strArray.Length; i++)
        {
            List<string> list = SplitCsvLine(strArray[i]);
            a = Mathf.Max(a, list.Count);
        }
        string[,] strArray2 = new string[a + 1, strArray.Length + 1];
        for (int j = 0; j < strArray.Length; j++)
        {
            List<string> list2 = SplitCsvLine(strArray[j]);
            for (int k = 0; k < list2.Count; k++)
            {
                strArray2[k, j] = list2[k];
                strArray2[k, j] = strArray2[k, j].Replace("\"\"", "\"");
            }
        }
        return strArray2;
    }

    public static List<string> SplitCsvLine(string line)
    {
        MatchList.Clear();
        MatchCollection matchs = Regex.Matches(line, "(((?<x>(?=[,\\r\\n]+))|\"(?<x>([^\"]|\"\")+)\"|(?<x>[^,\\r\\n]+)),?)", RegexOptions.ExplicitCapture);
        for (int i = 0; i < matchs.Count; i++)
        {
            Match match = matchs[i];
            MatchList.Add(match.Groups[1].Value);
        }
        return MatchList;
    }
}

