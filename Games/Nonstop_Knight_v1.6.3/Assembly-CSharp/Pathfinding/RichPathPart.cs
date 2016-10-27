namespace Pathfinding
{
    using Pathfinding.Util;
    using System;

    public abstract class RichPathPart : IAstarPooledObject
    {
        protected RichPathPart()
        {
        }

        public abstract void OnEnterPool();
    }
}

