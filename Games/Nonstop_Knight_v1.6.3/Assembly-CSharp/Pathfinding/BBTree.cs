namespace Pathfinding
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class BBTree
    {
        private BBTreeBox[] arr = new BBTreeBox[6];
        private int count;

        public void Clear()
        {
            this.count = 0;
        }

        [Conditional("ASTARDEBUG")]
        private static void DrawDebugNode(MeshNode node, float yoffset, Color color)
        {
            UnityEngine.Debug.DrawLine(((Vector3) node.GetVertex(1)) + ((Vector3) (Vector3.up * yoffset)), ((Vector3) node.GetVertex(2)) + ((Vector3) (Vector3.up * yoffset)), color);
            UnityEngine.Debug.DrawLine(((Vector3) node.GetVertex(0)) + ((Vector3) (Vector3.up * yoffset)), ((Vector3) node.GetVertex(1)) + ((Vector3) (Vector3.up * yoffset)), color);
            UnityEngine.Debug.DrawLine(((Vector3) node.GetVertex(2)) + ((Vector3) (Vector3.up * yoffset)), ((Vector3) node.GetVertex(0)) + ((Vector3) (Vector3.up * yoffset)), color);
        }

        [Conditional("ASTARDEBUG")]
        private static void DrawDebugRect(IntRect rect)
        {
            UnityEngine.Debug.DrawLine(new Vector3((float) rect.xmin, 0f, (float) rect.ymin), new Vector3((float) rect.xmax, 0f, (float) rect.ymin), Color.white);
            UnityEngine.Debug.DrawLine(new Vector3((float) rect.xmin, 0f, (float) rect.ymax), new Vector3((float) rect.xmax, 0f, (float) rect.ymax), Color.white);
            UnityEngine.Debug.DrawLine(new Vector3((float) rect.xmin, 0f, (float) rect.ymin), new Vector3((float) rect.xmin, 0f, (float) rect.ymax), Color.white);
            UnityEngine.Debug.DrawLine(new Vector3((float) rect.xmax, 0f, (float) rect.ymin), new Vector3((float) rect.xmax, 0f, (float) rect.ymax), Color.white);
        }

        private void EnsureCapacity(int c)
        {
            if (this.arr.Length < c)
            {
                BBTreeBox[] boxArray = new BBTreeBox[Math.Max(c, (int) (this.arr.Length * 1.5f))];
                for (int i = 0; i < this.count; i++)
                {
                    boxArray[i] = this.arr[i];
                }
                this.arr = boxArray;
            }
        }

        private static IntRect ExpandToContain(IntRect r, IntRect r2)
        {
            return IntRect.Union(r, r2);
        }

        private static int ExpansionRequired(IntRect r, IntRect r2)
        {
            int num = Math.Min(r.xmin, r2.xmin);
            int num2 = Math.Max(r.xmax, r2.xmax);
            int num3 = Math.Min(r.ymin, r2.ymin);
            int num4 = Math.Max(r.ymax, r2.ymax);
            return (((num2 - num) * (num4 - num3)) - RectArea(r));
        }

        private int GetBox(IntRect rect)
        {
            if (this.count >= this.arr.Length)
            {
                this.EnsureCapacity(this.count + 1);
            }
            this.arr[this.count] = new BBTreeBox(rect);
            this.count++;
            return (this.count - 1);
        }

        private int GetBox(MeshNode node)
        {
            if (this.count >= this.arr.Length)
            {
                this.EnsureCapacity(this.count + 1);
            }
            this.arr[this.count] = new BBTreeBox(node);
            this.count++;
            return (this.count - 1);
        }

        public void Insert(MeshNode node)
        {
            BBTreeBox box2;
            int index = this.GetBox(node);
            if (index == 0)
            {
                return;
            }
            BBTreeBox box = this.arr[index];
            int left = 0;
        Label_0023:
            box2 = this.arr[left];
            box2.rect = ExpandToContain(box2.rect, box.rect);
            if (box2.node != null)
            {
                box2.left = index;
                int num3 = this.GetBox(box2.node);
                box2.right = num3;
                box2.node = null;
                this.arr[left] = box2;
            }
            else
            {
                this.arr[left] = box2;
                int num4 = ExpansionRequired(this.arr[box2.left].rect, box.rect);
                int num5 = ExpansionRequired(this.arr[box2.right].rect, box.rect);
                if (num4 < num5)
                {
                    left = box2.left;
                }
                else if (num5 < num4)
                {
                    left = box2.right;
                }
                else
                {
                    left = (RectArea(this.arr[box2.left].rect) >= RectArea(this.arr[box2.right].rect)) ? box2.right : box2.left;
                }
                goto Label_0023;
            }
        }

        private static IntRect NodeBounds(MeshNode[] nodes, int from, int to)
        {
            if ((to - from) <= 0)
            {
                throw new ArgumentException();
            }
            Int3 vertex = nodes[from].GetVertex(0);
            Int2 num2 = new Int2(vertex.x, vertex.z);
            Int2 num3 = num2;
            for (int i = from; i < to; i++)
            {
                MeshNode node = nodes[i];
                int vertexCount = node.GetVertexCount();
                for (int j = 0; j < vertexCount; j++)
                {
                    Int3 num7 = node.GetVertex(j);
                    num2.x = Math.Min(num2.x, num7.x);
                    num2.y = Math.Min(num2.y, num7.z);
                    num3.x = Math.Max(num3.x, num7.x);
                    num3.y = Math.Max(num3.y, num7.z);
                }
            }
            return new IntRect(num2.x, num2.y, num3.x, num3.y);
        }

        private static bool NodeIntersectsCircle(MeshNode node, Vector3 p, float radius)
        {
            if (float.IsPositiveInfinity(radius))
            {
                return true;
            }
            Vector3 vector = p - node.ClosestPointOnNode(p);
            return (vector.sqrMagnitude < (radius * radius));
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
            if (this.count != 0)
            {
                this.OnDrawGizmos(0, 0);
            }
        }

        private void OnDrawGizmos(int boxi, int depth)
        {
            BBTreeBox box = this.arr[boxi];
            Vector3 vector = (Vector3) new Int3(box.rect.xmin, 0, box.rect.ymin);
            Vector3 vector2 = (Vector3) new Int3(box.rect.xmax, 0, box.rect.ymax);
            Vector3 center = (Vector3) ((vector + vector2) * 0.5f);
            Vector3 size = (Vector3) ((vector2 - center) * 2f);
            size = new Vector3(size.x, 1f, size.z);
            center.y += depth * 2;
            Gizmos.color = AstarMath.IntToColor(depth, 1f);
            Gizmos.DrawCube(center, size);
            if (box.node == null)
            {
                this.OnDrawGizmos(box.left, depth + 1);
                this.OnDrawGizmos(box.right, depth + 1);
            }
        }

        public NNInfoInternal Query(Vector3 p, NNConstraint constraint)
        {
            if (this.count == 0)
            {
                return new NNInfoInternal(null);
            }
            NNInfoInternal nnInfo = new NNInfoInternal();
            this.SearchBox(0, p, constraint, ref nnInfo);
            nnInfo.UpdateInfo();
            return nnInfo;
        }

        public NNInfoInternal QueryCircle(Vector3 p, float radius, NNConstraint constraint)
        {
            if (this.count == 0)
            {
                return new NNInfoInternal(null);
            }
            NNInfoInternal nnInfo = new NNInfoInternal(null);
            this.SearchBoxCircle(0, p, radius, constraint, ref nnInfo);
            nnInfo.UpdateInfo();
            return nnInfo;
        }

        public NNInfoInternal QueryClosest(Vector3 p, NNConstraint constraint, out float distance)
        {
            distance = float.PositiveInfinity;
            return this.QueryClosest(p, constraint, ref distance, new NNInfoInternal(null));
        }

        public NNInfoInternal QueryClosest(Vector3 p, NNConstraint constraint, ref float distance, NNInfoInternal previous)
        {
            if (this.count != 0)
            {
                this.SearchBoxClosest(0, p, ref distance, constraint, ref previous);
            }
            return previous;
        }

        public NNInfoInternal QueryClosestXZ(Vector3 p, NNConstraint constraint, ref float distance, NNInfoInternal previous)
        {
            if (this.count != 0)
            {
                this.SearchBoxClosestXZ(0, p, ref distance, constraint, ref previous);
            }
            return previous;
        }

        public MeshNode QueryInside(Vector3 p, NNConstraint constraint)
        {
            return ((this.count == 0) ? null : this.SearchBoxInside(0, p, constraint));
        }

        public void RebuildFrom(MeshNode[] nodes)
        {
            this.Clear();
            if (nodes.Length != 0)
            {
                if (nodes.Length == 1)
                {
                    this.GetBox(nodes[0]);
                }
                else
                {
                    this.EnsureCapacity(Mathf.CeilToInt(nodes.Length * 2.1f));
                    MeshNode[] nodeArray = new MeshNode[nodes.Length];
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        nodeArray[i] = nodes[i];
                    }
                    this.RebuildFromInternal(nodeArray, 0, nodes.Length, false);
                }
            }
        }

        private int RebuildFromInternal(MeshNode[] nodes, int from, int to, bool odd)
        {
            int num2;
            if ((to - from) <= 0)
            {
                throw new ArgumentException();
            }
            if ((to - from) == 1)
            {
                return this.GetBox(nodes[from]);
            }
            IntRect rect = NodeBounds(nodes, from, to);
            int box = this.GetBox(rect);
            if ((to - from) == 2)
            {
                this.arr[box].left = this.GetBox(nodes[from]);
                this.arr[box].right = this.GetBox(nodes[from + 1]);
                return box;
            }
            if (odd)
            {
                int divider = (rect.xmin + rect.xmax) / 2;
                num2 = SplitByX(nodes, from, to, divider);
            }
            else
            {
                int num4 = (rect.ymin + rect.ymax) / 2;
                num2 = SplitByZ(nodes, from, to, num4);
            }
            if ((num2 == from) || (num2 == to))
            {
                if (!odd)
                {
                    int num5 = (rect.xmin + rect.xmax) / 2;
                    num2 = SplitByX(nodes, from, to, num5);
                }
                else
                {
                    int num6 = (rect.ymin + rect.ymax) / 2;
                    num2 = SplitByZ(nodes, from, to, num6);
                }
                if ((num2 == from) || (num2 == to))
                {
                    num2 = (from + to) / 2;
                }
            }
            this.arr[box].left = this.RebuildFromInternal(nodes, from, num2, !odd);
            this.arr[box].right = this.RebuildFromInternal(nodes, num2, to, !odd);
            return box;
        }

        private static int RectArea(IntRect r)
        {
            return (r.Width * r.Height);
        }

        private static bool RectIntersectsCircle(IntRect r, Vector3 p, float radius)
        {
            if (float.IsPositiveInfinity(radius))
            {
                return true;
            }
            Vector3 vector = p;
            p.x = Math.Max(p.x, r.xmin * 0.001f);
            p.x = Math.Min(p.x, r.xmax * 0.001f);
            p.z = Math.Max(p.z, r.ymin * 0.001f);
            p.z = Math.Min(p.z, r.ymax * 0.001f);
            return ((((p.x - vector.x) * (p.x - vector.x)) + ((p.z - vector.z) * (p.z - vector.z))) < (radius * radius));
        }

        private void SearchBox(int boxi, Vector3 p, NNConstraint constraint, ref NNInfoInternal nnInfo)
        {
            BBTreeBox box = this.arr[boxi];
            if (box.node == null)
            {
                if (this.arr[box.left].Contains(p))
                {
                    this.SearchBox(box.left, p, constraint, ref nnInfo);
                }
                if (this.arr[box.right].Contains(p))
                {
                    this.SearchBox(box.right, p, constraint, ref nnInfo);
                }
            }
            else if (box.node.ContainsPoint((Int3) p))
            {
                if (nnInfo.node == null)
                {
                    nnInfo.node = box.node;
                }
                else
                {
                    Vector3 position = (Vector3) box.node.position;
                    Vector3 vector2 = (Vector3) nnInfo.node.position;
                    if (Mathf.Abs((float) (position.y - p.y)) < Mathf.Abs((float) (vector2.y - p.y)))
                    {
                        nnInfo.node = box.node;
                    }
                }
                if (constraint.Suitable(box.node))
                {
                    if (nnInfo.constrainedNode != null)
                    {
                        float introduced3 = Mathf.Abs((float) (box.node.position.y - p.y));
                        if (introduced3 >= Mathf.Abs((float) (nnInfo.constrainedNode.position.y - p.y)))
                        {
                            return;
                        }
                    }
                    nnInfo.constrainedNode = box.node;
                }
            }
        }

        private void SearchBoxCircle(int boxi, Vector3 p, float radius, NNConstraint constraint, ref NNInfoInternal nnInfo)
        {
            BBTreeBox box = this.arr[boxi];
            if (box.node == null)
            {
                if (RectIntersectsCircle(this.arr[box.left].rect, p, radius))
                {
                    this.SearchBoxCircle(box.left, p, radius, constraint, ref nnInfo);
                }
                if (RectIntersectsCircle(this.arr[box.right].rect, p, radius))
                {
                    this.SearchBoxCircle(box.right, p, radius, constraint, ref nnInfo);
                }
            }
            else if (NodeIntersectsCircle(box.node, p, radius))
            {
                Vector3 vector = box.node.ClosestPointOnNode(p);
                Vector3 vector2 = vector - p;
                float sqrMagnitude = vector2.sqrMagnitude;
                if (nnInfo.node == null)
                {
                    nnInfo.node = box.node;
                    nnInfo.clampedPosition = vector;
                }
                else
                {
                    Vector3 vector3 = nnInfo.clampedPosition - p;
                    if (sqrMagnitude < vector3.sqrMagnitude)
                    {
                        nnInfo.node = box.node;
                        nnInfo.clampedPosition = vector;
                    }
                }
                if ((constraint == null) || constraint.Suitable(box.node))
                {
                    if (nnInfo.constrainedNode != null)
                    {
                        Vector3 vector4 = nnInfo.constClampedPosition - p;
                        if (sqrMagnitude >= vector4.sqrMagnitude)
                        {
                            return;
                        }
                    }
                    nnInfo.constrainedNode = box.node;
                    nnInfo.constClampedPosition = vector;
                }
            }
        }

        private void SearchBoxClosest(int boxi, Vector3 p, ref float closestDist, NNConstraint constraint, ref NNInfoInternal nnInfo)
        {
            BBTreeBox box = this.arr[boxi];
            if (box.node != null)
            {
                if (NodeIntersectsCircle(box.node, p, closestDist))
                {
                    Vector3 vector = box.node.ClosestPointOnNode(p);
                    if ((constraint == null) || constraint.Suitable(box.node))
                    {
                        Vector3 vector2 = vector - p;
                        float sqrMagnitude = vector2.sqrMagnitude;
                        if ((nnInfo.constrainedNode == null) || (sqrMagnitude < (closestDist * closestDist)))
                        {
                            nnInfo.constrainedNode = box.node;
                            nnInfo.constClampedPosition = vector;
                            closestDist = (float) Math.Sqrt((double) sqrMagnitude);
                        }
                    }
                }
            }
            else
            {
                if (RectIntersectsCircle(this.arr[box.left].rect, p, closestDist))
                {
                    this.SearchBoxClosest(box.left, p, ref closestDist, constraint, ref nnInfo);
                }
                if (RectIntersectsCircle(this.arr[box.right].rect, p, closestDist))
                {
                    this.SearchBoxClosest(box.right, p, ref closestDist, constraint, ref nnInfo);
                }
            }
        }

        private void SearchBoxClosestXZ(int boxi, Vector3 p, ref float closestDist, NNConstraint constraint, ref NNInfoInternal nnInfo)
        {
            BBTreeBox box = this.arr[boxi];
            if (box.node != null)
            {
                Vector3 vector = box.node.ClosestPointOnNodeXZ(p);
                if ((constraint == null) || constraint.Suitable(box.node))
                {
                    float num = ((vector.x - p.x) * (vector.x - p.x)) + ((vector.z - p.z) * (vector.z - p.z));
                    if ((nnInfo.constrainedNode == null) || (num < (closestDist * closestDist)))
                    {
                        nnInfo.constrainedNode = box.node;
                        nnInfo.constClampedPosition = vector;
                        closestDist = (float) Math.Sqrt((double) num);
                    }
                }
            }
            else
            {
                if (RectIntersectsCircle(this.arr[box.left].rect, p, closestDist))
                {
                    this.SearchBoxClosestXZ(box.left, p, ref closestDist, constraint, ref nnInfo);
                }
                if (RectIntersectsCircle(this.arr[box.right].rect, p, closestDist))
                {
                    this.SearchBoxClosestXZ(box.right, p, ref closestDist, constraint, ref nnInfo);
                }
            }
        }

        private MeshNode SearchBoxInside(int boxi, Vector3 p, NNConstraint constraint)
        {
            BBTreeBox box = this.arr[boxi];
            if (box.node != null)
            {
                if (box.node.ContainsPoint((Int3) p) && ((constraint == null) || constraint.Suitable(box.node)))
                {
                    return box.node;
                }
            }
            else
            {
                MeshNode node;
                if (this.arr[box.left].Contains(p))
                {
                    node = this.SearchBoxInside(box.left, p, constraint);
                    if (node != null)
                    {
                        return node;
                    }
                }
                if (this.arr[box.right].Contains(p))
                {
                    node = this.SearchBoxInside(box.right, p, constraint);
                    if (node != null)
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        private static int SplitByX(MeshNode[] nodes, int from, int to, int divider)
        {
            int index = to;
            for (int i = from; i < index; i++)
            {
                if (nodes[i].position.x > divider)
                {
                    index--;
                    MeshNode node = nodes[index];
                    nodes[index] = nodes[i];
                    nodes[i] = node;
                    i--;
                }
            }
            return index;
        }

        private static int SplitByZ(MeshNode[] nodes, int from, int to, int divider)
        {
            int index = to;
            for (int i = from; i < index; i++)
            {
                if (nodes[i].position.z > divider)
                {
                    index--;
                    MeshNode node = nodes[index];
                    nodes[index] = nodes[i];
                    nodes[i] = node;
                    i--;
                }
            }
            return index;
        }

        public Rect Size
        {
            get
            {
                if (this.count == 0)
                {
                    return new Rect(0f, 0f, 0f, 0f);
                }
                IntRect rect = this.arr[0].rect;
                return Rect.MinMaxRect(rect.xmin * 0.001f, rect.ymin * 0.001f, rect.xmax * 0.001f, rect.ymax * 0.001f);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BBTreeBox
        {
            public IntRect rect;
            public MeshNode node;
            public int left;
            public int right;
            public BBTreeBox(IntRect rect)
            {
                this.node = null;
                this.rect = rect;
                this.left = this.right = -1;
            }

            public BBTreeBox(MeshNode node)
            {
                this.node = node;
                Int3 vertex = node.GetVertex(0);
                Int2 num2 = new Int2(vertex.x, vertex.z);
                Int2 num3 = num2;
                for (int i = 1; i < node.GetVertexCount(); i++)
                {
                    Int3 num5 = node.GetVertex(i);
                    num2.x = Math.Min(num2.x, num5.x);
                    num2.y = Math.Min(num2.y, num5.z);
                    num3.x = Math.Max(num3.x, num5.x);
                    num3.y = Math.Max(num3.y, num5.z);
                }
                this.rect = new IntRect(num2.x, num2.y, num3.x, num3.y);
                this.left = this.right = -1;
            }

            public bool IsLeaf
            {
                get
                {
                    return (this.node != null);
                }
            }
            public bool Contains(Vector3 p)
            {
                Int3 num = (Int3) p;
                return this.rect.Contains(num.x, num.z);
            }
        }
    }
}

