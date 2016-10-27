namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public class ABPath : Path
    {
        public bool calculatePartial;
        public GraphNode endHint;
        public GraphNode endNode;
        protected int[] endNodeCosts;
        public Vector3 endPoint;
        private GridNode gridSpecialCaseNode;
        public Vector3 originalEndPoint;
        public Vector3 originalStartPoint;
        protected PathNode partialBestTarget;
        public bool recalcStartEndCosts = true;
        public GraphNode startHint;
        public Int3 startIntPoint;
        public GraphNode startNode;
        public Vector3 startPoint;

        public override void CalculateStep(long targetTick)
        {
            for (int i = 0; base.CompleteState == PathCompleteState.NotCalculated; i++)
            {
                base.searchedNodes++;
                if (base.currentR.flag1)
                {
                    this.CompleteWith(base.currentR.node);
                    break;
                }
                if (base.currentR.H < this.partialBestTarget.H)
                {
                    this.partialBestTarget = base.currentR;
                }
                base.currentR.node.Open(this, base.currentR, base.pathHandler);
                if (base.pathHandler.heap.isEmpty)
                {
                    base.Error();
                    return;
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
                this.Trace(base.currentR);
            }
            else if (this.calculatePartial && (this.partialBestTarget != null))
            {
                base.CompleteState = PathCompleteState.Partial;
                this.Trace(this.partialBestTarget);
            }
        }

        public override void Cleanup()
        {
            if (this.startNode != null)
            {
                PathNode pathNode = base.pathHandler.GetPathNode(this.startNode);
                pathNode.flag1 = false;
                pathNode.flag2 = false;
            }
            if (this.endNode != null)
            {
                PathNode node2 = base.pathHandler.GetPathNode(this.endNode);
                node2.flag1 = false;
                node2.flag2 = false;
            }
            if (this.gridSpecialCaseNode != null)
            {
                PathNode node3 = base.pathHandler.GetPathNode(this.gridSpecialCaseNode);
                node3.flag1 = false;
                node3.flag2 = false;
                this.SetFlagOnSurroundingGridNodes(this.gridSpecialCaseNode, 1, false);
                this.SetFlagOnSurroundingGridNodes(this.gridSpecialCaseNode, 2, false);
            }
        }

        protected virtual void CompletePathIfStartIsValidTarget()
        {
            if (this.hasEndPoint && base.pathHandler.GetPathNode(this.startNode).flag1)
            {
                this.CompleteWith(this.startNode);
                this.Trace(base.pathHandler.GetPathNode(this.startNode));
            }
        }

        private void CompleteWith(GraphNode node)
        {
            if (this.endNode != node)
            {
                GridNode node2 = node as GridNode;
                if (node2 == null)
                {
                    throw new Exception("Some path is not cleaning up the flag1 field. This is a bug.");
                }
                this.endPoint = node2.ClosestPointOnNode(this.originalEndPoint);
                this.endNode = node;
            }
            base.CompleteState = PathCompleteState.Complete;
        }

        public static ABPath Construct(Vector3 start, Vector3 end, [Optional, DefaultParameterValue(null)] OnPathDelegate callback)
        {
            ABPath path = PathPool.GetPath<ABPath>();
            path.Setup(start, end, callback);
            return path;
        }

        public override string DebugString(PathLog logMode)
        {
            if ((logMode == PathLog.None) || (!base.error && (logMode == PathLog.OnlyErrors)))
            {
                return string.Empty;
            }
            StringBuilder text = new StringBuilder();
            base.DebugStringPrefix(logMode, text);
            if (!base.error && (logMode == PathLog.Heavy))
            {
                text.Append("\nSearch Iterations " + base.searchIterations);
                if (this.hasEndPoint && (this.endNode != null))
                {
                    PathNode pathNode = base.pathHandler.GetPathNode(this.endNode);
                    text.Append("\nEnd Node\n\tG: ");
                    text.Append(pathNode.G);
                    text.Append("\n\tH: ");
                    text.Append(pathNode.H);
                    text.Append("\n\tF: ");
                    text.Append(pathNode.F);
                    text.Append("\n\tPoint: ");
                    text.Append(this.endPoint.ToString());
                    text.Append("\n\tGraph: ");
                    text.Append(this.endNode.GraphIndex);
                }
                text.Append("\nStart Node");
                text.Append("\n\tPoint: ");
                text.Append(this.startPoint.ToString());
                text.Append("\n\tGraph: ");
                if (this.startNode != null)
                {
                    text.Append(this.startNode.GraphIndex);
                }
                else
                {
                    text.Append("< null startNode >");
                }
            }
            base.DebugStringSuffix(logMode, text);
            return text.ToString();
        }

        protected virtual bool EndPointGridGraphSpecialCase(GraphNode closestWalkableEndNode)
        {
            GridNode node = closestWalkableEndNode as GridNode;
            if (node != null)
            {
                GridGraph gridGraph = GridNode.GetGridGraph(node.GraphIndex);
                GridNode gridNode = AstarPath.active.GetNearest(this.originalEndPoint, NNConstraint.None, this.endHint).node as GridNode;
                if (((node != gridNode) && (gridNode != null)) && (node.GraphIndex == gridNode.GraphIndex))
                {
                    int num = node.NodeInGridIndex % gridGraph.width;
                    int num2 = node.NodeInGridIndex / gridGraph.width;
                    int num3 = gridNode.NodeInGridIndex % gridGraph.width;
                    int num4 = gridNode.NodeInGridIndex / gridGraph.width;
                    bool flag = false;
                    switch (gridGraph.neighbours)
                    {
                        case NumNeighbours.Four:
                            if (((num == num3) && (Math.Abs((int) (num2 - num4)) == 1)) || ((num2 == num4) && (Math.Abs((int) (num - num3)) == 1)))
                            {
                                flag = true;
                            }
                            break;

                        case NumNeighbours.Eight:
                            if ((Math.Abs((int) (num - num3)) <= 1) && (Math.Abs((int) (num2 - num4)) <= 1))
                            {
                                flag = true;
                            }
                            break;

                        case NumNeighbours.Six:
                            for (int i = 0; i < 6; i++)
                            {
                                int num6 = num3 + gridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[i]];
                                int num7 = num4 + gridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[i]];
                                if ((num == num6) && (num2 == num7))
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            break;

                        default:
                            throw new Exception("Unhandled NumNeighbours");
                    }
                    if (flag)
                    {
                        this.SetFlagOnSurroundingGridNodes(gridNode, 1, true);
                        this.endPoint = (Vector3) gridNode.position;
                        base.hTarget = gridNode.position;
                        this.endNode = gridNode;
                        base.hTargetNode = this.endNode;
                        this.gridSpecialCaseNode = gridNode;
                        return true;
                    }
                }
            }
            return false;
        }

        public override uint GetConnectionSpecialCost(GraphNode a, GraphNode b, uint currentCost)
        {
            if ((this.startNode != null) && (this.endNode != null))
            {
                if (a == this.startNode)
                {
                    Int3 num = this.startIntPoint - ((b != this.endNode) ? b.position : base.hTarget);
                    Int3 num2 = a.position - b.position;
                    return (uint) (num.costMagnitude * ((currentCost * 1.0) / ((double) num2.costMagnitude)));
                }
                if (b == this.startNode)
                {
                    Int3 num3 = this.startIntPoint - ((a != this.endNode) ? a.position : base.hTarget);
                    Int3 num4 = a.position - b.position;
                    return (uint) (num3.costMagnitude * ((currentCost * 1.0) / ((double) num4.costMagnitude)));
                }
                if (a == this.endNode)
                {
                    Int3 num5 = base.hTarget - b.position;
                    Int3 num6 = a.position - b.position;
                    return (uint) (num5.costMagnitude * ((currentCost * 1.0) / ((double) num6.costMagnitude)));
                }
                if (b != this.endNode)
                {
                    return currentCost;
                }
                Int3 num7 = base.hTarget - a.position;
                Int3 num8 = a.position - b.position;
                return (uint) (num7.costMagnitude * ((currentCost * 1.0) / ((double) num8.costMagnitude)));
            }
            if (a == this.startNode)
            {
                Int3 num9 = this.startIntPoint - b.position;
                Int3 num10 = a.position - b.position;
                return (uint) (num9.costMagnitude * ((currentCost * 1.0) / ((double) num10.costMagnitude)));
            }
            if (b == this.startNode)
            {
                Int3 num11 = this.startIntPoint - a.position;
                Int3 num12 = a.position - b.position;
                return (uint) (num11.costMagnitude * ((currentCost * 1.0) / ((double) num12.costMagnitude)));
            }
            return currentCost;
        }

        public Vector3 GetMovementVector(Vector3 point)
        {
            if ((base.vectorPath == null) || (base.vectorPath.Count == 0))
            {
                return Vector3.zero;
            }
            if (base.vectorPath.Count == 1)
            {
                return (base.vectorPath[0] - point);
            }
            float positiveInfinity = float.PositiveInfinity;
            int num2 = 0;
            for (int i = 0; i < (base.vectorPath.Count - 1); i++)
            {
                Vector3 vector2 = VectorMath.ClosestPointOnSegment(base.vectorPath[i], base.vectorPath[i + 1], point) - point;
                float sqrMagnitude = vector2.sqrMagnitude;
                if (sqrMagnitude < positiveInfinity)
                {
                    positiveInfinity = sqrMagnitude;
                    num2 = i;
                }
            }
            return (base.vectorPath[num2 + 1] - point);
        }

        public override void Initialize()
        {
            if (this.startNode != null)
            {
                base.pathHandler.GetPathNode(this.startNode).flag2 = true;
            }
            if (this.endNode != null)
            {
                base.pathHandler.GetPathNode(this.endNode).flag2 = true;
            }
            PathNode pathNode = base.pathHandler.GetPathNode(this.startNode);
            pathNode.node = this.startNode;
            pathNode.pathID = base.pathHandler.PathID;
            pathNode.parent = null;
            pathNode.cost = 0;
            pathNode.G = base.GetTraversalCost(this.startNode);
            pathNode.H = base.CalculateHScore(this.startNode);
            this.CompletePathIfStartIsValidTarget();
            if (base.CompleteState != PathCompleteState.Complete)
            {
                this.startNode.Open(this, pathNode, base.pathHandler);
                base.searchedNodes++;
                this.partialBestTarget = pathNode;
                if (base.pathHandler.heap.isEmpty)
                {
                    if (!this.calculatePartial)
                    {
                        base.Error();
                        return;
                    }
                    base.CompleteState = PathCompleteState.Partial;
                    this.Trace(this.partialBestTarget);
                }
                base.currentR = base.pathHandler.heap.Remove();
            }
        }

        public override void Prepare()
        {
            base.nnConstraint.tags = base.enabledTags;
            NNInfo info = AstarPath.active.GetNearest(this.startPoint, base.nnConstraint, this.startHint);
            PathNNConstraint nnConstraint = base.nnConstraint as PathNNConstraint;
            if (nnConstraint != null)
            {
                nnConstraint.SetStart(info.node);
            }
            this.startPoint = info.position;
            this.startIntPoint = (Int3) this.startPoint;
            this.startNode = info.node;
            if (this.startNode == null)
            {
                base.Error();
            }
            else if (!this.startNode.Walkable)
            {
                base.Error();
            }
            else if (this.hasEndPoint)
            {
                NNInfo info2 = AstarPath.active.GetNearest(this.endPoint, base.nnConstraint, this.endHint);
                this.endPoint = info2.position;
                this.endNode = info2.node;
                if ((this.startNode == null) && (this.endNode == null))
                {
                    base.Error();
                }
                else if (this.endNode == null)
                {
                    base.Error();
                }
                else if (!this.endNode.Walkable)
                {
                    base.Error();
                }
                else if (this.startNode.Area != this.endNode.Area)
                {
                    base.Error();
                }
                else if (!this.EndPointGridGraphSpecialCase(info2.node))
                {
                    base.hTarget = (Int3) this.endPoint;
                    base.hTargetNode = this.endNode;
                    base.pathHandler.GetPathNode(this.endNode).flag1 = true;
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            this.startNode = null;
            this.endNode = null;
            this.startHint = null;
            this.endHint = null;
            this.originalStartPoint = Vector3.zero;
            this.originalEndPoint = Vector3.zero;
            this.startPoint = Vector3.zero;
            this.endPoint = Vector3.zero;
            this.calculatePartial = false;
            this.partialBestTarget = null;
            this.startIntPoint = new Int3();
            base.hTarget = new Int3();
            this.endNodeCosts = null;
            this.gridSpecialCaseNode = null;
        }

        public void ResetCosts(Path p)
        {
        }

        private void SetFlagOnSurroundingGridNodes(GridNode gridNode, int flag, bool flagState)
        {
            GridGraph gridGraph = GridNode.GetGridGraph(gridNode.GraphIndex);
            int num = (gridGraph.neighbours != NumNeighbours.Four) ? ((gridGraph.neighbours != NumNeighbours.Eight) ? 6 : 8) : 4;
            int num2 = gridNode.NodeInGridIndex % gridGraph.width;
            int num3 = gridNode.NodeInGridIndex / gridGraph.width;
            if ((flag != 1) && (flag != 2))
            {
                throw new ArgumentOutOfRangeException("flag");
            }
            for (int i = 0; i < num; i++)
            {
                int num5;
                int num6;
                if (gridGraph.neighbours == NumNeighbours.Six)
                {
                    num5 = num2 + gridGraph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[i]];
                    num6 = num3 + gridGraph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[i]];
                }
                else
                {
                    num5 = num2 + gridGraph.neighbourXOffsets[i];
                    num6 = num3 + gridGraph.neighbourZOffsets[i];
                }
                if (((num5 >= 0) && (num6 >= 0)) && ((num5 < gridGraph.width) && (num6 < gridGraph.depth)))
                {
                    GridNode node = gridGraph.nodes[(num6 * gridGraph.width) + num5];
                    PathNode pathNode = base.pathHandler.GetPathNode(node);
                    if (flag == 1)
                    {
                        pathNode.flag1 = flagState;
                    }
                    else
                    {
                        pathNode.flag2 = flagState;
                    }
                }
            }
        }

        protected void Setup(Vector3 start, Vector3 end, OnPathDelegate callbackDelegate)
        {
            base.callback = callbackDelegate;
            this.UpdateStartEnd(start, end);
        }

        protected void UpdateStartEnd(Vector3 start, Vector3 end)
        {
            this.originalStartPoint = start;
            this.originalEndPoint = end;
            this.startPoint = start;
            this.endPoint = end;
            this.startIntPoint = (Int3) start;
            base.hTarget = (Int3) end;
        }

        protected virtual bool hasEndPoint
        {
            get
            {
                return true;
            }
        }
    }
}

