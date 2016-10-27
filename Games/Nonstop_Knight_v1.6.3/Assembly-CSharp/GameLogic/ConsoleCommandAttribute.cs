namespace GameLogic
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Class)]
    public class ConsoleCommandAttribute : Attribute
    {
        [CompilerGenerated]
        private string <CommandId>k__BackingField;

        public ConsoleCommandAttribute(string commandId)
        {
            this.CommandId = commandId;
        }

        public string CommandId
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

