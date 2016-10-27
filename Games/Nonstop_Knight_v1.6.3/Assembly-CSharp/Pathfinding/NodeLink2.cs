namespace Pathfinding
{
    using Pathfinding.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [AddComponentMenu("Pathfinding/Link2"), HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_node_link2.php")]
    public class NodeLink2 : GraphModifier
    {
        [CompilerGenerated]
        private PointNode <endNode>k__BackingField;
        [CompilerGenerated]
        private PointNode <startNode>k__BackingField;
        private Vector3 clamped1;
        private Vector3 clamped2;
        private GraphNode connectedNode1;
        private GraphNode connectedNode2;
        public float costFactor = 1f;
        public Transform end;
        private static readonly Color GizmosColor = new Color(0.8078431f, 0.5333334f, 0.1882353f, 0.5f);
        private static readonly Color GizmosColorSelected = new Color(0.9215686f, 0.4823529f, 0.1254902f, 1f);
        public bool oneWay;
        private bool postScanCalled;
        protected static Dictionary<GraphNode, NodeLink2> reference = new Dictionary<GraphNode, NodeLink2>();

        public void Apply(bool forceNewCheck)
        {
            NNConstraint none = NNConstraint.None;
            int graphIndex = (int) this.startNode.GraphIndex;
            none.graphMask = ~(((int) 1) << graphIndex);
            this.startNode.SetPosition((Int3) this.StartTransform.position);
            this.endNode.SetPosition((Int3) this.EndTransform.position);
            this.RemoveConnections(this.startNode);
            this.RemoveConnections(this.endNode);
            Int3 num3 = (Int3) (this.StartTransform.position - this.EndTransform.position);
            uint cost = (uint) Mathf.RoundToInt(num3.costMagnitude * this.costFactor);
            this.startNode.AddConnection(this.endNode, cost);
            this.endNode.AddConnection(this.startNode, cost);
            if ((this.connectedNode1 == null) || forceNewCheck)
            {
                NNInfo nearest = AstarPath.active.GetNearest(this.StartTransform.position, none);
                this.connectedNode1 = nearest.node;
                this.clamped1 = nearest.position;
            }
            if ((this.connectedNode2 == null) || forceNewCheck)
            {
                NNInfo info2 = AstarPath.active.GetNearest(this.EndTransform.position, none);
                this.connectedNode2 = info2.node;
                this.clamped2 = info2.position;
            }
            if ((this.connectedNode2 != null) && (this.connectedNode1 != null))
            {
                Int3 num4 = (Int3) (this.clamped1 - this.StartTransform.position);
                this.connectedNode1.AddConnection(this.startNode, (uint) Mathf.RoundToInt(num4.costMagnitude * this.costFactor));
                if (!this.oneWay)
                {
                    Int3 num5 = (Int3) (this.clamped2 - this.EndTransform.position);
                    this.connectedNode2.AddConnection(this.endNode, (uint) Mathf.RoundToInt(num5.costMagnitude * this.costFactor));
                }
                if (!this.oneWay)
                {
                    Int3 num6 = (Int3) (this.clamped1 - this.StartTransform.position);
                    this.startNode.AddConnection(this.connectedNode1, (uint) Mathf.RoundToInt(num6.costMagnitude * this.costFactor));
                }
                Int3 num7 = (Int3) (this.clamped2 - this.EndTransform.position);
                this.endNode.AddConnection(this.connectedNode2, (uint) Mathf.RoundToInt(num7.costMagnitude * this.costFactor));
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

        internal static void DeserializeReferences(GraphSerializationContext ctx)
        {
            int num = ctx.reader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                GraphModifier modifier;
                ulong key = ctx.reader.ReadUInt64();
                GraphNode node = ctx.DeserializeNodeReference();
                GraphNode node2 = ctx.DeserializeNodeReference();
                GraphNode node3 = ctx.DeserializeNodeReference();
                GraphNode node4 = ctx.DeserializeNodeReference();
                Vector3 vector = ctx.DeserializeVector3();
                Vector3 vector2 = ctx.DeserializeVector3();
                bool flag = ctx.reader.ReadBoolean();
                if (!GraphModifier.usedIDs.TryGetValue(key, out modifier))
                {
                    throw new Exception("Tried to deserialize a NodeLink2 reference, but the link could not be found in the scene.\nIf a NodeLink2 is included in serialized graph data, the same NodeLink2 component must be present in the scene when loading the graph data.");
                }
                NodeLink2 link = modifier as NodeLink2;
                if (link == null)
                {
                    throw new Exception("Tried to deserialize a NodeLink2 reference, but the link was not of the correct type or it has been destroyed.\nIf a NodeLink2 is included in serialized graph data, the same NodeLink2 component must be present in the scene when loading the graph data.");
                }
                reference[node] = link;
                reference[node2] = link;
                if (link.startNode != null)
                {
                    reference.Remove(link.startNode);
                }
                if (link.endNode != null)
                {
                    reference.Remove(link.endNode);
                }
                link.startNode = node as PointNode;
                link.endNode = node2 as PointNode;
                link.connectedNode1 = node3;
                link.connectedNode2 = node4;
                link.postScanCalled = flag;
                link.clamped1 = vector;
                link.clamped2 = vector2;
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

        public static NodeLink2 GetNodeLink(GraphNode node)
        {
            NodeLink2 link;
            reference.TryGetValue(node, out link);
            return link;
        }

        public void InternalOnPostScan()
        {
            if ((this.EndTransform != null) && (this.StartTransform != null))
            {
                if (AstarPath.active.astarData.pointGraph == null)
                {
                    PointGraph graph = AstarPath.active.astarData.AddGraph(typeof(PointGraph)) as PointGraph;
                    graph.name = "PointGraph (used for node links)";
                }
                if (this.startNode != null)
                {
                    reference.Remove(this.startNode);
                }
                if (this.endNode != null)
                {
                    reference.Remove(this.endNode);
                }
                this.startNode = AstarPath.active.astarData.pointGraph.AddNode((Int3) this.StartTransform.position);
                this.endNode = AstarPath.active.astarData.pointGraph.AddNode((Int3) this.EndTransform.position);
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
            if (((Application.isPlaying && (AstarPath.active != null)) && ((AstarPath.active.astarData != null) && (AstarPath.active.astarData.pointGraph != null))) && !AstarPath.active.isScanning)
            {
                AstarPath.RegisterSafeUpdate(new Action(this.OnGraphsPostUpdate));
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
            this.InternalOnPostScan();
        }

        private void RemoveConnections(GraphNode node)
        {
            node.ClearConnections(true);
        }

        internal static void SerializeReferences(GraphSerializationContext ctx)
        {
            List<NodeLink2> modifiersOfType = GraphModifier.GetModifiersOfType<NodeLink2>();
            ctx.writer.Write(modifiersOfType.Count);
            foreach (NodeLink2 link in modifiersOfType)
            {
                ctx.writer.Write(link.uniqueID);
                ctx.SerializeNodeReference(link.startNode);
                ctx.SerializeNodeReference(link.endNode);
                ctx.SerializeNodeReference(link.connectedNode1);
                ctx.SerializeNodeReference(link.connectedNode2);
                ctx.SerializeVector3(link.clamped1);
                ctx.SerializeVector3(link.clamped2);
                ctx.writer.Write(link.postScanCalled);
            }
        }

        public PointNode endNode
        {
            [CompilerGenerated]
            get
            {
                return this.<endNode>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<endNode>k__BackingField = value;
            }
        }

        [Obsolete("Use endNode instead (lowercase e)")]
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

        public PointNode startNode
        {
            [CompilerGenerated]
            get
            {
                return this.<startNode>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<startNode>k__BackingField = value;
            }
        }

        [Obsolete("Use startNode instead (lowercase s)")]
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

