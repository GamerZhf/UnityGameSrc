﻿namespace Pathfinding
{
    using System;

    public abstract class PathEndingCondition
    {
        protected Path path;

        protected PathEndingCondition()
        {
        }

        public PathEndingCondition(Path p)
        {
            if (p == null)
            {
                throw new ArgumentNullException("p");
            }
            this.path = p;
        }

        public abstract bool TargetFound(PathNode node);
    }
}

