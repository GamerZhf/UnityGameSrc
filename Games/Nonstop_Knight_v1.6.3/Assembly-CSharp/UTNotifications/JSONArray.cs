namespace UTNotifications
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class JSONArray : JSONNode, IEnumerable
    {
        private List<JSONNode> m_List = new List<JSONNode>();

        public override void Add(string aKey, JSONNode aItem)
        {
            this.m_List.Add(aItem);
        }

        [DebuggerHidden]
        public IEnumerator GetEnumerator()
        {
            <GetEnumerator>c__Iterator23C iteratorc = new <GetEnumerator>c__Iterator23C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public override JSONNode Remove(int aIndex)
        {
            if ((aIndex < 0) || (aIndex >= this.m_List.Count))
            {
                return null;
            }
            JSONNode node = this.m_List[aIndex];
            this.m_List.RemoveAt(aIndex);
            return node;
        }

        public override JSONNode Remove(JSONNode aNode)
        {
            this.m_List.Remove(aNode);
            return aNode;
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            aWriter.Write((byte) 1);
            aWriter.Write(this.m_List.Count);
            for (int i = 0; i < this.m_List.Count; i++)
            {
                this.m_List[i].Serialize(aWriter);
            }
        }

        public override string ToString()
        {
            string str = "[ ";
            foreach (JSONNode node in this.m_List)
            {
                if (str.Length > 2)
                {
                    str = str + ", ";
                }
                str = str + node.ToString();
            }
            return (str + " ]");
        }

        public override string ToString(string aPrefix)
        {
            string str = "[ ";
            foreach (JSONNode node in this.m_List)
            {
                if (str.Length > 3)
                {
                    str = str + ", ";
                }
                str = str + "\n" + aPrefix + "   ";
                str = str + node.ToString(aPrefix + "   ");
            }
            return (str + "\n" + aPrefix + "]");
        }

        public override IEnumerable<JSONNode> Childs
        {
            get
            {
                <>c__Iterator23B iteratorb = new <>c__Iterator23B();
                iteratorb.<>f__this = this;
                iteratorb.$PC = -2;
                return iteratorb;
            }
        }

        public override int Count
        {
            get
            {
                return this.m_List.Count;
            }
        }

        public override JSONNode this[int aIndex]
        {
            get
            {
                if ((aIndex >= 0) && (aIndex < this.m_List.Count))
                {
                    return this.m_List[aIndex];
                }
                return new JSONLazyCreator(this);
            }
            set
            {
                if ((aIndex < 0) || (aIndex >= this.m_List.Count))
                {
                    this.m_List.Add(value);
                }
                else
                {
                    this.m_List[aIndex] = value;
                }
            }
        }

        public override JSONNode this[string aKey]
        {
            get
            {
                return new JSONLazyCreator(this);
            }
            set
            {
                this.m_List.Add(value);
            }
        }

        [CompilerGenerated]
        private sealed class <>c__Iterator23B : IEnumerator, IDisposable, IEnumerable, IEnumerable<JSONNode>, IEnumerator<JSONNode>
        {
            internal JSONNode $current;
            internal int $PC;
            internal List<JSONNode>.Enumerator <$s_544>__0;
            internal JSONArray <>f__this;
            internal JSONNode <N>__1;

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
                            this.<$s_544>__0.Dispose();
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
                        this.<$s_544>__0 = this.<>f__this.m_List.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00A9;
                }
                try
                {
                    while (this.<$s_544>__0.MoveNext())
                    {
                        this.<N>__1 = this.<$s_544>__0.Current;
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
                    this.<$s_544>__0.Dispose();
                }
                this.$PC = -1;
            Label_00A9:
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
                JSONArray.<>c__Iterator23B iteratorb = new JSONArray.<>c__Iterator23B();
                iteratorb.<>f__this = this.<>f__this;
                return iteratorb;
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
        private sealed class <GetEnumerator>c__Iterator23C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal List<JSONNode>.Enumerator <$s_545>__0;
            internal JSONArray <>f__this;
            internal JSONNode <N>__1;

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
                            this.<$s_545>__0.Dispose();
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
                        this.<$s_545>__0 = this.<>f__this.m_List.GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00A9;
                }
                try
                {
                    while (this.<$s_545>__0.MoveNext())
                    {
                        this.<N>__1 = this.<$s_545>__0.Current;
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
                    this.<$s_545>__0.Dispose();
                }
                this.$PC = -1;
            Label_00A9:
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
    }
}

