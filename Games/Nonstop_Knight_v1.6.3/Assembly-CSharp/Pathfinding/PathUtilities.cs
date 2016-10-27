namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public static class PathUtilities
    {
        private static Dictionary<GraphNode, int> BFSMap;
        private static Queue<GraphNode> BFSQueue;

        public static List<GraphNode> BFS(GraphNode seed, int depth, [Optional, DefaultParameterValue(-1)] int tagMask)
        {
            GraphNodeDelegate delegate2;
            <BFS>c__AnonStorey260 storey = new <BFS>c__AnonStorey260();
            storey.tagMask = tagMask;
            if (BFSQueue == null)
            {
            }
            BFSQueue = new Queue<GraphNode>();
            storey.que = BFSQueue;
            if (BFSMap == null)
            {
            }
            BFSMap = new Dictionary<GraphNode, int>();
            storey.map = BFSMap;
            storey.que.Clear();
            storey.map.Clear();
            storey.result = ListPool<GraphNode>.Claim();
            storey.currentDist = -1;
            if (storey.tagMask == -1)
            {
                delegate2 = new GraphNodeDelegate(storey.<>m__3A);
            }
            else
            {
                delegate2 = new GraphNodeDelegate(storey.<>m__3B);
            }
            delegate2(seed);
            while (storey.que.Count > 0)
            {
                GraphNode node = storey.que.Dequeue();
                storey.currentDist = storey.map[node];
                if (storey.currentDist >= depth)
                {
                    break;
                }
                node.GetConnections(delegate2);
            }
            storey.que.Clear();
            storey.map.Clear();
            return storey.result;
        }

        public static void GetPointsAroundPoint(Vector3 p, IRaycastableGraph g, List<Vector3> previousPoints, float radius, float clearanceRadius)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            NavGraph graph = g as NavGraph;
            if (graph == null)
            {
                throw new ArgumentException("g is not a NavGraph");
            }
            NNInfoInternal nearestForce = graph.GetNearestForce(p, NNConstraint.Default);
            p = nearestForce.clampedPosition;
            if (nearestForce.node != null)
            {
                radius = Mathf.Max(radius, (1.4142f * clearanceRadius) * Mathf.Sqrt((float) previousPoints.Count));
                clearanceRadius *= clearanceRadius;
                for (int i = 0; i < previousPoints.Count; i++)
                {
                    Vector3 vector = previousPoints[i];
                    float magnitude = vector.magnitude;
                    if (magnitude > 0f)
                    {
                        vector = (Vector3) (vector / magnitude);
                    }
                    float from = radius;
                    vector = (Vector3) (vector * from);
                    bool flag = false;
                    int num4 = 0;
                    do
                    {
                        GraphHitInfo info;
                        Vector3 end = p + vector;
                        if (g.Linecast(p, end, nearestForce.node, out info))
                        {
                            end = info.point;
                        }
                        for (float j = 0.1f; j <= 1f; j += 0.05f)
                        {
                            Vector3 vector3 = ((Vector3) ((end - p) * j)) + p;
                            flag = true;
                            for (int k = 0; k < i; k++)
                            {
                                Vector3 vector4 = previousPoints[k] - vector3;
                                if (vector4.sqrMagnitude < clearanceRadius)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                previousPoints[i] = vector3;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            if (num4 > 8)
                            {
                                flag = true;
                            }
                            else
                            {
                                clearanceRadius *= 0.9f;
                                vector = (Vector3) (UnityEngine.Random.onUnitSphere * Mathf.Lerp(from, radius, (float) (num4 / 5)));
                                vector.y = 0f;
                                num4++;
                            }
                        }
                    }
                    while (!flag);
                }
            }
        }

        public static void GetPointsAroundPointWorld(Vector3 p, IRaycastableGraph g, List<Vector3> previousPoints, float radius, float clearanceRadius)
        {
            if (previousPoints.Count != 0)
            {
                Vector3 zero = Vector3.zero;
                for (int i = 0; i < previousPoints.Count; i++)
                {
                    zero += previousPoints[i];
                }
                zero = (Vector3) (zero / ((float) previousPoints.Count));
                for (int j = 0; j < previousPoints.Count; j++)
                {
                    List<Vector3> list;
                    int num3;
                    Vector3 vector2 = list[num3];
                    (list = previousPoints)[num3 = j] = vector2 - zero;
                }
                GetPointsAroundPoint(p, g, previousPoints, radius, clearanceRadius);
            }
        }

        public static List<Vector3> GetPointsOnNodes(List<GraphNode> nodes, int count, [Optional, DefaultParameterValue(0)] float clearanceRadius)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }
            if (nodes.Count == 0)
            {
                throw new ArgumentException("no nodes passed");
            }
            List<Vector3> list = ListPool<Vector3>.Claim(count);
            clearanceRadius *= clearanceRadius;
            if (((clearanceRadius > 0f) || (nodes[0] is TriangleMeshNode)) || (nodes[0] is GridNode))
            {
                List<float> list2 = ListPool<float>.Claim(nodes.Count);
                float item = 0f;
                for (int j = 0; j < nodes.Count; j++)
                {
                    float num3 = nodes[j].SurfaceArea() + 0.001f;
                    item += num3;
                    list2.Add(item);
                }
                for (int k = 0; k < count; k++)
                {
                    int num5 = 0;
                    int num6 = 10;
                    bool flag = false;
                    while (!flag)
                    {
                        flag = true;
                        if (num5 >= num6)
                        {
                            clearanceRadius *= 0.8099999f;
                            num6 += 10;
                            if (num6 > 100)
                            {
                                clearanceRadius = 0f;
                            }
                        }
                        float num7 = UnityEngine.Random.value * item;
                        int num8 = list2.BinarySearch(num7);
                        if (num8 < 0)
                        {
                            num8 = ~num8;
                        }
                        if (num8 >= nodes.Count)
                        {
                            flag = false;
                            continue;
                        }
                        Vector3 vector = nodes[num8].RandomPointOnSurface();
                        if (clearanceRadius > 0f)
                        {
                            for (int m = 0; m < list.Count; m++)
                            {
                                Vector3 vector2 = list[m] - vector;
                                if (vector2.sqrMagnitude < clearanceRadius)
                                {
                                    flag = false;
                                    break;
                                }
                            }
                        }
                        if (flag)
                        {
                            list.Add(vector);
                            break;
                        }
                        num5++;
                    }
                }
                ListPool<float>.Release(list2);
                return list;
            }
            for (int i = 0; i < count; i++)
            {
                list.Add(nodes[UnityEngine.Random.Range(0, nodes.Count)].RandomPointOnSurface());
            }
            return list;
        }

        public static List<GraphNode> GetReachableNodes(GraphNode seed, [Optional, DefaultParameterValue(-1)] int tagMask)
        {
            GraphNodeDelegate delegate2;
            <GetReachableNodes>c__AnonStorey25F storeyf = new <GetReachableNodes>c__AnonStorey25F();
            storeyf.tagMask = tagMask;
            storeyf.stack = StackPool<GraphNode>.Claim();
            storeyf.list = ListPool<GraphNode>.Claim();
            storeyf.map = new HashSet<GraphNode>();
            if (storeyf.tagMask == -1)
            {
                delegate2 = new GraphNodeDelegate(storeyf.<>m__38);
            }
            else
            {
                delegate2 = new GraphNodeDelegate(storeyf.<>m__39);
            }
            delegate2(seed);
            while (storeyf.stack.Count > 0)
            {
                storeyf.stack.Pop().GetConnections(delegate2);
            }
            StackPool<GraphNode>.Release(storeyf.stack);
            return storeyf.list;
        }

        public static List<Vector3> GetSpiralPoints(int count, float clearance)
        {
            List<Vector3> list = ListPool<Vector3>.Claim(count);
            float a = clearance / 6.283185f;
            float t = 0f;
            list.Add(InvoluteOfCircle(a, t));
            for (int i = 0; i < count; i++)
            {
                Vector3 vector = list[list.Count - 1];
                float num4 = (-t / 2f) + Mathf.Sqrt(((t * t) / 4f) + ((2f * clearance) / a));
                float num5 = t + num4;
                float num6 = t + (2f * num4);
                while ((num6 - num5) > 0.01f)
                {
                    float num7 = (num5 + num6) / 2f;
                    Vector3 vector3 = InvoluteOfCircle(a, num7) - vector;
                    if (vector3.sqrMagnitude < (clearance * clearance))
                    {
                        num5 = num7;
                    }
                    else
                    {
                        num6 = num7;
                    }
                }
                list.Add(InvoluteOfCircle(a, num6));
                t = num6;
            }
            return list;
        }

        private static Vector3 InvoluteOfCircle(float a, float t)
        {
            return new Vector3(a * (Mathf.Cos(t) + (t * Mathf.Sin(t))), 0f, a * (Mathf.Sin(t) - (t * Mathf.Cos(t))));
        }

        public static bool IsPathPossible(List<GraphNode> nodes)
        {
            if (nodes.Count != 0)
            {
                uint area = nodes[0].Area;
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (!nodes[i].Walkable || (nodes[i].Area != area))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsPathPossible(GraphNode n1, GraphNode n2)
        {
            return ((n1.Walkable && n2.Walkable) && (n1.Area == n2.Area));
        }

        public static bool IsPathPossible(List<GraphNode> nodes, int tagMask)
        {
            if (nodes.Count == 0)
            {
                return true;
            }
            if (((tagMask >> nodes[0].Tag) & 1) == 0)
            {
                return false;
            }
            if (!IsPathPossible(nodes))
            {
                return false;
            }
            List<GraphNode> reachableNodes = GetReachableNodes(nodes[0], tagMask);
            bool flag = true;
            for (int i = 1; i < nodes.Count; i++)
            {
                if (!reachableNodes.Contains(nodes[i]))
                {
                    flag = false;
                    break;
                }
            }
            ListPool<GraphNode>.Release(reachableNodes);
            return flag;
        }

        [CompilerGenerated]
        private sealed class <BFS>c__AnonStorey260
        {
            internal int currentDist;
            internal Dictionary<GraphNode, int> map;
            internal Queue<GraphNode> que;
            internal List<GraphNode> result;
            internal int tagMask;

            internal void <>m__3A(GraphNode node)
            {
                if (node.Walkable && !this.map.ContainsKey(node))
                {
                    this.map.Add(node, this.currentDist + 1);
                    this.result.Add(node);
                    this.que.Enqueue(node);
                }
            }

            internal void <>m__3B(GraphNode node)
            {
                if ((node.Walkable && (((this.tagMask >> node.Tag) & 1) != 0)) && !this.map.ContainsKey(node))
                {
                    this.map.Add(node, this.currentDist + 1);
                    this.result.Add(node);
                    this.que.Enqueue(node);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetReachableNodes>c__AnonStorey25F
        {
            internal List<GraphNode> list;
            internal HashSet<GraphNode> map;
            internal Stack<GraphNode> stack;
            internal int tagMask;

            internal void <>m__38(GraphNode node)
            {
                if (node.Walkable && this.map.Add(node))
                {
                    this.list.Add(node);
                    this.stack.Push(node);
                }
            }

            internal void <>m__39(GraphNode node)
            {
                if ((node.Walkable && (((this.tagMask >> node.Tag) & 1) != 0)) && this.map.Add(node))
                {
                    this.list.Add(node);
                    this.stack.Push(node);
                }
            }
        }
    }
}

