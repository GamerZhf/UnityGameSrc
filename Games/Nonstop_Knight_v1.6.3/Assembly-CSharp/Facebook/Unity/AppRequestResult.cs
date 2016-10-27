namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class AppRequestResult : ResultBase, IAppRequestResult, IResult
    {
        [CompilerGenerated]
        private string <RequestID>k__BackingField;
        [CompilerGenerated]
        private IEnumerable<string> <To>k__BackingField;
        public const string RequestIDKey = "request";
        public const string ToKey = "to";

        public AppRequestResult(string result) : base(result)
        {
            if (this.ResultDictionary != null)
            {
                string str;
                string str2;
                if (Utilities.TryGetValue<string>(this.ResultDictionary, "request", out str))
                {
                    this.RequestID = str;
                }
                if (Utilities.TryGetValue<string>(this.ResultDictionary, "to", out str2))
                {
                    char[] separator = new char[] { ',' };
                    this.To = str2.Split(separator);
                }
                else
                {
                    IEnumerable<object> enumerable;
                    if (Utilities.TryGetValue<IEnumerable<object>>(this.ResultDictionary, "to", out enumerable))
                    {
                        List<string> list = new List<string>();
                        IEnumerator<object> enumerator = enumerable.GetEnumerator();
                        try
                        {
                            while (enumerator.MoveNext())
                            {
                                object current = enumerator.Current;
                                string item = current as string;
                                if (item != null)
                                {
                                    list.Add(item);
                                }
                            }
                        }
                        finally
                        {
                            if (enumerator == null)
                            {
                            }
                            enumerator.Dispose();
                        }
                        this.To = list;
                    }
                }
            }
        }

        public string RequestID
        {
            [CompilerGenerated]
            get
            {
                return this.<RequestID>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RequestID>k__BackingField = value;
            }
        }

        public IEnumerable<string> To
        {
            [CompilerGenerated]
            get
            {
                return this.<To>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<To>k__BackingField = value;
            }
        }
    }
}

