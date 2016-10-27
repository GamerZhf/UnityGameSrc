namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class RandomPath : ABPath
    {
        public Vector3 aim;
        public float aimStrength;
        private PathNode chosenNodeR;
        private int maxGScore;
        private PathNode maxGScoreNodeR;
        private int nodesEvaluatedRep;
        private readonly System.Random rnd;
        public int searchLength;
        public int spread;
        public bool uniform;

        public RandomPath()
        {
            this.spread = 0x1388;
            this.rnd = new System.Random();
        }

        [Obsolete("This constructor is obsolete. Please use the pooling API and the Construct methods")]
        public RandomPath(Vector3 start, int length, [Optional, DefaultParameterValue(null)] OnPathDelegate callback)
        {
            this.spread = 0x1388;
            this.rnd = new System.Random();
            throw new Exception("This constructor is obsolete. Please use the pooling API and the Setup methods");
        }

        public override void CalculateStep(long targetTick)
        {
            for (int i = 0; base.CompleteState == PathCompleteState.NotCalculated; i++)
            {
                base.searchedNodes++;
                if (base.currentR.G >= this.searchLength)
                {
                    if (base.currentR.G <= (this.searchLength + this.spread))
                    {
                        this.nodesEvaluatedRep++;
                        if (this.rnd.NextDouble() <= (1f / ((float) this.nodesEvaluatedRep)))
                        {
                            this.chosenNodeR = base.currentR;
                        }
                        goto Label_00E6;
                    }
                    if (this.chosenNodeR == null)
                    {
                        this.chosenNodeR = base.currentR;
                    }
                    base.CompleteState = PathCompleteState.Complete;
                    break;
                }
                if (base.currentR.G > this.maxGScore)
                {
                    this.maxGScore = (int) base.currentR.G;
                    this.maxGScoreNodeR = base.currentR;
                }
            Label_00E6:
                base.currentR.node.Open(this, base.currentR, base.pathHandler);
                if (base.pathHandler.heap.isEmpty)
                {
                    if (this.chosenNodeR != null)
                    {
                        base.CompleteState = PathCompleteState.Complete;
                    }
                    else if (this.maxGScoreNodeR != null)
                    {
                        this.chosenNodeR = this.maxGScoreNodeR;
                        base.CompleteState = PathCompleteState.Complete;
                    }
                    else
                    {
                        base.Error();
                    }
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
            if (base.CompleteState == PathCompleteState.Complete)
            {
                this.Trace(this.chosenNodeR);
            }
        }

        public static RandomPath Construct(Vector3 start, int length, [Optional, DefaultParameterValue(null)] OnPathDelegate callback)
        {
            RandomPath path = PathPool.GetPath<RandomPath>();
            path.Setup(start, length, callback);
            return path;
        }

        public override void Initialize()
        {
            PathNode pathNode = base.pathHandler.GetPathNode(base.startNode);
            pathNode.node = base.startNode;
            if ((this.searchLength + this.spread) <= 0)
            {
                base.CompleteState = PathCompleteState.Complete;
                this.Trace(pathNode);
            }
            else
            {
                pathNode.pathID = base.pathID;
                pathNode.parent = null;
                pathNode.cost = 0;
                pathNode.G = base.GetTraversalCost(base.startNode);
                pathNode.H = base.CalculateHScore(base.startNode);
                base.startNode.Open(this, pathNode, base.pathHandler);
                base.searchedNodes++;
                if (base.pathHandler.heap.isEmpty)
                {
                    base.Error();
                }
                else
                {
                    base.currentR = base.pathHandler.heap.Remove();
                }
            }
        }

        public override void Prepare()
        {
            base.nnConstraint.tags = base.enabledTags;
            NNInfo info = AstarPath.active.GetNearest(base.startPoint, base.nnConstraint, base.startHint);
            base.startPoint = info.position;
            base.endPoint = base.startPoint;
            base.startIntPoint = (Int3) base.startPoint;
            base.hTarget = (Int3) this.aim;
            base.startNode = info.node;
            base.endNode = base.startNode;
            if ((base.startNode == null) || (base.endNode == null))
            {
                base.Error();
            }
            else if (!base.startNode.Walkable)
            {
                base.Error();
            }
            else
            {
                base.heuristicScale = this.aimStrength;
            }
        }

        public override void Reset()
        {
            base.Reset();
            this.searchLength = 0x1388;
            this.spread = 0x1388;
            this.uniform = true;
            this.aimStrength = 0f;
            this.chosenNodeR = null;
            this.maxGScoreNodeR = null;
            this.maxGScore = 0;
            this.aim = Vector3.zero;
            this.nodesEvaluatedRep = 0;
        }

        public override void ReturnPath()
        {
            if ((base.path != null) && (base.path.Count > 0))
            {
                base.endNode = base.path[base.path.Count - 1];
                base.endPoint = (Vector3) base.endNode.position;
                base.originalEndPoint = base.endPoint;
                base.hTarget = base.endNode.position;
            }
            if (base.callback != null)
            {
                base.callback(this);
            }
        }

        protected RandomPath Setup(Vector3 start, int length, OnPathDelegate callback)
        {
            base.callback = callback;
            this.searchLength = length;
            base.originalStartPoint = start;
            base.originalEndPoint = Vector3.zero;
            base.startPoint = start;
            base.endPoint = Vector3.zero;
            base.startIntPoint = (Int3) start;
            return this;
        }

        public override bool FloodingPath
        {
            get
            {
                return true;
            }
        }

        protected override bool hasEndPoint
        {
            get
            {
                return false;
            }
        }
    }
}

