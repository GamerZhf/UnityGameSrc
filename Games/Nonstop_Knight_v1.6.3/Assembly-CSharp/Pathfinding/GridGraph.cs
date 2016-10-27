namespace Pathfinding
{
    using Pathfinding.Serialization;
    using Pathfinding.Serialization.JsonFx;
    using Pathfinding.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    [JsonOptIn]
    public class GridGraph : NavGraph, IUpdatableGraph, IRaycastableGraph
    {
        [CompilerGenerated]
        private Matrix4x4 <boundsMatrix>k__BackingField;
        [CompilerGenerated]
        private Vector2 <size>k__BackingField;
        [JsonMember]
        public float aspectRatio = 1f;
        [JsonMember]
        public float autoLinkDistLimit = 10f;
        [JsonMember]
        public bool autoLinkGrids;
        [JsonMember]
        public Vector3 center;
        [JsonMember]
        public GraphCollision collision;
        [JsonMember]
        public bool cutCorners = true;
        public int depth;
        [JsonMember]
        public int erodeIterations;
        [JsonMember]
        public int erosionFirstTag = 1;
        [JsonMember]
        public bool erosionUseTags;
        public const int getNearestForceOverlap = 2;
        internal static readonly int[] hexagonNeighbourIndices = new int[] { 0, 1, 2, 3, 5, 7 };
        [JsonMember]
        public float isometricAngle;
        [JsonMember]
        public float maxClimb = 0.4f;
        [JsonMember]
        public int maxClimbAxis = 1;
        [JsonMember]
        public float maxSlope = 90f;
        [NonSerialized]
        public readonly uint[] neighbourCosts = new uint[8];
        [NonSerialized]
        public readonly int[] neighbourOffsets = new int[8];
        [JsonMember]
        public NumNeighbours neighbours = NumNeighbours.Eight;
        [NonSerialized]
        public readonly int[] neighbourXOffsets = new int[8];
        [NonSerialized]
        public readonly int[] neighbourZOffsets = new int[8];
        public GridNode[] nodes;
        [JsonMember]
        public float nodeSize = 1f;
        [JsonMember]
        public bool penaltyAngle;
        [JsonMember]
        public float penaltyAngleFactor = 100f;
        [JsonMember]
        public float penaltyAnglePower = 1f;
        [JsonMember]
        public bool penaltyPosition;
        [JsonMember]
        public float penaltyPositionFactor = 1f;
        [JsonMember]
        public float penaltyPositionOffset;
        [JsonMember]
        public Vector3 rotation;
        [JsonMember]
        public TextureData textureData = new TextureData();
        [JsonMember]
        public Vector2 unclampedSize = new Vector2(10f, 10f);
        [JsonMember]
        public bool uniformEdgeCosts;
        [JsonMember]
        public bool useJumpPointSearch;
        public int width;

        public GridGraph()
        {
            this.nodeSize = 1f;
            this.collision = new GraphCollision();
        }

        public static void CalculateConnections(GridNode node)
        {
            GridGraph graph = AstarData.GetGraph(node) as GridGraph;
            if (graph != null)
            {
                int nodeInGridIndex = node.NodeInGridIndex;
                int x = nodeInGridIndex % graph.width;
                int z = nodeInGridIndex / graph.width;
                graph.CalculateConnections(x, z, node);
            }
        }

        public virtual void CalculateConnections(int x, int z, GridNode node)
        {
            if (!node.Walkable)
            {
                node.ResetConnectionsInternal();
            }
            else
            {
                int nodeInGridIndex = node.NodeInGridIndex;
                if ((this.neighbours == NumNeighbours.Four) || (this.neighbours == NumNeighbours.Eight))
                {
                    int num2 = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        int num4 = x + this.neighbourXOffsets[i];
                        int num5 = z + this.neighbourZOffsets[i];
                        if ((((num4 >= 0) & (num5 >= 0)) & (num4 < this.width)) & (num5 < this.depth))
                        {
                            GridNode node2 = this.nodes[nodeInGridIndex + this.neighbourOffsets[i]];
                            if (this.IsValidConnection(node, node2))
                            {
                                num2 |= ((int) 1) << i;
                            }
                        }
                    }
                    int num6 = 0;
                    if (this.neighbours == NumNeighbours.Eight)
                    {
                        if (this.cutCorners)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (((((num2 >> j) | (num2 >> (j + 1))) | (num2 >> ((j + 1) - 4))) & 1) != 0)
                                {
                                    int index = j + 4;
                                    int num9 = x + this.neighbourXOffsets[index];
                                    int num10 = z + this.neighbourZOffsets[index];
                                    if ((((num9 >= 0) & (num10 >= 0)) & (num9 < this.width)) & (num10 < this.depth))
                                    {
                                        GridNode node3 = this.nodes[nodeInGridIndex + this.neighbourOffsets[index]];
                                        if (this.IsValidConnection(node, node3))
                                        {
                                            num6 |= ((int) 1) << index;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                if ((((num2 >> k) & 1) != 0) && ((((num2 >> (k + 1)) | (num2 >> ((k + 1) - 4))) & 1) != 0))
                                {
                                    GridNode node4 = this.nodes[nodeInGridIndex + this.neighbourOffsets[k + 4]];
                                    if (this.IsValidConnection(node, node4))
                                    {
                                        num6 |= ((int) 1) << (k + 4);
                                    }
                                }
                            }
                        }
                    }
                    node.SetAllConnectionInternal(num2 | num6);
                }
                else
                {
                    node.ResetConnectionsInternal();
                    for (int m = 0; m < hexagonNeighbourIndices.Length; m++)
                    {
                        int num13 = hexagonNeighbourIndices[m];
                        int num14 = x + this.neighbourXOffsets[num13];
                        int num15 = z + this.neighbourZOffsets[num13];
                        if ((((num14 >= 0) & (num15 >= 0)) & (num14 < this.width)) & (num15 < this.depth))
                        {
                            GridNode node5 = this.nodes[nodeInGridIndex + this.neighbourOffsets[num13]];
                            node.SetConnectionInternal(num13, this.IsValidConnection(node, node5));
                        }
                    }
                }
            }
        }

        [Obsolete("CalculateConnections no longer takes a node array, it just uses the one on the graph")]
        public virtual void CalculateConnections(GridNode[] nodes, int x, int z, GridNode node)
        {
            this.CalculateConnections(x, z, node);
        }

        public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
        {
            return GraphUpdateThreading.UnityThread;
        }

        public bool CheckConnection(GridNode node, int dir)
        {
            if (((this.neighbours == NumNeighbours.Eight) || (this.neighbours == NumNeighbours.Six)) || (dir < 4))
            {
                return this.HasNodeConnection(node, dir);
            }
            int num = ((dir - 4) - 1) & 3;
            int num2 = ((dir - 4) + 1) & 3;
            if (!this.HasNodeConnection(node, num) || !this.HasNodeConnection(node, num2))
            {
                return false;
            }
            GridNode node2 = this.nodes[node.NodeInGridIndex + this.neighbourOffsets[num]];
            GridNode node3 = this.nodes[node.NodeInGridIndex + this.neighbourOffsets[num2]];
            if (!node2.Walkable || !node3.Walkable)
            {
                return false;
            }
            return (this.HasNodeConnection(node3, num) && this.HasNodeConnection(node2, num2));
        }

        protected bool ClipLineSegmentToBounds(Vector3 a, Vector3 b, out Vector3 outA, out Vector3 outB)
        {
            if ((((a.x < 0f) || (a.z < 0f)) || ((a.x > this.width) || (a.z > this.depth))) || (((b.x < 0f) || (b.z < 0f)) || ((b.x > this.width) || (b.z > this.depth))))
            {
                bool flag;
                Vector3 vector = new Vector3(0f, 0f, 0f);
                Vector3 vector2 = new Vector3(0f, 0f, (float) this.depth);
                Vector3 vector3 = new Vector3((float) this.width, 0f, (float) this.depth);
                Vector3 vector4 = new Vector3((float) this.width, 0f, 0f);
                int num = 0;
                Vector3 vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector, vector2, out flag);
                if (flag)
                {
                    num++;
                    if (!VectorMath.RightOrColinearXZ(vector, vector2, a))
                    {
                        a = vector5;
                    }
                    else
                    {
                        b = vector5;
                    }
                }
                vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector2, vector3, out flag);
                if (flag)
                {
                    num++;
                    if (!VectorMath.RightOrColinearXZ(vector2, vector3, a))
                    {
                        a = vector5;
                    }
                    else
                    {
                        b = vector5;
                    }
                }
                vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector3, vector4, out flag);
                if (flag)
                {
                    num++;
                    if (!VectorMath.RightOrColinearXZ(vector3, vector4, a))
                    {
                        a = vector5;
                    }
                    else
                    {
                        b = vector5;
                    }
                }
                vector5 = VectorMath.SegmentIntersectionPointXZ(a, b, vector4, vector, out flag);
                if (flag)
                {
                    num++;
                    if (!VectorMath.RightOrColinearXZ(vector4, vector, a))
                    {
                        a = vector5;
                    }
                    else
                    {
                        b = vector5;
                    }
                }
                if (num == 0)
                {
                    outA = Vector3.zero;
                    outB = Vector3.zero;
                    return false;
                }
            }
            outA = a;
            outB = b;
            return true;
        }

        public override int CountNodes()
        {
            return this.nodes.Length;
        }

        protected static float CrossMagnitude(Vector2 a, Vector2 b)
        {
            return ((a.x * b.y) - (b.x * a.y));
        }

        public override void DeserializeExtraInfo(GraphSerializationContext ctx)
        {
            int num = ctx.reader.ReadInt32();
            if (num == -1)
            {
                this.nodes = null;
            }
            else
            {
                this.nodes = new GridNode[num];
                for (int i = 0; i < this.nodes.Length; i++)
                {
                    this.nodes[i] = new GridNode(base.active);
                    this.nodes[i].DeserializeNode(ctx);
                }
            }
        }

        public virtual void ErodeWalkableArea()
        {
            this.ErodeWalkableArea(0, 0, this.Width, this.Depth);
        }

        public virtual void ErodeWalkableArea(int xmin, int zmin, int xmax, int zmax)
        {
            xmin = Mathf.Clamp(xmin, 0, this.Width);
            xmax = Mathf.Clamp(xmax, 0, this.Width);
            zmin = Mathf.Clamp(zmin, 0, this.Depth);
            zmax = Mathf.Clamp(zmax, 0, this.Depth);
            if (!this.erosionUseTags)
            {
                for (int i = 0; i < this.erodeIterations; i++)
                {
                    for (int j = zmin; j < zmax; j++)
                    {
                        for (int m = xmin; m < xmax; m++)
                        {
                            GridNode node = this.nodes[(j * this.Width) + m];
                            if (node.Walkable && this.ErosionAnyFalseConnections(node))
                            {
                                node.Walkable = false;
                            }
                        }
                    }
                    for (int k = zmin; k < zmax; k++)
                    {
                        for (int n = xmin; n < xmax; n++)
                        {
                            GridNode node2 = this.nodes[(k * this.Width) + n];
                            this.CalculateConnections(n, k, node2);
                        }
                    }
                }
            }
            else if ((this.erodeIterations + this.erosionFirstTag) > 0x1f)
            {
                UnityEngine.Debug.LogError(string.Concat(new object[] { "Too few tags available for ", this.erodeIterations, " erode iterations and starting with tag ", this.erosionFirstTag, " (erodeIterations+erosionFirstTag > 31)" }));
            }
            else if (this.erosionFirstTag <= 0)
            {
                UnityEngine.Debug.LogError("First erosion tag must be greater or equal to 1");
            }
            else
            {
                for (int num6 = 0; num6 < this.erodeIterations; num6++)
                {
                    for (int num7 = zmin; num7 < zmax; num7++)
                    {
                        for (int num8 = xmin; num8 < xmax; num8++)
                        {
                            GridNode node3 = this.nodes[(num7 * this.width) + num8];
                            if ((node3.Walkable && (node3.Tag >= this.erosionFirstTag)) && (node3.Tag < (this.erosionFirstTag + num6)))
                            {
                                if (this.neighbours == NumNeighbours.Six)
                                {
                                    for (int num9 = 0; num9 < 6; num9++)
                                    {
                                        GridNode nodeConnection = this.GetNodeConnection(node3, hexagonNeighbourIndices[num9]);
                                        if (nodeConnection != null)
                                        {
                                            uint tag = nodeConnection.Tag;
                                            if ((tag > (this.erosionFirstTag + num6)) || (tag < this.erosionFirstTag))
                                            {
                                                nodeConnection.Tag = (uint) (this.erosionFirstTag + num6);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int num11 = 0; num11 < 4; num11++)
                                    {
                                        GridNode node5 = this.GetNodeConnection(node3, num11);
                                        if (node5 != null)
                                        {
                                            uint num12 = node5.Tag;
                                            if ((num12 > (this.erosionFirstTag + num6)) || (num12 < this.erosionFirstTag))
                                            {
                                                node5.Tag = (uint) (this.erosionFirstTag + num6);
                                            }
                                        }
                                    }
                                }
                            }
                            else if ((node3.Walkable && (num6 == 0)) && this.ErosionAnyFalseConnections(node3))
                            {
                                node3.Tag = (uint) (this.erosionFirstTag + num6);
                            }
                        }
                    }
                }
            }
        }

        private bool ErosionAnyFalseConnections(GridNode node)
        {
            if (this.neighbours == NumNeighbours.Six)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (!this.HasNodeConnection(node, hexagonNeighbourIndices[i]))
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int j = 0; j < 4; j++)
                {
                    if (!this.HasNodeConnection(node, j))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void GenerateMatrix()
        {
            Vector2 unclampedSize = this.unclampedSize;
            unclampedSize.x *= Mathf.Sign(unclampedSize.x);
            unclampedSize.y *= Mathf.Sign(unclampedSize.y);
            this.nodeSize = Mathf.Clamp(this.nodeSize, unclampedSize.x / 1024f, float.PositiveInfinity);
            this.nodeSize = Mathf.Clamp(this.nodeSize, unclampedSize.y / 1024f, float.PositiveInfinity);
            unclampedSize.x = (unclampedSize.x >= this.nodeSize) ? unclampedSize.x : this.nodeSize;
            unclampedSize.y = (unclampedSize.y >= this.nodeSize) ? unclampedSize.y : this.nodeSize;
            this.size = unclampedSize;
            Matrix4x4 matrixx = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 45f, 0f), Vector3.one);
            matrixx = Matrix4x4.Scale(new Vector3(Mathf.Cos(0.01745329f * this.isometricAngle), 1f, 1f)) * matrixx;
            matrixx = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, -45f, 0f), Vector3.one) * matrixx;
            this.boundsMatrix = Matrix4x4.TRS(this.center, Quaternion.Euler(this.rotation), new Vector3(this.aspectRatio, 1f, 1f)) * matrixx;
            this.width = Mathf.FloorToInt(this.size.x / this.nodeSize);
            this.depth = Mathf.FloorToInt(this.size.y / this.nodeSize);
            if (Mathf.Approximately(this.size.x / this.nodeSize, (float) Mathf.CeilToInt(this.size.x / this.nodeSize)))
            {
                this.width = Mathf.CeilToInt(this.size.x / this.nodeSize);
            }
            if (Mathf.Approximately(this.size.y / this.nodeSize, (float) Mathf.CeilToInt(this.size.y / this.nodeSize)))
            {
                this.depth = Mathf.CeilToInt(this.size.y / this.nodeSize);
            }
            Matrix4x4 m = Matrix4x4.TRS(this.boundsMatrix.MultiplyPoint3x4((Vector3) (-new Vector3(this.size.x, 0f, this.size.y) * 0.5f)), Quaternion.Euler(this.rotation), new Vector3(this.nodeSize * this.aspectRatio, 1f, this.nodeSize)) * matrixx;
            base.SetMatrix(m);
        }

        protected static void GetBoundsMinMax(Bounds b, Matrix4x4 matrix, out Vector3 min, out Vector3 max)
        {
            Vector3[] vectorArray = new Vector3[] { matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, b.extents.y, b.extents.z)), matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, b.extents.y, -b.extents.z)), matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, -b.extents.y, b.extents.z)), matrix.MultiplyPoint3x4(b.center + new Vector3(b.extents.x, -b.extents.y, -b.extents.z)), matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, b.extents.y, b.extents.z)), matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, b.extents.y, -b.extents.z)), matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, -b.extents.y, b.extents.z)), matrix.MultiplyPoint3x4(b.center + new Vector3(-b.extents.x, -b.extents.y, -b.extents.z)) };
            min = vectorArray[0];
            max = vectorArray[0];
            for (int i = 1; i < 8; i++)
            {
                min = Vector3.Min(min, vectorArray[i]);
                max = Vector3.Max(max, vectorArray[i]);
            }
        }

        public uint GetConnectionCost(int dir)
        {
            return this.neighbourCosts[dir];
        }

        public override NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
        {
            if ((this.nodes == null) || ((this.depth * this.width) != this.nodes.Length))
            {
                return new NNInfoInternal();
            }
            position = this.inverseMatrix.MultiplyPoint3x4(position);
            float f = position.x - 0.5f;
            float num2 = position.z - 0.5f;
            int num3 = Mathf.Clamp(Mathf.RoundToInt(f), 0, this.width - 1);
            int num4 = Mathf.Clamp(Mathf.RoundToInt(num2), 0, this.depth - 1);
            NNInfoInternal internal2 = new NNInfoInternal(this.nodes[(num4 * this.width) + num3]);
            float y = this.inverseMatrix.MultiplyPoint3x4((Vector3) this.nodes[(num4 * this.width) + num3].position).y;
            internal2.clampedPosition = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f, num3 - 0.5f, num3 + 0.5f) + 0.5f, y, Mathf.Clamp(num2, num4 - 0.5f, num4 + 0.5f) + 0.5f));
            return internal2;
        }

        public override NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
        {
            if ((this.nodes == null) || ((this.depth * this.width) != this.nodes.Length))
            {
                return new NNInfoInternal();
            }
            Vector3 vector = position;
            position = this.inverseMatrix.MultiplyPoint3x4(position);
            float f = position.x - 0.5f;
            float num2 = position.z - 0.5f;
            int num3 = Mathf.Clamp(Mathf.RoundToInt(f), 0, this.width - 1);
            int num4 = Mathf.Clamp(Mathf.RoundToInt(num2), 0, this.depth - 1);
            GridNode node = this.nodes[num3 + (num4 * this.width)];
            GridNode node2 = null;
            float positiveInfinity = float.PositiveInfinity;
            int num6 = 2;
            Vector3 zero = Vector3.zero;
            NNInfoInternal internal2 = new NNInfoInternal(null);
            if (constraint.Suitable(node))
            {
                node2 = node;
                Vector3 vector3 = ((Vector3) node2.position) - vector;
                positiveInfinity = vector3.sqrMagnitude;
                float y = this.inverseMatrix.MultiplyPoint3x4((Vector3) node.position).y;
                zero = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f, num3 - 0.5f, num3 + 0.5f) + 0.5f, y, Mathf.Clamp(num2, num4 - 0.5f, num4 + 0.5f) + 0.5f));
            }
            if (node2 != null)
            {
                internal2.node = node2;
                internal2.clampedPosition = zero;
                if (num6 == 0)
                {
                    return internal2;
                }
                num6--;
            }
            float num8 = !constraint.constrainDistance ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistance;
            float num9 = num8 * num8;
            int num10 = 1;
            while (true)
            {
                int num11;
                if ((this.nodeSize * num10) > num8)
                {
                    internal2.node = node2;
                    internal2.clampedPosition = zero;
                    return internal2;
                }
                bool flag = false;
                int num12 = num4 + num10;
                int num13 = num12 * this.width;
                for (num11 = num3 - num10; num11 <= (num3 + num10); num11++)
                {
                    if ((((num11 >= 0) && (num12 >= 0)) && (num11 < this.width)) && (num12 < this.depth))
                    {
                        flag = true;
                        if (constraint.Suitable(this.nodes[num11 + num13]))
                        {
                            Vector3 vector5 = ((Vector3) this.nodes[num11 + num13].position) - vector;
                            float sqrMagnitude = vector5.sqrMagnitude;
                            if ((sqrMagnitude < positiveInfinity) && (sqrMagnitude < num9))
                            {
                                positiveInfinity = sqrMagnitude;
                                node2 = this.nodes[num11 + num13];
                                zero = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f, num11 - 0.5f, num11 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) node2.position).y, Mathf.Clamp(num2, num12 - 0.5f, num12 + 0.5f) + 0.5f));
                            }
                        }
                    }
                }
                num12 = num4 - num10;
                num13 = num12 * this.width;
                for (num11 = num3 - num10; num11 <= (num3 + num10); num11++)
                {
                    if ((((num11 >= 0) && (num12 >= 0)) && (num11 < this.width)) && (num12 < this.depth))
                    {
                        flag = true;
                        if (constraint.Suitable(this.nodes[num11 + num13]))
                        {
                            Vector3 vector7 = ((Vector3) this.nodes[num11 + num13].position) - vector;
                            float num15 = vector7.sqrMagnitude;
                            if ((num15 < positiveInfinity) && (num15 < num9))
                            {
                                positiveInfinity = num15;
                                node2 = this.nodes[num11 + num13];
                                zero = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f, num11 - 0.5f, num11 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) node2.position).y, Mathf.Clamp(num2, num12 - 0.5f, num12 + 0.5f) + 0.5f));
                            }
                        }
                    }
                }
                num11 = num3 - num10;
                for (num12 = (num4 - num10) + 1; num12 <= ((num4 + num10) - 1); num12++)
                {
                    if ((((num11 >= 0) && (num12 >= 0)) && (num11 < this.width)) && (num12 < this.depth))
                    {
                        flag = true;
                        if (constraint.Suitable(this.nodes[num11 + (num12 * this.width)]))
                        {
                            Vector3 vector9 = ((Vector3) this.nodes[num11 + (num12 * this.width)].position) - vector;
                            float num16 = vector9.sqrMagnitude;
                            if ((num16 < positiveInfinity) && (num16 < num9))
                            {
                                positiveInfinity = num16;
                                node2 = this.nodes[num11 + (num12 * this.width)];
                                zero = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f, num11 - 0.5f, num11 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) node2.position).y, Mathf.Clamp(num2, num12 - 0.5f, num12 + 0.5f) + 0.5f));
                            }
                        }
                    }
                }
                num11 = num3 + num10;
                for (num12 = (num4 - num10) + 1; num12 <= ((num4 + num10) - 1); num12++)
                {
                    if ((((num11 >= 0) && (num12 >= 0)) && (num11 < this.width)) && (num12 < this.depth))
                    {
                        flag = true;
                        if (constraint.Suitable(this.nodes[num11 + (num12 * this.width)]))
                        {
                            Vector3 vector11 = ((Vector3) this.nodes[num11 + (num12 * this.width)].position) - vector;
                            float num17 = vector11.sqrMagnitude;
                            if ((num17 < positiveInfinity) && (num17 < num9))
                            {
                                positiveInfinity = num17;
                                node2 = this.nodes[num11 + (num12 * this.width)];
                                zero = this.matrix.MultiplyPoint3x4(new Vector3(Mathf.Clamp(f, num11 - 0.5f, num11 + 0.5f) + 0.5f, this.inverseMatrix.MultiplyPoint3x4((Vector3) node2.position).y, Mathf.Clamp(num2, num12 - 0.5f, num12 + 0.5f) + 0.5f));
                            }
                        }
                    }
                }
                if (node2 != null)
                {
                    if (num6 == 0)
                    {
                        internal2.node = node2;
                        internal2.clampedPosition = zero;
                        return internal2;
                    }
                    num6--;
                }
                if (!flag)
                {
                    internal2.node = node2;
                    internal2.clampedPosition = zero;
                    return internal2;
                }
                num10++;
            }
        }

        protected virtual GridNodeBase GetNeighbourAlongDirection(GridNodeBase node, int direction)
        {
            GridNode node2 = node as GridNode;
            if (node2.GetConnectionInternal(direction))
            {
                return this.nodes[node2.NodeInGridIndex + this.neighbourOffsets[direction]];
            }
            return null;
        }

        public GridNode GetNodeConnection(GridNode node, int dir)
        {
            if (!node.GetConnectionInternal(dir))
            {
                return null;
            }
            if (!node.EdgeNode)
            {
                return this.nodes[node.NodeInGridIndex + this.neighbourOffsets[dir]];
            }
            int nodeInGridIndex = node.NodeInGridIndex;
            int z = nodeInGridIndex / this.Width;
            int x = nodeInGridIndex - (z * this.Width);
            return this.GetNodeConnection(nodeInGridIndex, x, z, dir);
        }

        private GridNode GetNodeConnection(int index, int x, int z, int dir)
        {
            if (!this.nodes[index].GetConnectionInternal(dir))
            {
                return null;
            }
            int num = x + this.neighbourXOffsets[dir];
            if ((num < 0) || (num >= this.Width))
            {
                return null;
            }
            int num2 = z + this.neighbourZOffsets[dir];
            if ((num2 < 0) || (num2 >= this.Depth))
            {
                return null;
            }
            int num3 = index + this.neighbourOffsets[dir];
            return this.nodes[num3];
        }

        public override void GetNodes(GraphNodeDelegateCancelable del)
        {
            if (this.nodes != null)
            {
                for (int i = 0; (i < this.nodes.Length) && del(this.nodes[i]); i++)
                {
                }
            }
        }

        public List<GraphNode> GetNodesInArea(GraphUpdateShape shape)
        {
            return this.GetNodesInArea(shape.GetBounds(), shape);
        }

        public List<GraphNode> GetNodesInArea(Bounds b)
        {
            return this.GetNodesInArea(b, null);
        }

        private List<GraphNode> GetNodesInArea(Bounds b, GraphUpdateShape shape)
        {
            Vector3 vector;
            Vector3 vector2;
            if ((this.nodes == null) || ((this.width * this.depth) != this.nodes.Length))
            {
                return null;
            }
            List<GraphNode> list = ListPool<GraphNode>.Claim();
            GetBoundsMinMax(b, base.inverseMatrix, out vector, out vector2);
            int xmin = Mathf.RoundToInt(vector.x - 0.5f);
            int xmax = Mathf.RoundToInt(vector2.x - 0.5f);
            int ymin = Mathf.RoundToInt(vector.z - 0.5f);
            int ymax = Mathf.RoundToInt(vector2.z - 0.5f);
            IntRect a = new IntRect(xmin, ymin, xmax, ymax);
            IntRect rect2 = new IntRect(0, 0, this.width - 1, this.depth - 1);
            IntRect rect3 = IntRect.Intersection(a, rect2);
            for (int i = rect3.xmin; i <= rect3.xmax; i++)
            {
                for (int j = rect3.ymin; j <= rect3.ymax; j++)
                {
                    int index = (j * this.width) + i;
                    GraphNode item = this.nodes[index];
                    if (b.Contains((Vector3) item.position) && ((shape == null) || shape.Contains((Vector3) item.position)))
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        public Int3 GraphPointToWorld(int x, int z, float height)
        {
            return (Int3) this.matrix.MultiplyPoint3x4(new Vector3(x + 0.5f, height, z + 0.5f));
        }

        public bool HasNodeConnection(GridNode node, int dir)
        {
            if (!node.GetConnectionInternal(dir))
            {
                return false;
            }
            if (!node.EdgeNode)
            {
                return true;
            }
            int nodeInGridIndex = node.NodeInGridIndex;
            int z = nodeInGridIndex / this.Width;
            int x = nodeInGridIndex - (z * this.Width);
            return this.HasNodeConnection(nodeInGridIndex, x, z, dir);
        }

        public bool HasNodeConnection(int index, int x, int z, int dir)
        {
            if (!this.nodes[index].GetConnectionInternal(dir))
            {
                return false;
            }
            int num = x + this.neighbourXOffsets[dir];
            if ((num < 0) || (num >= this.Width))
            {
                return false;
            }
            int num2 = z + this.neighbourZOffsets[dir];
            return ((num2 >= 0) && (num2 < this.Depth));
        }

        public virtual bool IsValidConnection(GridNode n1, GridNode n2)
        {
            if (!n1.Walkable || !n2.Walkable)
            {
                return false;
            }
            return ((this.maxClimb <= 0f) || (Math.Abs((int) (n1.position[this.maxClimbAxis] - n2.position[this.maxClimbAxis])) <= (this.maxClimb * 1000f)));
        }

        public bool Linecast(Vector3 _a, Vector3 _b)
        {
            GraphHitInfo info;
            return this.Linecast(_a, _b, null, out info);
        }

        public bool Linecast(Vector3 _a, Vector3 _b, GraphNode hint)
        {
            GraphHitInfo info;
            return this.Linecast(_a, _b, hint, out info);
        }

        public bool Linecast(Vector3 _a, Vector3 _b, GraphNode hint, out GraphHitInfo hit)
        {
            return this.Linecast(_a, _b, hint, out hit, null);
        }

        public bool Linecast(Vector3 _a, Vector3 _b, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
        {
            int num3;
            int num4;
            hit = new GraphHitInfo();
            hit.origin = _a;
            Vector3 a = this.inverseMatrix.MultiplyPoint3x4(_a);
            Vector3 b = this.inverseMatrix.MultiplyPoint3x4(_b);
            if (!this.ClipLineSegmentToBounds(a, b, out a, out b))
            {
                return false;
            }
            GridNodeBase node = base.GetNearest(this.matrix.MultiplyPoint3x4(a), NNConstraint.None).node as GridNodeBase;
            GridNodeBase base3 = base.GetNearest(this.matrix.MultiplyPoint3x4(b), NNConstraint.None).node as GridNodeBase;
            if (!node.Walkable)
            {
                hit.node = node;
                hit.point = this.matrix.MultiplyPoint3x4(a);
                hit.tangentOrigin = hit.point;
                return true;
            }
            Vector2 vector3 = new Vector2(a.x, a.z);
            Vector2 vector4 = new Vector2(b.x, b.z);
            vector3 -= (Vector2) (Vector2.one * 0.5f);
            vector4 -= (Vector2) (Vector2.one * 0.5f);
            if ((node == null) || (base3 == null))
            {
                hit.node = null;
                hit.point = _a;
                return true;
            }
            Vector2 vector5 = vector4 - vector3;
            Int2 num = new Int2((int) Mathf.Sign(vector5.x), (int) Mathf.Sign(vector5.y));
            float num2 = CrossMagnitude(vector5, new Vector2((float) num.x, (float) num.y)) * 0.5f;
            if (vector5.y >= 0f)
            {
                if (vector5.x >= 0f)
                {
                    num3 = 1;
                    num4 = 2;
                }
                else
                {
                    num3 = 2;
                    num4 = 3;
                }
            }
            else if (vector5.x < 0f)
            {
                num3 = 3;
                num4 = 0;
            }
            else
            {
                num3 = 0;
                num4 = 1;
            }
            GridNodeBase item = node;
            while (item.NodeInGridIndex != base3.NodeInGridIndex)
            {
                if (trace != null)
                {
                    trace.Add(item);
                }
                Vector2 vector6 = new Vector2((float) (item.NodeInGridIndex % this.width), (float) (item.NodeInGridIndex / this.width));
                float num6 = CrossMagnitude(vector5, vector6 - vector3) + num2;
                int direction = (num6 >= 0f) ? num3 : num4;
                GridNodeBase neighbourAlongDirection = this.GetNeighbourAlongDirection(item, direction);
                if (neighbourAlongDirection != null)
                {
                    item = neighbourAlongDirection;
                }
                else
                {
                    Vector2 vector8;
                    Vector2 vector7 = vector6 + ((Vector2) (new Vector2((float) this.neighbourXOffsets[direction], (float) this.neighbourZOffsets[direction]) * 0.5f));
                    if (this.neighbourXOffsets[direction] == 0)
                    {
                        vector8 = new Vector2(1f, 0f);
                    }
                    else
                    {
                        vector8 = new Vector2(0f, 1f);
                    }
                    Vector2 vector9 = VectorMath.LineIntersectionPoint(vector7, vector7 + vector8, vector3, vector4);
                    Vector3 vector10 = this.inverseMatrix.MultiplyPoint3x4((Vector3) item.position);
                    Vector3 v = new Vector3(vector9.x + 0.5f, vector10.y, vector9.y + 0.5f);
                    Vector3 vector12 = new Vector3(vector7.x + 0.5f, vector10.y, vector7.y + 0.5f);
                    hit.point = this.matrix.MultiplyPoint3x4(v);
                    hit.tangentOrigin = this.matrix.MultiplyPoint3x4(vector12);
                    hit.tangent = this.matrix.MultiplyVector(new Vector3(vector8.x, 0f, vector8.y));
                    hit.node = item;
                    return true;
                }
            }
            if (trace != null)
            {
                trace.Add(item);
            }
            if (item == base3)
            {
                return false;
            }
            hit.point = (Vector3) item.position;
            hit.tangentOrigin = hit.point;
            return true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            this.RemoveGridGraphFromStatic();
        }

        public override void OnDrawGizmos(bool drawNodes)
        {
            Gizmos.matrix = this.boundsMatrix;
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(this.size.x, 0f, this.size.y));
            Gizmos.matrix = Matrix4x4.identity;
            if ((drawNodes && (this.nodes != null)) && (this.nodes.Length == (this.width * this.depth)))
            {
                PathHandler debugPathData = AstarPath.active.debugPathData;
                bool flag = AstarPath.active.showSearchTree && (debugPathData != null);
                for (int i = 0; i < this.depth; i++)
                {
                    for (int j = 0; j < this.width; j++)
                    {
                        GridNode node = this.nodes[(i * this.width) + j];
                        if (node.Walkable)
                        {
                            Gizmos.color = this.NodeColor(node, debugPathData);
                            Vector3 position = (Vector3) node.position;
                            if (flag)
                            {
                                if (NavGraph.InSearchTree(node, AstarPath.active.debugPath))
                                {
                                    PathNode pathNode = debugPathData.GetPathNode(node);
                                    if ((pathNode != null) && (pathNode.parent != null))
                                    {
                                        Gizmos.DrawLine(position, (Vector3) pathNode.parent.node.position);
                                    }
                                }
                            }
                            else
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    if (node.GetConnectionInternal(k))
                                    {
                                        GridNode node3 = this.nodes[node.NodeInGridIndex + this.neighbourOffsets[k]];
                                        Gizmos.DrawLine(position, (Vector3) node3.position);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void OnPostScan(AstarPath script)
        {
            AstarPath.OnPostScan = (OnScanDelegate) Delegate.Remove(AstarPath.OnPostScan, new OnScanDelegate(this.OnPostScan));
            if (this.autoLinkGrids && (this.autoLinkDistLimit > 0f))
            {
                throw new NotSupportedException();
            }
        }

        public override void PostDeserialization()
        {
            this.GenerateMatrix();
            this.SetUpOffsetsAndCosts();
            if ((this.nodes != null) && (this.nodes.Length != 0))
            {
                if ((this.width * this.depth) != this.nodes.Length)
                {
                    UnityEngine.Debug.LogError("Node data did not match with bounds data. Probably a change to the bounds/width/depth data was made after scanning the graph just prior to saving it. Nodes will be discarded");
                    this.nodes = new GridNode[0];
                }
                else
                {
                    GridNode.SetGridGraph(AstarPath.active.astarData.GetGraphIndex(this), this);
                    for (int i = 0; i < this.depth; i++)
                    {
                        for (int j = 0; j < this.width; j++)
                        {
                            GridNode node = this.nodes[(i * this.width) + j];
                            if (node == null)
                            {
                                UnityEngine.Debug.LogError("Deserialization Error : Couldn't cast the node to the appropriate type - GridGenerator");
                                return;
                            }
                            node.NodeInGridIndex = (i * this.width) + j;
                        }
                    }
                }
            }
        }

        public void RelocateNodes(Vector3 center, Quaternion rotation, float nodeSize, [Optional, DefaultParameterValue(1)] float aspectRatio, [Optional, DefaultParameterValue(0)] float isometricAngle)
        {
            Matrix4x4 matrix = base.matrix;
            this.center = center;
            this.rotation = rotation.eulerAngles;
            this.nodeSize = nodeSize;
            this.aspectRatio = aspectRatio;
            this.isometricAngle = isometricAngle;
            this.UpdateSizeFromWidthDepth();
            this.RelocateNodes(matrix, base.matrix);
        }

        private void RemoveGridGraphFromStatic()
        {
            GridNode.SetGridGraph(AstarPath.active.astarData.GetGraphIndex(this), null);
        }

        [DebuggerHidden]
        public override IEnumerable<Progress> ScanInternal()
        {
            <ScanInternal>c__Iterator14 iterator = new <ScanInternal>c__Iterator14();
            iterator.<>f__this = this;
            iterator.$PC = -2;
            return iterator;
        }

        public override void SerializeExtraInfo(GraphSerializationContext ctx)
        {
            if (this.nodes == null)
            {
                ctx.writer.Write(-1);
            }
            else
            {
                ctx.writer.Write(this.nodes.Length);
                for (int i = 0; i < this.nodes.Length; i++)
                {
                    this.nodes[i].SerializeNode(ctx);
                }
            }
        }

        public void SetNodeConnection(GridNode node, int dir, bool value)
        {
            int nodeInGridIndex = node.NodeInGridIndex;
            int z = nodeInGridIndex / this.Width;
            int x = nodeInGridIndex - (z * this.Width);
            this.SetNodeConnection(nodeInGridIndex, x, z, dir, value);
        }

        public void SetNodeConnection(int index, int x, int z, int dir, bool value)
        {
            this.nodes[index].SetConnectionInternal(dir, value);
        }

        public virtual void SetUpOffsetsAndCosts()
        {
            this.neighbourOffsets[0] = -this.width;
            this.neighbourOffsets[1] = 1;
            this.neighbourOffsets[2] = this.width;
            this.neighbourOffsets[3] = -1;
            this.neighbourOffsets[4] = -this.width + 1;
            this.neighbourOffsets[5] = this.width + 1;
            this.neighbourOffsets[6] = this.width - 1;
            this.neighbourOffsets[7] = -this.width - 1;
            uint num = (uint) Mathf.RoundToInt(this.nodeSize * 1000f);
            uint num2 = !this.uniformEdgeCosts ? ((uint) Mathf.RoundToInt((this.nodeSize * Mathf.Sqrt(2f)) * 1000f)) : num;
            this.neighbourCosts[0] = num;
            this.neighbourCosts[1] = num;
            this.neighbourCosts[2] = num;
            this.neighbourCosts[3] = num;
            this.neighbourCosts[4] = num2;
            this.neighbourCosts[5] = num2;
            this.neighbourCosts[6] = num2;
            this.neighbourCosts[7] = num2;
            this.neighbourXOffsets[0] = 0;
            this.neighbourXOffsets[1] = 1;
            this.neighbourXOffsets[2] = 0;
            this.neighbourXOffsets[3] = -1;
            this.neighbourXOffsets[4] = 1;
            this.neighbourXOffsets[5] = 1;
            this.neighbourXOffsets[6] = -1;
            this.neighbourXOffsets[7] = -1;
            this.neighbourZOffsets[0] = -1;
            this.neighbourZOffsets[1] = 0;
            this.neighbourZOffsets[2] = 1;
            this.neighbourZOffsets[3] = 0;
            this.neighbourZOffsets[4] = -1;
            this.neighbourZOffsets[5] = 1;
            this.neighbourZOffsets[6] = 1;
            this.neighbourZOffsets[7] = -1;
        }

        public bool SnappedLinecast(Vector3 a, Vector3 b, GraphNode hint, out GraphHitInfo hit)
        {
            return this.Linecast((Vector3) base.GetNearest(a, NNConstraint.None).node.position, (Vector3) base.GetNearest(b, NNConstraint.None).node.position, hint, out hit);
        }

        public void UpdateArea(GraphUpdateObject o)
        {
            if ((this.nodes == null) || (this.nodes.Length != (this.width * this.depth)))
            {
                UnityEngine.Debug.LogWarning("The Grid Graph is not scanned, cannot update area ");
            }
            else
            {
                Vector3 vector;
                Vector3 vector2;
                GetBoundsMinMax(o.bounds, base.inverseMatrix, out vector, out vector2);
                int xmin = Mathf.RoundToInt(vector.x - 0.5f);
                int xmax = Mathf.RoundToInt(vector2.x - 0.5f);
                int ymin = Mathf.RoundToInt(vector.z - 0.5f);
                int ymax = Mathf.RoundToInt(vector2.z - 0.5f);
                IntRect a = new IntRect(xmin, ymin, xmax, ymax);
                IntRect b = a;
                IntRect rect3 = new IntRect(0, 0, this.width - 1, this.depth - 1);
                IntRect rect4 = a;
                int range = !o.updateErosion ? 0 : this.erodeIterations;
                bool flag = o.updatePhysics || o.modifyWalkability;
                if ((o.updatePhysics && !o.modifyWalkability) && this.collision.collisionCheck)
                {
                    Vector3 vector3 = (Vector3) (new Vector3(this.collision.diameter, 0f, this.collision.diameter) * 0.5f);
                    vector -= (Vector3) (vector3 * 1.02f);
                    vector2 += (Vector3) (vector3 * 1.02f);
                    int introduced48 = Mathf.RoundToInt(vector.x - 0.5f);
                    int introduced49 = Mathf.RoundToInt(vector2.x - 0.5f);
                    rect4 = new IntRect(introduced48, Mathf.RoundToInt(vector.z - 0.5f), introduced49, Mathf.RoundToInt(vector2.z - 0.5f));
                    b = IntRect.Union(rect4, b);
                }
                if (flag || (range > 0))
                {
                    b = b.Expand(range + 1);
                }
                IntRect rect5 = IntRect.Intersection(b, rect3);
                for (int i = rect5.xmin; i <= rect5.xmax; i++)
                {
                    for (int k = rect5.ymin; k <= rect5.ymax; k++)
                    {
                        o.WillUpdateNode(this.nodes[(k * this.width) + i]);
                    }
                }
                if (o.updatePhysics && !o.modifyWalkability)
                {
                    this.collision.Initialize(base.matrix, this.nodeSize);
                    rect5 = IntRect.Intersection(rect4, rect3);
                    for (int m = rect5.xmin; m <= rect5.xmax; m++)
                    {
                        for (int n = rect5.ymin; n <= rect5.ymax; n++)
                        {
                            int index = (n * this.width) + m;
                            GridNode node = this.nodes[index];
                            this.UpdateNodePositionCollision(node, m, n, o.resetPenaltyOnPhysics);
                        }
                    }
                }
                rect5 = IntRect.Intersection(a, rect3);
                for (int j = rect5.xmin; j <= rect5.xmax; j++)
                {
                    for (int num12 = rect5.ymin; num12 <= rect5.ymax; num12++)
                    {
                        int num13 = (num12 * this.width) + j;
                        GridNode node2 = this.nodes[num13];
                        if (flag)
                        {
                            node2.Walkable = node2.WalkableErosion;
                            if (o.bounds.Contains((Vector3) node2.position))
                            {
                                o.Apply(node2);
                            }
                            node2.WalkableErosion = node2.Walkable;
                        }
                        else if (o.bounds.Contains((Vector3) node2.position))
                        {
                            o.Apply(node2);
                        }
                    }
                }
                if (flag && (range == 0))
                {
                    rect5 = IntRect.Intersection(b, rect3);
                    for (int num14 = rect5.xmin; num14 <= rect5.xmax; num14++)
                    {
                        for (int num15 = rect5.ymin; num15 <= rect5.ymax; num15++)
                        {
                            int num16 = (num15 * this.width) + num14;
                            GridNode node3 = this.nodes[num16];
                            this.CalculateConnections(num14, num15, node3);
                        }
                    }
                }
                else if (flag && (range > 0))
                {
                    IntRect rect6 = IntRect.Union(a, rect4).Expand(range);
                    IntRect rect7 = rect6.Expand(range);
                    rect6 = IntRect.Intersection(rect6, rect3);
                    rect7 = IntRect.Intersection(rect7, rect3);
                    for (int num17 = rect7.xmin; num17 <= rect7.xmax; num17++)
                    {
                        for (int num18 = rect7.ymin; num18 <= rect7.ymax; num18++)
                        {
                            int num19 = (num18 * this.width) + num17;
                            GridNode node4 = this.nodes[num19];
                            bool walkable = node4.Walkable;
                            node4.Walkable = node4.WalkableErosion;
                            if (!rect6.Contains(num17, num18))
                            {
                                node4.TmpWalkable = walkable;
                            }
                        }
                    }
                    for (int num20 = rect7.xmin; num20 <= rect7.xmax; num20++)
                    {
                        for (int num21 = rect7.ymin; num21 <= rect7.ymax; num21++)
                        {
                            int num22 = (num21 * this.width) + num20;
                            GridNode node5 = this.nodes[num22];
                            this.CalculateConnections(num20, num21, node5);
                        }
                    }
                    this.ErodeWalkableArea(rect7.xmin, rect7.ymin, rect7.xmax + 1, rect7.ymax + 1);
                    for (int num23 = rect7.xmin; num23 <= rect7.xmax; num23++)
                    {
                        for (int num24 = rect7.ymin; num24 <= rect7.ymax; num24++)
                        {
                            if (!rect6.Contains(num23, num24))
                            {
                                int num25 = (num24 * this.width) + num23;
                                GridNode node6 = this.nodes[num25];
                                node6.Walkable = node6.TmpWalkable;
                            }
                        }
                    }
                    for (int num26 = rect7.xmin; num26 <= rect7.xmax; num26++)
                    {
                        for (int num27 = rect7.ymin; num27 <= rect7.ymax; num27++)
                        {
                            int num28 = (num27 * this.width) + num26;
                            GridNode node7 = this.nodes[num28];
                            this.CalculateConnections(num26, num27, node7);
                        }
                    }
                }
            }
        }

        public void UpdateAreaInit(GraphUpdateObject o)
        {
        }

        public virtual void UpdateNodePositionCollision(GridNode node, int x, int z, [Optional, DefaultParameterValue(true)] bool resetPenalty)
        {
            RaycastHit hit;
            bool flag;
            node.position = this.GraphPointToWorld(x, z, 0f);
            Vector3 vector = this.collision.CheckHeight((Vector3) node.position, out hit, out flag);
            node.position = (Int3) vector;
            if (resetPenalty)
            {
                node.Penalty = base.initialPenalty;
                if (this.penaltyPosition)
                {
                    node.Penalty += (uint) Mathf.RoundToInt((node.position.y - this.penaltyPositionOffset) * this.penaltyPositionFactor);
                }
            }
            if ((flag && this.useRaycastNormal) && (this.collision.heightCheck && (hit.normal != Vector3.zero)))
            {
                float f = Vector3.Dot(hit.normal.normalized, this.collision.up);
                if (this.penaltyAngle && resetPenalty)
                {
                    node.Penalty += (uint) Mathf.RoundToInt((1f - Mathf.Pow(f, this.penaltyAnglePower)) * this.penaltyAngleFactor);
                }
                float num2 = Mathf.Cos(this.maxSlope * 0.01745329f);
                if (f < num2)
                {
                    flag = false;
                }
            }
            node.Walkable = flag && this.collision.Check((Vector3) node.position);
            node.WalkableErosion = node.Walkable;
        }

        public void UpdateSizeFromWidthDepth()
        {
            this.unclampedSize = (Vector2) (new Vector2((float) this.width, (float) this.depth) * this.nodeSize);
            this.GenerateMatrix();
        }

        public Matrix4x4 boundsMatrix
        {
            [CompilerGenerated]
            get
            {
                return this.<boundsMatrix>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<boundsMatrix>k__BackingField = value;
            }
        }

        public int Depth
        {
            get
            {
                return this.depth;
            }
            set
            {
                this.depth = value;
            }
        }

        public Vector2 size
        {
            [CompilerGenerated]
            get
            {
                return this.<size>k__BackingField;
            }
            [CompilerGenerated]
            protected set
            {
                this.<size>k__BackingField = value;
            }
        }

        public virtual bool uniformWidthDepthGrid
        {
            get
            {
                return true;
            }
        }

        public bool useRaycastNormal
        {
            get
            {
                return (Math.Abs((float) (90f - this.maxSlope)) > float.Epsilon);
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        [CompilerGenerated]
        private sealed class <ScanInternal>c__Iterator14 : IEnumerator, IEnumerable<Progress>, IEnumerator<Progress>, IDisposable, IEnumerable
        {
            internal Progress $current;
            internal int $PC;
            internal GridGraph <>f__this;
            internal int <graphIndex>__0;
            internal int <i>__1;
            internal GridNode <node>__5;
            internal GridNode <node>__8;
            internal int <progressCounter>__2;
            internal int <x>__4;
            internal int <x>__7;
            internal int <z>__3;
            internal int <z>__6;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        AstarPath.OnPostScan = (OnScanDelegate) Delegate.Combine(AstarPath.OnPostScan, new OnScanDelegate(this.<>f__this.OnPostScan));
                        if (this.<>f__this.nodeSize > 0f)
                        {
                            this.<>f__this.GenerateMatrix();
                            if ((this.<>f__this.width > 0x400) || (this.<>f__this.depth > 0x400))
                            {
                                UnityEngine.Debug.LogError("One of the grid's sides is longer than 1024 nodes");
                                break;
                            }
                            if (this.<>f__this.useJumpPointSearch)
                            {
                                UnityEngine.Debug.LogError("Trying to use Jump Point Search, but support for it is not enabled. Please enable it in the inspector (Grid Graph settings).");
                            }
                            this.<>f__this.SetUpOffsetsAndCosts();
                            this.<graphIndex>__0 = AstarPath.active.astarData.GetGraphIndex(this.<>f__this);
                            GridNode.SetGridGraph(this.<graphIndex>__0, this.<>f__this);
                            this.$current = new Progress(0.05f, "Creating nodes");
                            this.$PC = 1;
                            goto Label_04A7;
                        }
                        break;

                    case 1:
                        this.<>f__this.nodes = new GridNode[this.<>f__this.width * this.<>f__this.depth];
                        this.<i>__1 = 0;
                        while (this.<i>__1 < this.<>f__this.nodes.Length)
                        {
                            this.<>f__this.nodes[this.<i>__1] = new GridNode(this.<>f__this.active);
                            this.<>f__this.nodes[this.<i>__1].GraphIndex = (uint) this.<graphIndex>__0;
                            this.<i>__1++;
                        }
                        if (this.<>f__this.collision == null)
                        {
                            this.<>f__this.collision = new GraphCollision();
                        }
                        this.<>f__this.collision.Initialize(this.<>f__this.matrix, this.<>f__this.nodeSize);
                        this.<>f__this.textureData.Initialize();
                        this.<progressCounter>__2 = 0;
                        this.<z>__3 = 0;
                        while (this.<z>__3 < this.<>f__this.depth)
                        {
                            if (this.<progressCounter>__2 >= 0x3e8)
                            {
                                this.<progressCounter>__2 = 0;
                                this.$current = new Progress(Mathf.Lerp(0.1f, 0.7f, ((float) this.<z>__3) / ((float) this.<>f__this.depth)), "Calculating positions");
                                this.$PC = 2;
                                goto Label_04A7;
                            }
                        Label_0279:
                            this.<progressCounter>__2 += this.<>f__this.width;
                            this.<x>__4 = 0;
                            while (this.<x>__4 < this.<>f__this.width)
                            {
                                this.<node>__5 = this.<>f__this.nodes[(this.<z>__3 * this.<>f__this.width) + this.<x>__4];
                                this.<node>__5.NodeInGridIndex = (this.<z>__3 * this.<>f__this.width) + this.<x>__4;
                                this.<>f__this.UpdateNodePositionCollision(this.<node>__5, this.<x>__4, this.<z>__3, true);
                                this.<>f__this.textureData.Apply(this.<node>__5, this.<x>__4, this.<z>__3);
                                this.<x>__4++;
                            }
                            this.<z>__3++;
                        }
                        this.<z>__6 = 0;
                        while (this.<z>__6 < this.<>f__this.depth)
                        {
                            if (this.<progressCounter>__2 >= 0x3e8)
                            {
                                this.<progressCounter>__2 = 0;
                                this.$current = new Progress(Mathf.Lerp(0.1f, 0.7f, ((float) this.<z>__6) / ((float) this.<>f__this.depth)), "Calculating connections");
                                this.$PC = 3;
                                goto Label_04A7;
                            }
                        Label_03D6:
                            this.<x>__7 = 0;
                            while (this.<x>__7 < this.<>f__this.width)
                            {
                                this.<node>__8 = this.<>f__this.nodes[(this.<z>__6 * this.<>f__this.width) + this.<x>__7];
                                this.<>f__this.CalculateConnections(this.<x>__7, this.<z>__6, this.<node>__8);
                                this.<x>__7++;
                            }
                            this.<z>__6++;
                        }
                        this.$current = new Progress(0.95f, "Calculating erosion");
                        this.$PC = 4;
                        goto Label_04A7;

                    case 2:
                        goto Label_0279;

                    case 3:
                        goto Label_03D6;

                    case 4:
                        this.<>f__this.ErodeWalkableArea();
                        this.$PC = -1;
                        break;
                }
                return false;
            Label_04A7:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<Progress> IEnumerable<Progress>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                GridGraph.<ScanInternal>c__Iterator14 iterator = new GridGraph.<ScanInternal>c__Iterator14();
                iterator.<>f__this = this.<>f__this;
                return iterator;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Pathfinding.Progress>.GetEnumerator();
            }

            Progress IEnumerator<Progress>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        public class TextureData
        {
            public ChannelUse[] channels = new ChannelUse[3];
            private Color32[] data;
            public bool enabled;
            public float[] factors = new float[3];
            public Texture2D source;

            public void Apply(GridNode node, int x, int z)
            {
                if ((this.enabled && (this.data != null)) && ((x < this.source.width) && (z < this.source.height)))
                {
                    Color32 color = this.data[(z * this.source.width) + x];
                    if (this.channels[0] != ChannelUse.None)
                    {
                        this.ApplyChannel(node, x, z, color.r, this.channels[0], this.factors[0]);
                    }
                    if (this.channels[1] != ChannelUse.None)
                    {
                        this.ApplyChannel(node, x, z, color.g, this.channels[1], this.factors[1]);
                    }
                    if (this.channels[2] != ChannelUse.None)
                    {
                        this.ApplyChannel(node, x, z, color.b, this.channels[2], this.factors[2]);
                    }
                }
            }

            private void ApplyChannel(GridNode node, int x, int z, int value, ChannelUse channelUse, float factor)
            {
                switch (channelUse)
                {
                    case ChannelUse.Penalty:
                        node.Penalty += (uint) Mathf.RoundToInt(value * factor);
                        break;

                    case ChannelUse.Position:
                        node.position = GridNode.GetGridGraph(node.GraphIndex).GraphPointToWorld(x, z, (float) value);
                        break;

                    case ChannelUse.WalkablePenalty:
                        if (value != 0)
                        {
                            node.Penalty += (uint) Mathf.RoundToInt((value - 1) * factor);
                            break;
                        }
                        node.Walkable = false;
                        break;
                }
            }

            public void Initialize()
            {
                if (this.enabled && (this.source != null))
                {
                    for (int i = 0; i < this.channels.Length; i++)
                    {
                        if (this.channels[i] != ChannelUse.None)
                        {
                            try
                            {
                                this.data = this.source.GetPixels32();
                            }
                            catch (UnityException exception)
                            {
                                UnityEngine.Debug.LogWarning(exception.ToString());
                                this.data = null;
                            }
                            break;
                        }
                    }
                }
            }

            public enum ChannelUse
            {
                None,
                Penalty,
                Position,
                WalkablePenalty
            }
        }
    }
}

