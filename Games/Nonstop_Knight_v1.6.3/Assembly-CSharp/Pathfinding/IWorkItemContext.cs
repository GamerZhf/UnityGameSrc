namespace Pathfinding
{
    using System;

    public interface IWorkItemContext
    {
        void EnsureValidFloodFill();
        void QueueFloodFill();
    }
}

