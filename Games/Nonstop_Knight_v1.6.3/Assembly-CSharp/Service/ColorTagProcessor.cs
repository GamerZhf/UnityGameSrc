namespace Service
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class ColorTagProcessor : IRTLProcessor
    {
        private readonly Dictionary<string, string> buffer = new Dictionary<string, string>();
        private const string ENCLOSED_IN_COLORTAGS = "<[a-z]*=#[a-zA-Z0-9]*>(.*?)</[a-z]*>";
        private readonly Regex enclosedInColorTags = new Regex("<[a-z]*=#[a-zA-Z0-9]*>(.*?)</[a-z]*>");
        private readonly List<string> tmpDelete = new List<string>();

        public bool OnEnd(ref string finalString)
        {
            this.tmpDelete.Clear();
            foreach (KeyValuePair<string, string> pair in this.buffer)
            {
                if (finalString.Contains(pair.Key))
                {
                    finalString.Replace(pair.Key, pair.Value);
                    this.tmpDelete.Add(pair.Key);
                }
            }
            foreach (string str in this.tmpDelete)
            {
                this.buffer.Remove(str);
            }
            return true;
        }

        public bool OnStart(ref string originalInput)
        {
            Match match = this.enclosedInColorTags.Match(originalInput);
            while (match.Success)
            {
                if (match.Groups.Count >= 2)
                {
                    string str = match.Groups[0].ToString();
                    string key = match.Groups[1].ToString();
                    this.buffer.Add(key, str);
                    originalInput = originalInput.Replace(str, key);
                    match = match.NextMatch();
                }
            }
            return true;
        }
    }
}

