namespace Pathfinding
{
    using Pathfinding.Serialization;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class LevelGridNode : GridNodeBase
    {
        private static LayerGridGraph[] _gridGraphs = new LayerGridGraph[0];
        private const int ConnectionMask = 15;
        private const int ConnectionStride = 4;
        protected ushort gridConnections;
        protected static LayerGridGraph[] gridGraphs;
        public const int MaxLayerCount = 15;
        public const int NoConnection = 15;

        public LevelGridNode(AstarPath astar) : base(astar)
        {
        }

        public override void ClearConnections(bool alsoReverse)
        {
            if (alsoReverse)
            {
                LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
                int[] neighbourOffsets = gridGraph.neighbourOffsets;
                LevelGridNode[] nodes = gridGraph.nodes;
                for (int i = 0; i < 4; i++)
                {
                    int connectionValue = this.GetConnectionValue(i);
                    if (connectionValue != 15)
                    {
                        LevelGridNode node = nodes[(base.NodeInGridIndex + neighbourOffsets[i]) + ((gridGraph.lastScannedWidth * gridGraph.lastScannedDepth) * connectionValue)];
                        if (node != null)
                        {
                            node.SetConnectionValue((i + 2) % 4, 15);
                        }
                    }
                }
            }
            this.ResetAllGridConnections();
        }

        public override void DeserializeNode(GraphSerializationContext ctx)
        {
            base.DeserializeNode(ctx);
            base.position = ctx.DeserializeInt3();
            base.gridFlags = ctx.reader.ReadUInt16();
            this.gridConnections = ctx.reader.ReadUInt16();
        }

        public override void FloodFill(Stack<GraphNode> stack, uint region)
        {
            int nodeInGridIndex = base.NodeInGridIndex;
            LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            LevelGridNode[] nodes = gridGraph.nodes;
            for (int i = 0; i < 4; i++)
            {
                int connectionValue = this.GetConnectionValue(i);
                if (connectionValue != 15)
                {
                    LevelGridNode t = nodes[(nodeInGridIndex + neighbourOffsets[i]) + ((gridGraph.lastScannedWidth * gridGraph.lastScannedDepth) * connectionValue)];
                    if ((t != null) && (t.Area != region))
                    {
                        t.Area = region;
                        stack.Push(t);
                    }
                }
            }
        }

        public bool GetConnection(int i)
        {
            return (((this.gridConnections >> (i * 4)) & 15) != 15);
        }

        public override void GetConnections(GraphNodeDelegate del)
        {
            int nodeInGridIndex = base.NodeInGridIndex;
            LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            LevelGridNode[] nodes = gridGraph.nodes;
            for (int i = 0; i < 4; i++)
            {
                int connectionValue = this.GetConnectionValue(i);
                if (connectionValue != 15)
                {
                    LevelGridNode node = nodes[(nodeInGridIndex + neighbourOffsets[i]) + ((gridGraph.lastScannedWidth * gridGraph.lastScannedDepth) * connectionValue)];
                    if (node != null)
                    {
                        del(node);
                    }
                }
            }
        }

        public int GetConnectionValue(int dir)
        {
            return ((this.gridConnections >> (dir * 4)) & 15);
        }

        public static LayerGridGraph GetGridGraph(uint graphIndex)
        {
            return _gridGraphs[graphIndex];
        }

        public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
        {
            if (backwards)
            {
                return true;
            }
            LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            LevelGridNode[] nodes = gridGraph.nodes;
            int nodeInGridIndex = base.NodeInGridIndex;
            for (int i = 0; i < 4; i++)
            {
                int connectionValue = this.GetConnectionValue(i);
                if ((connectionValue != 15) && (other == nodes[(nodeInGridIndex + neighbourOffsets[i]) + ((gridGraph.lastScannedWidth * gridGraph.lastScannedDepth) * connectionValue)]))
                {
                    Vector3 vector = (Vector3) (((Vector3) (base.position + other.position)) * 0.5f);
                    Vector3 vector2 = Vector3.Cross(gridGraph.collision.up, (Vector3) (other.position - base.position));
                    vector2.Normalize();
                    vector2 = (Vector3) (vector2 * (gridGraph.nodeSize * 0.5f));
                    left.Add(vector - vector2);
                    right.Add(vector + vector2);
                    return true;
                }
            }
            return false;
        }

        public bool HasAnyGridConnections()
        {
            return (this.gridConnections != 0xffff);
        }

        public override void Open(Path path, PathNode pathNode, PathHandler handler)
        {
            LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            uint[] neighbourCosts = gridGraph.neighbourCosts;
            LevelGridNode[] nodes = gridGraph.nodes;
            int nodeInGridIndex = base.NodeInGridIndex;
            for (int i = 0; i < 4; i++)
            {
                int connectionValue = this.GetConnectionValue(i);
                if (connectionValue != 15)
                {
                    GraphNode node = nodes[(nodeInGridIndex + neighbourOffsets[i]) + ((gridGraph.lastScannedWidth * gridGraph.lastScannedDepth) * connectionValue)];
                    if (path.CanTraverse(node))
                    {
                        PathNode node2 = handler.GetPathNode(node);
                        if (node2.pathID != handler.PathID)
                        {
                            node2.parent = pathNode;
                            node2.pathID = handler.PathID;
                            node2.cost = neighbourCosts[i];
                            node2.H = path.CalculateHScore(node);
                            node.UpdateG(path, node2);
                            handler.heap.Add(node2);
                        }
                        else
                        {
                            uint num4 = neighbourCosts[i];
                            if (((pathNode.G + num4) + path.GetTraversalCost(node)) < node2.G)
                            {
                                node2.cost = num4;
                                node2.parent = pathNode;
                                node.UpdateRecursiveG(path, node2, handler);
                            }
                            else if (((node2.G + num4) + path.GetTraversalCost(this)) < pathNode.G)
                            {
                                pathNode.parent = node2;
                                pathNode.cost = num4;
                                this.UpdateRecursiveG(path, pathNode, handler);
                            }
                        }
                    }
                }
            }
        }

        public void ResetAllGridConnections()
        {
            this.gridConnections = 0xffff;
        }

        public override void SerializeNode(GraphSerializationContext ctx)
        {
            base.SerializeNode(ctx);
            ctx.SerializeInt3(base.position);
            ctx.writer.Write(base.gridFlags);
            ctx.writer.Write(this.gridConnections);
        }

        public void SetConnectionValue(int dir, int value)
        {
            this.gridConnections = (ushort) ((this.gridConnections & ~((ushort) (((int) 15) << (dir * 4)))) | ((ushort) (value << (dir * 4))));
        }

        public static void SetGridGraph(int graphIndex, LayerGridGraph graph)
        {
            if (_gridGraphs.Length <= graphIndex)
            {
                LayerGridGraph[] graphArray = new LayerGridGraph[graphIndex + 1];
                for (int i = 0; i < _gridGraphs.Length; i++)
                {
                    graphArray[i] = _gridGraphs[i];
                }
                _gridGraphs = graphArray;
            }
            _gridGraphs[graphIndex] = graph;
        }

        public void SetPosition(Int3 position)
        {
            base.position = position;
        }

        public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
        {
            handler.heap.Add(pathNode);
            base.UpdateG(path, pathNode);
            LayerGridGraph gridGraph = GetGridGraph(base.GraphIndex);
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            LevelGridNode[] nodes = gridGraph.nodes;
            int nodeInGridIndex = base.NodeInGridIndex;
            for (int i = 0; i < 4; i++)
            {
                int connectionValue = this.GetConnectionValue(i);
                if (connectionValue != 15)
                {
                    LevelGridNode node = nodes[(nodeInGridIndex + neighbourOffsets[i]) + ((gridGraph.lastScannedWidth * gridGraph.lastScannedDepth) * connectionValue)];
                    PathNode node2 = handler.GetPathNode(node);
                    if (((node2 != null) && (node2.parent == pathNode)) && (node2.pathID == handler.PathID))
                    {
                        node.UpdateRecursiveG(path, node2, handler);
                    }
                }
            }
        }
    }
}

