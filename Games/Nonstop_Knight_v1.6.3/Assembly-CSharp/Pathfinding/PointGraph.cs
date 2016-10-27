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
    public class PointGraph : NavGraph, IUpdatableGraph
    {
        [JsonMember]
        public Vector3 limits;
        private Int3 lookupCellSize;
        [JsonMember]
        public LayerMask mask;
        [JsonMember]
        public float maxDistance;
        private Int3 maxLookup;
        private Int3 minLookup;
        public int nodeCount;
        private Dictionary<Int3, PointNode> nodeLookup;
        public PointNode[] nodes;
        [JsonMember]
        public bool optimizeFor2D;
        [JsonMember]
        public bool optimizeForSparseGraph;
        [JsonMember]
        public bool raycast = true;
        [JsonMember]
        public bool recursive = true;
        [JsonMember]
        public Transform root;
        [JsonMember]
        public string searchTag;
        [JsonMember]
        public bool thickRaycast;
        [JsonMember]
        public float thickRaycastRadius = 1f;
        private static readonly Int3[] ThreeDNeighbours = new Int3[] { 
            new Int3(-1, 0, -1), new Int3(0, 0, -1), new Int3(1, 0, -1), new Int3(-1, 0, 0), new Int3(0, 0, 0), new Int3(1, 0, 0), new Int3(-1, 0, 1), new Int3(0, 0, 1), new Int3(1, 0, 1), new Int3(-1, -1, -1), new Int3(0, -1, -1), new Int3(1, -1, -1), new Int3(-1, -1, 0), new Int3(0, -1, 0), new Int3(1, -1, 0), new Int3(-1, -1, 1), 
            new Int3(0, -1, 1), new Int3(1, -1, 1), new Int3(-1, 1, -1), new Int3(0, 1, -1), new Int3(1, 1, -1), new Int3(-1, 1, 0), new Int3(0, 1, 0), new Int3(1, 1, 0), new Int3(-1, 1, 1), new Int3(0, 1, 1), new Int3(1, 1, 1)
         };
        [JsonMember]
        public bool use2DPhysics;

        public void AddChildren(ref int c, Transform tr)
        {
            IEnumerator enumerator = tr.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    this.nodes[c].SetPosition((Int3) current.position);
                    this.nodes[c].Walkable = true;
                    this.nodes[c].gameObject = current.gameObject;
                    c++;
                    this.AddChildren(ref c, current);
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

        public PointNode AddNode(Int3 position)
        {
            return this.AddNode<PointNode>(new PointNode(base.active), position);
        }

        public T AddNode<T>(T node, Int3 position) where T: PointNode
        {
            if ((this.nodes == null) || (this.nodeCount == this.nodes.Length))
            {
                PointNode[] nodeArray = new PointNode[(this.nodes == null) ? 4 : Math.Max((int) (this.nodes.Length + 4), (int) (this.nodes.Length * 2))];
                for (int i = 0; i < this.nodeCount; i++)
                {
                    nodeArray[i] = this.nodes[i];
                }
                this.nodes = nodeArray;
            }
            node.SetPosition(position);
            node.GraphIndex = base.graphIndex;
            node.Walkable = true;
            this.nodes[this.nodeCount] = node;
            this.nodeCount++;
            this.AddToLookup(node);
            return node;
        }

        public void AddToLookup(PointNode node)
        {
            if (this.nodeLookup != null)
            {
                PointNode node2;
                Int3 key = this.WorldToLookupSpace(node.position);
                if (this.nodeLookup.Count == 0)
                {
                    this.minLookup = key;
                    this.maxLookup = key;
                }
                else
                {
                    int introduced2 = Math.Min(this.minLookup.x, key.x);
                    int introduced3 = Math.Min(this.minLookup.y, key.y);
                    this.minLookup = new Int3(introduced2, introduced3, Math.Min(this.minLookup.z, key.z));
                    int introduced4 = Math.Max(this.minLookup.x, key.x);
                    int introduced5 = Math.Max(this.minLookup.y, key.y);
                    this.maxLookup = new Int3(introduced4, introduced5, Math.Max(this.minLookup.z, key.z));
                }
                if (node.next != null)
                {
                    throw new Exception("This node has already been added to the lookup structure");
                }
                if (this.nodeLookup.TryGetValue(key, out node2))
                {
                    node.next = node2.next;
                    node2.next = node;
                }
                else
                {
                    this.nodeLookup[key] = node;
                }
            }
        }

        public GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o)
        {
            return GraphUpdateThreading.UnityThread;
        }

        public static int CountChildren(Transform tr)
        {
            int num = 0;
            IEnumerator enumerator = tr.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    num++;
                    num += CountChildren(current);
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
            return num;
        }

        public override int CountNodes()
        {
            return this.nodeCount;
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
                this.nodes = new PointNode[num];
                this.nodeCount = num;
                for (int i = 0; i < this.nodes.Length; i++)
                {
                    if (ctx.reader.ReadInt32() != -1)
                    {
                        this.nodes[i] = new PointNode(base.active);
                        this.nodes[i].DeserializeNode(ctx);
                    }
                }
            }
        }

        public override NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
        {
            return this.GetNearestForce(position, constraint);
        }

        public override NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
        {
            if (this.nodes == null)
            {
                return new NNInfoInternal();
            }
            float num = !constraint.constrainDistance ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistanceSqr;
            float positiveInfinity = float.PositiveInfinity;
            GraphNode node = null;
            float num3 = float.PositiveInfinity;
            GraphNode node2 = null;
            if (this.optimizeForSparseGraph)
            {
                PointNode next;
                Int3 key = this.WorldToLookupSpace((Int3) position);
                Int3 num5 = key - this.minLookup;
                int num6 = 0;
                int introduced48 = Math.Max(num6, Math.Abs(num5.x));
                int introduced49 = Math.Max(introduced48, Math.Abs(num5.y));
                num6 = Math.Max(introduced49, Math.Abs(num5.z));
                num5 = key - this.maxLookup;
                int introduced50 = Math.Max(num6, Math.Abs(num5.x));
                int introduced51 = Math.Max(introduced50, Math.Abs(num5.y));
                num6 = Math.Max(introduced51, Math.Abs(num5.z));
                if (this.nodeLookup.TryGetValue(key, out next))
                {
                    while (next != null)
                    {
                        Vector3 vector = position - ((Vector3) next.position);
                        float sqrMagnitude = vector.sqrMagnitude;
                        if (sqrMagnitude < positiveInfinity)
                        {
                            positiveInfinity = sqrMagnitude;
                            node = next;
                        }
                        if ((constraint == null) || (((sqrMagnitude < num3) && (sqrMagnitude < num)) && constraint.Suitable(next)))
                        {
                            num3 = sqrMagnitude;
                            node2 = next;
                        }
                        next = next.next;
                    }
                }
                for (int i = 1; i <= num6; i++)
                {
                    if (i >= 20)
                    {
                        UnityEngine.Debug.LogWarning("Aborting GetNearest call at maximum distance because it has iterated too many times.\nIf you get this regularly, check your settings for PointGraph -> <b>Optimize For Sparse Graph</b> and PointGraph -> <b>Optimize For 2D</b>.\nThis happens when the closest node was very far away (20*link distance between nodes). When optimizing for sparse graphs, getting the nearest node from far away positions is <b>very slow</b>.\n");
                        break;
                    }
                    if (this.lookupCellSize.y == 0)
                    {
                        Int3 num9 = key + new Int3(-i, 0, -i);
                        for (int j = 0; j <= (2 * i); j++)
                        {
                            if (this.nodeLookup.TryGetValue(num9 + new Int3(j, 0, 0), out next))
                            {
                                while (next != null)
                                {
                                    Vector3 vector2 = position - ((Vector3) next.position);
                                    float num11 = vector2.sqrMagnitude;
                                    if (num11 < positiveInfinity)
                                    {
                                        positiveInfinity = num11;
                                        node = next;
                                    }
                                    if ((constraint == null) || (((num11 < num3) && (num11 < num)) && constraint.Suitable(next)))
                                    {
                                        num3 = num11;
                                        node2 = next;
                                    }
                                    next = next.next;
                                }
                            }
                            if (this.nodeLookup.TryGetValue(num9 + new Int3(j, 0, 2 * i), out next))
                            {
                                while (next != null)
                                {
                                    Vector3 vector3 = position - ((Vector3) next.position);
                                    float num12 = vector3.sqrMagnitude;
                                    if (num12 < positiveInfinity)
                                    {
                                        positiveInfinity = num12;
                                        node = next;
                                    }
                                    if ((constraint == null) || (((num12 < num3) && (num12 < num)) && constraint.Suitable(next)))
                                    {
                                        num3 = num12;
                                        node2 = next;
                                    }
                                    next = next.next;
                                }
                            }
                        }
                        for (int k = 1; k < (2 * i); k++)
                        {
                            if (this.nodeLookup.TryGetValue(num9 + new Int3(0, 0, k), out next))
                            {
                                while (next != null)
                                {
                                    Vector3 vector4 = position - ((Vector3) next.position);
                                    float num14 = vector4.sqrMagnitude;
                                    if (num14 < positiveInfinity)
                                    {
                                        positiveInfinity = num14;
                                        node = next;
                                    }
                                    if ((constraint == null) || (((num14 < num3) && (num14 < num)) && constraint.Suitable(next)))
                                    {
                                        num3 = num14;
                                        node2 = next;
                                    }
                                    next = next.next;
                                }
                            }
                            if (this.nodeLookup.TryGetValue(num9 + new Int3(2 * i, 0, k), out next))
                            {
                                while (next != null)
                                {
                                    Vector3 vector5 = position - ((Vector3) next.position);
                                    float num15 = vector5.sqrMagnitude;
                                    if (num15 < positiveInfinity)
                                    {
                                        positiveInfinity = num15;
                                        node = next;
                                    }
                                    if ((constraint == null) || (((num15 < num3) && (num15 < num)) && constraint.Suitable(next)))
                                    {
                                        num3 = num15;
                                        node2 = next;
                                    }
                                    next = next.next;
                                }
                            }
                        }
                    }
                    else
                    {
                        Int3 num16 = key + new Int3(-i, -i, -i);
                        for (int m = 0; m <= (2 * i); m++)
                        {
                            for (int num18 = 0; num18 <= (2 * i); num18++)
                            {
                                if (this.nodeLookup.TryGetValue(num16 + new Int3(m, num18, 0), out next))
                                {
                                    while (next != null)
                                    {
                                        Vector3 vector6 = position - ((Vector3) next.position);
                                        float num19 = vector6.sqrMagnitude;
                                        if (num19 < positiveInfinity)
                                        {
                                            positiveInfinity = num19;
                                            node = next;
                                        }
                                        if ((constraint == null) || (((num19 < num3) && (num19 < num)) && constraint.Suitable(next)))
                                        {
                                            num3 = num19;
                                            node2 = next;
                                        }
                                        next = next.next;
                                    }
                                }
                                if (this.nodeLookup.TryGetValue(num16 + new Int3(m, num18, 2 * i), out next))
                                {
                                    while (next != null)
                                    {
                                        Vector3 vector7 = position - ((Vector3) next.position);
                                        float num20 = vector7.sqrMagnitude;
                                        if (num20 < positiveInfinity)
                                        {
                                            positiveInfinity = num20;
                                            node = next;
                                        }
                                        if ((constraint == null) || (((num20 < num3) && (num20 < num)) && constraint.Suitable(next)))
                                        {
                                            num3 = num20;
                                            node2 = next;
                                        }
                                        next = next.next;
                                    }
                                }
                            }
                        }
                        for (int n = 1; n < (2 * i); n++)
                        {
                            for (int num22 = 0; num22 <= (2 * i); num22++)
                            {
                                if (this.nodeLookup.TryGetValue(num16 + new Int3(0, num22, n), out next))
                                {
                                    while (next != null)
                                    {
                                        Vector3 vector8 = position - ((Vector3) next.position);
                                        float num23 = vector8.sqrMagnitude;
                                        if (num23 < positiveInfinity)
                                        {
                                            positiveInfinity = num23;
                                            node = next;
                                        }
                                        if ((constraint == null) || (((num23 < num3) && (num23 < num)) && constraint.Suitable(next)))
                                        {
                                            num3 = num23;
                                            node2 = next;
                                        }
                                        next = next.next;
                                    }
                                }
                                if (this.nodeLookup.TryGetValue(num16 + new Int3(2 * i, num22, n), out next))
                                {
                                    while (next != null)
                                    {
                                        Vector3 vector9 = position - ((Vector3) next.position);
                                        float num24 = vector9.sqrMagnitude;
                                        if (num24 < positiveInfinity)
                                        {
                                            positiveInfinity = num24;
                                            node = next;
                                        }
                                        if ((constraint == null) || (((num24 < num3) && (num24 < num)) && constraint.Suitable(next)))
                                        {
                                            num3 = num24;
                                            node2 = next;
                                        }
                                        next = next.next;
                                    }
                                }
                            }
                        }
                        for (int num25 = 1; num25 < (2 * i); num25++)
                        {
                            for (int num26 = 1; num26 < (2 * i); num26++)
                            {
                                if (this.nodeLookup.TryGetValue(num16 + new Int3(num25, 0, num26), out next))
                                {
                                    while (next != null)
                                    {
                                        Vector3 vector10 = position - ((Vector3) next.position);
                                        float num27 = vector10.sqrMagnitude;
                                        if (num27 < positiveInfinity)
                                        {
                                            positiveInfinity = num27;
                                            node = next;
                                        }
                                        if ((constraint == null) || (((num27 < num3) && (num27 < num)) && constraint.Suitable(next)))
                                        {
                                            num3 = num27;
                                            node2 = next;
                                        }
                                        next = next.next;
                                    }
                                }
                                if (this.nodeLookup.TryGetValue(num16 + new Int3(num25, 2 * i, num26), out next))
                                {
                                    while (next != null)
                                    {
                                        Vector3 vector11 = position - ((Vector3) next.position);
                                        float num28 = vector11.sqrMagnitude;
                                        if (num28 < positiveInfinity)
                                        {
                                            positiveInfinity = num28;
                                            node = next;
                                        }
                                        if ((constraint == null) || (((num28 < num3) && (num28 < num)) && constraint.Suitable(next)))
                                        {
                                            num3 = num28;
                                            node2 = next;
                                        }
                                        next = next.next;
                                    }
                                }
                            }
                        }
                    }
                    if (node2 != null)
                    {
                        num6 = Math.Min(num6, i + 1);
                    }
                }
            }
            else
            {
                for (int num29 = 0; num29 < this.nodeCount; num29++)
                {
                    PointNode node4 = this.nodes[num29];
                    Vector3 vector12 = position - ((Vector3) node4.position);
                    float num30 = vector12.sqrMagnitude;
                    if (num30 < positiveInfinity)
                    {
                        positiveInfinity = num30;
                        node = node4;
                    }
                    if ((constraint == null) || (((num30 < num3) && (num30 < num)) && constraint.Suitable(node4)))
                    {
                        num3 = num30;
                        node2 = node4;
                    }
                }
            }
            NNInfoInternal internal2 = new NNInfoInternal(node);
            internal2.constrainedNode = node2;
            if (node2 != null)
            {
                internal2.constClampedPosition = (Vector3) node2.position;
                return internal2;
            }
            if (node != null)
            {
                internal2.constrainedNode = node;
                internal2.constClampedPosition = (Vector3) node.position;
            }
            return internal2;
        }

        public override void GetNodes(GraphNodeDelegateCancelable del)
        {
            if (this.nodes != null)
            {
                for (int i = 0; (i < this.nodeCount) && del(this.nodes[i]); i++)
                {
                }
            }
        }

        public virtual bool IsValidConnection(GraphNode a, GraphNode b, out float dist)
        {
            dist = 0f;
            if (a.Walkable && b.Walkable)
            {
                Vector3 vector = (Vector3) (a.position - b.position);
                if (((!Mathf.Approximately(this.limits.x, 0f) && (Mathf.Abs(vector.x) > this.limits.x)) || (!Mathf.Approximately(this.limits.y, 0f) && (Mathf.Abs(vector.y) > this.limits.y))) || (!Mathf.Approximately(this.limits.z, 0f) && (Mathf.Abs(vector.z) > this.limits.z)))
                {
                    return false;
                }
                dist = vector.magnitude;
                if ((this.maxDistance == 0f) || (dist < this.maxDistance))
                {
                    if (!this.raycast)
                    {
                        return true;
                    }
                    Ray ray = new Ray((Vector3) a.position, (Vector3) (b.position - a.position));
                    Ray ray2 = new Ray((Vector3) b.position, (Vector3) (a.position - b.position));
                    if (this.use2DPhysics)
                    {
                        if (this.thickRaycast)
                        {
                            if ((Physics2D.CircleCast(ray.origin, this.thickRaycastRadius, ray.direction, dist, (int) this.mask) == 0) && (Physics2D.CircleCast(ray2.origin, this.thickRaycastRadius, ray2.direction, dist, (int) this.mask) == 0))
                            {
                                return true;
                            }
                        }
                        else if ((Physics2D.Linecast((Vector3) a.position, (Vector3) b.position, (int) this.mask) == 0) && (Physics2D.Linecast((Vector3) b.position, (Vector3) a.position, (int) this.mask) == 0))
                        {
                            return true;
                        }
                    }
                    else if (this.thickRaycast)
                    {
                        if (!Physics.SphereCast(ray, this.thickRaycastRadius, dist, (int) this.mask) && !Physics.SphereCast(ray2, this.thickRaycastRadius, dist, (int) this.mask))
                        {
                            return true;
                        }
                    }
                    else if (!Physics.Linecast((Vector3) a.position, (Vector3) b.position, (int) this.mask) && !Physics.Linecast((Vector3) b.position, (Vector3) a.position, (int) this.mask))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override void PostDeserialization()
        {
            this.RebuildNodeLookup();
        }

        public void RebuildNodeLookup()
        {
            if (this.optimizeForSparseGraph)
            {
                if (this.maxDistance == 0f)
                {
                    this.lookupCellSize = (Int3) this.limits;
                }
                else
                {
                    this.lookupCellSize.x = Mathf.CeilToInt(1000f * ((this.limits.x == 0f) ? this.maxDistance : Mathf.Min(this.maxDistance, this.limits.x)));
                    this.lookupCellSize.y = Mathf.CeilToInt(1000f * ((this.limits.y == 0f) ? this.maxDistance : Mathf.Min(this.maxDistance, this.limits.y)));
                    this.lookupCellSize.z = Mathf.CeilToInt(1000f * ((this.limits.z == 0f) ? this.maxDistance : Mathf.Min(this.maxDistance, this.limits.z)));
                }
                if (this.optimizeFor2D)
                {
                    this.lookupCellSize.y = 0;
                }
                if (this.nodeLookup == null)
                {
                    this.nodeLookup = new Dictionary<Int3, PointNode>();
                }
                this.nodeLookup.Clear();
                for (int i = 0; i < this.nodeCount; i++)
                {
                    PointNode node = this.nodes[i];
                    this.AddToLookup(node);
                }
            }
        }

        public override void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
        {
            base.RelocateNodes(oldMatrix, newMatrix);
            this.RebuildNodeLookup();
        }

        [DebuggerHidden]
        public override IEnumerable<Progress> ScanInternal()
        {
            <ScanInternal>c__Iterator17 iterator = new <ScanInternal>c__Iterator17();
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
            ctx.writer.Write(this.nodeCount);
            for (int i = 0; i < this.nodeCount; i++)
            {
                if (this.nodes[i] == null)
                {
                    ctx.writer.Write(-1);
                }
                else
                {
                    ctx.writer.Write(0);
                    this.nodes[i].SerializeNode(ctx);
                }
            }
        }

        public void UpdateArea(GraphUpdateObject guo)
        {
            if (this.nodes != null)
            {
                for (int i = 0; i < this.nodeCount; i++)
                {
                    if (guo.bounds.Contains((Vector3) this.nodes[i].position))
                    {
                        guo.WillUpdateNode(this.nodes[i]);
                        guo.Apply(this.nodes[i]);
                    }
                }
                if (guo.updatePhysics)
                {
                    Bounds bounds = guo.bounds;
                    if (this.thickRaycast)
                    {
                        bounds.Expand((float) (this.thickRaycastRadius * 2f));
                    }
                    List<GraphNode> list = ListPool<GraphNode>.Claim();
                    List<uint> list2 = ListPool<uint>.Claim();
                    for (int j = 0; j < this.nodeCount; j++)
                    {
                        PointNode a = this.nodes[j];
                        Vector3 position = (Vector3) a.position;
                        List<GraphNode> list3 = null;
                        List<uint> list4 = null;
                        for (int k = 0; k < this.nodeCount; k++)
                        {
                            if (k != j)
                            {
                                Vector3 b = (Vector3) this.nodes[k].position;
                                if (VectorMath.SegmentIntersectsBounds(bounds, position, b))
                                {
                                    float num4;
                                    PointNode node = this.nodes[k];
                                    bool flag = a.ContainsConnection(node);
                                    bool flag2 = this.IsValidConnection(a, node, out num4);
                                    if (!flag && flag2)
                                    {
                                        if (list3 == null)
                                        {
                                            list.Clear();
                                            list2.Clear();
                                            list3 = list;
                                            list4 = list2;
                                            list3.AddRange(a.connections);
                                            list4.AddRange(a.connectionCosts);
                                        }
                                        uint item = (uint) Mathf.RoundToInt(num4 * 1000f);
                                        list3.Add(node);
                                        list4.Add(item);
                                    }
                                    else if (flag && !flag2)
                                    {
                                        if (list3 == null)
                                        {
                                            list.Clear();
                                            list2.Clear();
                                            list3 = list;
                                            list4 = list2;
                                            list3.AddRange(a.connections);
                                            list4.AddRange(a.connectionCosts);
                                        }
                                        int index = list3.IndexOf(node);
                                        if (index != -1)
                                        {
                                            list3.RemoveAt(index);
                                            list4.RemoveAt(index);
                                        }
                                    }
                                }
                            }
                        }
                        if (list3 != null)
                        {
                            a.connections = list3.ToArray();
                            a.connectionCosts = list4.ToArray();
                        }
                    }
                    ListPool<GraphNode>.Release(list);
                    ListPool<uint>.Release(list2);
                }
            }
        }

        public void UpdateAreaInit(GraphUpdateObject o)
        {
        }

        private Int3 WorldToLookupSpace(Int3 p)
        {
            Int3 zero = Int3.zero;
            zero.x = (this.lookupCellSize.x == 0) ? 0 : (p.x / this.lookupCellSize.x);
            zero.y = (this.lookupCellSize.y == 0) ? 0 : (p.y / this.lookupCellSize.y);
            zero.z = (this.lookupCellSize.z == 0) ? 0 : (p.z / this.lookupCellSize.z);
            return zero;
        }

        [CompilerGenerated]
        private sealed class <ScanInternal>c__Iterator17 : IEnumerator, IEnumerable<Progress>, IEnumerator<Progress>, IDisposable, IEnumerable
        {
            internal Progress $current;
            internal int $PC;
            internal IEnumerator <$s_34>__5;
            internal PointGraph <>f__this;
            internal int <c>__4;
            internal Transform <child>__6;
            internal List<PointNode> <connections>__9;
            internal List<uint> <costs>__10;
            internal float <dist>__18;
            internal float <dist>__21;
            internal GameObject[] <gos>__0;
            internal int <i>__1;
            internal int <i>__11;
            internal int <i>__2;
            internal int <i>__3;
            internal int <i>__7;
            internal int <j>__15;
            internal int <j>__19;
            internal int <l>__14;
            internal PointNode <node>__12;
            internal Int3 <np>__16;
            internal PointNode <other>__17;
            internal PointNode <other>__20;
            internal Int3 <p>__13;
            internal int <startID>__8;

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
                        this.$current = new Progress(0f, "Searching for GameObjects");
                        this.$PC = 1;
                        goto Label_0764;

                    case 1:
                        if (this.<>f__this.root != null)
                        {
                            if (!this.<>f__this.recursive)
                            {
                                this.<>f__this.nodes = new PointNode[this.<>f__this.root.childCount];
                                this.<>f__this.nodeCount = this.<>f__this.nodes.Length;
                                this.<i>__3 = 0;
                                while (this.<i>__3 < this.<>f__this.nodes.Length)
                                {
                                    this.<>f__this.nodes[this.<i>__3] = new PointNode(this.<>f__this.active);
                                    this.<i>__3++;
                                }
                                this.<c>__4 = 0;
                                this.<$s_34>__5 = this.<>f__this.root.GetEnumerator();
                                try
                                {
                                    while (this.<$s_34>__5.MoveNext())
                                    {
                                        this.<child>__6 = (Transform) this.<$s_34>__5.Current;
                                        this.<>f__this.nodes[this.<c>__4].SetPosition((Int3) this.<child>__6.position);
                                        this.<>f__this.nodes[this.<c>__4].Walkable = true;
                                        this.<>f__this.nodes[this.<c>__4].gameObject = this.<child>__6.gameObject;
                                        this.<c>__4++;
                                    }
                                }
                                finally
                                {
                                    IDisposable disposable = this.<$s_34>__5 as IDisposable;
                                    if (disposable == null)
                                    {
                                    }
                                    disposable.Dispose();
                                }
                            }
                            else
                            {
                                this.<>f__this.nodes = new PointNode[PointGraph.CountChildren(this.<>f__this.root)];
                                this.<>f__this.nodeCount = this.<>f__this.nodes.Length;
                                this.<i>__7 = 0;
                                while (this.<i>__7 < this.<>f__this.nodes.Length)
                                {
                                    this.<>f__this.nodes[this.<i>__7] = new PointNode(this.<>f__this.active);
                                    this.<i>__7++;
                                }
                                this.<startID>__8 = 0;
                                this.<>f__this.AddChildren(ref this.<startID>__8, this.<>f__this.root);
                            }
                            break;
                        }
                        this.<gos>__0 = (this.<>f__this.searchTag == null) ? null : GameObject.FindGameObjectsWithTag(this.<>f__this.searchTag);
                        if (this.<gos>__0 != null)
                        {
                            this.$current = new Progress(0.1f, "Creating nodes");
                            this.$PC = 2;
                            goto Label_0764;
                        }
                        this.<>f__this.nodes = new PointNode[0];
                        this.<>f__this.nodeCount = 0;
                        goto Label_0762;

                    case 2:
                        this.<>f__this.nodes = new PointNode[this.<gos>__0.Length];
                        this.<>f__this.nodeCount = this.<>f__this.nodes.Length;
                        this.<i>__1 = 0;
                        while (this.<i>__1 < this.<>f__this.nodes.Length)
                        {
                            this.<>f__this.nodes[this.<i>__1] = new PointNode(this.<>f__this.active);
                            this.<i>__1++;
                        }
                        this.<i>__2 = 0;
                        while (this.<i>__2 < this.<gos>__0.Length)
                        {
                            this.<>f__this.nodes[this.<i>__2].SetPosition((Int3) this.<gos>__0[this.<i>__2].transform.position);
                            this.<>f__this.nodes[this.<i>__2].Walkable = true;
                            this.<>f__this.nodes[this.<i>__2].gameObject = this.<gos>__0[this.<i>__2].gameObject;
                            this.<i>__2++;
                        }
                        break;

                    case 3:
                        this.<>f__this.RebuildNodeLookup();
                        goto Label_0467;

                    case 4:
                        goto Label_04F0;

                    default:
                        goto Label_0762;
                }
                if (this.<>f__this.optimizeForSparseGraph)
                {
                    this.$current = new Progress(0.15f, "Building node lookup");
                    this.$PC = 3;
                    goto Label_0764;
                }
            Label_0467:
                if (this.<>f__this.maxDistance >= 0f)
                {
                    this.<connections>__9 = new List<PointNode>();
                    this.<costs>__10 = new List<uint>();
                    this.<i>__11 = 0;
                    while (this.<i>__11 < this.<>f__this.nodes.Length)
                    {
                        if ((this.<i>__11 % 0x200) == 0)
                        {
                            this.$current = new Progress(Mathf.Lerp(0.15f, 1f, ((float) this.<i>__11) / ((float) this.<>f__this.nodes.Length)), "Connecting nodes");
                            this.$PC = 4;
                            goto Label_0764;
                        }
                    Label_04F0:
                        this.<connections>__9.Clear();
                        this.<costs>__10.Clear();
                        this.<node>__12 = this.<>f__this.nodes[this.<i>__11];
                        if (this.<>f__this.optimizeForSparseGraph)
                        {
                            this.<p>__13 = this.<>f__this.WorldToLookupSpace(this.<node>__12.position);
                            this.<l>__14 = (this.<>f__this.lookupCellSize.y != 0) ? PointGraph.ThreeDNeighbours.Length : 9;
                            this.<j>__15 = 0;
                            while (this.<j>__15 < this.<l>__14)
                            {
                                this.<np>__16 = this.<p>__13 + PointGraph.ThreeDNeighbours[this.<j>__15];
                                if (this.<>f__this.nodeLookup.TryGetValue(this.<np>__16, out this.<other>__17))
                                {
                                    while (this.<other>__17 != null)
                                    {
                                        if (this.<>f__this.IsValidConnection(this.<node>__12, this.<other>__17, out this.<dist>__18))
                                        {
                                            this.<connections>__9.Add(this.<other>__17);
                                            this.<costs>__10.Add((uint) Mathf.RoundToInt(this.<dist>__18 * 1000f));
                                        }
                                        this.<other>__17 = this.<other>__17.next;
                                    }
                                }
                                this.<j>__15++;
                            }
                        }
                        else
                        {
                            this.<j>__19 = 0;
                            while (this.<j>__19 < this.<>f__this.nodes.Length)
                            {
                                if (this.<i>__11 != this.<j>__19)
                                {
                                    this.<other>__20 = this.<>f__this.nodes[this.<j>__19];
                                    if (this.<>f__this.IsValidConnection(this.<node>__12, this.<other>__20, out this.<dist>__21))
                                    {
                                        this.<connections>__9.Add(this.<other>__20);
                                        this.<costs>__10.Add((uint) Mathf.RoundToInt(this.<dist>__21 * 1000f));
                                    }
                                }
                                this.<j>__19++;
                            }
                        }
                        this.<node>__12.connections = this.<connections>__9.ToArray();
                        this.<node>__12.connectionCosts = this.<costs>__10.ToArray();
                        this.<i>__11++;
                    }
                }
                this.$PC = -1;
            Label_0762:
                return false;
            Label_0764:
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
                PointGraph.<ScanInternal>c__Iterator17 iterator = new PointGraph.<ScanInternal>c__Iterator17();
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
    }
}

