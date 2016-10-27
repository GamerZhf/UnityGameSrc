using Loca;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class LocaSystem
{
    [CompilerGenerated]
    private string <DisplayLanguage>k__BackingField;
    [CompilerGenerated]
    private bool <Initialized>k__BackingField;
    [CompilerGenerated]
    private SystemLanguage <selectedLanguage>k__BackingField;
    private Dictionary<string, string> m_dict;
    private static Dictionary<SystemLanguage, string> m_languageMap;

    static LocaSystem()
    {
        Dictionary<SystemLanguage, string> dictionary = new Dictionary<SystemLanguage, string>();
        dictionary.Add(SystemLanguage.German, "de");
        dictionary.Add(SystemLanguage.English, "en");
        dictionary.Add(SystemLanguage.Spanish, "es-ES");
        dictionary.Add(SystemLanguage.French, "fr");
        dictionary.Add(SystemLanguage.Italian, "it");
        dictionary.Add(SystemLanguage.Japanese, "ja");
        dictionary.Add(SystemLanguage.Korean, "ko");
        dictionary.Add(SystemLanguage.Portuguese, "pt-BR");
        dictionary.Add(SystemLanguage.Russian, "ru");
        dictionary.Add(SystemLanguage.Turkish, "tr");
        dictionary.Add(SystemLanguage.ChineseTraditional, "zh-TW");
        dictionary.Add(SystemLanguage.ChineseSimplified, "zh-CN");
        dictionary.Add(SystemLanguage.Chinese, "zh-CN");
        dictionary.Add(SystemLanguage.Indonesian, "id");
        dictionary.Add(SystemLanguage.Vietnamese, "vi");
        dictionary.Add(SystemLanguage.Arabic, "ar");
        m_languageMap = dictionary;
    }

    public LocaSystem()
    {
        this.selectedLanguage = SystemLanguage.Unknown;
        this.Initialize(SystemLanguage.Unknown);
    }

    public string GetLocaKey()
    {
        string str = null;
        using (AndroidJavaClass class2 = new AndroidJavaClass("java.util.Locale"))
        {
            using (AndroidJavaObject obj2 = class2.CallStatic<AndroidJavaObject>("getDefault", new object[0]))
            {
                str = obj2.Call<string>("getLanguage", new object[0]);
            }
        }
        switch (str)
        {
            case null:
                return "en";

            case "in":
                str = "id";
                break;
        }
        return str;
    }

    private string GetLocaKeyForSystemLanguage(SystemLanguage language)
    {
        if (m_languageMap.ContainsKey(language))
        {
            return m_languageMap[language];
        }
        return "en";
    }

    public static List<string> GetUniqueLanguageCodes()
    {
        List<string> list = new List<string>();
        foreach (string str in m_languageMap.Values)
        {
            if (!list.Contains(str))
            {
                list.Add(str);
            }
        }
        return list;
    }

    public void Initialize([Optional, DefaultParameterValue(0x2a)] SystemLanguage language)
    {
        this.SelectLoca(language);
        _.SetTranslations(new LocaDict(this.m_dict, this.IsRightToLeft(this.selectedLanguage)));
        this.Initialized = true;
    }

    public bool IsArabic(string input)
    {
        return Regex.IsMatch(input, @"\p{IsArabic}+");
    }

    public bool IsRightToLeft(SystemLanguage language)
    {
        return (SystemLanguage.Arabic == language);
    }

    private bool LoadLoca(string lang)
    {
        TextAsset asset = Resources.Load("Loca/" + lang) as TextAsset;
        if (((asset != null) && (asset.bytes != null)) && (asset.bytes.Length != 0))
        {
            string json = Encoding.UTF8.GetString(asset.bytes);
            try
            {
                this.m_dict = JsonUtils.Deserialize<Dictionary<string, string>>(json, true);
                this.DisplayLanguage = lang;
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }
        return false;
    }

    private void SelectLoca(SystemLanguage language)
    {
        string locaKey = this.GetLocaKey();
        if (!string.IsNullOrEmpty(locaKey) && locaKey.Contains("zh-Hant"))
        {
            locaKey = "zh-TW";
        }
        if (((language == SystemLanguage.Unknown) && !string.IsNullOrEmpty(locaKey)) && this.LoadLoca(locaKey))
        {
            this.selectedLanguage = Application.systemLanguage;
        }
        else
        {
            if (language == SystemLanguage.Unknown)
            {
                language = Application.systemLanguage;
            }
            if (!m_languageMap.TryGetValue(language, out locaKey))
            {
                locaKey = "en";
                this.selectedLanguage = SystemLanguage.English;
                Debug.Log("no locamapping for " + language);
            }
            else
            {
                this.selectedLanguage = language;
            }
            if (!this.LoadLoca(locaKey))
            {
                this.selectedLanguage = SystemLanguage.English;
                Debug.LogError("loca load failed for " + language);
                if (!this.LoadLoca("en"))
                {
                    Debug.LogError("no default loca");
                    this.m_dict = new Dictionary<string, string>();
                }
            }
        }
    }

    public string DisplayLanguage
    {
        [CompilerGenerated]
        get
        {
            return this.<DisplayLanguage>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<DisplayLanguage>k__BackingField = value;
        }
    }

    public bool Initialized
    {
        [CompilerGenerated]
        get
        {
            return this.<Initialized>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<Initialized>k__BackingField = value;
        }
    }

    public SystemLanguage selectedLanguage
    {
        [CompilerGenerated]
        get
        {
            return this.<selectedLanguage>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<selectedLanguage>k__BackingField = value;
        }
    }
}

