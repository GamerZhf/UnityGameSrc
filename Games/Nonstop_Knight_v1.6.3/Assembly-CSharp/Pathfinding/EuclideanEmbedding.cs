namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Serializable]
    public class EuclideanEmbedding
    {
        private uint[] costs = new uint[8];
        [NonSerialized]
        public bool dirty;
        private object lockObj = new object();
        private int maxNodeIndex;
        public HeuristicOptimizationMode mode;
        private int pivotCount;
        public Transform pivotPointRoot;
        private GraphNode[] pivots;
        private const uint ra = 0xc39ec3;
        private const uint rc = 0x43fd43fd;
        private uint rval;
        public int seed;
        public int spreadOutCount = 1;

        private void ApplyGridGraphEndpointSpecialCase()
        {
            NavGraph[] graphs = AstarPath.active.graphs;
            for (int i = 0; i < graphs.Length; i++)
            {
                GridGraph graph = graphs[i] as GridGraph;
                if (graph != null)
                {
                    GridNode[] nodes = graph.nodes;
                    int num2 = (graph.neighbours != NumNeighbours.Four) ? ((graph.neighbours != NumNeighbours.Eight) ? 6 : 8) : 4;
                    for (int j = 0; j < graph.depth; j++)
                    {
                        for (int k = 0; k < graph.width; k++)
                        {
                            GridNode node = nodes[(j * graph.width) + k];
                            if (!node.Walkable)
                            {
                                int num5 = node.NodeIndex * this.pivotCount;
                                for (int m = 0; m < this.pivotCount; m++)
                                {
                                    this.costs[num5 + m] = uint.MaxValue;
                                }
                                for (int n = 0; n < num2; n++)
                                {
                                    int num8;
                                    int num9;
                                    if (graph.neighbours == NumNeighbours.Six)
                                    {
                                        num8 = k + graph.neighbourXOffsets[GridGraph.hexagonNeighbourIndices[n]];
                                        num9 = j + graph.neighbourZOffsets[GridGraph.hexagonNeighbourIndices[n]];
                                    }
                                    else
                                    {
                                        num8 = k + graph.neighbourXOffsets[n];
                                        num9 = j + graph.neighbourZOffsets[n];
                                    }
                                    if (((num8 >= 0) && (num9 >= 0)) && ((num8 < graph.width) && (num9 < graph.depth)))
                                    {
                                        GridNode node2 = graph.nodes[(num9 * graph.width) + num8];
                                        if (node2.Walkable)
                                        {
                                            for (int num10 = 0; num10 < this.pivotCount; num10++)
                                            {
                                                uint num11 = this.costs[(node2.NodeIndex * this.pivotCount) + num10] + graph.neighbourCosts[n];
                                                this.costs[num5 + num10] = Math.Min(this.costs[num5 + num10], num11);
                                                Debug.DrawLine((Vector3) node.position, (Vector3) node2.position, Color.blue, 1f);
                                            }
                                        }
                                    }
                                }
                                for (int num12 = 0; num12 < this.pivotCount; num12++)
                                {
                                    if (this.costs[num5 + num12] == uint.MaxValue)
                                    {
                                        this.costs[num5 + num12] = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void EnsureCapacity(int index)
        {
            if (index > this.maxNodeIndex)
            {
                object lockObj = this.lockObj;
                lock (lockObj)
                {
                    if (index > this.maxNodeIndex)
                    {
                        if (index >= this.costs.Length)
                        {
                            uint[] numArray = new uint[Math.Max((int) (index * 2), (int) (this.pivots.Length * 2))];
                            for (int i = 0; i < this.costs.Length; i++)
                            {
                                numArray[i] = this.costs[i];
                            }
                            this.costs = numArray;
                        }
                        this.maxNodeIndex = index;
                    }
                }
            }
        }

        private void GetClosestWalkableNodesToChildrenRecursively(Transform tr, List<GraphNode> nodes)
        {
            IEnumerator enumerator = tr.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    NNInfo nearest = AstarPath.active.GetNearest(current.position, NNConstraint.Default);
                    if ((nearest.node != null) && nearest.node.Walkable)
                    {
                        nodes.Add(nearest.node);
                    }
                    this.GetClosestWalkableNodesToChildrenRecursively(current, nodes);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
        }

        public uint GetHeuristic(int nodeIndex1, int nodeIndex2)
        {
            nodeIndex1 *= this.pivotCount;
            nodeIndex2 *= this.pivotCount;
            if ((nodeIndex1 >= this.costs.Length) || (nodeIndex2 >= this.costs.Length))
            {
                this.EnsureCapacity((nodeIndex1 <= nodeIndex2) ? nodeIndex2 : nodeIndex1);
            }
            uint num = 0;
            for (int i = 0; i < this.pivotCount; i++)
            {
                uint num3 = (uint) Math.Abs((int) (this.costs[nodeIndex1 + i] - this.costs[nodeIndex2 + i]));
                if (num3 > num)
                {
                    num = num3;
                }
            }
            return num;
        }

        private uint GetRandom()
        {
            this.rval = (0xc39ec3 * this.rval) + 0x43fd43fd;
            return this.rval;
        }

        public void OnDrawGizmos()
        {
            if (this.pivots != null)
            {
                for (int i = 0; i < this.pivots.Length; i++)
                {
                    Gizmos.color = new Color(0.6235294f, 0.3686275f, 0.7607843f, 0.8f);
                    if ((this.pivots[i] != null) && !this.pivots[i].Destroyed)
                    {
                        Gizmos.DrawCube((Vector3) this.pivots[i].position, Vector3.one);
                    }
                }
            }
        }

        private GraphNode PickAnyWalkableNode()
        {
            <PickAnyWalkableNode>c__AnonStorey257 storey = new <PickAnyWalkableNode>c__AnonStorey257();
            NavGraph[] graphs = AstarPath.active.graphs;
            storey.first = null;
            for (int i = 0; i < graphs.Length; i++)
            {
                graphs[i].GetNodes(new GraphNodeDelegateCancelable(storey.<>m__2D));
            }
            return storey.first;
        }

        private void PickNRandomNodes(int count, List<GraphNode> buffer)
        {
            <PickNRandomNodes>c__AnonStorey256 storey = new <PickNRandomNodes>c__AnonStorey256();
            storey.count = count;
            storey.buffer = buffer;
            storey.<>f__this = this;
            storey.n = 0;
            NavGraph[] graphs = AstarPath.active.graphs;
            for (int i = 0; i < graphs.Length; i++)
            {
                graphs[i].GetNodes(new GraphNodeDelegateCancelable(storey.<>m__2C));
            }
        }

        public void RecalculateCosts()
        {
            <RecalculateCosts>c__AnonStorey258 storey = new <RecalculateCosts>c__AnonStorey258();
            storey.<>f__this = this;
            if (this.pivots == null)
            {
                this.RecalculatePivots();
            }
            if (this.mode != HeuristicOptimizationMode.None)
            {
                this.pivotCount = 0;
                for (int i = 0; i < this.pivots.Length; i++)
                {
                    if ((this.pivots[i] != null) && (this.pivots[i].Destroyed || !this.pivots[i].Walkable))
                    {
                        throw new Exception("Invalid pivot nodes (destroyed or unwalkable)");
                    }
                }
                if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
                {
                    for (int j = 0; j < this.pivots.Length; j++)
                    {
                        if (this.pivots[j] == null)
                        {
                            throw new Exception("Invalid pivot nodes (null)");
                        }
                    }
                }
                Debug.Log("Recalculating costs...");
                this.pivotCount = this.pivots.Length;
                storey.startCostCalculation = null;
                storey.numComplete = 0;
                storey.onComplete = new OnPathDelegate(storey.<>m__2E);
                storey.startCostCalculation = new Action<int>(storey.<>m__2F);
                if (this.mode != HeuristicOptimizationMode.RandomSpreadOut)
                {
                    for (int k = 0; k < this.pivots.Length; k++)
                    {
                        storey.startCostCalculation(k);
                    }
                }
                else
                {
                    storey.startCostCalculation(0);
                }
                this.dirty = false;
            }
        }

        public void RecalculatePivots()
        {
            if (this.mode == HeuristicOptimizationMode.None)
            {
                this.pivotCount = 0;
                this.pivots = null;
                return;
            }
            this.rval = (uint) this.seed;
            List<GraphNode> buffer = ListPool<GraphNode>.Claim();
            switch (this.mode)
            {
                case HeuristicOptimizationMode.Random:
                    this.PickNRandomNodes(this.spreadOutCount, buffer);
                    goto Label_0128;

                case HeuristicOptimizationMode.RandomSpreadOut:
                {
                    if (this.pivotPointRoot != null)
                    {
                        this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, buffer);
                    }
                    if (buffer.Count != 0)
                    {
                        break;
                    }
                    GraphNode item = this.PickAnyWalkableNode();
                    if (item != null)
                    {
                        buffer.Add(item);
                        break;
                    }
                    Debug.LogError("Could not find any walkable node in any of the graphs.");
                    ListPool<GraphNode>.Release(buffer);
                    return;
                }
                case HeuristicOptimizationMode.Custom:
                    if (this.pivotPointRoot == null)
                    {
                        throw new Exception("heuristicOptimizationMode is HeuristicOptimizationMode.Custom, but no 'customHeuristicOptimizationPivotsRoot' is set");
                    }
                    this.GetClosestWalkableNodesToChildrenRecursively(this.pivotPointRoot, buffer);
                    goto Label_0128;

                default:
                    throw new Exception("Invalid HeuristicOptimizationMode: " + this.mode);
            }
            int num = this.spreadOutCount - buffer.Count;
            for (int i = 0; i < num; i++)
            {
                buffer.Add(null);
            }
        Label_0128:
            this.pivots = buffer.ToArray();
            ListPool<GraphNode>.Release(buffer);
        }

        [CompilerGenerated]
        private sealed class <PickAnyWalkableNode>c__AnonStorey257
        {
            internal GraphNode first;

            internal bool <>m__2D(GraphNode node)
            {
                if ((node != null) && node.Walkable)
                {
                    this.first = node;
                    return false;
                }
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <PickNRandomNodes>c__AnonStorey256
        {
            internal EuclideanEmbedding <>f__this;
            internal List<GraphNode> buffer;
            internal int count;
            internal int n;

            internal bool <>m__2C(GraphNode node)
            {
                if (!node.Destroyed && node.Walkable)
                {
                    this.n++;
                    if ((((ulong) this.<>f__this.GetRandom()) % ((long) this.n)) < this.count)
                    {
                        if (this.buffer.Count < this.count)
                        {
                            this.buffer.Add(node);
                        }
                        else
                        {
                            this.buffer[(int) (((ulong) this.<>f__this.GetRandom()) % ((long) this.buffer.Count))] = node;
                        }
                    }
                }
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <RecalculateCosts>c__AnonStorey258
        {
            internal EuclideanEmbedding <>f__this;
            internal int numComplete;
            internal OnPathDelegate onComplete;
            internal Action<int> startCostCalculation;

            internal void <>m__2E(Path path)
            {
                this.numComplete++;
                if (this.numComplete == this.<>f__this.pivotCount)
                {
                    Debug.Log("Grid graph special case!");
                    this.<>f__this.ApplyGridGraphEndpointSpecialCase();
                }
            }

            internal void <>m__2F(int k)
            {
                <RecalculateCosts>c__AnonStorey259 storey = new <RecalculateCosts>c__AnonStorey259();
                storey.<>f__ref$600 = this;
                storey.k = k;
                storey.pivot = this.<>f__this.pivots[storey.k];
                storey.fp = null;
                storey.fp = FloodPath.Construct(storey.pivot, this.onComplete);
                storey.fp.immediateCallback = new OnPathDelegate(storey.<>m__30);
                AstarPath.StartPath(storey.fp, true);
            }

            private sealed class <RecalculateCosts>c__AnonStorey259
            {
                internal EuclideanEmbedding.<RecalculateCosts>c__AnonStorey258 <>f__ref$600;
                internal FloodPath fp;
                internal int k;
                internal GraphNode pivot;

                internal void <>m__30(Path _p)
                {
                    <RecalculateCosts>c__AnonStorey25A storeya = new <RecalculateCosts>c__AnonStorey25A();
                    storeya.<>f__ref$600 = this.<>f__ref$600;
                    storeya.<>f__ref$601 = this;
                    _p.Claim(this.<>f__ref$600.<>f__this);
                    MeshNode pivot = this.pivot as MeshNode;
                    storeya.costOffset = 0;
                    if ((pivot != null) && (pivot.connectionCosts != null))
                    {
                        for (int j = 0; j < pivot.connectionCosts.Length; j++)
                        {
                            storeya.costOffset = Math.Max(storeya.costOffset, pivot.connectionCosts[j]);
                        }
                    }
                    NavGraph[] graphs = AstarPath.active.graphs;
                    for (int i = graphs.Length - 1; i >= 0; i--)
                    {
                        graphs[i].GetNodes(new GraphNodeDelegateCancelable(storeya.<>m__31));
                    }
                    if ((this.<>f__ref$600.<>f__this.mode == HeuristicOptimizationMode.RandomSpreadOut) && (this.k < (this.<>f__ref$600.<>f__this.pivots.Length - 1)))
                    {
                        if (this.<>f__ref$600.<>f__this.pivots[this.k + 1] == null)
                        {
                            int nodeIndex = -1;
                            uint num4 = 0;
                            int num5 = this.<>f__ref$600.<>f__this.maxNodeIndex / this.<>f__ref$600.<>f__this.pivotCount;
                            for (int k = 1; k < num5; k++)
                            {
                                uint num7 = 0x40000000;
                                for (int m = 0; m <= this.k; m++)
                                {
                                    num7 = Math.Min(num7, this.<>f__ref$600.<>f__this.costs[(k * this.<>f__ref$600.<>f__this.pivotCount) + m]);
                                }
                                GraphNode node = this.fp.pathHandler.GetPathNode(k).node;
                                if (((num7 > num4) || (nodeIndex == -1)) && (((node != null) && !node.Destroyed) && node.Walkable))
                                {
                                    nodeIndex = k;
                                    num4 = num7;
                                }
                            }
                            if (nodeIndex == -1)
                            {
                                Debug.LogError("Failed generating random pivot points for heuristic optimizations");
                                return;
                            }
                            this.<>f__ref$600.<>f__this.pivots[this.k + 1] = this.fp.pathHandler.GetPathNode(nodeIndex).node;
                        }
                        this.<>f__ref$600.startCostCalculation(this.k + 1);
                    }
                    _p.Release(this.<>f__ref$600.<>f__this, false);
                }

                private sealed class <RecalculateCosts>c__AnonStorey25A
                {
                    internal EuclideanEmbedding.<RecalculateCosts>c__AnonStorey258 <>f__ref$600;
                    internal EuclideanEmbedding.<RecalculateCosts>c__AnonStorey258.<RecalculateCosts>c__AnonStorey259 <>f__ref$601;
                    internal uint costOffset;

                    internal bool <>m__31(GraphNode node)
                    {
                        int index = (node.NodeIndex * this.<>f__ref$600.<>f__this.pivotCount) + this.<>f__ref$601.k;
                        this.<>f__ref$600.<>f__this.EnsureCapacity(index);
                        PathNode pathNode = this.<>f__ref$601.fp.pathHandler.GetPathNode(node);
                        if (this.costOffset > 0)
                        {
                            this.<>f__ref$600.<>f__this.costs[index] = ((pathNode.pathID != this.<>f__ref$601.fp.pathID) || (pathNode.parent == null)) ? 0 : Math.Max((uint) (pathNode.parent.G - this.costOffset), (uint) 0);
                        }
                        else
                        {
                            this.<>f__ref$600.<>f__this.costs[index] = (pathNode.pathID != this.<>f__ref$601.fp.pathID) ? 0 : pathNode.G;
                        }
                        return true;
                    }
                }
            }
        }
    }
}

