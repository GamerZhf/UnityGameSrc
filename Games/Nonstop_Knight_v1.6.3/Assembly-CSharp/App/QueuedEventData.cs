namespace App
{
    using System;
    using System.Collections.Generic;

    public abstract class QueuedEventData
    {
        public List<object> Handlers;
        public object Param1;
        public object Param2;
        public object Param3;
        public object Param4;

        protected QueuedEventData()
        {
        }

        public abstract void dispatch(object target);
    }
}

