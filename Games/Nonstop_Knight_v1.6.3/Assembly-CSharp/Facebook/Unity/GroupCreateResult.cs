namespace Facebook.Unity
{
    using System;
    using System.Runtime.CompilerServices;

    internal class GroupCreateResult : ResultBase, IGroupCreateResult, IResult
    {
        [CompilerGenerated]
        private string <GroupId>k__BackingField;
        public const string IDKey = "id";

        public GroupCreateResult(string result) : base(result)
        {
            string str;
            if ((this.ResultDictionary != null) && Utilities.TryGetValue<string>(this.ResultDictionary, "id", out str))
            {
                this.GroupId = str;
            }
        }

        public string GroupId
        {
            [CompilerGenerated]
            get
            {
                return this.<GroupId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<GroupId>k__BackingField = value;
            }
        }
    }
}

