namespace Pathfinding
{
    using System;
    using UnityEngine;

    public class EndingConditionProximity : ABPathEndingCondition
    {
        public float maxDistance;

        public EndingConditionProximity(ABPath p, float maxDistance) : base(p)
        {
            this.maxDistance = 10f;
            this.maxDistance = maxDistance;
        }

        public override bool TargetFound(PathNode node)
        {
            Vector3 vector = ((Vector3) node.node.position) - base.abPath.originalEndPoint;
            return (vector.sqrMagnitude <= (this.maxDistance * this.maxDistance));
        }
    }
}

