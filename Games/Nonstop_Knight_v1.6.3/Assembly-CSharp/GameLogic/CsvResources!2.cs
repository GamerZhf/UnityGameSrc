namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public abstract class CsvResources<K, T>
    {
        protected Dictionary<K, T> m_resources;

        protected CsvResources()
        {
            this.m_resources = new Dictionary<K, T>();
        }

        protected void addResource(K id, T res)
        {
            this.m_resources.Add(id, res);
        }

        public bool containsResource(K id)
        {
            return this.m_resources.ContainsKey(id);
        }

        public int getNumResources()
        {
            return this.m_resources.Count;
        }

        public T getResource(K id)
        {
            return this.m_resources[id];
        }

        protected bool parseBool(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return bool.Parse(str);
        }

        protected L parseClass<L>(string str) where L: class
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            char[] separator = new char[] { ';' };
            string[] strArray = str.Split(separator);
            for (int i = 0; i < strArray.Length; i++)
            {
                char[] chArray2 = new char[] { '=' };
                string[] strArray2 = strArray[i].Split(chArray2);
                if (strArray2.Length != 2)
                {
                    Debug.LogWarning("Invalid key-value pair");
                }
                else
                {
                    string str2 = strArray2[0];
                    string str3 = strArray2[1];
                    builder.Append("'" + str2 + "'");
                    builder.Append(":");
                    if (str3[0] == '[')
                    {
                        builder.Append('[');
                        char[] chArray3 = new char[] { '+' };
                        string[] strArray3 = str3.Substring(1, str3.Length - 2).Split(chArray3);
                        for (int j = 0; j < strArray3.Length; j++)
                        {
                            builder.Append("'" + strArray3[j] + "'");
                            if (j < (strArray3.Length - 1))
                            {
                                builder.Append(",");
                            }
                        }
                        builder.Append(']');
                    }
                    else
                    {
                        builder.Append("'" + str3 + "'");
                    }
                    if (i < (strArray.Length - 1))
                    {
                        builder.Append(",");
                    }
                }
            }
            builder.Append("}");
            return JsonUtils.Deserialize<L>(builder.ToString(), true);
        }

        protected Color parseColorFromHex(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Color.white;
            }
            return ColorUtil.HexToColor(str, 1f);
        }

        protected double parseDouble(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0.0;
            }
            return double.Parse(str);
        }

        protected List<L> parseEnumList<L>(string str)
        {
            List<L> list = new List<L>();
            if (!string.IsNullOrEmpty(str))
            {
                char[] separator = new char[] { ';' };
                string[] strArray = str.Replace(" ", string.Empty).Split(separator);
                for (int i = 0; i < strArray.Length; i++)
                {
                    list.Add((L) Enum.Parse(typeof(L), strArray[i]));
                }
            }
            return list;
        }

        protected L parseEnumType<L>(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default(L);
            }
            return (L) Enum.Parse(typeof(L), str);
        }

        protected float parseFloat(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0f;
            }
            return float.Parse(str);
        }

        protected int parseInt(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            return int.Parse(str);
        }

        protected OrderedDict<L, string> parseOrderedEnumStringDict<L>(string str)
        {
            OrderedDict<L, string> dict = new OrderedDict<L, string>();
            if (!string.IsNullOrEmpty(str))
            {
                char[] separator = new char[] { ';' };
                string[] strArray = str.Replace(" ", string.Empty).Split(separator);
                for (int i = 0; i < strArray.Length; i++)
                {
                    char[] chArray2 = new char[] { '=' };
                    string[] strArray2 = strArray[i].Split(chArray2);
                    if (strArray2.Length > 1)
                    {
                        dict.add((L) Enum.Parse(typeof(L), strArray2[0]), strArray2[1]);
                    }
                    else
                    {
                        dict.add((L) Enum.Parse(typeof(L), strArray2[0]), string.Empty);
                    }
                }
            }
            return dict;
        }

        protected KeyValuePair<string, float> parseStringFloatPair(string str)
        {
            float num;
            if (string.IsNullOrEmpty(str))
            {
                return new KeyValuePair<string, float>();
            }
            char[] separator = new char[] { '=' };
            string[] strArray = str.Split(separator);
            if (strArray.Length > 1)
            {
                float.TryParse(strArray[1], out num);
            }
            else
            {
                num = 0f;
            }
            return new KeyValuePair<string, float>(strArray[0], num);
        }

        protected Dictionary<string, int> parseStringIntDict(string str, [Optional, DefaultParameterValue(1)] int defaultValue)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            List<KeyValuePair<string, int>> list = this.parseStringIntList(str, defaultValue);
            for (int i = 0; i < list.Count; i++)
            {
                KeyValuePair<string, int> pair = list[i];
                KeyValuePair<string, int> pair2 = list[i];
                dictionary.Add(pair.Key, pair2.Value);
            }
            return dictionary;
        }

        protected List<KeyValuePair<string, int>> parseStringIntList(string str, [Optional, DefaultParameterValue(1)] int defaultValue)
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            if (!string.IsNullOrEmpty(str))
            {
                char[] separator = new char[] { ';' };
                string[] strArray = str.Replace(" ", string.Empty).Split(separator);
                for (int i = 0; i < strArray.Length; i++)
                {
                    char[] chArray2 = new char[] { '=' };
                    string[] strArray2 = strArray[i].Split(chArray2);
                    int result = defaultValue;
                    if (strArray2.Length > 1)
                    {
                        int.TryParse(strArray2[1], out result);
                    }
                    list.Add(new KeyValuePair<string, int>(strArray2[0], result));
                }
            }
            return list;
        }

        protected KeyValuePair<string, int> parseStringIntPair(string str)
        {
            int num;
            if (string.IsNullOrEmpty(str))
            {
                return new KeyValuePair<string, int>();
            }
            char[] separator = new char[] { '=' };
            string[] strArray = str.Split(separator);
            if (strArray.Length > 1)
            {
                int.TryParse(strArray[1], out num);
            }
            else
            {
                num = 0;
            }
            return new KeyValuePair<string, int>(strArray[0], num);
        }

        protected List<string> parseStringList(string str)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrEmpty(str))
            {
                string str2 = str.Replace(" ", string.Empty);
                char[] separator = new char[] { ';' };
                list.AddRange(str2.Split(separator));
            }
            return list;
        }
    }
}

