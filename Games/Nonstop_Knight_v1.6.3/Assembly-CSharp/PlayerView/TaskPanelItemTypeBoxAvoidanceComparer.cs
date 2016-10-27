namespace PlayerView
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct TaskPanelItemTypeBoxAvoidanceComparer : IEqualityComparer<TaskPanelItemType>
    {
        public bool Equals(TaskPanelItemType x, TaskPanelItemType y)
        {
            return (x == y);
        }

        public int GetHashCode(TaskPanelItemType obj)
        {
            return (int) obj;
        }
    }
}

