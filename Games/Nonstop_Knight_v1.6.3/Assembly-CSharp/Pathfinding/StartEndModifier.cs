namespace Pathfinding
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class StartEndModifier : PathModifier
    {
        public bool addPoints;
        public Func<Vector3> adjustStartPoint;
        private List<GraphNode> connectionBuffer;
        private GraphNodeDelegate connectionBufferAddDelegate;
        public Exactness exactEndPoint = Exactness.ClosestOnNode;
        public Exactness exactStartPoint = Exactness.ClosestOnNode;
        public LayerMask mask = -1;
        public bool useGraphRaycasting;
        public bool useRaycasting;

        public override void Apply(Path _p)
        {
            ABPath path = _p as ABPath;
            if ((path != null) && (path.vectorPath.Count != 0))
            {
                bool flag;
                bool flag2;
                if ((path.vectorPath.Count == 1) && !this.addPoints)
                {
                    path.vectorPath.Add(path.vectorPath[0]);
                }
                Vector3 item = this.Snap(path, this.exactStartPoint, true, out flag);
                Vector3 vector2 = this.Snap(path, this.exactEndPoint, false, out flag2);
                if ((flag || this.addPoints) && (this.exactStartPoint != Exactness.SnapToNode))
                {
                    path.vectorPath.Insert(0, item);
                }
                else
                {
                    path.vectorPath[0] = item;
                }
                if ((flag2 || this.addPoints) && (this.exactEndPoint != Exactness.SnapToNode))
                {
                    path.vectorPath.Add(vector2);
                }
                else
                {
                    path.vectorPath[path.vectorPath.Count - 1] = vector2;
                }
            }
        }

        public Vector3 GetClampedPoint(Vector3 from, Vector3 to, GraphNode hint)
        {
            RaycastHit hit;
            Vector3 end = to;
            if (this.useRaycasting && Physics.Linecast(from, to, out hit, (int) this.mask))
            {
                end = hit.point;
            }
            if (this.useGraphRaycasting && (hint != null))
            {
                GraphHitInfo info;
                IRaycastableGraph graph = AstarData.GetGraph(hint) as IRaycastableGraph;
                if ((graph != null) && graph.Linecast(from, end, hint, out info))
                {
                    end = info.point;
                }
            }
            return end;
        }

        private Vector3 Snap(ABPath path, Exactness mode, bool start, out bool forceAddPoint)
        {
            Vector3 originalEndPoint;
            GraphNode node2;
            int num = !start ? (path.path.Count - 1) : 0;
            GraphNode hint = path.path[num];
            Vector3 position = (Vector3) hint.position;
            forceAddPoint = false;
            switch (mode)
            {
                case Exactness.SnapToNode:
                    return position;

                case Exactness.Original:
                case Exactness.Interpolate:
                case Exactness.NodeConnection:
                    if (!start)
                    {
                        originalEndPoint = path.originalEndPoint;
                        break;
                    }
                    originalEndPoint = (this.adjustStartPoint == null) ? path.originalStartPoint : this.adjustStartPoint();
                    break;

                case Exactness.ClosestOnNode:
                    return this.GetClampedPoint(position, !start ? path.endPoint : path.startPoint, hint);

                default:
                    throw new ArgumentException("Invalid mode");
            }
            switch (mode)
            {
                case Exactness.Original:
                    return this.GetClampedPoint(position, originalEndPoint, hint);

                case Exactness.Interpolate:
                {
                    Vector3 point = this.GetClampedPoint(position, originalEndPoint, hint);
                    node2 = path.path[Mathf.Clamp(num + (!start ? -1 : 1), 0, path.path.Count - 1)];
                    return VectorMath.ClosestPointOnSegment(position, (Vector3) node2.position, point);
                }
                case Exactness.NodeConnection:
                {
                    if (this.connectionBuffer == null)
                    {
                    }
                    this.connectionBuffer = new List<GraphNode>();
                    if (this.connectionBufferAddDelegate == null)
                    {
                    }
                    this.connectionBufferAddDelegate = new GraphNodeDelegate(this.connectionBuffer.Add);
                    node2 = path.path[Mathf.Clamp(num + (!start ? -1 : 1), 0, path.path.Count - 1)];
                    hint.GetConnections(this.connectionBufferAddDelegate);
                    Vector3 vector4 = position;
                    float positiveInfinity = float.PositiveInfinity;
                    for (int i = this.connectionBuffer.Count - 1; i >= 0; i--)
                    {
                        GraphNode node3 = this.connectionBuffer[i];
                        Vector3 vector5 = VectorMath.ClosestPointOnSegment(position, (Vector3) node3.position, originalEndPoint);
                        Vector3 vector6 = vector5 - originalEndPoint;
                        float sqrMagnitude = vector6.sqrMagnitude;
                        if (sqrMagnitude < positiveInfinity)
                        {
                            vector4 = vector5;
                            positiveInfinity = sqrMagnitude;
                            forceAddPoint = node3 != node2;
                        }
                    }
                    this.connectionBuffer.Clear();
                    return vector4;
                }
            }
            throw new ArgumentException("Cannot reach this point, but the compiler is not smart enough to realize that.");
        }

        public override int Order
        {
            get
            {
                return 0;
            }
        }

        public enum Exactness
        {
            SnapToNode,
            Original,
            Interpolate,
            ClosestOnNode,
            NodeConnection
        }
    }
}

