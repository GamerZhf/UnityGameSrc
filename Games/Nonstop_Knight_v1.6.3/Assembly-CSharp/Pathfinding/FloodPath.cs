namespace Pathfinding
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class FloodPath : Path
    {
        public Vector3 originalStartPoint;
        protected Dictionary<GraphNode, GraphNode> parents;
        public bool saveParents = true;
        public GraphNode startNode;
        public Vector3 startPoint;

        public override void CalculateStep(long targetTick)
        {
            for (int i = 0; base.CompleteState == PathCompleteState.NotCalculated; i++)
            {
                base.searchedNodes++;
                base.currentR.node.Open(this, base.currentR, base.pathHandler);
                if (this.saveParents)
                {
                    this.parents[base.currentR.node] = base.currentR.parent.node;
                }
                if (base.pathHandler.heap.isEmpty)
                {
                    base.CompleteState = PathCompleteState.Complete;
                    break;
                }
                base.currentR = base.pathHandler.heap.Remove();
                if (i > 500)
                {
                    if (DateTime.UtcNow.Ticks >= targetTick)
                    {
                        return;
                    }
                    i = 0;
                    if (base.searchedNodes > 0xf4240)
                    {
                        throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
                    }
                }
            }
        }

        public static FloodPath Construct(GraphNode start, [Optional, DefaultParameterValue(null)] OnPathDelegate callback)
        {
            if (start == null)
            {
                throw new ArgumentNullException("start");
            }
            FloodPath path = PathPool.GetPath<FloodPath>();
            path.Setup(start, callback);
            return path;
        }

        public static FloodPath Construct(Vector3 start, [Optional, DefaultParameterValue(null)] OnPathDelegate callback)
        {
            FloodPath path = PathPool.GetPath<FloodPath>();
            path.Setup(start, callback);
            return path;
        }

        public GraphNode GetParent(GraphNode node)
        {
            return this.parents[node];
        }

        public bool HasPathTo(GraphNode node)
        {
            return ((this.parents != null) && this.parents.ContainsKey(node));
        }

        public override void Initialize()
        {
            PathNode pathNode = base.pathHandler.GetPathNode(this.startNode);
            pathNode.node = this.startNode;
            pathNode.pathID = base.pathHandler.PathID;
            pathNode.parent = null;
            pathNode.cost = 0;
            pathNode.G = base.GetTraversalCost(this.startNode);
            pathNode.H = base.CalculateHScore(this.startNode);
            this.parents[this.startNode] = null;
            this.startNode.Open(this, pathNode, base.pathHandler);
            base.searchedNodes++;
            if (base.pathHandler.heap.isEmpty)
            {
                base.CompleteState = PathCompleteState.Complete;
            }
            base.currentR = base.pathHandler.heap.Remove();
        }

        public override void Prepare()
        {
            if (this.startNode == null)
            {
                base.nnConstraint.tags = base.enabledTags;
                NNInfo nearest = AstarPath.active.GetNearest(this.originalStartPoint, base.nnConstraint);
                this.startPoint = nearest.position;
                this.startNode = nearest.node;
            }
            else
            {
                this.startPoint = (Vector3) this.startNode.position;
            }
            if (this.startNode == null)
            {
                base.Error();
            }
            else if (!this.startNode.Walkable)
            {
                base.Error();
            }
        }

        public override void Reset()
        {
            base.Reset();
            this.originalStartPoint = Vector3.zero;
            this.startPoint = Vector3.zero;
            this.startNode = null;
            this.parents = new Dictionary<GraphNode, GraphNode>();
            this.saveParents = true;
        }

        protected void Setup(GraphNode start, OnPathDelegate callback)
        {
            base.callback = callback;
            this.originalStartPoint = (Vector3) start.position;
            this.startNode = start;
            this.startPoint = (Vector3) start.position;
            base.heuristic = Heuristic.None;
        }

        protected void Setup(Vector3 start, OnPathDelegate callback)
        {
            base.callback = callback;
            this.originalStartPoint = start;
            this.startPoint = start;
            base.heuristic = Heuristic.None;
        }

        public override bool FloodingPath
        {
            get
            {
                return true;
            }
        }
    }
}

