using App;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

[Extension]
public static class StringExtensions
{
    [Extension]
    public static string ToLowerLoca(string self)
    {
        if (Binder.LocaSystem.selectedLanguage == SystemLanguage.Turkish)
        {
            return self.ToLower(new CultureInfo("tr-TR", false));
        }
        return self.ToLower();
    }

    [Extension]
    public static string ToUpperLoca(string self)
    {
        if (Binder.LocaSystem.selectedLanguage == SystemLanguage.Turkish)
        {
            return self.ToUpper(new CultureInfo("tr-TR", false));
        }
        return self.ToUpper();
    }
}

