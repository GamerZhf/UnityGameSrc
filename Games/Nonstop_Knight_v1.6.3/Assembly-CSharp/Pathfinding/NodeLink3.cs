namespace Pathfinding
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_node_link3.php"), AddComponentMenu("Pathfinding/Link3")]
    public class NodeLink3 : GraphModifier
    {
        private Vector3 clamped1;
        private Vector3 clamped2;
        private MeshNode connectedNode1;
        private MeshNode connectedNode2;
        public float costFactor = 1f;
        public Transform end;
        private NodeLink3Node endNode;
        private static readonly Color GizmosColor = new Color(0.8078431f, 0.5333334f, 0.1882353f, 0.5f);
        private static readonly Color GizmosColorSelected = new Color(0.9215686f, 0.4823529f, 0.1254902f, 1f);
        public bool oneWay;
        private bool postScanCalled;
        protected static Dictionary<GraphNode, NodeLink3> reference = new Dictionary<GraphNode, NodeLink3>();
        private NodeLink3Node startNode;

        public void Apply(bool forceNewCheck)
        {
            NNConstraint none = NNConstraint.None;
            none.distanceXZ = true;
            int graphIndex = (int) this.startNode.GraphIndex;
            none.graphMask = ~(((int) 1) << graphIndex);
            bool flag = true;
            NNInfo nearest = AstarPath.active.GetNearest(this.StartTransform.position, none);
            flag &= (nearest.node == this.connectedNode1) && (nearest.node != null);
            this.connectedNode1 = nearest.node as MeshNode;
            this.clamped1 = nearest.position;
            if (this.connectedNode1 != null)
            {
                Debug.DrawRay((Vector3) this.connectedNode1.position, (Vector3) (Vector3.up * 5f), Color.red);
            }
            NNInfo info2 = AstarPath.active.GetNearest(this.EndTransform.position, none);
            flag &= (info2.node == this.connectedNode2) && (info2.node != null);
            this.connectedNode2 = info2.node as MeshNode;
            this.clamped2 = info2.position;
            if (this.connectedNode2 != null)
            {
                Debug.DrawRay((Vector3) this.connectedNode2.position, (Vector3) (Vector3.up * 5f), Color.cyan);
            }
            if ((this.connectedNode2 != null) && (this.connectedNode1 != null))
            {
                this.startNode.SetPosition((Int3) this.StartTransform.position);
                this.endNode.SetPosition((Int3) this.EndTransform.position);
                if (!flag || forceNewCheck)
                {
                    this.RemoveConnections(this.startNode);
                    this.RemoveConnections(this.endNode);
                    Int3 num12 = (Int3) (this.StartTransform.position - this.EndTransform.position);
                    uint cost = (uint) Mathf.RoundToInt(num12.costMagnitude * this.costFactor);
                    this.startNode.AddConnection(this.endNode, cost);
                    this.endNode.AddConnection(this.startNode, cost);
                    Int3 rhs = this.connectedNode2.position - this.connectedNode1.position;
                    for (int i = 0; i < this.connectedNode1.GetVertexCount(); i++)
                    {
                        Int3 vertex = this.connectedNode1.GetVertex(i);
                        Int3 lineEnd = this.connectedNode1.GetVertex((i + 1) % this.connectedNode1.GetVertexCount());
                        Int3 num13 = lineEnd - vertex;
                        if (Int3.DotLong(num13.Normal2D(), rhs) <= 0L)
                        {
                            for (int j = 0; j < this.connectedNode2.GetVertexCount(); j++)
                            {
                                Int3 point = this.connectedNode2.GetVertex(j);
                                Int3 num9 = this.connectedNode2.GetVertex((j + 1) % this.connectedNode2.GetVertexCount());
                                Int3 num14 = num9 - point;
                                if ((Int3.DotLong(num14.Normal2D(), rhs) >= 0L) && (Int3.Angle(num9 - point, lineEnd - vertex) > 2.9670598109563189))
                                {
                                    float num10 = 0f;
                                    float num11 = 1f;
                                    num11 = Math.Min(num11, VectorMath.ClosestPointOnLineFactor(vertex, lineEnd, point));
                                    num10 = Math.Max(num10, VectorMath.ClosestPointOnLineFactor(vertex, lineEnd, num9));
                                    if (num11 >= num10)
                                    {
                                        Vector3 vector = ((Vector3) (((Vector3) (lineEnd - vertex)) * num10)) + ((Vector3) vertex);
                                        Vector3 vector2 = ((Vector3) (((Vector3) (lineEnd - vertex)) * num11)) + ((Vector3) vertex);
                                        this.startNode.portalA = vector;
                                        this.startNode.portalB = vector2;
                                        this.endNode.portalA = vector2;
                                        this.endNode.portalB = vector;
                                        Int3 num15 = (Int3) (this.clamped1 - this.StartTransform.position);
                                        this.connectedNode1.AddConnection(this.startNode, (uint) Mathf.RoundToInt(num15.costMagnitude * this.costFactor));
                                        Int3 num16 = (Int3) (this.clamped2 - this.EndTransform.position);
                                        this.connectedNode2.AddConnection(this.endNode, (uint) Mathf.RoundToInt(num16.costMagnitude * this.costFactor));
                                        Int3 num17 = (Int3) (this.clamped1 - this.StartTransform.position);
                                        this.startNode.AddConnection(this.connectedNode1, (uint) Mathf.RoundToInt(num17.costMagnitude * this.costFactor));
                                        Int3 num18 = (Int3) (this.clamped2 - this.EndTransform.position);
                                        this.endNode.AddConnection(this.connectedNode2, (uint) Mathf.RoundToInt(num18.costMagnitude * this.costFactor));
                                        return;
                                    }
                                    Debug.LogError(string.Concat(new object[] { "Wait wut!? ", num10, " ", num11, " ", vertex, " ", lineEnd, " ", point, " ", num9, "\nTODO, fix this error" }));
                                }
                            }
                        }
                    }
                }
            }
        }

        [ContextMenu("Recalculate neighbours")]
        private void ContextApplyForce()
        {
            if (Application.isPlaying)
            {
                this.Apply(true);
                if (AstarPath.active != null)
                {
                    AstarPath.active.FloodFill();
                }
            }
        }

        private void DrawCircle(Vector3 o, float r, int detail, Color col)
        {
            Vector3 from = new Vector3(Mathf.Cos(0f) * r, 0f, Mathf.Sin(0f) * r) + o;
            Gizmos.color = col;
            for (int i = 0; i <= detail; i++)
            {
                float f = ((i * 3.141593f) * 2f) / ((float) detail);
                Vector3 to = new Vector3(Mathf.Cos(f) * r, 0f, Mathf.Sin(f) * r) + o;
                Gizmos.DrawLine(from, to);
                from = to;
            }
        }

        private void DrawGizmoBezier(Vector3 p1, Vector3 p2)
        {
            Vector3 rhs = p2 - p1;
            if (rhs != Vector3.zero)
            {
                Vector3 vector2 = Vector3.Cross(Vector3.up, rhs);
                Vector3 vector3 = (Vector3) (Vector3.Cross(rhs, vector2).normalized * (rhs.magnitude * 0.1f));
                Vector3 vector4 = p1 + vector3;
                Vector3 vector5 = p2 + vector3;
                Vector3 from = p1;
                for (int i = 1; i <= 20; i++)
                {
                    float t = ((float) i) / 20f;
                    Vector3 to = AstarSplines.CubicBezier(p1, vector4, vector5, p2, t);
                    Gizmos.DrawLine(from, to);
                    from = to;
                }
            }
        }

        public static NodeLink3 GetNodeLink(GraphNode node)
        {
            NodeLink3 link;
            reference.TryGetValue(node, out link);
            return link;
        }

        public void InternalOnPostScan()
        {
            if (AstarPath.active.astarData.pointGraph == null)
            {
                AstarPath.active.astarData.AddGraph(new PointGraph());
            }
            this.startNode = AstarPath.active.astarData.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3) this.StartTransform.position);
            this.startNode.link = this;
            this.endNode = AstarPath.active.astarData.pointGraph.AddNode<NodeLink3Node>(new NodeLink3Node(AstarPath.active), (Int3) this.EndTransform.position);
            this.endNode.link = this;
            this.connectedNode1 = null;
            this.connectedNode2 = null;
            if ((this.startNode == null) || (this.endNode == null))
            {
                this.startNode = null;
                this.endNode = null;
            }
            else
            {
                this.postScanCalled = true;
                reference[this.startNode] = this;
                reference[this.endNode] = this;
                this.Apply(true);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.postScanCalled = false;
            if (this.startNode != null)
            {
                reference.Remove(this.startNode);
            }
            if (this.endNode != null)
            {
                reference.Remove(this.endNode);
            }
            if ((this.startNode != null) && (this.endNode != null))
            {
                this.startNode.RemoveConnection(this.endNode);
                this.endNode.RemoveConnection(this.startNode);
                if ((this.connectedNode1 != null) && (this.connectedNode2 != null))
                {
                    this.startNode.RemoveConnection(this.connectedNode1);
                    this.connectedNode1.RemoveConnection(this.startNode);
                    this.endNode.RemoveConnection(this.connectedNode2);
                    this.connectedNode2.RemoveConnection(this.endNode);
                }
            }
        }

        public void OnDrawGizmos()
        {
            this.OnDrawGizmos(false);
        }

        public void OnDrawGizmos(bool selected)
        {
            Color col = !selected ? GizmosColor : GizmosColorSelected;
            if (this.StartTransform != null)
            {
                this.DrawCircle(this.StartTransform.position, 0.4f, 10, col);
            }
            if (this.EndTransform != null)
            {
                this.DrawCircle(this.EndTransform.position, 0.4f, 10, col);
            }
            if ((this.StartTransform != null) && (this.EndTransform != null))
            {
                Gizmos.color = col;
                this.DrawGizmoBezier(this.StartTransform.position, this.EndTransform.position);
                if (selected)
                {
                    Vector3 normalized = Vector3.Cross(Vector3.up, this.EndTransform.position - this.StartTransform.position).normalized;
                    this.DrawGizmoBezier(this.StartTransform.position + ((Vector3) (normalized * 0.1f)), this.EndTransform.position + ((Vector3) (normalized * 0.1f)));
                    this.DrawGizmoBezier(this.StartTransform.position - ((Vector3) (normalized * 0.1f)), this.EndTransform.position - ((Vector3) (normalized * 0.1f)));
                }
            }
        }

        public virtual void OnDrawGizmosSelected()
        {
            this.OnDrawGizmos(true);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if ((Application.isPlaying && (AstarPath.active != null)) && ((AstarPath.active.astarData != null) && (AstarPath.active.astarData.pointGraph != null)))
            {
                this.OnGraphsPostUpdate();
            }
        }

        public override void OnGraphsPostUpdate()
        {
            if (!AstarPath.active.isScanning)
            {
                if ((this.connectedNode1 != null) && this.connectedNode1.Destroyed)
                {
                    this.connectedNode1 = null;
                }
                if ((this.connectedNode2 != null) && this.connectedNode2.Destroyed)
                {
                    this.connectedNode2 = null;
                }
                if (!this.postScanCalled)
                {
                    this.OnPostScan();
                }
                else
                {
                    this.Apply(false);
                }
            }
        }

        public override void OnPostScan()
        {
            if (AstarPath.active.isScanning)
            {
                this.InternalOnPostScan();
            }
            else
            {
                AstarPath.active.AddWorkItem(new AstarWorkItem(delegate (bool force) {
                    this.InternalOnPostScan();
                    return true;
                }));
            }
        }

        private void RemoveConnections(GraphNode node)
        {
            node.ClearConnections(true);
        }

        public GraphNode EndNode
        {
            get
            {
                return this.endNode;
            }
        }

        public Transform EndTransform
        {
            get
            {
                return this.end;
            }
        }

        public GraphNode StartNode
        {
            get
            {
                return this.startNode;
            }
        }

        public Transform StartTransform
        {
            get
            {
                return base.transform;
            }
        }
    }
}

