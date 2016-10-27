namespace Facebook.Unity
{
    using System;
    using System.Runtime.CompilerServices;

    internal class ShareResult : ResultBase, IResult, IShareResult
    {
        [CompilerGenerated]
        private string <PostId>k__BackingField;

        internal ShareResult(string result) : base(result)
        {
            object obj2;
            if ((this.ResultDictionary != null) && this.ResultDictionary.TryGetValue("id", out obj2))
            {
                this.PostId = obj2 as string;
            }
        }

        public string PostId
        {
            [CompilerGenerated]
            get
            {
                return this.<PostId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PostId>k__BackingField = value;
            }
        }
    }
}

