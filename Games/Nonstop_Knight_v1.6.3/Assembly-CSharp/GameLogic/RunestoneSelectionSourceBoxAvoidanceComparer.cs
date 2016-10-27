namespace GameLogic
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct RunestoneSelectionSourceBoxAvoidanceComparer : IEqualityComparer<RunestoneSelectionSource>
    {
        public bool Equals(RunestoneSelectionSource x, RunestoneSelectionSource y)
        {
            return (x == y);
        }

        public int GetHashCode(RunestoneSelectionSource obj)
        {
            return (int) obj;
        }
    }
}

