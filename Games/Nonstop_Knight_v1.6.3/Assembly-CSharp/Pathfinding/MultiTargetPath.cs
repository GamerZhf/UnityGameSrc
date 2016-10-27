namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public class MultiTargetPath : ABPath
    {
        public OnPathDelegate[] callbacks;
        public int chosenTarget = -1;
        public HeuristicMode heuristicMode = HeuristicMode.Sequential;
        public bool inverted = true;
        public List<GraphNode>[] nodePaths;
        public Vector3[] originalTargetPoints;
        public bool pathsForAll = true;
        private int sequentialTarget;
        protected int targetNodeCount;
        public GraphNode[] targetNodes;
        public Vector3[] targetPoints;
        public bool[] targetsFound;
        public List<Vector3>[] vectorPaths;

        public override void CalculateStep(long targetTick)
        {
            for (int i = 0; base.CompleteState == PathCompleteState.NotCalculated; i++)
            {
                base.searchedNodes++;
                if (base.currentR.flag1)
                {
                    for (int j = 0; j < this.targetNodes.Length; j++)
                    {
                        if (!this.targetsFound[j] && (base.currentR.node == this.targetNodes[j]))
                        {
                            this.FoundTarget(base.currentR, j);
                            if (base.CompleteState != PathCompleteState.NotCalculated)
                            {
                                break;
                            }
                        }
                    }
                    if (this.targetNodeCount <= 0)
                    {
                        base.CompleteState = PathCompleteState.Complete;
                        break;
                    }
                }
                base.currentR.node.Open(this, base.currentR, base.pathHandler);
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
                }
            }
        }

        private void ChooseShortestPath()
        {
            this.chosenTarget = -1;
            if (this.nodePaths != null)
            {
                uint num = 0x7fffffff;
                for (int i = 0; i < this.nodePaths.Length; i++)
                {
                    List<GraphNode> list = this.nodePaths[i];
                    if (list != null)
                    {
                        uint g = base.pathHandler.GetPathNode(list[!this.inverted ? (list.Count - 1) : 0]).G;
                        if ((this.chosenTarget == -1) || (g < num))
                        {
                            this.chosenTarget = i;
                            num = g;
                        }
                    }
                }
            }
        }

        public override void Cleanup()
        {
            this.ChooseShortestPath();
            this.ResetFlags();
        }

        public static MultiTargetPath Construct(Vector3[] startPoints, Vector3 target, OnPathDelegate[] callbackDelegates, [Optional, DefaultParameterValue(null)] OnPathDelegate callback)
        {
            MultiTargetPath path = Construct(target, startPoints, callbackDelegates, callback);
            path.inverted = true;
            return path;
        }

        public static MultiTargetPath Construct(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, [Optional, DefaultParameterValue(null)] OnPathDelegate callback)
        {
            MultiTargetPath path = PathPool.GetPath<MultiTargetPath>();
            path.Setup(start, targets, callbackDelegates, callback);
            return path;
        }

        public override string DebugString(PathLog logMode)
        {
            if ((logMode == PathLog.None) || (!base.error && (logMode == PathLog.OnlyErrors)))
            {
                return string.Empty;
            }
            StringBuilder debugStringBuilder = base.pathHandler.DebugStringBuilder;
            debugStringBuilder.Length = 0;
            base.DebugStringPrefix(logMode, debugStringBuilder);
            if (!base.error)
            {
                debugStringBuilder.Append("\nShortest path was ");
                debugStringBuilder.Append((this.chosenTarget != -1) ? this.nodePaths[this.chosenTarget].Count.ToString() : "undefined");
                debugStringBuilder.Append(" nodes long");
                if (logMode == PathLog.Heavy)
                {
                    debugStringBuilder.Append("\nPaths (").Append(this.targetsFound.Length).Append("):");
                    for (int i = 0; i < this.targetsFound.Length; i++)
                    {
                        debugStringBuilder.Append("\n\n\tPath ").Append(i).Append(" Found: ").Append(this.targetsFound[i]);
                        if (this.nodePaths[i] != null)
                        {
                            debugStringBuilder.Append("\n\t\tLength: ");
                            debugStringBuilder.Append(this.nodePaths[i].Count);
                            if (this.nodePaths[i][this.nodePaths[i].Count - 1] != null)
                            {
                                PathNode pathNode = base.pathHandler.GetPathNode(base.endNode);
                                if (pathNode != null)
                                {
                                    debugStringBuilder.Append("\n\t\tEnd Node");
                                    debugStringBuilder.Append("\n\t\t\tG: ");
                                    debugStringBuilder.Append(pathNode.G);
                                    debugStringBuilder.Append("\n\t\t\tH: ");
                                    debugStringBuilder.Append(pathNode.H);
                                    debugStringBuilder.Append("\n\t\t\tF: ");
                                    debugStringBuilder.Append(pathNode.F);
                                    debugStringBuilder.Append("\n\t\t\tPoint: ");
                                    debugStringBuilder.Append(this.endPoint.ToString());
                                    debugStringBuilder.Append("\n\t\t\tGraph: ");
                                    debugStringBuilder.Append(base.endNode.GraphIndex);
                                }
                                else
                                {
                                    debugStringBuilder.Append("\n\t\tEnd Node: Null");
                                }
                            }
                        }
                    }
                    debugStringBuilder.Append("\nStart Node");
                    debugStringBuilder.Append("\n\tPoint: ");
                    debugStringBuilder.Append(this.endPoint.ToString());
                    debugStringBuilder.Append("\n\tGraph: ");
                    debugStringBuilder.Append(base.startNode.GraphIndex);
                    debugStringBuilder.Append("\nBinary Heap size at completion: ");
                    debugStringBuilder.AppendLine((base.pathHandler.heap != null) ? ((base.pathHandler.heap.numberOfItems - 2)).ToString() : "Null");
                }
            }
            base.DebugStringSuffix(logMode, debugStringBuilder);
            return debugStringBuilder.ToString();
        }

        protected void FoundTarget(PathNode nodeR, int i)
        {
            nodeR.flag1 = false;
            this.Trace(nodeR);
            this.vectorPaths[i] = base.vectorPath;
            this.nodePaths[i] = base.path;
            base.vectorPath = ListPool<Vector3>.Claim();
            base.path = ListPool<GraphNode>.Claim();
            this.targetsFound[i] = true;
            this.targetNodeCount--;
            if (!this.pathsForAll)
            {
                base.CompleteState = PathCompleteState.Complete;
                this.targetNodeCount = 0;
            }
            else if (this.targetNodeCount <= 0)
            {
                base.CompleteState = PathCompleteState.Complete;
            }
            else
            {
                this.RecalculateHTarget(false);
            }
        }

        public override void Initialize()
        {
            PathNode pathNode = base.pathHandler.GetPathNode(base.startNode);
            pathNode.node = base.startNode;
            pathNode.pathID = base.pathID;
            pathNode.parent = null;
            pathNode.cost = 0;
            pathNode.G = base.GetTraversalCost(base.startNode);
            pathNode.H = base.CalculateHScore(base.startNode);
            for (int i = 0; i < this.targetNodes.Length; i++)
            {
                if (base.startNode == this.targetNodes[i])
                {
                    this.FoundTarget(pathNode, i);
                }
                else if (this.targetNodes[i] != null)
                {
                    base.pathHandler.GetPathNode(this.targetNodes[i]).flag1 = true;
                }
            }
            if (this.targetNodeCount <= 0)
            {
                base.CompleteState = PathCompleteState.Complete;
            }
            else
            {
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

        public override void OnEnterPool()
        {
            if (this.vectorPaths != null)
            {
                for (int i = 0; i < this.vectorPaths.Length; i++)
                {
                    if (this.vectorPaths[i] != null)
                    {
                        ListPool<Vector3>.Release(this.vectorPaths[i]);
                    }
                }
            }
            this.vectorPaths = null;
            base.vectorPath = null;
            if (this.nodePaths != null)
            {
                for (int j = 0; j < this.nodePaths.Length; j++)
                {
                    if (this.nodePaths[j] != null)
                    {
                        ListPool<GraphNode>.Release(this.nodePaths[j]);
                    }
                }
            }
            this.nodePaths = null;
            base.path = null;
            base.OnEnterPool();
        }

        public override void Prepare()
        {
            base.nnConstraint.tags = base.enabledTags;
            NNInfo info = AstarPath.active.GetNearest(base.startPoint, base.nnConstraint, base.startHint);
            base.startNode = info.node;
            if (base.startNode == null)
            {
                base.Error();
            }
            else if (!base.startNode.Walkable)
            {
                base.Error();
            }
            else
            {
                PathNNConstraint nnConstraint = base.nnConstraint as PathNNConstraint;
                if (nnConstraint != null)
                {
                    nnConstraint.SetStart(info.node);
                }
                this.vectorPaths = new List<Vector3>[this.targetPoints.Length];
                this.nodePaths = new List<GraphNode>[this.targetPoints.Length];
                this.targetNodes = new GraphNode[this.targetPoints.Length];
                this.targetsFound = new bool[this.targetPoints.Length];
                this.targetNodeCount = this.targetPoints.Length;
                bool flag = false;
                bool flag2 = false;
                bool flag3 = false;
                for (int i = 0; i < this.targetPoints.Length; i++)
                {
                    NNInfo nearest = AstarPath.active.GetNearest(this.targetPoints[i], base.nnConstraint);
                    this.targetNodes[i] = nearest.node;
                    this.targetPoints[i] = nearest.position;
                    if (this.targetNodes[i] != null)
                    {
                        flag3 = true;
                        base.endNode = this.targetNodes[i];
                    }
                    bool flag4 = false;
                    if ((nearest.node != null) && nearest.node.Walkable)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag4 = true;
                    }
                    if ((nearest.node != null) && (nearest.node.Area == base.startNode.Area))
                    {
                        flag2 = true;
                    }
                    else
                    {
                        flag4 = true;
                    }
                    if (flag4)
                    {
                        this.targetsFound[i] = true;
                        this.targetNodeCount--;
                    }
                }
                base.startPoint = info.position;
                base.startIntPoint = (Int3) base.startPoint;
                if ((base.startNode == null) || !flag3)
                {
                    base.Error();
                }
                else if (!base.startNode.Walkable)
                {
                    base.Error();
                }
                else if (!flag)
                {
                    base.Error();
                }
                else if (!flag2)
                {
                    base.Error();
                }
                else
                {
                    this.RecalculateHTarget(true);
                }
            }
        }

        protected void RebuildOpenList()
        {
            BinaryHeap heap = base.pathHandler.heap;
            for (int i = 0; i < heap.numberOfItems; i++)
            {
                PathNode node = heap.GetNode(i);
                node.H = base.CalculateHScore(node.node);
                heap.SetF(i, node.F);
            }
            base.pathHandler.heap.Rebuild();
        }

        private void RecalculateHTarget(bool firstTime)
        {
            Vector3 position;
            if (!this.pathsForAll)
            {
                base.heuristic = Heuristic.None;
                base.heuristicScale = 0f;
                return;
            }
            switch (this.heuristicMode)
            {
                case HeuristicMode.None:
                    base.heuristic = Heuristic.None;
                    base.heuristicScale = 0f;
                    goto Label_0269;

                case HeuristicMode.Average:
                    if (firstTime)
                    {
                        break;
                    }
                    return;

                case HeuristicMode.MovingAverage:
                    break;

                case HeuristicMode.Midpoint:
                    if (firstTime)
                    {
                        goto Label_00EF;
                    }
                    return;

                case HeuristicMode.MovingMidpoint:
                    goto Label_00EF;

                case HeuristicMode.Sequential:
                    if (firstTime || this.targetsFound[this.sequentialTarget])
                    {
                        float num5 = 0f;
                        for (int k = 0; k < this.targetPoints.Length; k++)
                        {
                            if (!this.targetsFound[k])
                            {
                                Int3 num8 = this.targetNodes[k].position - base.startNode.position;
                                float sqrMagnitude = num8.sqrMagnitude;
                                if (sqrMagnitude > num5)
                                {
                                    num5 = sqrMagnitude;
                                    base.hTarget = (Int3) this.targetPoints[k];
                                    this.sequentialTarget = k;
                                }
                            }
                        }
                        goto Label_0269;
                    }
                    return;

                default:
                    goto Label_0269;
            }
            Vector3 zero = Vector3.zero;
            int num = 0;
            for (int i = 0; i < this.targetPoints.Length; i++)
            {
                if (!this.targetsFound[i])
                {
                    zero += (Vector3) this.targetNodes[i].position;
                    num++;
                }
            }
            if (num == 0)
            {
                throw new Exception("Should not happen");
            }
            zero = (Vector3) (zero / ((float) num));
            base.hTarget = (Int3) zero;
            goto Label_0269;
        Label_00EF:
            position = Vector3.zero;
            Vector3 rhs = Vector3.zero;
            bool flag = false;
            for (int j = 0; j < this.targetPoints.Length; j++)
            {
                if (!this.targetsFound[j])
                {
                    if (!flag)
                    {
                        position = (Vector3) this.targetNodes[j].position;
                        rhs = (Vector3) this.targetNodes[j].position;
                        flag = true;
                    }
                    else
                    {
                        position = Vector3.Min((Vector3) this.targetNodes[j].position, position);
                        rhs = Vector3.Max((Vector3) this.targetNodes[j].position, rhs);
                    }
                }
            }
            Int3 num4 = (Int3) ((position + rhs) * 0.5f);
            base.hTarget = num4;
        Label_0269:
            if (!firstTime)
            {
                this.RebuildOpenList();
            }
        }

        private void ResetFlags()
        {
            if (this.targetNodes != null)
            {
                for (int i = 0; i < this.targetNodes.Length; i++)
                {
                    if (this.targetNodes[i] != null)
                    {
                        base.pathHandler.GetPathNode(this.targetNodes[i]).flag1 = false;
                    }
                }
            }
        }

        public override void ReturnPath()
        {
            if (base.error)
            {
                if (this.callbacks != null)
                {
                    for (int i = 0; i < this.callbacks.Length; i++)
                    {
                        if (this.callbacks[i] != null)
                        {
                            this.callbacks[i](this);
                        }
                    }
                }
                if (base.callback != null)
                {
                    base.callback(this);
                }
            }
            else
            {
                bool flag = false;
                if (this.inverted)
                {
                    base.endPoint = base.startPoint;
                    base.endNode = base.startNode;
                    base.originalEndPoint = base.originalStartPoint;
                }
                for (int j = 0; j < this.nodePaths.Length; j++)
                {
                    if (this.nodePaths[j] != null)
                    {
                        base.CompleteState = PathCompleteState.Complete;
                        flag = true;
                    }
                    else
                    {
                        base.CompleteState = PathCompleteState.Error;
                    }
                    if ((this.callbacks != null) && (this.callbacks[j] != null))
                    {
                        this.SetPathParametersForReturn(j);
                        this.callbacks[j](this);
                        this.vectorPaths[j] = base.vectorPath;
                    }
                }
                if (flag)
                {
                    base.CompleteState = PathCompleteState.Complete;
                    this.SetPathParametersForReturn(this.chosenTarget);
                }
                else
                {
                    base.CompleteState = PathCompleteState.Error;
                }
                if (base.callback != null)
                {
                    base.callback(this);
                }
            }
        }

        private void SetPathParametersForReturn(int target)
        {
            base.path = this.nodePaths[target];
            base.vectorPath = this.vectorPaths[target];
            if (this.inverted)
            {
                base.startNode = this.targetNodes[target];
                base.startPoint = this.targetPoints[target];
                base.originalStartPoint = this.originalTargetPoints[target];
            }
            else
            {
                base.endNode = this.targetNodes[target];
                base.endPoint = this.targetPoints[target];
                base.originalEndPoint = this.originalTargetPoints[target];
            }
        }

        protected void Setup(Vector3 start, Vector3[] targets, OnPathDelegate[] callbackDelegates, OnPathDelegate callback)
        {
            this.inverted = false;
            base.callback = callback;
            this.callbacks = callbackDelegates;
            this.targetPoints = targets;
            base.originalStartPoint = start;
            base.startPoint = start;
            base.startIntPoint = (Int3) start;
            if (targets.Length == 0)
            {
                base.Error();
            }
            else
            {
                base.endPoint = targets[0];
                this.originalTargetPoints = new Vector3[this.targetPoints.Length];
                for (int i = 0; i < this.targetPoints.Length; i++)
                {
                    this.originalTargetPoints[i] = this.targetPoints[i];
                }
            }
        }

        protected override void Trace(PathNode node)
        {
            base.Trace(node);
            if (this.inverted)
            {
                int num = base.path.Count / 2;
                for (int i = 0; i < num; i++)
                {
                    GraphNode node2 = base.path[i];
                    base.path[i] = base.path[(base.path.Count - i) - 1];
                    base.path[(base.path.Count - i) - 1] = node2;
                }
                for (int j = 0; j < num; j++)
                {
                    Vector3 vector = base.vectorPath[j];
                    base.vectorPath[j] = base.vectorPath[(base.vectorPath.Count - j) - 1];
                    base.vectorPath[(base.vectorPath.Count - j) - 1] = vector;
                }
            }
        }

        public enum HeuristicMode
        {
            None,
            Average,
            MovingAverage,
            Midpoint,
            MovingMidpoint,
            Sequential
        }
    }
}

