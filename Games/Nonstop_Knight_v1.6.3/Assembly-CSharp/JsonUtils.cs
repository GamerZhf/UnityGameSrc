using Pathfinding.Serialization.JsonFx;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static class JsonUtils
{
    public static JsonReaderSettings DefaultReaderSettings;
    public static JsonWriterSettings DefaultWriterSettings;

    [CompilerGenerated]
    private static bool <DeserializeField`1>m__180<T>(char x)
    {
        return (x == '{');
    }

    [CompilerGenerated]
    private static bool <DeserializeField`1>m__181<T>(char x)
    {
        return (x == '}');
    }

    public static JsonReader CreateCustomJsonReader(string json, bool allowNullValueTypes)
    {
        if (DefaultReaderSettings == null)
        {
            DefaultReaderSettings = new JsonReaderSettings();
            DefaultReaderSettings.AddTypeConverter(new Vector3JsonConverter());
            DefaultReaderSettings.AddTypeConverter(new Vector2JsonConverter());
            DefaultReaderSettings.AddTypeConverter(new QuaternionJsonConverter());
            DefaultReaderSettings.AddTypeConverter(new ColorJsonConverter());
            DefaultReaderSettings.AddTypeConverter(new ChestTypeIntDictConverter());
            DefaultReaderSettings.AddTypeConverter(new ChestTypeLootTableDictConverter());
            DefaultReaderSettings.AddTypeConverter(new IntIntDictKeyConverter());
            DefaultReaderSettings.AddTypeConverter(new IntDoubleDictKeyConverter());
            DefaultReaderSettings.AddTypeConverter(new StringTournamentEntryListDictConverter());
            DefaultReaderSettings.AddTypeConverter(new ItemTypeStringDictConverter());
            DefaultReaderSettings.AllowNullValueTypes = allowNullValueTypes;
        }
        return new JsonReader(json, DefaultReaderSettings);
    }

    public static JsonWriter CreateCustomJsonWriter(StringBuilder result)
    {
        if (DefaultWriterSettings == null)
        {
            DefaultWriterSettings = JsonDataWriter.CreateSettings(false);
            DefaultWriterSettings.AddTypeConverter(new Vector3JsonConverter());
            DefaultWriterSettings.AddTypeConverter(new Vector2JsonConverter());
            DefaultWriterSettings.AddTypeConverter(new QuaternionJsonConverter());
            DefaultWriterSettings.AddTypeConverter(new ColorJsonConverter());
        }
        return new JsonWriter(result, DefaultWriterSettings);
    }

    public static T Deserialize<T>(string json, [Optional, DefaultParameterValue(true)] bool allowNullValueTypes)
    {
        if (json == null)
        {
            throw new NullReferenceException("JSON string cannot be null.");
        }
        return (T) CreateCustomJsonReader(json, allowNullValueTypes).Deserialize(typeof(T));
    }

    public static T Deserialize<T>(string json, int startIdx, [Optional, DefaultParameterValue(true)] bool allowNullValueTypes)
    {
        return (T) CreateCustomJsonReader(json, allowNullValueTypes).Deserialize(startIdx, typeof(T));
    }

    public static T DeserializeField<T>(string json, string key)
    {
        string str = "\"" + key + "\":";
        int index = json.IndexOf(str);
        if (index == -1)
        {
            Debug.LogError("Top-level key not found from JSON: " + key);
            return default(T);
        }
        if (index != json.LastIndexOf(str))
        {
            while (index != -1)
            {
                string source = json.Substring(0, index);
                int num3 = Enumerable.Count<char>(source, new Func<char, bool>(JsonUtils.<DeserializeField`1>m__180<T>));
                int num4 = Enumerable.Count<char>(source, new Func<char, bool>(JsonUtils.<DeserializeField`1>m__181<T>));
                int num5 = num3 - num4;
                if (num5 == 1)
                {
                    break;
                }
                if ((index + 1) < json.Length)
                {
                    index = json.IndexOf(str, (int) (index + 1));
                }
                else
                {
                    Debug.LogError("Top-level key not found from JSON: " + key);
                    return default(T);
                }
            }
        }
        int startIdx = index + str.Length;
        return Deserialize<T>(json, startIdx, true);
    }

    public static void Populate(ref object targetObject, string json, [Optional, DefaultParameterValue(true)] bool allowNullValueTypes)
    {
        CreateCustomJsonReader(json, allowNullValueTypes).PopulateObject(ref targetObject);
    }

    public static string Serialize(object dataObject)
    {
        StringBuilder result = new StringBuilder();
        CreateCustomJsonWriter(result).Write(dataObject);
        return result.ToString();
    }

    public static string Serialize(object dataObject, JsonWriter jsonWriter)
    {
        jsonWriter.Write(dataObject);
        return jsonWriter.ToString();
    }
}

