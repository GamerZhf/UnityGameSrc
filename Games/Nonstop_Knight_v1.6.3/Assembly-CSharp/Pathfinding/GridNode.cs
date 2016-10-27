namespace Pathfinding
{
    using Pathfinding.Serialization;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GridNode : GridNodeBase
    {
        private static GridGraph[] _gridGraphs = new GridGraph[0];
        private const int GridFlagsConnectionBit0 = 1;
        private const int GridFlagsConnectionMask = 0xff;
        private const int GridFlagsConnectionOffset = 0;
        private const int GridFlagsEdgeNodeMask = 0x400;
        private const int GridFlagsEdgeNodeOffset = 10;

        public GridNode(AstarPath astar) : base(astar)
        {
        }

        public override void ClearConnections(bool alsoReverse)
        {
            if (alsoReverse)
            {
                GridGraph gridGraph = GetGridGraph(base.GraphIndex);
                for (int i = 0; i < 8; i++)
                {
                    GridNode nodeConnection = gridGraph.GetNodeConnection(this, i);
                    if (nodeConnection != null)
                    {
                        nodeConnection.SetConnectionInternal((i >= 4) ? 7 : ((i + 2) % 4), false);
                    }
                }
            }
            this.ResetConnectionsInternal();
        }

        public Vector3 ClosestPointOnNode(Vector3 p)
        {
            GridGraph gridGraph = GetGridGraph(base.GraphIndex);
            p = gridGraph.inverseMatrix.MultiplyPoint3x4(p);
            float num = this.position.x - 0.5f;
            float num2 = this.position.z - 0.5f;
            int num3 = base.nodeInGridIndex % gridGraph.width;
            int num4 = base.nodeInGridIndex / gridGraph.width;
            float y = gridGraph.inverseMatrix.MultiplyPoint3x4(p).y;
            Vector3 v = new Vector3(Mathf.Clamp(num, num3 - 0.5f, num3 + 0.5f) + 0.5f, y, Mathf.Clamp(num2, num4 - 0.5f, num4 + 0.5f) + 0.5f);
            return gridGraph.matrix.MultiplyPoint3x4(v);
        }

        public override bool ContainsConnection(GraphNode node)
        {
            GridGraph gridGraph = GetGridGraph(base.GraphIndex);
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            GridNode[] nodes = gridGraph.nodes;
            for (int i = 0; i < 8; i++)
            {
                if (this.GetConnectionInternal(i))
                {
                    GridNode node2 = nodes[base.nodeInGridIndex + neighbourOffsets[i]];
                    if (node2 == node)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void DeserializeNode(GraphSerializationContext ctx)
        {
            base.DeserializeNode(ctx);
            base.position = ctx.DeserializeInt3();
            base.gridFlags = ctx.reader.ReadUInt16();
        }

        public override void FloodFill(Stack<GraphNode> stack, uint region)
        {
            GridGraph gridGraph = GetGridGraph(base.GraphIndex);
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            GridNode[] nodes = gridGraph.nodes;
            for (int i = 0; i < 8; i++)
            {
                if (this.GetConnectionInternal(i))
                {
                    GridNode t = nodes[base.nodeInGridIndex + neighbourOffsets[i]];
                    if ((t != null) && (t.Area != region))
                    {
                        t.Area = region;
                        stack.Push(t);
                    }
                }
            }
        }

        public bool GetConnectionInternal(int dir)
        {
            return (((base.gridFlags >> dir) & 1) != 0);
        }

        public override void GetConnections(GraphNodeDelegate del)
        {
            GridGraph gridGraph = GetGridGraph(base.GraphIndex);
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            GridNode[] nodes = gridGraph.nodes;
            for (int i = 0; i < 8; i++)
            {
                if (this.GetConnectionInternal(i))
                {
                    GridNode node = nodes[base.nodeInGridIndex + neighbourOffsets[i]];
                    if (node != null)
                    {
                        del(node);
                    }
                }
            }
        }

        public static GridGraph GetGridGraph(uint graphIndex)
        {
            return _gridGraphs[graphIndex];
        }

        public override bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
        {
            if (backwards)
            {
                return true;
            }
            GridGraph gridGraph = GetGridGraph(base.GraphIndex);
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            GridNode[] nodes = gridGraph.nodes;
            for (int i = 0; i < 4; i++)
            {
                if (this.GetConnectionInternal(i) && (other == nodes[base.nodeInGridIndex + neighbourOffsets[i]]))
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
            for (int j = 4; j < 8; j++)
            {
                if (this.GetConnectionInternal(j) && (other == nodes[base.nodeInGridIndex + neighbourOffsets[j]]))
                {
                    bool flag = false;
                    bool flag2 = false;
                    if (this.GetConnectionInternal(j - 4))
                    {
                        GridNode node = nodes[base.nodeInGridIndex + neighbourOffsets[j - 4]];
                        if (node.Walkable && node.GetConnectionInternal(((j - 4) + 1) % 4))
                        {
                            flag = true;
                        }
                    }
                    if (this.GetConnectionInternal(((j - 4) + 1) % 4))
                    {
                        GridNode node2 = nodes[base.nodeInGridIndex + neighbourOffsets[((j - 4) + 1) % 4]];
                        if (node2.Walkable && node2.GetConnectionInternal(j - 4))
                        {
                            flag2 = true;
                        }
                    }
                    Vector3 vector3 = (Vector3) (((Vector3) (base.position + other.position)) * 0.5f);
                    Vector3 vector4 = Vector3.Cross(gridGraph.collision.up, (Vector3) (other.position - base.position));
                    vector4.Normalize();
                    vector4 = (Vector3) (vector4 * (gridGraph.nodeSize * 1.4142f));
                    left.Add(vector3 - (!flag2 ? Vector3.zero : vector4));
                    right.Add(vector3 + (!flag ? Vector3.zero : vector4));
                    return true;
                }
            }
            return false;
        }

        public override void Open(Path path, PathNode pathNode, PathHandler handler)
        {
            GridGraph gridGraph = GetGridGraph(base.GraphIndex);
            ushort pathID = handler.PathID;
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            uint[] neighbourCosts = gridGraph.neighbourCosts;
            GridNode[] nodes = gridGraph.nodes;
            for (int i = 0; i < 8; i++)
            {
                if (this.GetConnectionInternal(i))
                {
                    GridNode node = nodes[base.nodeInGridIndex + neighbourOffsets[i]];
                    if (path.CanTraverse(node))
                    {
                        PathNode node2 = handler.GetPathNode(node);
                        uint num3 = neighbourCosts[i];
                        if (node2.pathID != pathID)
                        {
                            node2.parent = pathNode;
                            node2.pathID = pathID;
                            node2.cost = num3;
                            node2.H = path.CalculateHScore(node);
                            node.UpdateG(path, node2);
                            handler.heap.Add(node2);
                        }
                        else if (((pathNode.G + num3) + path.GetTraversalCost(node)) < node2.G)
                        {
                            node2.cost = num3;
                            node2.parent = pathNode;
                            node.UpdateRecursiveG(path, node2, handler);
                        }
                        else if (((node2.G + num3) + path.GetTraversalCost(this)) < pathNode.G)
                        {
                            pathNode.parent = node2;
                            pathNode.cost = num3;
                            this.UpdateRecursiveG(path, pathNode, handler);
                        }
                    }
                }
            }
        }

        public void ResetConnectionsInternal()
        {
            base.gridFlags = (ushort) (base.gridFlags & -256);
        }

        public override void SerializeNode(GraphSerializationContext ctx)
        {
            base.SerializeNode(ctx);
            ctx.SerializeInt3(base.position);
            ctx.writer.Write(base.gridFlags);
        }

        public void SetAllConnectionInternal(int connections)
        {
            base.gridFlags = (ushort) ((base.gridFlags & -256) | connections);
        }

        public void SetConnectionInternal(int dir, bool value)
        {
            base.gridFlags = (ushort) ((base.gridFlags & ~(((int) 1) << dir)) | (((value == null) ? 0 : 1) << dir));
        }

        public static void SetGridGraph(int graphIndex, GridGraph graph)
        {
            if (_gridGraphs.Length <= graphIndex)
            {
                GridGraph[] graphArray = new GridGraph[graphIndex + 1];
                for (int i = 0; i < _gridGraphs.Length; i++)
                {
                    graphArray[i] = _gridGraphs[i];
                }
                _gridGraphs = graphArray;
            }
            _gridGraphs[graphIndex] = graph;
        }

        public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
        {
            GridGraph gridGraph = GetGridGraph(base.GraphIndex);
            int[] neighbourOffsets = gridGraph.neighbourOffsets;
            GridNode[] nodes = gridGraph.nodes;
            base.UpdateG(path, pathNode);
            handler.heap.Add(pathNode);
            ushort pathID = handler.PathID;
            for (int i = 0; i < 8; i++)
            {
                if (this.GetConnectionInternal(i))
                {
                    GridNode node = nodes[base.nodeInGridIndex + neighbourOffsets[i]];
                    PathNode node2 = handler.GetPathNode(node);
                    if ((node2.parent == pathNode) && (node2.pathID == pathID))
                    {
                        node.UpdateRecursiveG(path, node2, handler);
                    }
                }
            }
        }

        public bool EdgeNode
        {
            get
            {
                return ((base.gridFlags & 0x400) != 0);
            }
            set
            {
                base.gridFlags = (ushort) ((base.gridFlags & -1025) | (!value ? 0 : 0x400));
            }
        }

        internal ushort InternalGridFlags
        {
            get
            {
                return base.gridFlags;
            }
            set
            {
                base.gridFlags = value;
            }
        }

        public int XCoordInGrid
        {
            get
            {
                return (base.nodeInGridIndex % GetGridGraph(base.GraphIndex).width);
            }
        }

        public int ZCoordInGrid
        {
            get
            {
                return (base.nodeInGridIndex / GetGridGraph(base.GraphIndex).width);
            }
        }
    }
}

