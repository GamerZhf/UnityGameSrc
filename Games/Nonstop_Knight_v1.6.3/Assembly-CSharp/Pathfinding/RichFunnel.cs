namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class RichFunnel : RichPathPart
    {
        private int checkForDestroyedNodesCounter;
        private int currentNode;
        private Vector3 currentPosition;
        public Vector3 exactEnd;
        public Vector3 exactStart;
        public FunnelSimplification funnelSimplificationMode = FunnelSimplification.Iterative;
        private NavGraph graph = null;
        private readonly List<Vector3> left = ListPool<Vector3>.Claim();
        private List<TriangleMeshNode> nodes = new List<TriangleMeshNode>();
        private RichPath path;
        private readonly List<Vector3> right = ListPool<Vector3>.Claim();
        private int[] triBuffer = new int[3];

        public void BuildFunnelCorridor(List<GraphNode> nodes, int start, int end)
        {
            this.exactStart = (nodes[start] as MeshNode).ClosestPointOnNode(this.exactStart);
            this.exactEnd = (nodes[end] as MeshNode).ClosestPointOnNode(this.exactEnd);
            this.left.Clear();
            this.right.Clear();
            this.left.Add(this.exactStart);
            this.right.Add(this.exactStart);
            this.nodes.Clear();
            IRaycastableGraph graph = this.graph as IRaycastableGraph;
            if ((graph == null) || (this.funnelSimplificationMode == FunnelSimplification.None))
            {
                if (this.nodes.Capacity < (end - start))
                {
                    this.nodes.Capacity = end - start;
                }
                for (int j = start; j <= end; j++)
                {
                    TriangleMeshNode item = nodes[j] as TriangleMeshNode;
                    if (item != null)
                    {
                        this.nodes.Add(item);
                    }
                }
            }
            else
            {
                List<GraphNode> result = ListPool<GraphNode>.Claim(end - start);
                switch (this.funnelSimplificationMode)
                {
                    case FunnelSimplification.Iterative:
                        this.SimplifyPath(graph, nodes, start, end, result, this.exactStart, this.exactEnd);
                        break;

                    case FunnelSimplification.RecursiveBinary:
                        SimplifyPath2(graph, nodes, start, end, result, this.exactStart, this.exactEnd);
                        break;

                    case FunnelSimplification.RecursiveTrinary:
                        SimplifyPath3(graph, nodes, start, end, result, this.exactStart, this.exactEnd, 0);
                        break;
                }
                if (this.nodes.Capacity < result.Count)
                {
                    this.nodes.Capacity = result.Count;
                }
                for (int k = 0; k < result.Count; k++)
                {
                    TriangleMeshNode node = result[k] as TriangleMeshNode;
                    if (node != null)
                    {
                        this.nodes.Add(node);
                    }
                }
                ListPool<GraphNode>.Release(result);
            }
            for (int i = 0; i < (this.nodes.Count - 1); i++)
            {
                this.nodes[i].GetPortal(this.nodes[i + 1], this.left, this.right, false);
            }
            this.left.Add(this.exactEnd);
            this.right.Add(this.exactEnd);
        }

        public bool FindNextCorners(Vector3 origin, int startIndex, List<Vector3> funnelPath, int numCorners, out bool lastCorner)
        {
            lastCorner = false;
            if (this.left == null)
            {
                throw new Exception("left list is null");
            }
            if (this.right == null)
            {
                throw new Exception("right list is null");
            }
            if (funnelPath == null)
            {
                throw new ArgumentNullException("funnelPath");
            }
            if (this.left.Count != this.right.Count)
            {
                throw new ArgumentException("left and right lists must have equal length");
            }
            int count = this.left.Count;
            if (count == 0)
            {
                throw new ArgumentException("no diagonals");
            }
            if ((count - startIndex) < 3)
            {
                funnelPath.Add(this.left[count - 1]);
                lastCorner = true;
                return true;
            }
            while ((this.left[startIndex + 1] == this.left[startIndex + 2]) && (this.right[startIndex + 1] == this.right[startIndex + 2]))
            {
                startIndex++;
                if ((count - startIndex) <= 3)
                {
                    return false;
                }
            }
            Vector3 p = this.left[startIndex + 2];
            if (p == this.left[startIndex + 1])
            {
                p = this.right[startIndex + 2];
            }
            while (VectorMath.IsColinearXZ(origin, this.left[startIndex + 1], this.right[startIndex + 1]) || (VectorMath.RightOrColinearXZ(this.left[startIndex + 1], this.right[startIndex + 1], p) == VectorMath.RightOrColinearXZ(this.left[startIndex + 1], this.right[startIndex + 1], origin)))
            {
                startIndex++;
                if ((count - startIndex) < 3)
                {
                    funnelPath.Add(this.left[count - 1]);
                    lastCorner = true;
                    return true;
                }
                p = this.left[startIndex + 2];
                if (p == this.left[startIndex + 1])
                {
                    p = this.right[startIndex + 2];
                }
            }
            Vector3 a = origin;
            Vector3 b = this.left[startIndex + 1];
            Vector3 vector4 = this.right[startIndex + 1];
            int num2 = startIndex;
            int num3 = startIndex + 1;
            int num4 = startIndex + 1;
            for (int i = startIndex + 2; i < count; i++)
            {
                if (funnelPath.Count >= numCorners)
                {
                    return true;
                }
                if (funnelPath.Count > 0x7d0)
                {
                    Debug.LogWarning("Avoiding infinite loop. Remove this check if you have this long paths.");
                    break;
                }
                Vector3 c = this.left[i];
                Vector3 vector6 = this.right[i];
                if (VectorMath.SignedTriangleAreaTimes2XZ(a, vector4, vector6) >= 0f)
                {
                    if ((a == vector4) || (VectorMath.SignedTriangleAreaTimes2XZ(a, b, vector6) <= 0f))
                    {
                        vector4 = vector6;
                        num3 = i;
                    }
                    else
                    {
                        funnelPath.Add(b);
                        a = b;
                        num2 = num4;
                        b = a;
                        vector4 = a;
                        num4 = num2;
                        num3 = num2;
                        i = num2;
                        continue;
                    }
                }
                if (VectorMath.SignedTriangleAreaTimes2XZ(a, b, c) <= 0f)
                {
                    if ((a == b) || (VectorMath.SignedTriangleAreaTimes2XZ(a, vector4, c) >= 0f))
                    {
                        b = c;
                        num4 = i;
                    }
                    else
                    {
                        funnelPath.Add(vector4);
                        a = vector4;
                        num2 = num3;
                        b = a;
                        vector4 = a;
                        num4 = num2;
                        num3 = num2;
                        i = num2;
                    }
                }
            }
            lastCorner = true;
            funnelPath.Add(this.left[count - 1]);
            return true;
        }

        public void FindWalls(List<Vector3> wallBuffer, float range)
        {
            this.FindWalls(this.currentNode, wallBuffer, this.currentPosition, range);
        }

        private void FindWalls(int nodeIndex, List<Vector3> wallBuffer, Vector3 position, float range)
        {
            if (range > 0f)
            {
                bool flag = false;
                bool flag2 = false;
                range *= range;
                position.y = 0f;
                for (int i = 0; !flag || !flag2; i = (i >= 0) ? (-i - 1) : -i)
                {
                    if (((i >= 0) || !flag) && ((i <= 0) || !flag2))
                    {
                        if ((i < 0) && ((nodeIndex + i) < 0))
                        {
                            flag = true;
                        }
                        else if ((i > 0) && ((nodeIndex + i) >= this.nodes.Count))
                        {
                            flag2 = true;
                        }
                        else
                        {
                            TriangleMeshNode node = (((nodeIndex + i) - 1) >= 0) ? this.nodes[(nodeIndex + i) - 1] : null;
                            TriangleMeshNode node2 = this.nodes[nodeIndex + i];
                            TriangleMeshNode node3 = (((nodeIndex + i) + 1) < this.nodes.Count) ? this.nodes[(nodeIndex + i) + 1] : null;
                            if (node2.Destroyed)
                            {
                                break;
                            }
                            Vector3 vector = node2.ClosestPointOnNodeXZ(position) - position;
                            if (vector.sqrMagnitude > range)
                            {
                                if (i < 0)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    flag2 = true;
                                }
                            }
                            else
                            {
                                for (int j = 0; j < 3; j++)
                                {
                                    this.triBuffer[j] = 0;
                                }
                                for (int k = 0; k < node2.connections.Length; k++)
                                {
                                    TriangleMeshNode node4 = node2.connections[k] as TriangleMeshNode;
                                    if (node4 != null)
                                    {
                                        int index = -1;
                                        for (int n = 0; n < 3; n++)
                                        {
                                            for (int num6 = 0; num6 < 3; num6++)
                                            {
                                                if ((node2.GetVertex(n) == node4.GetVertex((num6 + 1) % 3)) && (node2.GetVertex((n + 1) % 3) == node4.GetVertex(num6)))
                                                {
                                                    index = n;
                                                    n = 3;
                                                    break;
                                                }
                                            }
                                        }
                                        if (index != -1)
                                        {
                                            this.triBuffer[index] = ((node4 != node) && (node4 != node3)) ? 1 : 2;
                                        }
                                    }
                                }
                                for (int m = 0; m < 3; m++)
                                {
                                    if (this.triBuffer[m] == 0)
                                    {
                                        wallBuffer.Add((Vector3) node2.GetVertex(m));
                                        wallBuffer.Add((Vector3) node2.GetVertex((m + 1) % 3));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public RichFunnel Initialize(RichPath path, NavGraph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            if (this.graph != null)
            {
                throw new InvalidOperationException("Trying to initialize an already initialized object. " + graph);
            }
            this.graph = graph;
            this.path = path;
            return this;
        }

        public override void OnEnterPool()
        {
            this.left.Clear();
            this.right.Clear();
            this.nodes.Clear();
            this.graph = null;
            this.currentNode = 0;
            this.checkForDestroyedNodesCounter = 0;
        }

        public void SimplifyPath(IRaycastableGraph graph, List<GraphNode> nodes, int start, int end, List<GraphNode> result, Vector3 startPoint, Vector3 endPoint)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            if (start > end)
            {
                throw new ArgumentException("start >= end");
            }
            int num = start;
            int num2 = 0;
        Label_0028:
            if (num2++ > 0x3e8)
            {
                Debug.LogError("!!!");
            }
            else if (start == end)
            {
                result.Add(nodes[end]);
            }
            else
            {
                int count = result.Count;
                int num4 = end + 1;
                int num5 = start + 1;
                bool flag = false;
                while (num4 > (num5 + 1))
                {
                    GraphHitInfo info;
                    int num6 = (num4 + num5) / 2;
                    Vector3 vector = (start != num) ? ((Vector3) nodes[start].position) : startPoint;
                    Vector3 vector2 = (num6 != end) ? ((Vector3) nodes[num6].position) : endPoint;
                    if (graph.Linecast(vector, vector2, nodes[start], out info))
                    {
                        num4 = num6;
                    }
                    else
                    {
                        flag = true;
                        num5 = num6;
                    }
                }
                if (!flag)
                {
                    result.Add(nodes[start]);
                    start = num5;
                }
                else
                {
                    GraphHitInfo info2;
                    Vector3 vector3 = (start != num) ? ((Vector3) nodes[start].position) : startPoint;
                    Vector3 vector4 = (num5 != end) ? ((Vector3) nodes[num5].position) : endPoint;
                    graph.Linecast(vector3, vector4, nodes[start], out info2, result);
                    long num7 = 0L;
                    long num8 = 0L;
                    for (int i = start; i <= num5; i++)
                    {
                        num7 += nodes[i].Penalty + ((this.path.seeker == null) ? ((long) 0) : ((long) this.path.seeker.tagPenalties[nodes[i].Tag]));
                    }
                    for (int j = count; j < result.Count; j++)
                    {
                        num8 += result[j].Penalty + ((this.path.seeker == null) ? ((long) 0) : ((long) this.path.seeker.tagPenalties[result[j].Tag]));
                    }
                    if ((((num7 * 1.4) * ((num5 - start) + 1)) < (num8 * (result.Count - count))) || (result[result.Count - 1] != nodes[num5]))
                    {
                        result.RemoveRange(count, result.Count - count);
                        result.Add(nodes[start]);
                        start++;
                    }
                    else
                    {
                        result.RemoveAt(result.Count - 1);
                        start = num5;
                    }
                }
                goto Label_0028;
            }
        }

        public static void SimplifyPath2(IRaycastableGraph rcg, List<GraphNode> nodes, int start, int end, List<GraphNode> result, Vector3 startPoint, Vector3 endPoint)
        {
            int count = result.Count;
            if (end <= (start + 1))
            {
                result.Add(nodes[start]);
                result.Add(nodes[end]);
            }
            else
            {
                GraphHitInfo info;
                if (rcg.Linecast(startPoint, endPoint, nodes[start], out info, result) || (result[result.Count - 1] != nodes[end]))
                {
                    result.RemoveRange(count, result.Count - count);
                    int num2 = -1;
                    float positiveInfinity = float.PositiveInfinity;
                    for (int i = start + 1; i < end; i++)
                    {
                        float num5 = VectorMath.SqrDistancePointSegment(startPoint, endPoint, (Vector3) nodes[i].position);
                        if ((num2 == -1) || (num5 < positiveInfinity))
                        {
                            num2 = i;
                            positiveInfinity = num5;
                        }
                    }
                    SimplifyPath2(rcg, nodes, start, num2, result, startPoint, (Vector3) nodes[num2].position);
                    result.RemoveAt(result.Count - 1);
                    SimplifyPath2(rcg, nodes, num2, end, result, (Vector3) nodes[num2].position, endPoint);
                }
            }
        }

        public static void SimplifyPath3(IRaycastableGraph rcg, List<GraphNode> nodes, int start, int end, List<GraphNode> result, Vector3 startPoint, Vector3 endPoint, [Optional, DefaultParameterValue(0)] int depth)
        {
            if (start == end)
            {
                result.Add(nodes[start]);
            }
            else if ((start + 1) == end)
            {
                result.Add(nodes[start]);
                result.Add(nodes[end]);
            }
            else
            {
                GraphHitInfo info;
                int count = result.Count;
                if (rcg.Linecast(startPoint, endPoint, nodes[start], out info, result) || (result[result.Count - 1] != nodes[end]))
                {
                    result.RemoveRange(count, result.Count - count);
                    int num2 = 0;
                    float num3 = 0f;
                    for (int i = start + 1; i < (end - 1); i++)
                    {
                        float num5 = VectorMath.SqrDistancePointSegment(startPoint, endPoint, (Vector3) nodes[i].position);
                        if (num5 > num3)
                        {
                            num2 = i;
                            num3 = num5;
                        }
                    }
                    int num6 = (num2 + start) / 2;
                    int num7 = (num2 + end) / 2;
                    if (num6 == num7)
                    {
                        SimplifyPath3(rcg, nodes, start, num6, result, startPoint, (Vector3) nodes[num6].position, 0);
                        result.RemoveAt(result.Count - 1);
                        SimplifyPath3(rcg, nodes, num6, end, result, (Vector3) nodes[num6].position, endPoint, depth + 1);
                    }
                    else
                    {
                        SimplifyPath3(rcg, nodes, start, num6, result, startPoint, (Vector3) nodes[num6].position, depth + 1);
                        result.RemoveAt(result.Count - 1);
                        SimplifyPath3(rcg, nodes, num6, num7, result, (Vector3) nodes[num6].position, (Vector3) nodes[num7].position, depth + 1);
                        result.RemoveAt(result.Count - 1);
                        SimplifyPath3(rcg, nodes, num7, end, result, (Vector3) nodes[num7].position, endPoint, depth + 1);
                    }
                }
            }
        }

        public Vector3 Update(Vector3 position, List<Vector3> buffer, int numCorners, out bool lastCorner, out bool requiresRepath)
        {
            lastCorner = false;
            requiresRepath = false;
            Int3 p = (Int3) position;
            if (this.nodes[this.currentNode].Destroyed)
            {
                requiresRepath = true;
                lastCorner = false;
                buffer.Add(position);
                return position;
            }
            if (this.nodes[this.currentNode].ContainsPoint(p))
            {
                if (this.checkForDestroyedNodesCounter >= 10)
                {
                    this.checkForDestroyedNodesCounter = 0;
                    int num2 = 0;
                    int count = this.nodes.Count;
                    while (num2 < count)
                    {
                        if (this.nodes[num2].Destroyed)
                        {
                            requiresRepath = true;
                            break;
                        }
                        num2++;
                    }
                }
                else
                {
                    this.checkForDestroyedNodesCounter++;
                }
            }
            else
            {
                bool flag = false;
                int num4 = this.currentNode + 1;
                int num5 = Math.Min(this.currentNode + 3, this.nodes.Count);
                while ((num4 < num5) && !flag)
                {
                    if (this.nodes[num4].Destroyed)
                    {
                        requiresRepath = true;
                        lastCorner = false;
                        buffer.Add(position);
                        return position;
                    }
                    if (this.nodes[num4].ContainsPoint(p))
                    {
                        this.currentNode = num4;
                        flag = true;
                    }
                    num4++;
                }
                int num6 = this.currentNode - 1;
                int num7 = Math.Max(this.currentNode - 3, 0);
                while ((num6 > num7) && !flag)
                {
                    if (this.nodes[num6].Destroyed)
                    {
                        requiresRepath = true;
                        lastCorner = false;
                        buffer.Add(position);
                        return position;
                    }
                    if (this.nodes[num6].ContainsPoint(p))
                    {
                        this.currentNode = num6;
                        flag = true;
                    }
                    num6--;
                }
                if (!flag)
                {
                    <Update>c__AnonStorey23F storeyf = new <Update>c__AnonStorey23F();
                    storeyf.<>f__this = this;
                    int num8 = 0;
                    storeyf.closestIsNeighbourOf = 0;
                    storeyf.closestDist = float.PositiveInfinity;
                    storeyf.closestIsInPath = false;
                    storeyf.closestNode = null;
                    storeyf.containingIndex = this.nodes.Count - 1;
                    this.checkForDestroyedNodesCounter = 0;
                    int num9 = 0;
                    int num10 = this.nodes.Count;
                    while (num9 < num10)
                    {
                        if (this.nodes[num9].Destroyed)
                        {
                            requiresRepath = true;
                            lastCorner = false;
                            buffer.Add(position);
                            return position;
                        }
                        Vector3 vector2 = this.nodes[num9].ClosestPointOnNode(position) - position;
                        float sqrMagnitude = vector2.sqrMagnitude;
                        if (sqrMagnitude < storeyf.closestDist)
                        {
                            storeyf.closestDist = sqrMagnitude;
                            num8 = num9;
                            storeyf.closestNode = this.nodes[num9];
                            storeyf.closestIsInPath = true;
                        }
                        num9++;
                    }
                    storeyf.posCopy = position;
                    GraphNodeDelegate del = new GraphNodeDelegate(storeyf.<>m__1);
                    while (storeyf.containingIndex >= 0)
                    {
                        this.nodes[storeyf.containingIndex].GetConnections(del);
                        storeyf.containingIndex--;
                    }
                    if (storeyf.closestIsInPath)
                    {
                        this.currentNode = num8;
                        position = this.nodes[num8].ClosestPointOnNodeXZ(position);
                    }
                    else
                    {
                        position = storeyf.closestNode.ClosestPointOnNodeXZ(position);
                        this.exactStart = position;
                        this.UpdateFunnelCorridor(storeyf.closestIsNeighbourOf, storeyf.closestNode);
                        this.currentNode = 0;
                    }
                }
            }
            this.currentPosition = position;
            if (!this.FindNextCorners(position, this.currentNode, buffer, numCorners, out lastCorner))
            {
                Debug.LogError("Oh oh");
                buffer.Add(position);
                return position;
            }
            return position;
        }

        public void UpdateFunnelCorridor(int splitIndex, TriangleMeshNode prefix)
        {
            if (splitIndex > 0)
            {
                this.nodes.RemoveRange(0, splitIndex - 1);
                this.nodes[0] = prefix;
            }
            else
            {
                this.nodes.Insert(0, prefix);
            }
            this.left.Clear();
            this.right.Clear();
            this.left.Add(this.exactStart);
            this.right.Add(this.exactStart);
            for (int i = 0; i < (this.nodes.Count - 1); i++)
            {
                this.nodes[i].GetPortal(this.nodes[i + 1], this.left, this.right, false);
            }
            this.left.Add(this.exactEnd);
            this.right.Add(this.exactEnd);
        }

        [CompilerGenerated]
        private sealed class <Update>c__AnonStorey23F
        {
            internal RichFunnel <>f__this;
            internal float closestDist;
            internal bool closestIsInPath;
            internal int closestIsNeighbourOf;
            internal TriangleMeshNode closestNode;
            internal int containingIndex;
            internal Vector3 posCopy;

            internal void <>m__1(GraphNode node)
            {
                if (((this.containingIndex <= 0) || (node != this.<>f__this.nodes[this.containingIndex - 1])) && ((this.containingIndex >= (this.<>f__this.nodes.Count - 1)) || (node != this.<>f__this.nodes[this.containingIndex + 1])))
                {
                    TriangleMeshNode node2 = node as TriangleMeshNode;
                    if (node2 != null)
                    {
                        Vector3 vector2 = node2.ClosestPointOnNode(this.posCopy) - this.posCopy;
                        float sqrMagnitude = vector2.sqrMagnitude;
                        if (sqrMagnitude < this.closestDist)
                        {
                            this.closestDist = sqrMagnitude;
                            this.closestIsNeighbourOf = this.containingIndex;
                            this.closestNode = node2;
                            this.closestIsInPath = false;
                        }
                    }
                }
            }
        }

        public enum FunnelSimplification
        {
            None,
            Iterative,
            RecursiveBinary,
            RecursiveTrinary
        }
    }
}

