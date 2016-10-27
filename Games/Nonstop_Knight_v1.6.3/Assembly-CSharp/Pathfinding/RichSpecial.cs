namespace Pathfinding
{
    using System;
    using UnityEngine;

    public class RichSpecial : RichPathPart
    {
        public Transform first;
        public NodeLink2 nodeLink;
        public bool reverse;
        public Transform second;

        public RichSpecial Initialize(NodeLink2 nodeLink, GraphNode first)
        {
            this.nodeLink = nodeLink;
            if (first == nodeLink.startNode)
            {
                this.first = nodeLink.StartTransform;
                this.second = nodeLink.EndTransform;
                this.reverse = false;
            }
            else
            {
                this.first = nodeLink.EndTransform;
                this.second = nodeLink.StartTransform;
                this.reverse = true;
            }
            return this;
        }

        public override void OnEnterPool()
        {
            this.nodeLink = null;
        }
    }
}

