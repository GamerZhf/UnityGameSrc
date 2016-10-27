using ArabicSupport;
using Loca;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public class _
{
    [CompilerGenerated]
    private static MatchEvaluator <>f__am$cache2;
    public static string DebugPrefix = string.Empty;
    private static LocaDict dict = new LocaDict(new Dictionary<string, string>(), false);

    public static string ApplyArabicReverse(string _phrase)
    {
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = delegate (Match _match) {
                if (_match.Value == "<")
                {
                    return ">";
                }
                if (_match.Value == ">")
                {
                    return "<";
                }
                if (_match.Value == "(")
                {
                    return ")";
                }
                if (_match.Value == ")")
                {
                    return "(";
                }
                return _match.Value;
            };
        }
        return ArabicFixer.Fix(Regex.Replace(_phrase, @"\<|\>|\(|\)", <>f__am$cache2), true, false);
    }

    public static string K(string _str)
    {
        return _str;
    }

    public static string KT(string _key, string _str)
    {
        if (!dict.Translations.ContainsKey(_key))
        {
            dict.Translations[_key] = DebugPrefix + _str;
        }
        return _key;
    }

    public static string L(string _str, [Optional, DefaultParameterValue(null)] object _p, [Optional, DefaultParameterValue(false)] bool _ignoreRTL)
    {
        return (DebugPrefix + dict.Lookup(_str, _p, _ignoreRTL));
    }

    public static void SetDebugMode(bool _isDebug)
    {
        if (_isDebug)
        {
            DebugPrefix = "!!";
        }
        else
        {
            DebugPrefix = string.Empty;
        }
    }

    public static void SetTranslations(LocaDict _dict)
    {
        dict.ClearTranslations();
        dict.AddTranslations(_dict);
    }

    public static string T(string _str, [Optional, DefaultParameterValue(null)] object _p)
    {
        return (DebugPrefix + dict.Lookup(_str, _p, false));
    }
}

