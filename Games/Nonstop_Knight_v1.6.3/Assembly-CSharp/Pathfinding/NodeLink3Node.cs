namespace Pathfinding
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class NodeLink3Node : PointNode
    {
        public NodeLink3 link;
        public Vector3 portalA;
        public Vector3 portalB;

        public NodeLink3Node(AstarPath active) : base(active)
        {
        }

        public GraphNode GetOther(GraphNode a)
        {
            if (base.connections.Length < 2)
            {
                return null;
            }
            if (base.connections.Length != 2)
            {
                throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + base.connections.Length);
            }
            return ((a != base.connections[0]) ? (base.connections[0] as NodeLink3Node).GetOtherInternal(this) : (base.connections[1] as NodeLink3Node).GetOtherInternal(this));
        }

        private GraphNode GetOtherInternal(GraphNode a)
        {
            if (base.connections.Length < 2)
            {
                return null;
            }
            return ((a != base.connections[0]) ? base.connections[0] : base.connections[1]);
        }

        public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
        {
            if (base.connections.Length < 2)
            {
                return false;
            }
            if (base.connections.Length != 2)
            {
                throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + base.connections.Length);
            }
            if (left != null)
            {
                left.Add(this.portalA);
                right.Add(this.portalB);
            }
            return true;
        }
    }
}

