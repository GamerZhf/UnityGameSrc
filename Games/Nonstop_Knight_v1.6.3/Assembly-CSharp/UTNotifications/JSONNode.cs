namespace UTNotifications
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class JSONNode
    {
        public virtual void Add(JSONNode aItem)
        {
            this.Add(string.Empty, aItem);
        }

        public virtual void Add(string aKey, JSONNode aItem)
        {
        }

        public static JSONNode Deserialize(BinaryReader aReader)
        {
            JSONBinaryTag tag = (JSONBinaryTag) aReader.ReadByte();
            switch (tag)
            {
                case JSONBinaryTag.Array:
                {
                    int num = aReader.ReadInt32();
                    JSONArray array = new JSONArray();
                    for (int i = 0; i < num; i++)
                    {
                        array.Add(Deserialize(aReader));
                    }
                    return array;
                }
                case JSONBinaryTag.Class:
                {
                    int num3 = aReader.ReadInt32();
                    JSONClass class2 = new JSONClass();
                    for (int j = 0; j < num3; j++)
                    {
                        string aKey = aReader.ReadString();
                        JSONNode aItem = Deserialize(aReader);
                        class2.Add(aKey, aItem);
                    }
                    return class2;
                }
                case JSONBinaryTag.Value:
                    return new JSONData(aReader.ReadString());

                case JSONBinaryTag.IntValue:
                    return new JSONData(aReader.ReadInt32());

                case JSONBinaryTag.DoubleValue:
                    return new JSONData(aReader.ReadDouble());

                case JSONBinaryTag.BoolValue:
                    return new JSONData(aReader.ReadBoolean());

                case JSONBinaryTag.FloatValue:
                    return new JSONData(aReader.ReadSingle());
            }
            throw new Exception("Error deserializing JSON. Unknown tag: " + tag);
        }

        public override bool Equals(object obj)
        {
            return object.ReferenceEquals(this, obj);
        }

        internal static string Escape(string aText)
        {
            string str = string.Empty;
            foreach (char ch in aText)
            {
                char ch2 = ch;
                switch (ch2)
                {
                    case '\b':
                    {
                        str = str + @"\b";
                        continue;
                    }
                    case '\t':
                    {
                        str = str + @"\t";
                        continue;
                    }
                    case '\n':
                    {
                        str = str + @"\n";
                        continue;
                    }
                    case '\f':
                    {
                        str = str + @"\f";
                        continue;
                    }
                    case '\r':
                    {
                        str = str + @"\r";
                        continue;
                    }
                    default:
                    {
                        if (ch2 != '"')
                        {
                            if (ch2 != '\\')
                            {
                                break;
                            }
                            str = str + @"\\";
                        }
                        else
                        {
                            str = str + "\\\"";
                        }
                        continue;
                    }
                }
                str = str + ch;
            }
            return str;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static JSONNode LoadFromBase64(string aBase64)
        {
            MemoryStream aData = new MemoryStream(Convert.FromBase64String(aBase64));
            aData.Position = 0L;
            return LoadFromStream(aData);
        }

        public static JSONNode LoadFromCompressedBase64(string aBase64)
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public static JSONNode LoadFromCompressedFile(string aFileName)
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public static JSONNode LoadFromCompressedStream(Stream aData)
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public static JSONNode LoadFromFile(string aFileName)
        {
            using (FileStream stream = File.OpenRead(aFileName))
            {
                return LoadFromStream(stream);
            }
        }

        public static JSONNode LoadFromStream(Stream aData)
        {
            using (BinaryReader reader = new BinaryReader(aData))
            {
                return Deserialize(reader);
            }
        }

        public static bool operator ==(JSONNode a, object b)
        {
            return (((b == null) && (a is JSONLazyCreator)) || object.ReferenceEquals(a, b));
        }

        public static implicit operator JSONNode(string s)
        {
            return new JSONData(s);
        }

        public static implicit operator string(JSONNode d)
        {
            return ((d != null) ? d.Value : null);
        }

        public static bool operator !=(JSONNode a, object b)
        {
            return (a != b);
        }

        public static JSONNode Parse(string aJSON)
        {
            Stack<JSONNode> stack = new Stack<JSONNode>();
            JSONNode node = null;
            int num = 0;
            string aItem = string.Empty;
            string aKey = string.Empty;
            bool flag = false;
            while (num < aJSON.Length)
            {
                char ch;
                char ch2 = aJSON[num];
                switch (ch2)
                {
                    case '\t':
                    case ' ':
                        if (flag)
                        {
                            aItem = aItem + aJSON[num];
                        }
                        goto Label_0467;

                    case '\n':
                    case '\r':
                        goto Label_0467;

                    case '"':
                        flag ^= true;
                        goto Label_0467;

                    default:
                        switch (ch2)
                        {
                            case '[':
                                if (!flag)
                                {
                                    goto Label_0151;
                                }
                                aItem = aItem + aJSON[num];
                                goto Label_0467;

                            case '\\':
                                num++;
                                if (!flag)
                                {
                                    goto Label_0467;
                                }
                                ch = aJSON[num];
                                switch (ch)
                                {
                                    case 'n':
                                        aItem = aItem + '\n';
                                        goto Label_0467;

                                    case 'r':
                                        aItem = aItem + '\r';
                                        goto Label_0467;

                                    case 't':
                                        aItem = aItem + '\t';
                                        goto Label_0467;

                                    case 'u':
                                    {
                                        string s = aJSON.Substring(num + 1, 4);
                                        aItem = aItem + ((char) int.Parse(s, NumberStyles.AllowHexSpecifier));
                                        num += 4;
                                        goto Label_0467;
                                    }
                                    case 'b':
                                        aItem = aItem + '\b';
                                        goto Label_0467;

                                    case 'f':
                                        goto Label_03F6;
                                }
                                goto Label_0437;

                            case ']':
                                goto Label_01C5;

                            default:
                                switch (ch2)
                                {
                                    case '{':
                                        if (!flag)
                                        {
                                            break;
                                        }
                                        aItem = aItem + aJSON[num];
                                        goto Label_0467;

                                    case '}':
                                        goto Label_01C5;
                                }
                                break;
                        }
                        switch (ch2)
                        {
                            case ',':
                                if (flag)
                                {
                                    aItem = aItem + aJSON[num];
                                }
                                else
                                {
                                    if (aItem != string.Empty)
                                    {
                                        if (node is JSONArray)
                                        {
                                            node.Add(aItem);
                                        }
                                        else if (aKey != string.Empty)
                                        {
                                            node.Add(aKey, aItem);
                                        }
                                    }
                                    aKey = string.Empty;
                                    aItem = string.Empty;
                                }
                                goto Label_0467;

                            case ':':
                                if (flag)
                                {
                                    aItem = aItem + aJSON[num];
                                }
                                else
                                {
                                    aKey = aItem;
                                    aItem = string.Empty;
                                }
                                goto Label_0467;

                            default:
                                aItem = aItem + aJSON[num];
                                goto Label_0467;
                        }
                        break;
                }
                stack.Push(new JSONClass());
                if (node != null)
                {
                    aKey = aKey.Trim();
                    if (node is JSONArray)
                    {
                        node.Add(stack.Peek());
                    }
                    else if (aKey != string.Empty)
                    {
                        node.Add(aKey, stack.Peek());
                    }
                }
                aKey = string.Empty;
                aItem = string.Empty;
                node = stack.Peek();
                goto Label_0467;
            Label_0151:
                stack.Push(new JSONArray());
                if (node != null)
                {
                    aKey = aKey.Trim();
                    if (node is JSONArray)
                    {
                        node.Add(stack.Peek());
                    }
                    else if (aKey != string.Empty)
                    {
                        node.Add(aKey, stack.Peek());
                    }
                }
                aKey = string.Empty;
                aItem = string.Empty;
                node = stack.Peek();
                goto Label_0467;
            Label_01C5:
                if (flag)
                {
                    aItem = aItem + aJSON[num];
                }
                else
                {
                    if (stack.Count == 0)
                    {
                        throw new Exception("JSON Parse: Too many closing brackets");
                    }
                    stack.Pop();
                    if (aItem != string.Empty)
                    {
                        aKey = aKey.Trim();
                        if (node is JSONArray)
                        {
                            node.Add(aItem);
                        }
                        else if (aKey != string.Empty)
                        {
                            node.Add(aKey, aItem);
                        }
                    }
                    aKey = string.Empty;
                    aItem = string.Empty;
                    if (stack.Count > 0)
                    {
                        node = stack.Peek();
                    }
                }
                goto Label_0467;
            Label_03F6:
                aItem = aItem + '\f';
                goto Label_0467;
            Label_0437:
                aItem = aItem + ch;
            Label_0467:
                num++;
            }
            if (flag)
            {
                throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
            }
            return node;
        }

        public virtual JSONNode Remove(int aIndex)
        {
            return null;
        }

        public virtual JSONNode Remove(string aKey)
        {
            return null;
        }

        public virtual JSONNode Remove(JSONNode aNode)
        {
            return aNode;
        }

        public string SaveToBase64()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                this.SaveToStream(stream);
                stream.Position = 0L;
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public string SaveToCompressedBase64()
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public void SaveToCompressedFile(string aFileName)
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public void SaveToCompressedStream(Stream aData)
        {
            throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
        }

        public void SaveToFile(string aFileName)
        {
            Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
            using (FileStream stream = File.OpenWrite(aFileName))
            {
                this.SaveToStream(stream);
            }
        }

        public void SaveToStream(Stream aData)
        {
            BinaryWriter aWriter = new BinaryWriter(aData);
            this.Serialize(aWriter);
        }

        public virtual void Serialize(BinaryWriter aWriter)
        {
        }

        public override string ToString()
        {
            return "JSONNode";
        }

        public virtual string ToString(string aPrefix)
        {
            return "JSONNode";
        }

        public virtual JSONArray AsArray
        {
            get
            {
                return (this as JSONArray);
            }
        }

        public virtual bool AsBool
        {
            get
            {
                bool result = false;
                if (bool.TryParse(this.Value, out result))
                {
                    return result;
                }
                return !string.IsNullOrEmpty(this.Value);
            }
            set
            {
                this.Value = !value ? "false" : "true";
            }
        }

        public virtual double AsDouble
        {
            get
            {
                double result = 0.0;
                if (double.TryParse(this.Value, out result))
                {
                    return result;
                }
                return 0.0;
            }
            set
            {
                this.Value = value.ToString();
            }
        }

        public virtual float AsFloat
        {
            get
            {
                float result = 0f;
                if (float.TryParse(this.Value, out result))
                {
                    return result;
                }
                return 0f;
            }
            set
            {
                this.Value = value.ToString();
            }
        }

        public virtual int AsInt
        {
            get
            {
                int result = 0;
                if (int.TryParse(this.Value, out result))
                {
                    return result;
                }
                return 0;
            }
            set
            {
                this.Value = value.ToString();
            }
        }

        public virtual JSONClass AsObject
        {
            get
            {
                return (this as JSONClass);
            }
        }

        public virtual IEnumerable<JSONNode> Childs
        {
            get
            {
                <>c__Iterator239 iterator = new <>c__Iterator239();
                iterator.$PC = -2;
                return iterator;
            }
        }

        public virtual int Count
        {
            get
            {
                return 0;
            }
        }

        public IEnumerable<JSONNode> DeepChilds
        {
            get
            {
                <>c__Iterator23A iteratora = new <>c__Iterator23A();
                iteratora.<>f__this = this;
                iteratora.$PC = -2;
                return iteratora;
            }
        }

        public virtual JSONNode this[int aIndex]
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual JSONNode this[string aKey]
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public virtual string Value
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator239 : IEnumerator, IDisposable, IEnumerable, IEnumerable<JSONNode>, IEnumerator<JSONNode>
        {
            internal JSONNode $current;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<JSONNode> IEnumerable<JSONNode>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new JSONNode.<>c__Iterator239();
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<UTNotifications.JSONNode>.GetEnumerator();
            }

            JSONNode IEnumerator<JSONNode>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator23A : IEnumerator, IDisposable, IEnumerable, IEnumerable<JSONNode>, IEnumerator<JSONNode>
        {
            internal JSONNode $current;
            internal int $PC;
            internal IEnumerator<JSONNode> <$s_540>__0;
            internal IEnumerator<JSONNode> <$s_541>__2;
            internal JSONNode <>f__this;
            internal JSONNode <C>__1;
            internal JSONNode <D>__3;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                            try
                            {
                            }
                            finally
                            {
                                if (this.<$s_541>__2 == null)
                                {
                                }
                                this.<$s_541>__2.Dispose();
                            }
                        }
                        finally
                        {
                            if (this.<$s_540>__0 == null)
                            {
                            }
                            this.<$s_540>__0.Dispose();
                        }
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<$s_540>__0 = this.<>f__this.Childs.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0116;
                }
                try
                {
                    switch (num)
                    {
                        case 1:
                            goto Label_0077;
                    }
                    while (this.<$s_540>__0.MoveNext())
                    {
                        this.<C>__1 = this.<$s_540>__0.Current;
                        this.<$s_541>__2 = this.<C>__1.DeepChilds.GetEnumerator();
                        num = 0xfffffffd;
                    Label_0077:
                        try
                        {
                            while (this.<$s_541>__2.MoveNext())
                            {
                                this.<D>__3 = this.<$s_541>__2.Current;
                                this.$current = this.<D>__3;
                                this.$PC = 1;
                                flag = true;
                                return true;
                            }
                            continue;
                        }
                        finally
                        {
                            if (!flag)
                            {
                            }
                            if (this.<$s_541>__2 == null)
                            {
                            }
                            this.<$s_541>__2.Dispose();
                        }
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    if (this.<$s_540>__0 == null)
                    {
                    }
                    this.<$s_540>__0.Dispose();
                }
                this.$PC = -1;
            Label_0116:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<JSONNode> IEnumerable<JSONNode>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                JSONNode.<>c__Iterator23A iteratora = new JSONNode.<>c__Iterator23A();
                iteratora.<>f__this = this.<>f__this;
                return iteratora;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<UTNotifications.JSONNode>.GetEnumerator();
            }

            JSONNode IEnumerator<JSONNode>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

