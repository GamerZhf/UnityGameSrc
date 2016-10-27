namespace Pathfinding
{
    using Pathfinding.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TriangleMeshNode : MeshNode
    {
        protected static INavmeshHolder[] _navmeshHolders = new INavmeshHolder[0];
        protected static readonly object lockObject = new object();
        public int v0;
        public int v1;
        public int v2;

        public TriangleMeshNode(AstarPath astar) : base(astar)
        {
        }

        public override Vector3 ClosestPointOnNode(Vector3 p)
        {
            INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
            return Polygon.ClosestPointOnTriangle((Vector3) navmeshHolder.GetVertex(this.v0), (Vector3) navmeshHolder.GetVertex(this.v1), (Vector3) navmeshHolder.GetVertex(this.v2), p);
        }

        public override Vector3 ClosestPointOnNodeXZ(Vector3 p)
        {
            INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
            Int3 vertex = navmeshHolder.GetVertex(this.v0);
            Int3 num2 = navmeshHolder.GetVertex(this.v1);
            Int3 num3 = navmeshHolder.GetVertex(this.v2);
            Vector2 vector = Polygon.ClosestPointOnTriangle(new Vector2(vertex.x * 0.001f, vertex.z * 0.001f), new Vector2(num2.x * 0.001f, num2.z * 0.001f), new Vector2(num3.x * 0.001f, num3.z * 0.001f), new Vector2(p.x * 0.001f, p.z * 0.001f));
            return new Vector3(vector.x, p.y, vector.y);
        }

        public override bool ContainsPoint(Int3 p)
        {
            INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
            Int3 vertex = navmeshHolder.GetVertex(this.v0);
            Int3 num2 = navmeshHolder.GetVertex(this.v1);
            Int3 num3 = navmeshHolder.GetVertex(this.v2);
            if ((((num2.x - vertex.x) * (p.z - vertex.z)) - ((p.x - vertex.x) * (num2.z - vertex.z))) > 0L)
            {
                return false;
            }
            if ((((num3.x - num2.x) * (p.z - num2.z)) - ((p.x - num2.x) * (num3.z - num2.z))) > 0L)
            {
                return false;
            }
            if ((((vertex.x - num3.x) * (p.z - num3.z)) - ((p.x - num3.x) * (vertex.z - num3.z))) > 0L)
            {
                return false;
            }
            return true;
        }

        public override void DeserializeNode(GraphSerializationContext ctx)
        {
            base.DeserializeNode(ctx);
            this.v0 = ctx.reader.ReadInt32();
            this.v1 = ctx.reader.ReadInt32();
            this.v2 = ctx.reader.ReadInt32();
        }

        public static INavmeshHolder GetNavmeshHolder(uint graphIndex)
        {
            return _navmeshHolders[graphIndex];
        }

        public override bool GetPortal(GraphNode _other, List<Vector3> left, List<Vector3> right, bool backwards)
        {
            int num;
            int num2;
            return this.GetPortal(_other, left, right, backwards, out num, out num2);
        }

        public bool GetPortal(GraphNode _other, List<Vector3> left, List<Vector3> right, bool backwards, out int aIndex, out int bIndex)
        {
            aIndex = -1;
            bIndex = -1;
            if (_other.GraphIndex != base.GraphIndex)
            {
                return false;
            }
            TriangleMeshNode other = _other as TriangleMeshNode;
            int tileIndex = (this.GetVertexIndex(0) >> 12) & 0x7ffff;
            int num2 = (other.GetVertexIndex(0) >> 12) & 0x7ffff;
            if ((tileIndex != num2) && (GetNavmeshHolder(base.GraphIndex) is RecastGraph))
            {
                int num4;
                int num5;
                int num6;
                int num7;
                int num8;
                for (int i = 0; i < base.connections.Length; i++)
                {
                    if (base.connections[i].GraphIndex != base.GraphIndex)
                    {
                        NodeLink3Node node2 = base.connections[i] as NodeLink3Node;
                        if (((node2 != null) && (node2.GetOther(this) == other)) && (left != null))
                        {
                            node2.GetPortal(other, left, right, false);
                            return true;
                        }
                    }
                }
                INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
                navmeshHolder.GetTileCoordinates(tileIndex, out num4, out num6);
                navmeshHolder.GetTileCoordinates(num2, out num5, out num7);
                if (Math.Abs((int) (num4 - num5)) == 1)
                {
                    num8 = 0;
                }
                else if (Math.Abs((int) (num6 - num7)) == 1)
                {
                    num8 = 2;
                }
                else
                {
                    object[] objArray1 = new object[] { "Tiles not adjacent (", num4, ", ", num6, ") (", num5, ", ", num7, ")" };
                    throw new Exception(string.Concat(objArray1));
                }
                int vertexCount = this.GetVertexCount();
                int num10 = other.GetVertexCount();
                int num11 = -1;
                int num12 = -1;
                for (int j = 0; j < vertexCount; j++)
                {
                    int num14 = this.GetVertex(j)[num8];
                    for (int k = 0; k < num10; k++)
                    {
                        if ((num14 == other.GetVertex((k + 1) % num10)[num8]) && (this.GetVertex((j + 1) % vertexCount)[num8] == other.GetVertex(k)[num8]))
                        {
                            num11 = j;
                            num12 = k;
                            j = vertexCount;
                            break;
                        }
                    }
                }
                aIndex = num11;
                bIndex = num12;
                if (num11 != -1)
                {
                    Int3 vertex = this.GetVertex(num11);
                    Int3 num17 = this.GetVertex((num11 + 1) % vertexCount);
                    int num18 = (num8 != 2) ? 2 : 0;
                    int num19 = Math.Min(vertex[num18], num17[num18]);
                    int num20 = Math.Max(vertex[num18], num17[num18]);
                    num19 = Math.Max(num19, Math.Min(other.GetVertex(num12)[num18], other.GetVertex((num12 + 1) % num10)[num18]));
                    num20 = Math.Min(num20, Math.Max(other.GetVertex(num12)[num18], other.GetVertex((num12 + 1) % num10)[num18]));
                    if (vertex[num18] < num17[num18])
                    {
                        vertex[num18] = num19;
                        num17[num18] = num20;
                    }
                    else
                    {
                        vertex[num18] = num20;
                        num17[num18] = num19;
                    }
                    if (left != null)
                    {
                        left.Add((Vector3) vertex);
                        right.Add((Vector3) num17);
                    }
                    return true;
                }
            }
            else if (!backwards)
            {
                int num21 = -1;
                int num22 = -1;
                int num23 = this.GetVertexCount();
                int num24 = other.GetVertexCount();
                for (int m = 0; m < num23; m++)
                {
                    int vertexIndex = this.GetVertexIndex(m);
                    for (int n = 0; n < num24; n++)
                    {
                        if ((vertexIndex == other.GetVertexIndex((n + 1) % num24)) && (this.GetVertexIndex((m + 1) % num23) == other.GetVertexIndex(n)))
                        {
                            num21 = m;
                            num22 = n;
                            m = num23;
                            break;
                        }
                    }
                }
                aIndex = num21;
                bIndex = num22;
                if (num21 == -1)
                {
                    for (int num28 = 0; num28 < base.connections.Length; num28++)
                    {
                        if (base.connections[num28].GraphIndex != base.GraphIndex)
                        {
                            NodeLink3Node node3 = base.connections[num28] as NodeLink3Node;
                            if (((node3 != null) && (node3.GetOther(this) == other)) && (left != null))
                            {
                                node3.GetPortal(other, left, right, false);
                                return true;
                            }
                        }
                    }
                    return false;
                }
                if (left != null)
                {
                    left.Add((Vector3) this.GetVertex(num21));
                    right.Add((Vector3) this.GetVertex((num21 + 1) % num23));
                }
            }
            return true;
        }

        public override Int3 GetVertex(int i)
        {
            return GetNavmeshHolder(base.GraphIndex).GetVertex(this.GetVertexIndex(i));
        }

        public int GetVertexArrayIndex(int i)
        {
            return GetNavmeshHolder(base.GraphIndex).GetVertexArrayIndex((i != 0) ? ((i != 1) ? this.v2 : this.v1) : this.v0);
        }

        public override int GetVertexCount()
        {
            return 3;
        }

        public int GetVertexIndex(int i)
        {
            return ((i != 0) ? ((i != 1) ? this.v2 : this.v1) : this.v0);
        }

        public override void Open(Path path, PathNode pathNode, PathHandler handler)
        {
            if (base.connections != null)
            {
                bool flag = pathNode.flag2;
                for (int i = base.connections.Length - 1; i >= 0; i--)
                {
                    GraphNode node = base.connections[i];
                    if (path.CanTraverse(node))
                    {
                        PathNode node2 = handler.GetPathNode(node);
                        if (node2 != pathNode.parent)
                        {
                            uint currentCost = base.connectionCosts[i];
                            if (flag || node2.flag2)
                            {
                                currentCost = path.GetConnectionSpecialCost(this, node, currentCost);
                            }
                            if (node2.pathID != handler.PathID)
                            {
                                node2.node = node;
                                node2.parent = pathNode;
                                node2.pathID = handler.PathID;
                                node2.cost = currentCost;
                                node2.H = path.CalculateHScore(node);
                                node.UpdateG(path, node2);
                                handler.heap.Add(node2);
                            }
                            else if (((pathNode.G + currentCost) + path.GetTraversalCost(node)) < node2.G)
                            {
                                node2.cost = currentCost;
                                node2.parent = pathNode;
                                node.UpdateRecursiveG(path, node2, handler);
                            }
                            else if ((((node2.G + currentCost) + path.GetTraversalCost(this)) < pathNode.G) && node.ContainsConnection(this))
                            {
                                pathNode.parent = node2;
                                pathNode.cost = currentCost;
                                this.UpdateRecursiveG(path, pathNode, handler);
                            }
                        }
                    }
                }
            }
        }

        public override Vector3 RandomPointOnSurface()
        {
            float num;
            float num2;
            do
            {
                num = UnityEngine.Random.value;
                num2 = UnityEngine.Random.value;
            }
            while ((num + num2) > 1f);
            INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
            return (((Vector3) ((((Vector3) (navmeshHolder.GetVertex(this.v1) - navmeshHolder.GetVertex(this.v0))) * num) + (((Vector3) (navmeshHolder.GetVertex(this.v2) - navmeshHolder.GetVertex(this.v0))) * num2))) + ((Vector3) navmeshHolder.GetVertex(this.v0)));
        }

        public override void SerializeNode(GraphSerializationContext ctx)
        {
            base.SerializeNode(ctx);
            ctx.writer.Write(this.v0);
            ctx.writer.Write(this.v1);
            ctx.writer.Write(this.v2);
        }

        public static void SetNavmeshHolder(int graphIndex, INavmeshHolder graph)
        {
            if (_navmeshHolders.Length <= graphIndex)
            {
                object lockObject = TriangleMeshNode.lockObject;
                lock (lockObject)
                {
                    if (_navmeshHolders.Length <= graphIndex)
                    {
                        INavmeshHolder[] holderArray = new INavmeshHolder[graphIndex + 1];
                        for (int i = 0; i < _navmeshHolders.Length; i++)
                        {
                            holderArray[i] = _navmeshHolders[i];
                        }
                        _navmeshHolders = holderArray;
                    }
                }
            }
            _navmeshHolders[graphIndex] = graph;
        }

        public int SharedEdge(GraphNode other)
        {
            int num;
            int num2;
            this.GetPortal(other, null, null, false, out num, out num2);
            return num;
        }

        public override float SurfaceArea()
        {
            INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
            return (Math.Abs(VectorMath.SignedTriangleAreaTimes2XZ(navmeshHolder.GetVertex(this.v0), navmeshHolder.GetVertex(this.v1), navmeshHolder.GetVertex(this.v2))) * 0.5f);
        }

        public void UpdatePositionFromVertices()
        {
            INavmeshHolder navmeshHolder = GetNavmeshHolder(base.GraphIndex);
            base.position = (Int3) (((navmeshHolder.GetVertex(this.v0) + navmeshHolder.GetVertex(this.v1)) + navmeshHolder.GetVertex(this.v2)) * 0.333333f);
        }

        public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
        {
            base.UpdateG(path, pathNode);
            handler.heap.Add(pathNode);
            if (base.connections != null)
            {
                for (int i = 0; i < base.connections.Length; i++)
                {
                    GraphNode node = base.connections[i];
                    PathNode node2 = handler.GetPathNode(node);
                    if ((node2.parent == pathNode) && (node2.pathID == handler.PathID))
                    {
                        node.UpdateRecursiveG(path, node2, handler);
                    }
                }
            }
        }
    }
}

