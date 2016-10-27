namespace Appboy.Utilities
{
    using System;
    using System.IO;

    public class JSONData : JSONNode
    {
        private string m_Data;

        public JSONData(bool aData)
        {
            this.AsBool = aData;
        }

        public JSONData(double aData)
        {
            this.AsDouble = aData;
        }

        public JSONData(int aData)
        {
            this.AsInt = aData;
        }

        public JSONData(float aData)
        {
            this.AsFloat = aData;
        }

        public JSONData(string aData)
        {
            this.m_Data = aData;
        }

        public override void Serialize(BinaryWriter aWriter)
        {
            JSONData data = new JSONData(string.Empty);
            data.AsInt = this.AsInt;
            if (data.m_Data == this.m_Data)
            {
                aWriter.Write((byte) 4);
                aWriter.Write(this.AsInt);
            }
            else
            {
                data.AsFloat = this.AsFloat;
                if (data.m_Data == this.m_Data)
                {
                    aWriter.Write((byte) 7);
                    aWriter.Write(this.AsFloat);
                }
                else
                {
                    data.AsDouble = this.AsDouble;
                    if (data.m_Data == this.m_Data)
                    {
                        aWriter.Write((byte) 5);
                        aWriter.Write(this.AsDouble);
                    }
                    else
                    {
                        data.AsBool = this.AsBool;
                        if (data.m_Data == this.m_Data)
                        {
                            aWriter.Write((byte) 6);
                            aWriter.Write(this.AsBool);
                        }
                        else
                        {
                            aWriter.Write((byte) 3);
                            aWriter.Write(this.m_Data);
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return ("\"" + JSONNode.Escape(this.m_Data) + "\"");
        }

        public override string ToString(string aPrefix)
        {
            return ("\"" + JSONNode.Escape(this.m_Data) + "\"");
        }

        public override string Value
        {
            get
            {
                return this.m_Data;
            }
            set
            {
                this.m_Data = value;
            }
        }
    }
}

