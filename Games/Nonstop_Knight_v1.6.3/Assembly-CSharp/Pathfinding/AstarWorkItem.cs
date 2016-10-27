namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct AstarWorkItem
    {
        public Action init;
        public Action<IWorkItemContext> initWithContext;
        public Func<bool, bool> update;
        public Func<IWorkItemContext, bool, bool> updateWithContext;
        public AstarWorkItem(Func<bool, bool> update)
        {
            this.init = null;
            this.initWithContext = null;
            this.updateWithContext = null;
            this.update = update;
        }

        public AstarWorkItem(Func<IWorkItemContext, bool, bool> update)
        {
            this.init = null;
            this.initWithContext = null;
            this.updateWithContext = update;
            this.update = null;
        }

        public AstarWorkItem(Action init, [Optional, DefaultParameterValue(null)] Func<bool, bool> update)
        {
            this.init = init;
            this.initWithContext = null;
            this.update = update;
            this.updateWithContext = null;
        }

        public AstarWorkItem(Action<IWorkItemContext> init, [Optional, DefaultParameterValue(null)] Func<IWorkItemContext, bool, bool> update)
        {
            this.init = null;
            this.initWithContext = init;
            this.update = null;
            this.updateWithContext = update;
        }
    }
}

