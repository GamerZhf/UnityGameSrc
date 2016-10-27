namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct GraphHitInfo
    {
        public Vector3 origin;
        public Vector3 point;
        public GraphNode node;
        public Vector3 tangentOrigin;
        public Vector3 tangent;
        public GraphHitInfo(Vector3 point)
        {
            this.tangentOrigin = Vector3.zero;
            this.origin = Vector3.zero;
            this.point = point;
            this.node = null;
            this.tangent = Vector3.zero;
        }

        public float distance
        {
            get
            {
                Vector3 vector = this.point - this.origin;
                return vector.magnitude;
            }
        }
    }
}

