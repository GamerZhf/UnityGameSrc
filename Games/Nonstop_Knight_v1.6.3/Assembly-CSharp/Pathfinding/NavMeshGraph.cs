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

    [Serializable, JsonOptIn]
    public class NavMeshGraph : NavGraph, IUpdatableGraph, IRaycastableGraph, INavmesh, INavmeshHolder
    {
        private BBTree _bbTree;
        [NonSerialized]
        private Int3[] _vertices;
        [JsonMember]
        public bool accurateNearestNode = true;
        public TriangleMeshNode[] nodes;
        [JsonMember]
        public Vector3 offset;
        [NonSerialized]
        private Vector3[] originalVertices;
        [JsonMember]
        public Vector3 rotation;
        [JsonMember]
        public float scale = 1f;
        [JsonMember]
        public Mesh sourceMesh;
        [NonSerialized]
        public int[] triangles;

        public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
        {
            return GraphUpdateThreading.UnityThread;
        }

        private static Vector3 ClosestPointOnNode(TriangleMeshNode node, Int3[] vertices, Vector3 pos)
        {
            return Polygon.ClosestPointOnTriangle((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], (Vector3) vertices[node.v2], pos);
        }

        [Obsolete("Use TriangleMeshNode.ContainsPoint instead")]
        public bool ContainsPoint(TriangleMeshNode node, Vector3 pos)
        {
            return ((VectorMath.IsClockwiseXZ((Vector3) this.vertices[node.v0], (Vector3) this.vertices[node.v1], pos) && VectorMath.IsClockwiseXZ((Vector3) this.vertices[node.v1], (Vector3) this.vertices[node.v2], pos)) && VectorMath.IsClockwiseXZ((Vector3) this.vertices[node.v2], (Vector3) this.vertices[node.v0], pos));
        }

        [Obsolete("Use TriangleMeshNode.ContainsPoint instead")]
        public static bool ContainsPoint(TriangleMeshNode node, Vector3 pos, Int3[] vertices)
        {
            if (!VectorMath.IsClockwiseMarginXZ((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], (Vector3) vertices[node.v2]))
            {
                UnityEngine.Debug.LogError("Noes!");
            }
            return ((VectorMath.IsClockwiseMarginXZ((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], pos) && VectorMath.IsClockwiseMarginXZ((Vector3) vertices[node.v1], (Vector3) vertices[node.v2], pos)) && VectorMath.IsClockwiseMarginXZ((Vector3) vertices[node.v2], (Vector3) vertices[node.v0], pos));
        }

        public override void DeserializeExtraInfo(GraphSerializationContext ctx)
        {
            TriangleMeshNode.SetNavmeshHolder((int) ctx.graphIndex, this);
            int num2 = ctx.reader.ReadInt32();
            int num3 = ctx.reader.ReadInt32();
            if (num2 == -1)
            {
                this.nodes = new TriangleMeshNode[0];
                this._vertices = new Int3[0];
                this.originalVertices = new Vector3[0];
            }
            this.nodes = new TriangleMeshNode[num2];
            this._vertices = new Int3[num3];
            this.originalVertices = new Vector3[num3];
            for (int i = 0; i < num3; i++)
            {
                this._vertices[i] = ctx.DeserializeInt3();
                this.originalVertices[i] = ctx.DeserializeVector3();
            }
            this.bbTree = new BBTree();
            for (int j = 0; j < num2; j++)
            {
                this.nodes[j] = new TriangleMeshNode(base.active);
                TriangleMeshNode node = this.nodes[j];
                node.DeserializeNode(ctx);
                node.UpdatePositionFromVertices();
            }
            this.bbTree.RebuildFrom(this.nodes);
        }

        public void GenerateMatrix()
        {
            base.SetMatrix(Matrix4x4.TRS(this.offset, Quaternion.Euler(this.rotation), new Vector3(this.scale, this.scale, this.scale)));
        }

        private void GenerateNodes(Vector3[] vectorVertices, int[] triangles, out Vector3[] originalVertices, out Int3[] vertices)
        {
            if ((vectorVertices.Length == 0) || (triangles.Length == 0))
            {
                originalVertices = vectorVertices;
                vertices = new Int3[0];
                this.nodes = new TriangleMeshNode[0];
            }
            else
            {
                vertices = new Int3[vectorVertices.Length];
                int index = 0;
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] = (Int3) this.matrix.MultiplyPoint3x4(vectorVertices[i]);
                }
                Dictionary<Int3, int> dictionary = new Dictionary<Int3, int>();
                int[] numArray = new int[vertices.Length];
                for (int j = 0; j < vertices.Length; j++)
                {
                    if (!dictionary.ContainsKey(vertices[j]))
                    {
                        numArray[index] = j;
                        dictionary.Add(vertices[j], index);
                        index++;
                    }
                }
                for (int k = 0; k < triangles.Length; k++)
                {
                    Int3 num5 = vertices[triangles[k]];
                    triangles[k] = dictionary[num5];
                }
                Int3[] numArray2 = vertices;
                vertices = new Int3[index];
                originalVertices = new Vector3[index];
                for (int m = 0; m < index; m++)
                {
                    vertices[m] = numArray2[numArray[m]];
                    originalVertices[m] = vectorVertices[numArray[m]];
                }
                this.nodes = new TriangleMeshNode[triangles.Length / 3];
                int graphIndex = base.active.astarData.GetGraphIndex(this);
                for (int n = 0; n < this.nodes.Length; n++)
                {
                    this.nodes[n] = new TriangleMeshNode(base.active);
                    TriangleMeshNode node = this.nodes[n];
                    node.GraphIndex = (uint) graphIndex;
                    node.Penalty = base.initialPenalty;
                    node.Walkable = true;
                    node.v0 = triangles[n * 3];
                    node.v1 = triangles[(n * 3) + 1];
                    node.v2 = triangles[(n * 3) + 2];
                    if (!VectorMath.IsClockwiseXZ(vertices[node.v0], vertices[node.v1], vertices[node.v2]))
                    {
                        int num9 = node.v0;
                        node.v0 = node.v2;
                        node.v2 = num9;
                    }
                    if (VectorMath.IsColinearXZ(vertices[node.v0], vertices[node.v1], vertices[node.v2]))
                    {
                        UnityEngine.Debug.DrawLine((Vector3) vertices[node.v0], (Vector3) vertices[node.v1], Color.red);
                        UnityEngine.Debug.DrawLine((Vector3) vertices[node.v1], (Vector3) vertices[node.v2], Color.red);
                        UnityEngine.Debug.DrawLine((Vector3) vertices[node.v2], (Vector3) vertices[node.v0], Color.red);
                    }
                    node.UpdatePositionFromVertices();
                }
                Dictionary<Int2, TriangleMeshNode> dictionary2 = new Dictionary<Int2, TriangleMeshNode>();
                int num10 = 0;
                int num11 = 0;
                while (num10 < triangles.Length)
                {
                    dictionary2[new Int2(triangles[num10], triangles[num10 + 1])] = this.nodes[num11];
                    dictionary2[new Int2(triangles[num10 + 1], triangles[num10 + 2])] = this.nodes[num11];
                    dictionary2[new Int2(triangles[num10 + 2], triangles[num10])] = this.nodes[num11];
                    num11++;
                    num10 += 3;
                }
                List<MeshNode> list = new List<MeshNode>();
                List<uint> list2 = new List<uint>();
                int num12 = 0;
                int num13 = 0;
                while (num12 < triangles.Length)
                {
                    list.Clear();
                    list2.Clear();
                    TriangleMeshNode node2 = this.nodes[num13];
                    for (int num14 = 0; num14 < 3; num14++)
                    {
                        TriangleMeshNode node3;
                        if (dictionary2.TryGetValue(new Int2(triangles[num12 + ((num14 + 1) % 3)], triangles[num12 + num14]), out node3))
                        {
                            list.Add(node3);
                            Int3 num15 = node2.position - node3.position;
                            list2.Add((uint) num15.costMagnitude);
                        }
                    }
                    node2.connections = list.ToArray();
                    node2.connectionCosts = list2.ToArray();
                    num13++;
                    num12 += 3;
                }
                RebuildBBTree(this);
            }
        }

        public override NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
        {
            return GetNearest(this, this.nodes, position, constraint, this.accurateNearestNode);
        }

        public static NNInfoInternal GetNearest(NavMeshGraph graph, GraphNode[] nodes, Vector3 position, NNConstraint constraint, bool accurateNearestNode)
        {
            if ((nodes == null) || (nodes.Length == 0))
            {
                UnityEngine.Debug.LogError("NavGraph hasn't been generated yet or does not contain any nodes");
                return new NNInfoInternal();
            }
            if (constraint == null)
            {
                constraint = NNConstraint.None;
            }
            Int3[] vertices = graph.vertices;
            if (graph.bbTree == null)
            {
                return GetNearestForce(graph, graph, position, constraint, accurateNearestNode);
            }
            float radius = ((graph.bbTree.Size.width + graph.bbTree.Size.height) * 0.5f) * 0.02f;
            NNInfoInternal internal2 = graph.bbTree.QueryCircle(position, radius, constraint);
            if (internal2.node == null)
            {
                for (int i = 1; i <= 8; i++)
                {
                    internal2 = graph.bbTree.QueryCircle(position, (i * i) * radius, constraint);
                    if ((internal2.node != null) || ((((i - 1) * (i - 1)) * radius) > (AstarPath.active.maxNearestNodeDistance * 2f)))
                    {
                        break;
                    }
                }
            }
            if (internal2.node != null)
            {
                internal2.clampedPosition = ClosestPointOnNode(internal2.node as TriangleMeshNode, vertices, position);
            }
            if (internal2.constrainedNode != null)
            {
                if (constraint.constrainDistance)
                {
                    Vector3 vector = ((Vector3) internal2.constrainedNode.position) - position;
                    if (vector.sqrMagnitude > AstarPath.active.maxNearestNodeDistanceSqr)
                    {
                        internal2.constrainedNode = null;
                        return internal2;
                    }
                }
                internal2.constClampedPosition = ClosestPointOnNode(internal2.constrainedNode as TriangleMeshNode, vertices, position);
            }
            return internal2;
        }

        public override NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
        {
            return GetNearestForce(this, this, position, constraint, this.accurateNearestNode);
        }

        public static NNInfoInternal GetNearestForce(NavGraph graph, INavmeshHolder navmesh, Vector3 position, NNConstraint constraint, bool accurateNearestNode)
        {
            NNInfoInternal internal2 = GetNearestForceBoth(graph, navmesh, position, constraint, accurateNearestNode);
            internal2.node = internal2.constrainedNode;
            internal2.clampedPosition = internal2.constClampedPosition;
            return internal2;
        }

        public static NNInfoInternal GetNearestForceBoth(NavGraph graph, INavmeshHolder navmesh, Vector3 position, NNConstraint constraint, bool accurateNearestNode)
        {
            <GetNearestForceBoth>c__AnonStorey253 storey = new <GetNearestForceBoth>c__AnonStorey253();
            storey.accurateNearestNode = accurateNearestNode;
            storey.position = position;
            storey.constraint = constraint;
            storey.pos = (Int3) storey.position;
            storey.minDist = -1f;
            storey.minNode = null;
            storey.minConstDist = -1f;
            storey.minConstNode = null;
            storey.maxDistSqr = !storey.constraint.constrainDistance ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistanceSqr;
            GraphNodeDelegateCancelable del = new GraphNodeDelegateCancelable(storey.<>m__25);
            graph.GetNodes(del);
            NNInfoInternal internal2 = new NNInfoInternal(storey.minNode);
            if (internal2.node != null)
            {
                Vector3 vector = (internal2.node as TriangleMeshNode).ClosestPointOnNode(storey.position);
                internal2.clampedPosition = vector;
            }
            internal2.constrainedNode = storey.minConstNode;
            if (internal2.constrainedNode != null)
            {
                Vector3 vector2 = (internal2.constrainedNode as TriangleMeshNode).ClosestPointOnNode(storey.position);
                internal2.constClampedPosition = vector2;
            }
            return internal2;
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

        public void GetTileCoordinates(int tileIndex, out int x, out int z)
        {
            x = z = 0;
        }

        public Int3 GetVertex(int index)
        {
            return this.vertices[index];
        }

        public int GetVertexArrayIndex(int index)
        {
            return index;
        }

        public bool Linecast(Vector3 origin, Vector3 end)
        {
            return this.Linecast(origin, end, base.GetNearest(origin, NNConstraint.None).node);
        }

        public bool Linecast(Vector3 origin, Vector3 end, GraphNode hint)
        {
            GraphHitInfo info;
            return Linecast(this, origin, end, hint, out info, null);
        }

        public bool Linecast(Vector3 origin, Vector3 end, GraphNode hint, out GraphHitInfo hit)
        {
            return Linecast(this, origin, end, hint, out hit, null);
        }

        public static bool Linecast(INavmesh graph, Vector3 tmp_origin, Vector3 tmp_end, GraphNode hint, out GraphHitInfo hit)
        {
            return Linecast(graph, tmp_origin, tmp_end, hint, out hit, null);
        }

        public bool Linecast(Vector3 origin, Vector3 end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
        {
            return Linecast(this, origin, end, hint, out hit, trace);
        }

        public static bool Linecast(INavmesh graph, Vector3 tmp_origin, Vector3 tmp_end, GraphNode hint, out GraphHitInfo hit, List<GraphNode> trace)
        {
            Int3 p = (Int3) tmp_end;
            Int3 num2 = (Int3) tmp_origin;
            hit = new GraphHitInfo();
            if (float.IsNaN((tmp_origin.x + tmp_origin.y) + tmp_origin.z))
            {
                throw new ArgumentException("origin is NaN");
            }
            if (float.IsNaN((tmp_end.x + tmp_end.y) + tmp_end.z))
            {
                throw new ArgumentException("end is NaN");
            }
            TriangleMeshNode item = hint as TriangleMeshNode;
            if (item == null)
            {
                item = (graph as NavGraph).GetNearest(tmp_origin, NNConstraint.None).node as TriangleMeshNode;
                if (item == null)
                {
                    UnityEngine.Debug.LogError("Could not find a valid node to start from");
                    hit.point = tmp_origin;
                    return true;
                }
            }
            if (num2 == p)
            {
                hit.node = item;
                return false;
            }
            num2 = (Int3) item.ClosestPointOnNode((Vector3) num2);
            hit.origin = (Vector3) num2;
            if (!item.Walkable)
            {
                hit.point = (Vector3) num2;
                hit.tangentOrigin = (Vector3) num2;
                return true;
            }
            List<Vector3> list = ListPool<Vector3>.Claim();
            List<Vector3> list2 = ListPool<Vector3>.Claim();
            int num3 = 0;
            while (true)
            {
                num3++;
                if (num3 > 0x7d0)
                {
                    UnityEngine.Debug.LogError("Linecast was stuck in infinite loop. Breaking.");
                    ListPool<Vector3>.Release(list);
                    ListPool<Vector3>.Release(list2);
                    return true;
                }
                TriangleMeshNode node2 = null;
                if (trace != null)
                {
                    trace.Add(item);
                }
                if (item.ContainsPoint(p))
                {
                    ListPool<Vector3>.Release(list);
                    ListPool<Vector3>.Release(list2);
                    return false;
                }
                for (int i = 0; i < item.connections.Length; i++)
                {
                    if (item.connections[i].GraphIndex == item.GraphIndex)
                    {
                        list.Clear();
                        list2.Clear();
                        if (item.GetPortal(item.connections[i], list, list2, false))
                        {
                            float num5;
                            float num6;
                            Vector3 a = list[0];
                            Vector3 b = list2[0];
                            if (((VectorMath.RightXZ(a, b, hit.origin) || !VectorMath.RightXZ(a, b, tmp_end)) && VectorMath.LineIntersectionFactorXZ(a, b, hit.origin, tmp_end, out num5, out num6)) && ((num6 >= 0f) && ((num5 >= 0f) && (num5 <= 1f))))
                            {
                                node2 = item.connections[i] as TriangleMeshNode;
                                break;
                            }
                        }
                    }
                }
                if (node2 == null)
                {
                    int vertexCount = item.GetVertexCount();
                    for (int j = 0; j < vertexCount; j++)
                    {
                        float num9;
                        float num10;
                        Vector3 vertex = (Vector3) item.GetVertex(j);
                        Vector3 vector4 = (Vector3) item.GetVertex((j + 1) % vertexCount);
                        if (((VectorMath.RightXZ(vertex, vector4, hit.origin) || !VectorMath.RightXZ(vertex, vector4, tmp_end)) && VectorMath.LineIntersectionFactorXZ(vertex, vector4, hit.origin, tmp_end, out num9, out num10)) && ((num10 >= 0f) && ((num9 >= 0f) && (num9 <= 1f))))
                        {
                            Vector3 vector5 = vertex + ((Vector3) ((vector4 - vertex) * num9));
                            hit.point = vector5;
                            hit.node = item;
                            hit.tangent = vector4 - vertex;
                            hit.tangentOrigin = vertex;
                            ListPool<Vector3>.Release(list);
                            ListPool<Vector3>.Release(list2);
                            return true;
                        }
                    }
                    UnityEngine.Debug.LogWarning("Linecast failing because point not inside node, and line does not hit any edges of it");
                    ListPool<Vector3>.Release(list);
                    ListPool<Vector3>.Release(list2);
                    return false;
                }
                item = node2;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            TriangleMeshNode.SetNavmeshHolder(base.active.astarData.GetGraphIndex(this), null);
        }

        public override void OnDrawGizmos(bool drawNodes)
        {
            if (drawNodes)
            {
                Matrix4x4 matrix = base.matrix;
                this.GenerateMatrix();
                if (this.nodes == null)
                {
                }
                if (this.nodes != null)
                {
                    if (matrix != base.matrix)
                    {
                        this.RelocateNodes(matrix, base.matrix);
                    }
                    PathHandler debugPathData = AstarPath.active.debugPathData;
                    for (int i = 0; i < this.nodes.Length; i++)
                    {
                        TriangleMeshNode node = this.nodes[i];
                        Gizmos.color = this.NodeColor(node, AstarPath.active.debugPathData);
                        if (node.Walkable)
                        {
                            if ((AstarPath.active.showSearchTree && (debugPathData != null)) && (debugPathData.GetPathNode(node).parent != null))
                            {
                                Gizmos.DrawLine((Vector3) node.position, (Vector3) debugPathData.GetPathNode(node).parent.node.position);
                            }
                            else
                            {
                                for (int j = 0; j < node.connections.Length; j++)
                                {
                                    Gizmos.DrawLine((Vector3) node.position, Vector3.Lerp((Vector3) node.position, (Vector3) node.connections[j].position, 0.45f));
                                }
                            }
                            Gizmos.color = AstarColor.MeshEdgeColor;
                        }
                        else
                        {
                            Gizmos.color = AstarColor.UnwalkableNode;
                        }
                        Gizmos.DrawLine((Vector3) this.vertices[node.v0], (Vector3) this.vertices[node.v1]);
                        Gizmos.DrawLine((Vector3) this.vertices[node.v1], (Vector3) this.vertices[node.v2]);
                        Gizmos.DrawLine((Vector3) this.vertices[node.v2], (Vector3) this.vertices[node.v0]);
                    }
                }
            }
        }

        public void PostProcess()
        {
        }

        public static void RebuildBBTree(NavMeshGraph graph)
        {
            BBTree tree;
            BBTree bbTree = graph.bbTree;
            if (bbTree != null)
            {
                tree = bbTree;
            }
            else
            {
                tree = new BBTree();
            }
            tree.RebuildFrom(graph.nodes);
            graph.bbTree = tree;
        }

        public override void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
        {
            if (((this.vertices != null) && (this.vertices.Length != 0)) && ((this.originalVertices != null) && (this.originalVertices.Length == this.vertices.Length)))
            {
                for (int i = 0; i < this._vertices.Length; i++)
                {
                    this._vertices[i] = (Int3) newMatrix.MultiplyPoint3x4(this.originalVertices[i]);
                }
                for (int j = 0; j < this.nodes.Length; j++)
                {
                    TriangleMeshNode node = this.nodes[j];
                    node.UpdatePositionFromVertices();
                    if (node.connections != null)
                    {
                        for (int k = 0; k < node.connections.Length; k++)
                        {
                            Int3 num4 = node.position - node.connections[k].position;
                            node.connectionCosts[k] = (uint) num4.costMagnitude;
                        }
                    }
                }
                base.SetMatrix(newMatrix);
                RebuildBBTree(this);
            }
        }

        [DebuggerHidden]
        public override IEnumerable<Progress> ScanInternal()
        {
            <ScanInternal>c__Iterator16 iterator = new <ScanInternal>c__Iterator16();
            iterator.<>f__this = this;
            iterator.$PC = -2;
            return iterator;
        }

        public void ScanInternal(OnScanStatus statusCallback)
        {
            if (this.sourceMesh != null)
            {
                this.GenerateMatrix();
                Vector3[] vertices = this.sourceMesh.vertices;
                this.triangles = this.sourceMesh.triangles;
                TriangleMeshNode.SetNavmeshHolder(base.active.astarData.GetGraphIndex(this), this);
                this.GenerateNodes(vertices, this.triangles, out this.originalVertices, out this._vertices);
            }
        }

        public void ScanInternal(string objMeshPath)
        {
            Mesh mesh = ObjImporter.ImportFile(objMeshPath);
            if (mesh == null)
            {
                UnityEngine.Debug.LogError("Couldn't read .obj file at '" + objMeshPath + "'");
            }
            else
            {
                this.sourceMesh = mesh;
                IEnumerator<Progress> enumerator = this.ScanInternal().GetEnumerator();
                while (enumerator.MoveNext())
                {
                }
            }
        }

        public override void SerializeExtraInfo(GraphSerializationContext ctx)
        {
            if (((this.nodes == null) || (this.originalVertices == null)) || ((this._vertices == null) || (this.originalVertices.Length != this._vertices.Length)))
            {
                ctx.writer.Write(-1);
                ctx.writer.Write(-1);
            }
            else
            {
                ctx.writer.Write(this.nodes.Length);
                ctx.writer.Write(this._vertices.Length);
                for (int i = 0; i < this._vertices.Length; i++)
                {
                    ctx.SerializeInt3(this._vertices[i]);
                    ctx.SerializeVector3(this.originalVertices[i]);
                }
                for (int j = 0; j < this.nodes.Length; j++)
                {
                    this.nodes[j].SerializeNode(ctx);
                }
            }
        }

        public void UpdateArea(GraphUpdateObject o)
        {
            UpdateArea(o, this);
        }

        public static void UpdateArea(GraphUpdateObject o, INavmesh graph)
        {
            <UpdateArea>c__AnonStorey254 storey = new <UpdateArea>c__AnonStorey254();
            storey.o = o;
            Bounds bounds = storey.o.bounds;
            storey.r = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
            int xmin = Mathf.FloorToInt(bounds.min.x * 1000f);
            int ymin = Mathf.FloorToInt(bounds.min.z * 1000f);
            int xmax = Mathf.FloorToInt(bounds.max.x * 1000f);
            storey.r2 = new IntRect(xmin, ymin, xmax, Mathf.FloorToInt(bounds.max.z * 1000f));
            storey.a = new Int3(storey.r2.xmin, 0, storey.r2.ymin);
            storey.b = new Int3(storey.r2.xmin, 0, storey.r2.ymax);
            storey.c = new Int3(storey.r2.xmax, 0, storey.r2.ymin);
            storey.d = new Int3(storey.r2.xmax, 0, storey.r2.ymax);
            Int3 min = (Int3) bounds.min;
            storey.ymin = min.y;
            Int3 max = (Int3) bounds.max;
            storey.ymax = max.y;
            graph.GetNodes(new GraphNodeDelegateCancelable(storey.<>m__26));
        }

        public void UpdateAreaInit(GraphUpdateObject o)
        {
        }

        public BBTree bbTree
        {
            get
            {
                return this._bbTree;
            }
            set
            {
                this._bbTree = value;
            }
        }

        public TriangleMeshNode[] TriNodes
        {
            get
            {
                return this.nodes;
            }
        }

        public Int3[] vertices
        {
            get
            {
                return this._vertices;
            }
            set
            {
                this._vertices = value;
            }
        }

        [CompilerGenerated]
        private sealed class <GetNearestForceBoth>c__AnonStorey253
        {
            internal bool accurateNearestNode;
            internal NNConstraint constraint;
            internal float maxDistSqr;
            internal float minConstDist;
            internal GraphNode minConstNode;
            internal float minDist;
            internal GraphNode minNode;
            internal Int3 pos;
            internal Vector3 position;

            internal bool <>m__25(GraphNode _node)
            {
                TriangleMeshNode node = _node as TriangleMeshNode;
                if (this.accurateNearestNode)
                {
                    Vector3 vector = node.ClosestPointOnNode(this.position);
                    Vector3 vector2 = ((Vector3) this.pos) - vector;
                    float sqrMagnitude = vector2.sqrMagnitude;
                    if ((this.minNode == null) || (sqrMagnitude < this.minDist))
                    {
                        this.minDist = sqrMagnitude;
                        this.minNode = node;
                    }
                    if (((sqrMagnitude < this.maxDistSqr) && this.constraint.Suitable(node)) && ((this.minConstNode == null) || (sqrMagnitude < this.minConstDist)))
                    {
                        this.minConstDist = sqrMagnitude;
                        this.minConstNode = node;
                    }
                }
                else if (!node.ContainsPoint((Int3) this.position))
                {
                    Int3 num4 = node.position - this.pos;
                    float num2 = num4.sqrMagnitude;
                    if ((this.minNode == null) || (num2 < this.minDist))
                    {
                        this.minDist = num2;
                        this.minNode = node;
                    }
                    if (((num2 < this.maxDistSqr) && this.constraint.Suitable(node)) && ((this.minConstNode == null) || (num2 < this.minConstDist)))
                    {
                        this.minConstDist = num2;
                        this.minConstNode = node;
                    }
                }
                else
                {
                    int num3 = Math.Abs((int) (node.position.y - this.pos.y));
                    if ((this.minNode == null) || (num3 < this.minDist))
                    {
                        this.minDist = num3;
                        this.minNode = node;
                    }
                    if (((num3 < this.maxDistSqr) && this.constraint.Suitable(node)) && ((this.minConstNode == null) || (num3 < this.minConstDist)))
                    {
                        this.minConstDist = num3;
                        this.minConstNode = node;
                    }
                }
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <ScanInternal>c__Iterator16 : IEnumerator, IEnumerable<Progress>, IEnumerator<Progress>, IDisposable, IEnumerable
        {
            internal Progress $current;
            internal int $PC;
            private static OnScanStatus <>f__am$cache3;
            internal NavMeshGraph <>f__this;

            private static void <>m__27(Progress p)
            {
            }

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    if (<>f__am$cache3 == null)
                    {
                        <>f__am$cache3 = new OnScanStatus(NavMeshGraph.<ScanInternal>c__Iterator16.<>m__27);
                    }
                    this.<>f__this.ScanInternal(<>f__am$cache3);
                }
                return false;
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
                NavMeshGraph.<ScanInternal>c__Iterator16 iterator = new NavMeshGraph.<ScanInternal>c__Iterator16();
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

        [CompilerGenerated]
        private sealed class <UpdateArea>c__AnonStorey254
        {
            internal Int3 a;
            internal Int3 b;
            internal Int3 c;
            internal Int3 d;
            internal GraphUpdateObject o;
            internal Rect r;
            internal IntRect r2;
            internal int ymax;
            internal int ymin;

            internal bool <>m__26(GraphNode _node)
            {
                TriangleMeshNode node = _node as TriangleMeshNode;
                bool flag = false;
                int num = 0;
                int num2 = 0;
                int num3 = 0;
                int num4 = 0;
                for (int i = 0; i < 3; i++)
                {
                    Int3 vertex = node.GetVertex(i);
                    Vector3 vector = (Vector3) vertex;
                    if (this.r2.Contains(vertex.x, vertex.z))
                    {
                        flag = true;
                        break;
                    }
                    if (vector.x < this.r.xMin)
                    {
                        num++;
                    }
                    if (vector.x > this.r.xMax)
                    {
                        num2++;
                    }
                    if (vector.z < this.r.yMin)
                    {
                        num3++;
                    }
                    if (vector.z > this.r.yMax)
                    {
                        num4++;
                    }
                }
                if (flag || (((num != 3) && (num2 != 3)) && ((num3 != 3) && (num4 != 3))))
                {
                    for (int j = 0; j < 3; j++)
                    {
                        int num8 = (j <= 1) ? (j + 1) : 0;
                        Int3 num9 = node.GetVertex(j);
                        Int3 num10 = node.GetVertex(num8);
                        if (VectorMath.SegmentsIntersectXZ(this.a, this.b, num9, num10))
                        {
                            flag = true;
                            break;
                        }
                        if (VectorMath.SegmentsIntersectXZ(this.a, this.c, num9, num10))
                        {
                            flag = true;
                            break;
                        }
                        if (VectorMath.SegmentsIntersectXZ(this.c, this.d, num9, num10))
                        {
                            flag = true;
                            break;
                        }
                        if (VectorMath.SegmentsIntersectXZ(this.d, this.b, num9, num10))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if ((flag || node.ContainsPoint(this.a)) || ((node.ContainsPoint(this.b) || node.ContainsPoint(this.c)) || node.ContainsPoint(this.d)))
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        int num11 = 0;
                        int num12 = 0;
                        for (int k = 0; k < 3; k++)
                        {
                            Int3 num14 = node.GetVertex(k);
                            if (num14.y < this.ymin)
                            {
                                num12++;
                            }
                            if (num14.y > this.ymax)
                            {
                                num11++;
                            }
                        }
                        if ((num12 == 3) || (num11 == 3))
                        {
                            return true;
                        }
                        this.o.WillUpdateNode(node);
                        this.o.Apply(node);
                    }
                }
                return true;
            }
        }
    }
}

