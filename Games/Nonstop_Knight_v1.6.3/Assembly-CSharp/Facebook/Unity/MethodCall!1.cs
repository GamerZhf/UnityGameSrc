namespace Facebook.Unity
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal abstract class MethodCall<T> where T: IResult
    {
        [CompilerGenerated]
        private FacebookDelegate<T> <Callback>k__BackingField;
        [CompilerGenerated]
        private FacebookBase <FacebookImpl>k__BackingField;
        [CompilerGenerated]
        private string <MethodName>k__BackingField;
        [CompilerGenerated]
        private MethodArguments <Parameters>k__BackingField;

        public MethodCall(FacebookBase facebookImpl, string methodName)
        {
            this.Parameters = new MethodArguments();
            this.FacebookImpl = facebookImpl;
            this.MethodName = methodName;
        }

        public abstract void Call([Optional, DefaultParameterValue(null)] MethodArguments args);

        public FacebookDelegate<T> Callback
        {
            [CompilerGenerated]
            protected get
            {
                return this.<Callback>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Callback>k__BackingField = value;
            }
        }

        protected FacebookBase FacebookImpl
        {
            [CompilerGenerated]
            get
            {
                return this.<FacebookImpl>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<FacebookImpl>k__BackingField = value;
            }
        }

        public string MethodName
        {
            [CompilerGenerated]
            get
            {
                return this.<MethodName>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<MethodName>k__BackingField = value;
            }
        }

        protected MethodArguments Parameters
        {
            [CompilerGenerated]
            get
            {
                return this.<Parameters>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Parameters>k__BackingField = value;
            }
        }
    }
}

