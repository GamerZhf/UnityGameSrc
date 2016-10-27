namespace Facebook.Unity
{
    using Facebook.MiniJSON;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal abstract class ResultBase : IInternalResult, IResult
    {
        [CompilerGenerated]
        private string <CallbackId>k__BackingField;
        [CompilerGenerated]
        private bool <Cancelled>k__BackingField;
        [CompilerGenerated]
        private string <Error>k__BackingField;
        [CompilerGenerated]
        private string <RawResult>k__BackingField;
        [CompilerGenerated]
        private IDictionary<string, object> <ResultDictionary>k__BackingField;

        internal ResultBase(string result)
        {
            string error = null;
            bool cancelled = false;
            string callbackId = null;
            if (!string.IsNullOrEmpty(result))
            {
                Dictionary<string, object> dictionary = Json.Deserialize(result) as Dictionary<string, object>;
                if (dictionary != null)
                {
                    this.ResultDictionary = dictionary;
                    error = GetErrorValue(dictionary);
                    cancelled = GetCancelledValue(dictionary);
                    callbackId = GetCallbackId(dictionary);
                }
            }
            this.Init(result, error, cancelled, callbackId);
        }

        internal ResultBase(string result, string error, bool cancelled)
        {
            this.Init(result, error, cancelled, null);
        }

        private static string GetCallbackId(IDictionary<string, object> result)
        {
            string str;
            if ((result != null) && Utilities.TryGetValue<string>(result, "callback_id", out str))
            {
                return str;
            }
            return null;
        }

        private static bool GetCancelledValue(IDictionary<string, object> result)
        {
            object obj2;
            if ((result != null) && result.TryGetValue("cancelled", out obj2))
            {
                bool? nullable = obj2 as bool?;
                if (nullable.HasValue)
                {
                    return (!nullable.HasValue ? false : nullable.Value);
                }
                string str = obj2 as string;
                if (str != null)
                {
                    return Convert.ToBoolean(str);
                }
                int? nullable2 = obj2 as int?;
                if (nullable2.HasValue)
                {
                    return (nullable2.HasValue && (nullable2.Value != 0));
                }
            }
            return false;
        }

        private static string GetErrorValue(IDictionary<string, object> result)
        {
            string str;
            if ((result != null) && Utilities.TryGetValue<string>(result, "error", out str))
            {
                return str;
            }
            return null;
        }

        protected void Init(string result, string error, bool cancelled, string callbackId)
        {
            this.RawResult = result;
            this.Cancelled = cancelled;
            this.Error = error;
            this.CallbackId = callbackId;
        }

        public override string ToString()
        {
            object[] args = new object[] { this.Error, this.ResultDictionary, this.RawResult, this.Cancelled };
            return string.Format("[BaseResult: Error={0}, Result={1}, RawResult={2}, Cancelled={3}]", args);
        }

        public virtual string CallbackId
        {
            [CompilerGenerated]
            get
            {
                return this.<CallbackId>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<CallbackId>k__BackingField = value;
            }
        }

        public virtual bool Cancelled
        {
            [CompilerGenerated]
            get
            {
                return this.<Cancelled>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<Cancelled>k__BackingField = value;
            }
        }

        public virtual string Error
        {
            [CompilerGenerated]
            get
            {
                return this.<Error>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<Error>k__BackingField = value;
            }
        }

        public virtual string RawResult
        {
            [CompilerGenerated]
            get
            {
                return this.<RawResult>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<RawResult>k__BackingField = value;
            }
        }

        public virtual IDictionary<string, object> ResultDictionary
        {
            [CompilerGenerated]
            get
            {
                return this.<ResultDictionary>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<ResultDictionary>k__BackingField = value;
            }
        }
    }
}

