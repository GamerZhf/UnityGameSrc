namespace Appboy.Utilities
{
    using System;
    using System.Reflection;

    internal class JSONLazyCreator : JSONNode
    {
        private string m_Key;
        private JSONNode m_Node;

        public JSONLazyCreator(JSONNode aNode)
        {
            this.m_Node = aNode;
            this.m_Key = null;
        }

        public JSONLazyCreator(JSONNode aNode, string aKey)
        {
            this.m_Node = aNode;
            this.m_Key = aKey;
        }

        public override void Add(JSONNode aItem)
        {
            JSONArray aVal = new JSONArray();
            aVal.Add(aItem);
            this.Set(aVal);
        }

        public override void Add(string aKey, JSONNode aItem)
        {
            JSONClass aVal = new JSONClass();
            aVal.Add(aKey, aItem);
            this.Set(aVal);
        }

        public override bool Equals(object obj)
        {
            return ((obj == null) || object.ReferenceEquals(this, obj));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(JSONLazyCreator a, object b)
        {
            return ((b == null) || object.ReferenceEquals(a, b));
        }

        public static bool operator !=(JSONLazyCreator a, object b)
        {
            return (a != b);
        }

        private void Set(JSONNode aVal)
        {
            if (this.m_Key == null)
            {
                this.m_Node.Add(aVal);
            }
            else
            {
                this.m_Node.Add(this.m_Key, aVal);
            }
            this.m_Node = null;
        }

        public override string ToString()
        {
            return string.Empty;
        }

        public override string ToString(string aPrefix)
        {
            return string.Empty;
        }

        public override JSONArray AsArray
        {
            get
            {
                JSONArray aVal = new JSONArray();
                this.Set(aVal);
                return aVal;
            }
        }

        public override bool AsBool
        {
            get
            {
                JSONData aVal = new JSONData(false);
                this.Set(aVal);
                return false;
            }
            set
            {
                JSONData aVal = new JSONData(value);
                this.Set(aVal);
            }
        }

        public override double AsDouble
        {
            get
            {
                JSONData aVal = new JSONData(0.0);
                this.Set(aVal);
                return 0.0;
            }
            set
            {
                JSONData aVal = new JSONData(value);
                this.Set(aVal);
            }
        }

        public override float AsFloat
        {
            get
            {
                JSONData aVal = new JSONData(0f);
                this.Set(aVal);
                return 0f;
            }
            set
            {
                JSONData aVal = new JSONData(value);
                this.Set(aVal);
            }
        }

        public override int AsInt
        {
            get
            {
                JSONData aVal = new JSONData(0);
                this.Set(aVal);
                return 0;
            }
            set
            {
                JSONData aVal = new JSONData(value);
                this.Set(aVal);
            }
        }

        public override JSONClass AsObject
        {
            get
            {
                JSONClass aVal = new JSONClass();
                this.Set(aVal);
                return aVal;
            }
        }

        public override JSONNode this[int aIndex]
        {
            get
            {
                return new JSONLazyCreator(this);
            }
            set
            {
                JSONArray aVal = new JSONArray();
                aVal.Add(value);
                this.Set(aVal);
            }
        }

        public override JSONNode this[string aKey]
        {
            get
            {
                return new JSONLazyCreator(this, aKey);
            }
            set
            {
                JSONClass aVal = new JSONClass();
                aVal.Add(aKey, value);
                this.Set(aVal);
            }
        }
    }
}

