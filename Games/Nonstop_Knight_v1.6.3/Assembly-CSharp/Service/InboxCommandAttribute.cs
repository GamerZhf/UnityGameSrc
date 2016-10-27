namespace Service
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class)]
    public class InboxCommandAttribute : Attribute
    {
        [CompilerGenerated]
        private InboxCommandIdType <CommandId>k__BackingField;

        public InboxCommandAttribute(InboxCommandIdType commandId)
        {
            this.CommandId = commandId;
        }

        public InboxCommandIdType CommandId
        {
            [CompilerGenerated]
            get
            {
                return this.<CommandId>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<CommandId>k__BackingField = value;
            }
        }
    }
}

