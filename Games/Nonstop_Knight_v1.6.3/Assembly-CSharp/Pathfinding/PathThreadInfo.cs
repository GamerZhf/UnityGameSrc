namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PathThreadInfo
    {
        public readonly int threadIndex;
        public readonly AstarPath astar;
        public readonly PathHandler runData;
        public PathThreadInfo(int index, AstarPath astar, PathHandler runData)
        {
            this.threadIndex = index;
            this.astar = astar;
            this.runData = runData;
        }
    }
}

