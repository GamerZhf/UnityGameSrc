using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static class LangUtil
{
    public static void AddOrUpdateDictionaryEntry<K, V>(Dictionary<K, V> dictionary, K key, V value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void CombineDictionaries<K, V>(Dictionary<K, V> master, Dictionary<K, V> slave, bool skipDuplicates)
    {
        foreach (KeyValuePair<K, V> pair in slave)
        {
            if (!skipDuplicates || !master.ContainsKey(pair.Key))
            {
                master.Add(pair.Key, pair.Value);
            }
        }
    }

    public static string DictionaryContentToString<K, V>(Dictionary<K, V> dict, [Optional, DefaultParameterValue(true)] bool newlineSeparator)
    {
        string str = string.Empty;
        foreach (KeyValuePair<K, V> pair in dict)
        {
            string str2 = str;
            object[] objArray1 = new object[] { str2, pair.Key, " : ", pair.Value };
            str = string.Concat(objArray1);
            if (newlineSeparator)
            {
                str = str + "\n";
            }
            else
            {
                str = str + ", ";
            }
        }
        return str;
    }

    public static string FirstLetterToUpper(string str)
    {
        if (str == null)
        {
            return null;
        }
        if (str.Length > 1)
        {
            return (char.ToUpper(str[0]) + str.Substring(1));
        }
        return str.ToUpper();
    }

    public static int GetCharSum(string str)
    {
        int num = 0;
        for (int i = 0; i < str.Length; i++)
        {
            char ch = str[i];
            num += ch;
        }
        return num;
    }

    public static List<T> GetEnumValues<T>()
    {
        List<T> list = new List<T>();
        IEnumerator enumerator = Enum.GetValues(typeof(T)).GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                T current = (T) enumerator.Current;
                list.Add(current);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        return list;
    }

    public static List<T> GetEnumValuesWithException<T>(T except)
    {
        List<T> list = new List<T>();
        IEnumerator enumerator = Enum.GetValues(typeof(T)).GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                T current = (T) enumerator.Current;
                if (!current.Equals(except))
                {
                    list.Add(current);
                }
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        return list;
    }

    public static K GetFirstKeyFromDict<K, V>(Dictionary<K, V> dict)
    {
        foreach (KeyValuePair<K, V> pair in dict)
        {
            return pair.Key;
        }
        return default(K);
    }

    public static V GetFirstValueFromDict<K, V>(Dictionary<K, V> dict)
    {
        foreach (KeyValuePair<K, V> pair in dict)
        {
            return pair.Value;
        }
        return default(V);
    }

    public static T GetKeyFromDictionaryWithProbability<T>(Dictionary<T, float> probabilityDict)
    {
        float num = 0f;
        foreach (float num2 in probabilityDict.Values)
        {
            num += num2;
        }
        if ((Mathf.Round(num * 100f) / 100f) != 1f)
        {
            Debug.LogError("Probabilies don't sum up to 1.");
            return default(T);
        }
        T local = default(T);
        float num3 = UnityEngine.Random.Range((float) 0f, (float) 1f);
        float num4 = 0f;
        foreach (KeyValuePair<T, float> pair in probabilityDict)
        {
            float num5 = pair.Value;
            num4 += num5;
            if (num3 <= num4)
            {
                return pair.Key;
            }
        }
        return local;
    }

    public static T GetKeyFromDictionaryWithWeights<T>(Dictionary<T, int> weightedDict, [Optional, DefaultParameterValue(null)] uint? random_value)
    {
        int max = 0;
        foreach (int num2 in weightedDict.Values)
        {
            max += num2;
        }
        int num3 = random_value.HasValue ? MathUtil.RandomFromRange(0, max, random_value.Value) : UnityEngine.Random.Range(0, max + 1);
        int num4 = 0;
        foreach (KeyValuePair<T, int> pair in weightedDict)
        {
            num4 += pair.Value;
            if (num3 <= num4)
            {
                return pair.Key;
            }
        }
        return default(T);
    }

    public static T GetKeyWithHighestDoubleValueFromDictionary<T>(Dictionary<T, double> dict)
    {
        T key = default(T);
        double minValue = double.MinValue;
        foreach (KeyValuePair<T, double> pair in dict)
        {
            if (pair.Value > minValue)
            {
                key = pair.Key;
                minValue = pair.Value;
            }
        }
        return key;
    }

    public static T GetKeyWithHighestIntValueFromDictionary<T>(Dictionary<T, int> dict)
    {
        T key = default(T);
        int num = -2147483648;
        foreach (KeyValuePair<T, int> pair in dict)
        {
            if (pair.Value > num)
            {
                key = pair.Key;
                num = pair.Value;
            }
        }
        return key;
    }

    public static T GetNextCycledValueFromEnum<T>(T currentValue)
    {
        Array values = Enum.GetValues(typeof(T));
        int index = Array.IndexOf(values, currentValue) + 1;
        if (index >= values.Length)
        {
            index = 0;
        }
        return (T) values.GetValue(index);
    }

    public static T GetPreviousCycledValueFromEnum<T>(T currentValue)
    {
        Array values = Enum.GetValues(typeof(T));
        int index = Array.IndexOf(values, currentValue) - 1;
        if (index < 0)
        {
            index = values.Length - 1;
        }
        return (T) values.GetValue(index);
    }

    public static K GetRandomKeyFromDict<K, V>(Dictionary<K, V> dict)
    {
        int num = UnityEngine.Random.Range(0, dict.Count);
        int num2 = 0;
        foreach (KeyValuePair<K, V> pair in dict)
        {
            if (num2 == num)
            {
                return pair.Key;
            }
            num2++;
        }
        return default(K);
    }

    public static T GetRandomValueFromArray<T>(T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static V GetRandomValueFromDict<K, V>(Dictionary<K, V> dict)
    {
        int num = UnityEngine.Random.Range(0, dict.Count);
        int num2 = 0;
        foreach (KeyValuePair<K, V> pair in dict)
        {
            if (num2 == num)
            {
                return pair.Value;
            }
            num2++;
        }
        return default(V);
    }

    public static T GetRandomValueFromEnum<T>()
    {
        Array values = Enum.GetValues(typeof(T));
        return (T) values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    public static T GetRandomValueFromList<T>(List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static List<T> HashSetToList<T>(HashSet<T> hashSet)
    {
        List<T> list = new List<T>();
        foreach (T local in hashSet)
        {
            list.Add(local);
        }
        return list;
    }

    public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
    {
        Dictionary<K, V> dictionary = new Dictionary<K, V>();
        IEnumerator enumerator = table.Keys.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                object current = enumerator.Current;
                dictionary.Add((K) current, (V) table[current]);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        return dictionary;
    }

    public static bool ListContentEqualsIgnoreOrder<T>(List<T> listA, List<T> listB)
    {
        if (listA.Count != listB.Count)
        {
            return false;
        }
        for (int i = 0; i < listA.Count; i++)
        {
            bool flag = false;
            for (int j = 0; j < listB.Count; j++)
            {
                T local = listA[i];
                if (local.Equals(listB[j]))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                return false;
            }
        }
        return true;
    }

    public static List<K> MergeArrays<K>(params K[][] arrays)
    {
        List<K> list = new List<K>();
        for (int i = 0; i < arrays.Length; i++)
        {
            list.AddRange(arrays[i]);
        }
        return list;
    }

    public static void PrintDictionaryWithWeights<T>(Dictionary<T, int> weightedDict)
    {
        StringBuilder builder = new StringBuilder();
        foreach (KeyValuePair<T, int> pair in weightedDict)
        {
            builder.Append(string.Concat(new object[] { pair.Key, ":", pair.Value, "  " }));
        }
        Debug.Log(builder.ToString());
    }

    public static void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count; i > 1; i--)
        {
            int num2 = UnityEngine.Random.Range(0, i);
            T local = list[num2];
            list[num2] = list[i - 1];
            list[i - 1] = local;
        }
    }

    public static T[] Shuffle<T>(T[] array)
    {
        for (int i = array.Length; i > 1; i--)
        {
            int index = UnityEngine.Random.Range(0, i);
            T local = array[index];
            array[index] = array[i - 1];
            array[i - 1] = local;
        }
        return array;
    }

    public static void Shuffle<T>(ref T[] array)
    {
        for (int i = array.Length; i > 1; i--)
        {
            int index = UnityEngine.Random.Range(0, i);
            T local = array[index];
            array[index] = array[i - 1];
            array[i - 1] = local;
        }
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T local = lhs;
        lhs = rhs;
        rhs = local;
    }

    public static float TryGetFloatValueFromDictionary<T>(Dictionary<T, float> dict, T key)
    {
        if (!dict.ContainsKey(key))
        {
            return 0f;
        }
        return dict[key];
    }

    public static int TryGetIntValueFromDictionary<T>(Dictionary<T, int> dict, T key)
    {
        if (!dict.ContainsKey(key))
        {
            return 0;
        }
        return dict[key];
    }

    public static T TryParseEnum<T>(string value)
    {
        try
        {
            return (T) Enum.Parse(typeof(T), value);
        }
        catch
        {
            return default(T);
        }
    }
}

