namespace LitJson
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class JsonMapper
    {
        [CompilerGenerated]
        private static WrapperFactory <>f__am$cache10;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache11;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache12;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache13;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache14;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache15;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache16;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache17;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache18;
        [CompilerGenerated]
        private static ExporterFunc <>f__am$cache19;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache1A;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache1B;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache1C;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache1D;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache1E;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache1F;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache20;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache21;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache22;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache23;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache24;
        [CompilerGenerated]
        private static ImporterFunc <>f__am$cache25;
        [CompilerGenerated]
        private static WrapperFactory <>f__am$cache26;
        [CompilerGenerated]
        private static WrapperFactory <>f__am$cache27;
        [CompilerGenerated]
        private static WrapperFactory <>f__am$cache28;
        private static IDictionary<Type, ArrayMetadata> array_metadata = new Dictionary<Type, ArrayMetadata>();
        private static readonly object array_metadata_lock = new object();
        private static IDictionary<Type, ExporterFunc> base_exporters_table = new Dictionary<Type, ExporterFunc>();
        private static IDictionary<Type, IDictionary<Type, ImporterFunc>> base_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
        private static IDictionary<Type, IDictionary<Type, MethodInfo>> conv_ops = new Dictionary<Type, IDictionary<Type, MethodInfo>>();
        private static readonly object conv_ops_lock = new object();
        private static IDictionary<Type, ExporterFunc> custom_exporters_table = new Dictionary<Type, ExporterFunc>();
        private static IDictionary<Type, IDictionary<Type, ImporterFunc>> custom_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
        private static IFormatProvider datetime_format = DateTimeFormatInfo.InvariantInfo;
        private static int max_nesting_depth = 100;
        private static IDictionary<Type, ObjectMetadata> object_metadata = new Dictionary<Type, ObjectMetadata>();
        private static readonly object object_metadata_lock = new object();
        private static JsonWriter static_writer = new JsonWriter();
        private static readonly object static_writer_lock = new object();
        private static IDictionary<Type, IList<PropertyMetadata>> type_properties = new Dictionary<Type, IList<PropertyMetadata>>();
        private static readonly object type_properties_lock = new object();

        static JsonMapper()
        {
            RegisterBaseExporters();
            RegisterBaseImporters();
        }

        private static void AddArrayMetadata(Type type)
        {
            if (!array_metadata.ContainsKey(type))
            {
                ArrayMetadata metadata = new ArrayMetadata();
                metadata.IsArray = type.IsArray;
                if (type.GetInterface("System.Collections.IList") != null)
                {
                    metadata.IsList = true;
                }
                foreach (PropertyInfo info in type.GetProperties())
                {
                    if (info.Name == "Item")
                    {
                        ParameterInfo[] indexParameters = info.GetIndexParameters();
                        if ((indexParameters.Length == 1) && (indexParameters[0].ParameterType == typeof(int)))
                        {
                            metadata.ElementType = info.PropertyType;
                        }
                    }
                }
                object obj2 = array_metadata_lock;
                Monitor.Enter(obj2);
                try
                {
                    array_metadata.Add(type, metadata);
                }
                catch (ArgumentException)
                {
                }
                finally
                {
                    Monitor.Exit(obj2);
                }
            }
        }

        private static void AddObjectMetadata(Type type)
        {
            if (!object_metadata.ContainsKey(type))
            {
                ObjectMetadata metadata = new ObjectMetadata();
                if (type.GetInterface("System.Collections.IDictionary") != null)
                {
                    metadata.IsDictionary = true;
                }
                metadata.Properties = new Dictionary<string, PropertyMetadata>();
                foreach (PropertyInfo info in type.GetProperties())
                {
                    if (info.Name == "Item")
                    {
                        ParameterInfo[] indexParameters = info.GetIndexParameters();
                        if ((indexParameters.Length == 1) && (indexParameters[0].ParameterType == typeof(string)))
                        {
                            metadata.ElementType = info.PropertyType;
                        }
                    }
                    else
                    {
                        PropertyMetadata metadata2 = new PropertyMetadata();
                        metadata2.Info = info;
                        metadata2.Type = info.PropertyType;
                        metadata.Properties.Add(info.Name, metadata2);
                    }
                }
                foreach (FieldInfo info2 in type.GetFields())
                {
                    PropertyMetadata metadata3 = new PropertyMetadata();
                    metadata3.Info = info2;
                    metadata3.IsField = true;
                    metadata3.Type = info2.FieldType;
                    metadata.Properties.Add(info2.Name, metadata3);
                }
                object obj2 = object_metadata_lock;
                Monitor.Enter(obj2);
                try
                {
                    object_metadata.Add(type, metadata);
                }
                catch (ArgumentException)
                {
                }
                finally
                {
                    Monitor.Exit(obj2);
                }
            }
        }

        private static void AddTypeProperties(Type type)
        {
            if (!type_properties.ContainsKey(type))
            {
                IList<PropertyMetadata> list = new List<PropertyMetadata>();
                foreach (PropertyInfo info in type.GetProperties())
                {
                    if (info.Name != "Item")
                    {
                        PropertyMetadata item = new PropertyMetadata();
                        item.Info = info;
                        item.IsField = false;
                        list.Add(item);
                    }
                }
                foreach (FieldInfo info2 in type.GetFields())
                {
                    PropertyMetadata metadata2 = new PropertyMetadata();
                    metadata2.Info = info2;
                    metadata2.IsField = true;
                    list.Add(metadata2);
                }
                object obj2 = type_properties_lock;
                Monitor.Enter(obj2);
                try
                {
                    type_properties.Add(type, list);
                }
                catch (ArgumentException)
                {
                }
                finally
                {
                    Monitor.Exit(obj2);
                }
            }
        }

        private static MethodInfo GetConvOp(Type t1, Type t2)
        {
            object obj2 = conv_ops_lock;
            lock (obj2)
            {
                if (!conv_ops.ContainsKey(t1))
                {
                    conv_ops.Add(t1, new Dictionary<Type, MethodInfo>());
                }
            }
            if (conv_ops[t1].ContainsKey(t2))
            {
                return conv_ops[t1][t2];
            }
            Type[] types = new Type[] { t2 };
            MethodInfo method = t1.GetMethod("op_Implicit", types);
            object obj3 = conv_ops_lock;
            Monitor.Enter(obj3);
            try
            {
                conv_ops[t1].Add(t2, method);
            }
            catch (ArgumentException)
            {
                return conv_ops[t1][t2];
            }
            finally
            {
                Monitor.Exit(obj3);
            }
            return method;
        }

        private static void ReadSkip(JsonReader reader)
        {
            if (<>f__am$cache10 == null)
            {
                <>f__am$cache10 = delegate {
                    return new JsonMockWrapper();
                };
            }
            ToWrapper(<>f__am$cache10, reader);
        }

        private static IJsonWrapper ReadValue(WrapperFactory factory, JsonReader reader)
        {
            reader.Read();
            if ((reader.Token == JsonToken.ArrayEnd) || (reader.Token == JsonToken.Null))
            {
                return null;
            }
            IJsonWrapper wrapper = factory();
            if (reader.Token == JsonToken.String)
            {
                wrapper.SetString((string) reader.Value);
                return wrapper;
            }
            if (reader.Token == JsonToken.Double)
            {
                wrapper.SetDouble((double) reader.Value);
                return wrapper;
            }
            if (reader.Token == JsonToken.Int)
            {
                wrapper.SetInt((int) reader.Value);
                return wrapper;
            }
            if (reader.Token == JsonToken.Long)
            {
                wrapper.SetLong((long) reader.Value);
                return wrapper;
            }
            if (reader.Token == JsonToken.Boolean)
            {
                wrapper.SetBoolean((bool) reader.Value);
                return wrapper;
            }
            if (reader.Token == JsonToken.ArrayStart)
            {
                wrapper.SetJsonType(JsonType.Array);
                while (true)
                {
                    IJsonWrapper wrapper2 = ReadValue(factory, reader);
                    if ((wrapper2 == null) && (reader.Token == JsonToken.ArrayEnd))
                    {
                        return wrapper;
                    }
                    wrapper.Add(wrapper2);
                }
            }
            if (reader.Token != JsonToken.ObjectStart)
            {
                return wrapper;
            }
            wrapper.SetJsonType(JsonType.Object);
            while (true)
            {
                reader.Read();
                if (reader.Token == JsonToken.ObjectEnd)
                {
                    return wrapper;
                }
                string str = (string) reader.Value;
                wrapper[str] = ReadValue(factory, reader);
            }
        }

        private static object ReadValue(Type inst_type, JsonReader reader)
        {
            IList list;
            Type elementType;
            object obj3;
            reader.Read();
            if (reader.Token == JsonToken.ArrayEnd)
            {
                return null;
            }
            if (reader.Token == JsonToken.Null)
            {
                if (!inst_type.IsClass)
                {
                    throw new JsonException(string.Format("Can't assign null to an instance of type {0}", inst_type));
                }
                return null;
            }
            if (((reader.Token == JsonToken.Double) || (reader.Token == JsonToken.Int)) || (((reader.Token == JsonToken.Long) || (reader.Token == JsonToken.String)) || (reader.Token == JsonToken.Boolean)))
            {
                Type c = reader.Value.GetType();
                if (inst_type.IsAssignableFrom(c))
                {
                    return reader.Value;
                }
                if (custom_importers_table.ContainsKey(c) && custom_importers_table[c].ContainsKey(inst_type))
                {
                    ImporterFunc func = custom_importers_table[c][inst_type];
                    return func(reader.Value);
                }
                if (base_importers_table.ContainsKey(c) && base_importers_table[c].ContainsKey(inst_type))
                {
                    ImporterFunc func2 = base_importers_table[c][inst_type];
                    return func2(reader.Value);
                }
                if (inst_type.IsEnum)
                {
                    return Enum.ToObject(inst_type, reader.Value);
                }
                MethodInfo convOp = GetConvOp(inst_type, c);
                if (convOp == null)
                {
                    throw new JsonException(string.Format("Can't assign value '{0}' (type {1}) to type {2}", reader.Value, c, inst_type));
                }
                object[] parameters = new object[] { reader.Value };
                return convOp.Invoke(null, parameters);
            }
            object obj2 = null;
            if (reader.Token != JsonToken.ArrayStart)
            {
                goto Label_028A;
            }
            AddArrayMetadata(inst_type);
            ArrayMetadata metadata = array_metadata[inst_type];
            if (!metadata.IsArray && !metadata.IsList)
            {
                throw new JsonException(string.Format("Type {0} can't act as an array", inst_type));
            }
            if (!metadata.IsArray)
            {
                list = (IList) Activator.CreateInstance(inst_type);
                elementType = metadata.ElementType;
            }
            else
            {
                list = new ArrayList();
                elementType = inst_type.GetElementType();
            }
        Label_01FD:
            obj3 = ReadValue(elementType, reader);
            if ((obj3 != null) || (reader.Token != JsonToken.ArrayEnd))
            {
                list.Add(obj3);
                goto Label_01FD;
            }
            if (!metadata.IsArray)
            {
                return list;
            }
            int count = list.Count;
            obj2 = Array.CreateInstance(elementType, count);
            for (int i = 0; i < count; i++)
            {
                ((Array) obj2).SetValue(list[i], i);
            }
            return obj2;
        Label_028A:
            if (reader.Token != JsonToken.ObjectStart)
            {
                return obj2;
            }
            AddObjectMetadata(inst_type);
            ObjectMetadata metadata2 = object_metadata[inst_type];
            obj2 = Activator.CreateInstance(inst_type);
            while (true)
            {
                reader.Read();
                if (reader.Token == JsonToken.ObjectEnd)
                {
                    return obj2;
                }
                string key = (string) reader.Value;
                if (metadata2.Properties.ContainsKey(key))
                {
                    PropertyMetadata metadata3 = metadata2.Properties[key];
                    if (metadata3.IsField)
                    {
                        ((FieldInfo) metadata3.Info).SetValue(obj2, ReadValue(metadata3.Type, reader));
                    }
                    else
                    {
                        PropertyInfo info = (PropertyInfo) metadata3.Info;
                        if (info.CanWrite)
                        {
                            info.SetValue(obj2, ReadValue(metadata3.Type, reader), null);
                        }
                        else
                        {
                            ReadValue(metadata3.Type, reader);
                        }
                    }
                }
                else if (!metadata2.IsDictionary)
                {
                    if (!reader.SkipNonMembers)
                    {
                        throw new JsonException(string.Format("The type {0} doesn't have the property '{1}'", inst_type, key));
                    }
                    ReadSkip(reader);
                }
                else
                {
                    ((IDictionary) obj2).Add(key, ReadValue(metadata2.ElementType, reader));
                }
            }
        }

        private static void RegisterBaseExporters()
        {
            if (<>f__am$cache11 == null)
            {
                <>f__am$cache11 = delegate (object obj, JsonWriter writer) {
                    writer.Write(Convert.ToInt32((byte) obj));
                };
            }
            base_exporters_table[typeof(byte)] = <>f__am$cache11;
            if (<>f__am$cache12 == null)
            {
                <>f__am$cache12 = delegate (object obj, JsonWriter writer) {
                    writer.Write(Convert.ToString((char) obj));
                };
            }
            base_exporters_table[typeof(char)] = <>f__am$cache12;
            if (<>f__am$cache13 == null)
            {
                <>f__am$cache13 = delegate (object obj, JsonWriter writer) {
                    writer.Write(Convert.ToString((DateTime) obj, datetime_format));
                };
            }
            base_exporters_table[typeof(DateTime)] = <>f__am$cache13;
            if (<>f__am$cache14 == null)
            {
                <>f__am$cache14 = delegate (object obj, JsonWriter writer) {
                    writer.Write((decimal) obj);
                };
            }
            base_exporters_table[typeof(decimal)] = <>f__am$cache14;
            if (<>f__am$cache15 == null)
            {
                <>f__am$cache15 = delegate (object obj, JsonWriter writer) {
                    writer.Write(Convert.ToInt32((sbyte) obj));
                };
            }
            base_exporters_table[typeof(sbyte)] = <>f__am$cache15;
            if (<>f__am$cache16 == null)
            {
                <>f__am$cache16 = delegate (object obj, JsonWriter writer) {
                    writer.Write(Convert.ToInt32((short) obj));
                };
            }
            base_exporters_table[typeof(short)] = <>f__am$cache16;
            if (<>f__am$cache17 == null)
            {
                <>f__am$cache17 = delegate (object obj, JsonWriter writer) {
                    writer.Write(Convert.ToInt32((ushort) obj));
                };
            }
            base_exporters_table[typeof(ushort)] = <>f__am$cache17;
            if (<>f__am$cache18 == null)
            {
                <>f__am$cache18 = delegate (object obj, JsonWriter writer) {
                    writer.Write(Convert.ToUInt64((uint) obj));
                };
            }
            base_exporters_table[typeof(uint)] = <>f__am$cache18;
            if (<>f__am$cache19 == null)
            {
                <>f__am$cache19 = delegate (object obj, JsonWriter writer) {
                    writer.Write((ulong) obj);
                };
            }
            base_exporters_table[typeof(ulong)] = <>f__am$cache19;
        }

        private static void RegisterBaseImporters()
        {
            if (<>f__am$cache1A == null)
            {
                <>f__am$cache1A = delegate (object input) {
                    return Convert.ToByte((int) input);
                };
            }
            ImporterFunc importer = <>f__am$cache1A;
            RegisterImporter(base_importers_table, typeof(int), typeof(byte), importer);
            if (<>f__am$cache1B == null)
            {
                <>f__am$cache1B = delegate (object input) {
                    return Convert.ToUInt64((int) input);
                };
            }
            importer = <>f__am$cache1B;
            RegisterImporter(base_importers_table, typeof(int), typeof(ulong), importer);
            if (<>f__am$cache1C == null)
            {
                <>f__am$cache1C = delegate (object input) {
                    return Convert.ToSByte((int) input);
                };
            }
            importer = <>f__am$cache1C;
            RegisterImporter(base_importers_table, typeof(int), typeof(sbyte), importer);
            if (<>f__am$cache1D == null)
            {
                <>f__am$cache1D = delegate (object input) {
                    return Convert.ToInt16((int) input);
                };
            }
            importer = <>f__am$cache1D;
            RegisterImporter(base_importers_table, typeof(int), typeof(short), importer);
            if (<>f__am$cache1E == null)
            {
                <>f__am$cache1E = delegate (object input) {
                    return Convert.ToUInt16((int) input);
                };
            }
            importer = <>f__am$cache1E;
            RegisterImporter(base_importers_table, typeof(int), typeof(ushort), importer);
            if (<>f__am$cache1F == null)
            {
                <>f__am$cache1F = delegate (object input) {
                    return Convert.ToUInt32((int) input);
                };
            }
            importer = <>f__am$cache1F;
            RegisterImporter(base_importers_table, typeof(int), typeof(uint), importer);
            if (<>f__am$cache20 == null)
            {
                <>f__am$cache20 = delegate (object input) {
                    return Convert.ToSingle((int) input);
                };
            }
            importer = <>f__am$cache20;
            RegisterImporter(base_importers_table, typeof(int), typeof(float), importer);
            if (<>f__am$cache21 == null)
            {
                <>f__am$cache21 = delegate (object input) {
                    return Convert.ToDouble((int) input);
                };
            }
            importer = <>f__am$cache21;
            RegisterImporter(base_importers_table, typeof(int), typeof(double), importer);
            if (<>f__am$cache22 == null)
            {
                <>f__am$cache22 = delegate (object input) {
                    return Convert.ToDecimal((double) input);
                };
            }
            importer = <>f__am$cache22;
            RegisterImporter(base_importers_table, typeof(double), typeof(decimal), importer);
            if (<>f__am$cache23 == null)
            {
                <>f__am$cache23 = delegate (object input) {
                    return Convert.ToUInt32((long) input);
                };
            }
            importer = <>f__am$cache23;
            RegisterImporter(base_importers_table, typeof(long), typeof(uint), importer);
            if (<>f__am$cache24 == null)
            {
                <>f__am$cache24 = delegate (object input) {
                    return Convert.ToChar((string) input);
                };
            }
            importer = <>f__am$cache24;
            RegisterImporter(base_importers_table, typeof(string), typeof(char), importer);
            if (<>f__am$cache25 == null)
            {
                <>f__am$cache25 = delegate (object input) {
                    return Convert.ToDateTime((string) input, datetime_format);
                };
            }
            importer = <>f__am$cache25;
            RegisterImporter(base_importers_table, typeof(string), typeof(DateTime), importer);
        }

        public static void RegisterExporter<T>(ExporterFunc<T> exporter)
        {
            <RegisterExporter>c__AnonStorey261<T> storey = new <RegisterExporter>c__AnonStorey261<T>();
            storey.exporter = exporter;
            ExporterFunc func = new ExporterFunc(storey.<>m__55);
            custom_exporters_table[typeof(T)] = func;
        }

        public static void RegisterImporter<TJson, TValue>(ImporterFunc<TJson, TValue> importer)
        {
            <RegisterImporter>c__AnonStorey262<TJson, TValue> storey = new <RegisterImporter>c__AnonStorey262<TJson, TValue>();
            storey.importer = importer;
            ImporterFunc func = new ImporterFunc(storey.<>m__56);
            RegisterImporter(custom_importers_table, typeof(TJson), typeof(TValue), func);
        }

        private static void RegisterImporter(IDictionary<Type, IDictionary<Type, ImporterFunc>> table, Type json_type, Type value_type, ImporterFunc importer)
        {
            if (!table.ContainsKey(json_type))
            {
                table.Add(json_type, new Dictionary<Type, ImporterFunc>());
            }
            table[json_type][value_type] = importer;
        }

        public static string ToJson(object obj)
        {
            object obj2 = static_writer_lock;
            lock (obj2)
            {
                static_writer.Reset();
                WriteValue(obj, static_writer, true, 0);
                return static_writer.ToString();
            }
        }

        public static void ToJson(object obj, JsonWriter writer)
        {
            WriteValue(obj, writer, false, 0);
        }

        public static JsonData ToObject(JsonReader reader)
        {
            if (<>f__am$cache26 == null)
            {
                <>f__am$cache26 = delegate {
                    return new JsonData();
                };
            }
            return (JsonData) ToWrapper(<>f__am$cache26, reader);
        }

        public static T ToObject<T>(JsonReader reader)
        {
            return (T) ReadValue(typeof(T), reader);
        }

        public static JsonData ToObject(TextReader reader)
        {
            JsonReader reader2 = new JsonReader(reader);
            if (<>f__am$cache27 == null)
            {
                <>f__am$cache27 = delegate {
                    return new JsonData();
                };
            }
            return (JsonData) ToWrapper(<>f__am$cache27, reader2);
        }

        public static T ToObject<T>(TextReader reader)
        {
            JsonReader reader2 = new JsonReader(reader);
            return (T) ReadValue(typeof(T), reader2);
        }

        public static JsonData ToObject(string json)
        {
            if (<>f__am$cache28 == null)
            {
                <>f__am$cache28 = delegate {
                    return new JsonData();
                };
            }
            return (JsonData) ToWrapper(<>f__am$cache28, json);
        }

        public static T ToObject<T>(string json)
        {
            JsonReader reader = new JsonReader(json);
            return (T) ReadValue(typeof(T), reader);
        }

        public static IJsonWrapper ToWrapper(WrapperFactory factory, JsonReader reader)
        {
            return ReadValue(factory, reader);
        }

        public static IJsonWrapper ToWrapper(WrapperFactory factory, string json)
        {
            JsonReader reader = new JsonReader(json);
            return ReadValue(factory, reader);
        }

        public static void UnregisterExporters()
        {
            custom_exporters_table.Clear();
        }

        public static void UnregisterImporters()
        {
            custom_importers_table.Clear();
        }

        private static void WriteValue(object obj, JsonWriter writer, bool writer_is_private, int depth)
        {
            if (depth > max_nesting_depth)
            {
                throw new JsonException(string.Format("Max allowed object depth reached while trying to export from type {0}", obj.GetType()));
            }
            if (obj == null)
            {
                writer.Write((string) null);
            }
            else if (obj is IJsonWrapper)
            {
                if (writer_is_private)
                {
                    writer.TextWriter.Write(((IJsonWrapper) obj).ToJson());
                }
                else
                {
                    ((IJsonWrapper) obj).ToJson(writer);
                }
            }
            else if (obj is string)
            {
                writer.Write((string) obj);
            }
            else if (obj is double)
            {
                writer.Write((double) obj);
            }
            else if (obj is int)
            {
                writer.Write((int) obj);
            }
            else if (obj is bool)
            {
                writer.Write((bool) obj);
            }
            else if (obj is long)
            {
                writer.Write((long) obj);
            }
            else if (obj is Array)
            {
                writer.WriteArrayStart();
                IEnumerator enumerator = ((Array) obj).GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        WriteValue(current, writer, writer_is_private, depth + 1);
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
                writer.WriteArrayEnd();
            }
            else if (obj is IList)
            {
                writer.WriteArrayStart();
                IEnumerator enumerator2 = ((IList) obj).GetEnumerator();
                try
                {
                    while (enumerator2.MoveNext())
                    {
                        object obj3 = enumerator2.Current;
                        WriteValue(obj3, writer, writer_is_private, depth + 1);
                    }
                }
                finally
                {
                    IDisposable disposable2 = enumerator2 as IDisposable;
                    if (disposable2 == null)
                    {
                    }
                    disposable2.Dispose();
                }
                writer.WriteArrayEnd();
            }
            else if (obj is IDictionary)
            {
                writer.WriteObjectStart();
                IDictionaryEnumerator enumerator3 = ((IDictionary) obj).GetEnumerator();
                try
                {
                    while (enumerator3.MoveNext())
                    {
                        DictionaryEntry entry = (DictionaryEntry) enumerator3.Current;
                        writer.WritePropertyName((string) entry.Key);
                        WriteValue(entry.Value, writer, writer_is_private, depth + 1);
                    }
                }
                finally
                {
                    IDisposable disposable3 = enumerator3 as IDisposable;
                    if (disposable3 == null)
                    {
                    }
                    disposable3.Dispose();
                }
                writer.WriteObjectEnd();
            }
            else
            {
                Type key = obj.GetType();
                if (custom_exporters_table.ContainsKey(key))
                {
                    ExporterFunc func = custom_exporters_table[key];
                    func(obj, writer);
                }
                else if (base_exporters_table.ContainsKey(key))
                {
                    ExporterFunc func2 = base_exporters_table[key];
                    func2(obj, writer);
                }
                else if (obj is Enum)
                {
                    Type underlyingType = Enum.GetUnderlyingType(key);
                    if (((underlyingType == typeof(long)) || (underlyingType == typeof(uint))) || (underlyingType == typeof(ulong)))
                    {
                        writer.Write((ulong) obj);
                    }
                    else
                    {
                        writer.Write((int) obj);
                    }
                }
                else
                {
                    AddTypeProperties(key);
                    IList<PropertyMetadata> list = type_properties[key];
                    writer.WriteObjectStart();
                    IEnumerator<PropertyMetadata> enumerator4 = list.GetEnumerator();
                    try
                    {
                        while (enumerator4.MoveNext())
                        {
                            PropertyMetadata metadata = enumerator4.Current;
                            if (metadata.IsField)
                            {
                                writer.WritePropertyName(metadata.Info.Name);
                                WriteValue(((FieldInfo) metadata.Info).GetValue(obj), writer, writer_is_private, depth + 1);
                            }
                            else
                            {
                                PropertyInfo info = (PropertyInfo) metadata.Info;
                                if (info.CanRead)
                                {
                                    writer.WritePropertyName(metadata.Info.Name);
                                    WriteValue(info.GetValue(obj, null), writer, writer_is_private, depth + 1);
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator4 == null)
                        {
                        }
                        enumerator4.Dispose();
                    }
                    writer.WriteObjectEnd();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterExporter>c__AnonStorey261<T>
        {
            internal ExporterFunc<T> exporter;

            internal void <>m__55(object obj, JsonWriter writer)
            {
                this.exporter((T) obj, writer);
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterImporter>c__AnonStorey262<TJson, TValue>
        {
            internal ImporterFunc<TJson, TValue> importer;

            internal object <>m__56(object input)
            {
                return this.importer((TJson) input);
            }
        }
    }
}

