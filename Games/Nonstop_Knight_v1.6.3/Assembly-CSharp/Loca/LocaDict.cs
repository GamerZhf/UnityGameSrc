namespace Loca
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class LocaDict
    {
        private char placeholderChar;
        private bool rightToLeft;
        public readonly Dictionary<string, string> Translations;

        public LocaDict()
        {
            this.placeholderChar = '$';
            this.Translations = new Dictionary<string, string>();
        }

        public LocaDict(Dictionary<string, string> _translations, [Optional, DefaultParameterValue(false)] bool _rtl)
        {
            this.placeholderChar = '$';
            this.Translations = _translations;
            this.rightToLeft = _rtl;
        }

        public void AddTranslation(string _phrase, string _context, string _translation)
        {
            this.Translations[GetKey(_phrase, _context)] = _translation;
        }

        public void AddTranslations(LocaDict _dict)
        {
            foreach (KeyValuePair<string, string> pair in _dict.Translations)
            {
                this.Translations.Add(pair.Key, pair.Value);
            }
            this.rightToLeft = _dict.rightToLeft;
        }

        public void ClearTranslations()
        {
            this.Translations.Clear();
        }

        public static string GetKey(string _phrase, string _context)
        {
            return (!string.IsNullOrEmpty(_context) ? (_phrase + "]@[" + _context) : _phrase);
        }

        public string Lookup(string _phrase, [Optional, DefaultParameterValue(null)] object _p, [Optional, DefaultParameterValue(false)] bool _ignoreRTL)
        {
            string key = _phrase;
            if (_p != null)
            {
                PropertyInfo property = _p.GetType().GetProperty("_context");
                string str2 = (property == null) ? null : property.GetValue(_p, null).ToString();
                key = GetKey(_phrase, str2);
            }
            if (this.Translations.ContainsKey(key))
            {
                _phrase = this.Translations[key];
            }
            if (_p != null)
            {
                PropertyInfo info2 = _p.GetType().GetProperty("parameters");
                if (info2 != null)
                {
                    Dictionary<string, string> dictionary = info2.GetValue(_p, null) as Dictionary<string, string>;
                    if (dictionary != null)
                    {
                        foreach (KeyValuePair<string, string> pair in dictionary)
                        {
                            string introduced11;
                            _phrase = _phrase.Replace(introduced11, !_ignoreRTL ? this.ProcessRightToLeft(pair.Value) : pair.Value);
                        }
                    }
                }
                foreach (PropertyInfo info3 in _p.GetType().GetProperties())
                {
                    if (!"parameters".Equals(info3.Name) && !"_context".Equals(info3.Name))
                    {
                        _phrase = _phrase.Replace(this.placeholderChar + info3.Name + this.placeholderChar, info3.GetValue(_p, null).ToString());
                    }
                }
            }
            return (!_ignoreRTL ? this.ProcessRightToLeft(_phrase) : _phrase);
        }

        private string ProcessRightToLeft(string _phrase)
        {
            if (!this.rightToLeft)
            {
                return _phrase;
            }
            return _.ApplyArabicReverse(_phrase);
        }
    }
}

