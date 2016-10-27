namespace AiView
{
    using System;
    using System.Runtime.CompilerServices;

    public static class Binder
    {
        [CompilerGenerated]
        private static IAiBehaviourSystem <AiBehaviourSystem>k__BackingField;

        public static IAiBehaviourSystem AiBehaviourSystem
        {
            [CompilerGenerated]
            get
            {
                return <AiBehaviourSystem>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                <AiBehaviourSystem>k__BackingField = value;
            }
        }
    }
}

