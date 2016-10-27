namespace Appboy.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class JSONClass : JSONNode, IEnumerable
    {
        private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();

        public override void Add(string aKey, JSONNode aItem)
        {
            if (!string.IsNullOrEmpty(aKey))
            {
                if (this.m_Dict.ContainsKey(aKey))
                {
                    this.m_Dict[aKey] = aItem;
                }
                else
                {
                    this.m_Dict.Add(aKey, aItem);
                }
            }
            else
            {
                this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
            }
        }

        [DebuggerHidden]
        public IEnumerator GetEnumerator()
        {
            <GetEnumerator>c__Iterator5 iterator = new <GetEnumerator>c__Iterator5();
            iterator.<>f__this = this;
            return iterator;
        }

        public override JSONNode Remove(JSONNode aNode)
        {
            <Remove>c__AnonStorey6 storey = new <Remove>c__AnonStorey6();
            storey.aNode = aNode;
            try
            {
                KeyValuePair<string, JSONNode> pair = Enumerable.First<KeyValuePair<string, JSONNode>>(Enumerable.Where<KeyValuePair<string, JSONNode>>(this.m_Dict, new Func<KeyValuePair<string, JSONNode>, bool>(storey.<>m__0)));
                this.m_Dict.Remove(pair.Key);
                return storey.aNode;
            }
            catch
            {
                return null;
            }
        }

        public override JSONNode Remove(int aIndex)
        {
            if ((aIndex < 0) || (aIndex >= this.m_Dict.Count))
            {
                return null;
            }
            KeyValuePair<string, JSONNode> pair = Enumerable.ElementAt<KeyValuePair<string, JSONNode>>(this.m_Dict, aIndex);
            this.m_Dict.Remove(pair.Key);
            return pair.Value;
        }

        public override JSONNode Remove(string aKey)
        {
            if (!this.m_Dict.ContainsKey(aKey))
            {
                return null;
            }
            JSONNode node = this.m_Dict[aKey];
            this.m_Dict.Remove(aKey);
            return node;
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            aWriter.Write((byte) 2);
            aWriter.Write(this.m_Dict.Count);
            foreach (string str in this.m_Dict.Keys)
            {
                aWriter.Write(str);
                this.m_Dict[str].Serialize(aWriter);
            }
        }

        public override string ToString()
        {
            string str = "{";
            foreach (KeyValuePair<string, JSONNode> pair in this.m_Dict)
            {
                if (str.Length > 2)
                {
                    str = str + ", ";
                }
                string str2 = str;
                string[] textArray1 = new string[] { str2, "\"", JSONNode.Escape(pair.Key), "\":", pair.Value.ToString() };
                str = string.Concat(textArray1);
            }
            return (str + "}");
        }

        public override string ToString(string aPrefix)
        {
            string str = "{ ";
            foreach (KeyValuePair<string, JSONNode> pair in this.m_Dict)
            {
                if (str.Length > 3)
                {
                    str = str + ", ";
                }
                str = str + "\n" + aPrefix + "   ";
                string str2 = str;
                string[] textArray1 = new string[] { str2, "\"", JSONNode.Escape(pair.Key), "\" : ", pair.Value.ToString(aPrefix + "   ") };
                str = string.Concat(textArray1);
            }
            return (str + "\n" + aPrefix + "}");
        }

        public override IEnumerable<JSONNode> Childs
        {
            get
            {
                <>c__Iterator4 iterator = new <>c__Iterator4();
                iterator.<>f__this = this;
                iterator.$PC = -2;
                return iterator;
            }
        }

        public override int Count
        {
            get
            {
                return this.m_Dict.Count;
            }
        }

        public override JSONNode this[string aKey]
        {
            get
            {
                if (this.m_Dict.ContainsKey(aKey))
                {
                    return this.m_Dict[aKey];
                }
                return new JSONLazyCreator(this, aKey);
            }
            set
            {
                if (this.m_Dict.ContainsKey(aKey))
                {
                    this.m_Dict[aKey] = value;
                }
                else
                {
                    this.m_Dict.Add(aKey, value);
                }
            }
        }

        public override JSONNode this[int aIndex]
        {
            get
            {
                if ((aIndex >= 0) && (aIndex < this.m_Dict.Count))
                {
                    return Enumerable.ElementAt<KeyValuePair<string, JSONNode>>(this.m_Dict, aIndex).Value;
                }
                return null;
            }
            set
            {
                if ((aIndex >= 0) && (aIndex < this.m_Dict.Count))
                {
                    string key = Enumerable.ElementAt<KeyValuePair<string, JSONNode>>(this.m_Dict, aIndex).Key;
                    this.m_Dict[key] = value;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator4 : IDisposable, IEnumerable<JSONNode>, IEnumerator<JSONNode>, IEnumerator, IEnumerable
        {
            internal JSONNode $current;
            internal int $PC;
            internal Dictionary<string, JSONNode>.Enumerator <$s_19>__0;
            internal JSONClass <>f__this;
            internal KeyValuePair<string, JSONNode> <N>__1;

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
                        }
                        finally
                        {
                            this.<$s_19>__0.Dispose();
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
                        this.<$s_19>__0 = this.<>f__this.m_Dict.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00AE;
                }
                try
                {
                    while (this.<$s_19>__0.MoveNext())
                    {
                        this.<N>__1 = this.<$s_19>__0.Current;
                        this.$current = this.<N>__1.Value;
                        this.$PC = 1;
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_19>__0.Dispose();
                }
                this.$PC = -1;
            Label_00AE:
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
                JSONClass.<>c__Iterator4 iterator = new JSONClass.<>c__Iterator4();
                iterator.<>f__this = this.<>f__this;
                return iterator;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Appboy.Utilities.JSONNode>.GetEnumerator();
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
        private sealed class <GetEnumerator>c__Iterator5 : IEnumerator<object>, IDisposable, IEnumerator
        {
            internal object $current;
            internal int $PC;
            internal Dictionary<string, JSONNode>.Enumerator <$s_20>__0;
            internal JSONClass <>f__this;
            internal KeyValuePair<string, JSONNode> <N>__1;

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
                        }
                        finally
                        {
                            this.<$s_20>__0.Dispose();
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
                        this.<$s_20>__0 = this.<>f__this.m_Dict.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00AE;
                }
                try
                {
                    while (this.<$s_20>__0.MoveNext())
                    {
                        this.<N>__1 = this.<$s_20>__0.Current;
                        this.$current = this.<N>__1;
                        this.$PC = 1;
                        flag = true;
                        return true;
                    }
                }
                finally
                {
                    if (!flag)
                    {
                    }
                    this.<$s_20>__0.Dispose();
                }
                this.$PC = -1;
            Label_00AE:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
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
        private sealed class <Remove>c__AnonStorey6
        {
            internal JSONNode aNode;

            internal bool <>m__0(KeyValuePair<string, JSONNode> k)
            {
                return (k.Value == this.aNode);
            }
        }
    }
}

