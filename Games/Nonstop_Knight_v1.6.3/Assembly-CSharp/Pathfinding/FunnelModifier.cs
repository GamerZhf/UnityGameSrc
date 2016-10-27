namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable, AddComponentMenu("Pathfinding/Modifiers/Funnel"), HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_funnel_modifier.php")]
    public class FunnelModifier : MonoModifier
    {
        public override void Apply(Path p)
        {
            List<GraphNode> path = p.path;
            List<Vector3> vectorPath = p.vectorPath;
            if (((path != null) && (path.Count != 0)) && ((vectorPath != null) && (vectorPath.Count == path.Count)))
            {
                List<Vector3> funnelPath = ListPool<Vector3>.Claim();
                List<Vector3> left = ListPool<Vector3>.Claim(path.Count + 1);
                List<Vector3> right = ListPool<Vector3>.Claim(path.Count + 1);
                left.Add(vectorPath[0]);
                right.Add(vectorPath[0]);
                for (int i = 0; i < (path.Count - 1); i++)
                {
                    if (!path[i].GetPortal(path[i + 1], left, right, false))
                    {
                        left.Add((Vector3) path[i].position);
                        right.Add((Vector3) path[i].position);
                        left.Add((Vector3) path[i + 1].position);
                        right.Add((Vector3) path[i + 1].position);
                    }
                }
                left.Add(vectorPath[vectorPath.Count - 1]);
                right.Add(vectorPath[vectorPath.Count - 1]);
                if (!RunFunnel(left, right, funnelPath))
                {
                    funnelPath.Add(vectorPath[0]);
                    funnelPath.Add(vectorPath[vectorPath.Count - 1]);
                }
                ListPool<Vector3>.Release(p.vectorPath);
                p.vectorPath = funnelPath;
                ListPool<Vector3>.Release(left);
                ListPool<Vector3>.Release(right);
            }
        }

        public static bool RunFunnel(List<Vector3> left, List<Vector3> right, List<Vector3> funnelPath)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            if (funnelPath == null)
            {
                throw new ArgumentNullException("funnelPath");
            }
            if (left.Count != right.Count)
            {
                throw new ArgumentException("left and right lists must have equal length");
            }
            if (left.Count <= 3)
            {
                return false;
            }
            while ((left[1] == left[2]) && (right[1] == right[2]))
            {
                left.RemoveAt(1);
                right.RemoveAt(1);
                if (left.Count <= 3)
                {
                    return false;
                }
            }
            Vector3 p = left[2];
            if (p == left[1])
            {
                p = right[2];
            }
            while (VectorMath.IsColinearXZ(left[0], left[1], right[1]) || (VectorMath.RightOrColinearXZ(left[1], right[1], p) == VectorMath.RightOrColinearXZ(left[1], right[1], left[0])))
            {
                left.RemoveAt(1);
                right.RemoveAt(1);
                if (left.Count <= 3)
                {
                    return false;
                }
                p = left[2];
                if (p == left[1])
                {
                    p = right[2];
                }
            }
            if (!VectorMath.IsClockwiseXZ(left[0], left[1], right[1]) && !VectorMath.IsColinearXZ(left[0], left[1], right[1]))
            {
                List<Vector3> list = left;
                left = right;
                right = list;
            }
            funnelPath.Add(left[0]);
            Vector3 a = left[0];
            Vector3 b = left[1];
            Vector3 vector4 = right[1];
            int num = 0;
            int num2 = 1;
            int num3 = 1;
            for (int i = 2; i < left.Count; i++)
            {
                if (funnelPath.Count > 0x7d0)
                {
                    Debug.LogWarning("Avoiding infinite loop. Remove this check if you have this long paths.");
                    break;
                }
                Vector3 c = left[i];
                Vector3 vector6 = right[i];
                if (VectorMath.SignedTriangleAreaTimes2XZ(a, vector4, vector6) >= 0f)
                {
                    if ((a == vector4) || (VectorMath.SignedTriangleAreaTimes2XZ(a, b, vector6) <= 0f))
                    {
                        vector4 = vector6;
                        num2 = i;
                    }
                    else
                    {
                        funnelPath.Add(b);
                        a = b;
                        num = num3;
                        b = a;
                        vector4 = a;
                        num3 = num;
                        num2 = num;
                        i = num;
                        continue;
                    }
                }
                if (VectorMath.SignedTriangleAreaTimes2XZ(a, b, c) <= 0f)
                {
                    if ((a == b) || (VectorMath.SignedTriangleAreaTimes2XZ(a, vector4, c) >= 0f))
                    {
                        b = c;
                        num3 = i;
                    }
                    else
                    {
                        funnelPath.Add(vector4);
                        a = vector4;
                        num = num2;
                        b = a;
                        vector4 = a;
                        num3 = num;
                        num2 = num;
                        i = num;
                    }
                }
            }
            funnelPath.Add(left[left.Count - 1]);
            return true;
        }

        public override int Order
        {
            get
            {
                return 10;
            }
        }
    }
}

