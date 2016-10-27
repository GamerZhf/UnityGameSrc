namespace Pathfinding
{
    using System;
    using UnityEngine;

    public class FloodPathConstraint : NNConstraint
    {
        private readonly FloodPath path;

        public FloodPathConstraint(FloodPath path)
        {
            if (path == null)
            {
                Debug.LogWarning("FloodPathConstraint should not be used with a NULL path");
            }
            this.path = path;
        }

        public override bool Suitable(GraphNode node)
        {
            return (base.Suitable(node) && this.path.HasPathTo(node));
        }
    }
}

