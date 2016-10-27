namespace Pathfinding.RVO
{
    using System;
    using UnityEngine;

    public class ObstacleVertex
    {
        public bool convex;
        public Vector2 dir;
        public float height;
        public bool ignore;
        public RVOLayer layer = RVOLayer.DefaultObstacle;
        public ObstacleVertex next;
        public Vector3 position;
        public ObstacleVertex prev;
        public bool split;
    }
}

