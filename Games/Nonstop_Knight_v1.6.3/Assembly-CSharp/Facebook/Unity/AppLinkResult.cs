namespace Facebook.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class AppLinkResult : ResultBase, IAppLinkResult, IResult
    {
        [CompilerGenerated]
        private IDictionary<string, object> <Extras>k__BackingField;
        [CompilerGenerated]
        private string <Ref>k__BackingField;
        [CompilerGenerated]
        private string <TargetUrl>k__BackingField;
        [CompilerGenerated]
        private string <Url>k__BackingField;

        public AppLinkResult(string result) : base(result)
        {
            if (this.ResultDictionary != null)
            {
                string str;
                string str2;
                string str3;
                IDictionary<string, object> dictionary;
                if (Utilities.TryGetValue<string>(this.ResultDictionary, "url", out str))
                {
                    this.Url = str;
                }
                if (Utilities.TryGetValue<string>(this.ResultDictionary, "target_url", out str2))
                {
                    this.TargetUrl = str2;
                }
                if (Utilities.TryGetValue<string>(this.ResultDictionary, "ref", out str3))
                {
                    this.Ref = str3;
                }
                if (Utilities.TryGetValue<IDictionary<string, object>>(this.ResultDictionary, "extras", out dictionary))
                {
                    this.Extras = dictionary;
                }
            }
        }

        public IDictionary<string, object> Extras
        {
            [CompilerGenerated]
            get
            {
                return this.<Extras>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Extras>k__BackingField = value;
            }
        }

        public string Ref
        {
            [CompilerGenerated]
            get
            {
                return this.<Ref>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Ref>k__BackingField = value;
            }
        }

        public string TargetUrl
        {
            [CompilerGenerated]
            get
            {
                return this.<TargetUrl>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TargetUrl>k__BackingField = value;
            }
        }

        public string Url
        {
            [CompilerGenerated]
            get
            {
                return this.<Url>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Url>k__BackingField = value;
            }
        }
    }
}

