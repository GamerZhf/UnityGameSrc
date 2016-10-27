namespace Pathfinding
{
    using System;
    using UnityEngine;

    public class LinkedLevelNode
    {
        public float height;
        public RaycastHit hit;
        public LinkedLevelNode next;
        public Vector3 position;
        public bool walkable;
    }
}

