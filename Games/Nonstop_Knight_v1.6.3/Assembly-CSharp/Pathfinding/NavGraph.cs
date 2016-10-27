namespace Pathfinding
{
    using Pathfinding.Serialization;
    using Pathfinding.Serialization.JsonFx;
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class NavGraph
    {
        private byte[] _sguid;
        [CompilerGenerated]
        private static GraphNodeDelegateCancelable <>f__am$cacheA;
        public AstarPath active;
        [JsonMember]
        public bool drawGizmos = true;
        public uint graphIndex;
        [JsonMember]
        public bool infoScreenOpen;
        [JsonMember]
        public uint initialPenalty;
        public Matrix4x4 inverseMatrix = Matrix4x4.identity;
        public Matrix4x4 matrix = Matrix4x4.identity;
        [JsonMember]
        public string name;
        [JsonMember]
        public bool open;

        protected NavGraph()
        {
        }

        public virtual void Awake()
        {
        }

        public virtual int CountNodes()
        {
            <CountNodes>c__AnonStorey24F storeyf = new <CountNodes>c__AnonStorey24F();
            storeyf.count = 0;
            GraphNodeDelegateCancelable del = new GraphNodeDelegateCancelable(storeyf.<>m__1E);
            this.GetNodes(del);
            return storeyf.count;
        }

        public virtual void DeserializeExtraInfo(GraphSerializationContext ctx)
        {
        }

        public NNInfoInternal GetNearest(Vector3 position)
        {
            return this.GetNearest(position, NNConstraint.None);
        }

        public NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint)
        {
            return this.GetNearest(position, constraint, null);
        }

        public virtual NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
        {
            <GetNearest>c__AnonStorey251 storey = new <GetNearest>c__AnonStorey251();
            storey.position = position;
            storey.constraint = constraint;
            storey.maxDistSqr = !storey.constraint.constrainDistance ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistanceSqr;
            storey.minDist = float.PositiveInfinity;
            storey.minNode = null;
            storey.minConstDist = float.PositiveInfinity;
            storey.minConstNode = null;
            this.GetNodes(new GraphNodeDelegateCancelable(storey.<>m__20));
            NNInfoInternal internal2 = new NNInfoInternal(storey.minNode);
            internal2.constrainedNode = storey.minConstNode;
            if (storey.minConstNode != null)
            {
                internal2.constClampedPosition = (Vector3) storey.minConstNode.position;
                return internal2;
            }
            if (storey.minNode != null)
            {
                internal2.constrainedNode = storey.minNode;
                internal2.constClampedPosition = (Vector3) storey.minNode.position;
            }
            return internal2;
        }

        public virtual NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
        {
            return this.GetNearest(position, constraint);
        }

        public abstract void GetNodes(GraphNodeDelegateCancelable del);
        public static bool InSearchTree(GraphNode node, Path path)
        {
            if ((path != null) && (path.pathHandler != null))
            {
                return (path.pathHandler.GetPathNode(node).pathID == path.pathID);
            }
            return true;
        }

        public virtual Color NodeColor(GraphNode node, PathHandler data)
        {
            Color areaColor;
            GraphDebugMode debugMode = AstarPath.active.debugMode;
            switch (debugMode)
            {
                case GraphDebugMode.Areas:
                    areaColor = AstarColor.GetAreaColor(node.Area);
                    break;

                case GraphDebugMode.Penalty:
                    areaColor = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (node.Penalty - AstarPath.active.debugFloor) / (AstarPath.active.debugRoof - AstarPath.active.debugFloor));
                    break;

                case GraphDebugMode.Connections:
                    areaColor = AstarColor.NodeConnection;
                    break;

                case GraphDebugMode.Tags:
                    areaColor = AstarColor.GetAreaColor(node.Tag);
                    break;

                default:
                {
                    float g;
                    if (data == null)
                    {
                        return AstarColor.NodeConnection;
                    }
                    PathNode pathNode = data.GetPathNode(node);
                    switch (debugMode)
                    {
                        case GraphDebugMode.G:
                            g = pathNode.G;
                            break;

                        case GraphDebugMode.H:
                            g = pathNode.H;
                            break;

                        default:
                            g = pathNode.F;
                            break;
                    }
                    areaColor = Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (g - AstarPath.active.debugFloor) / (AstarPath.active.debugRoof - AstarPath.active.debugFloor));
                    break;
                }
            }
            areaColor.a *= 0.5f;
            return areaColor;
        }

        public virtual void OnDestroy()
        {
            if (<>f__am$cacheA == null)
            {
                <>f__am$cacheA = delegate (GraphNode node) {
                    node.Destroy();
                    return true;
                };
            }
            this.GetNodes(<>f__am$cacheA);
        }

        public virtual void OnDrawGizmos(bool drawNodes)
        {
            <OnDrawGizmos>c__AnonStorey252 storey = new <OnDrawGizmos>c__AnonStorey252();
            storey.<>f__this = this;
            if (drawNodes)
            {
                storey.data = AstarPath.active.debugPathData;
                storey.node = null;
                storey.drawConnection = new GraphNodeDelegate(storey.<>m__22);
                this.GetNodes(new GraphNodeDelegateCancelable(storey.<>m__23));
            }
        }

        public virtual void PostDeserialization()
        {
        }

        public virtual void RelocateNodes(Matrix4x4 oldMatrix, Matrix4x4 newMatrix)
        {
            <RelocateNodes>c__AnonStorey250 storey = new <RelocateNodes>c__AnonStorey250();
            Matrix4x4 inverse = oldMatrix.inverse;
            storey.m = newMatrix * inverse;
            this.GetNodes(new GraphNodeDelegateCancelable(storey.<>m__1F));
            this.SetMatrix(newMatrix);
        }

        [Obsolete("Please use AstarPath.active.Scan or if you really want this.ScanInternal which has the same functionality as this method had")]
        public void Scan()
        {
            throw new Exception("This method is deprecated. Please use AstarPath.active.Scan or if you really want this.ScanInternal which has the same functionality as this method had.");
        }

        public void ScanGraph()
        {
            if (AstarPath.OnPreScan != null)
            {
                AstarPath.OnPreScan(AstarPath.active);
            }
            if (AstarPath.OnGraphPreScan != null)
            {
                AstarPath.OnGraphPreScan(this);
            }
            IEnumerator<Progress> enumerator = this.ScanInternal().GetEnumerator();
            while (enumerator.MoveNext())
            {
            }
            if (AstarPath.OnGraphPostScan != null)
            {
                AstarPath.OnGraphPostScan(this);
            }
            if (AstarPath.OnPostScan != null)
            {
                AstarPath.OnPostScan(AstarPath.active);
            }
        }

        public abstract IEnumerable<Progress> ScanInternal();
        public virtual void SerializeExtraInfo(GraphSerializationContext ctx)
        {
        }

        public void SetMatrix(Matrix4x4 m)
        {
            this.matrix = m;
            this.inverseMatrix = m.inverse;
        }

        internal virtual void UnloadGizmoMeshes()
        {
        }

        [JsonMember]
        public Pathfinding.Util.Guid guid
        {
            get
            {
                if ((this._sguid == null) || (this._sguid.Length != 0x10))
                {
                    this._sguid = Pathfinding.Util.Guid.NewGuid().ToByteArray();
                }
                return new Pathfinding.Util.Guid(this._sguid);
            }
            set
            {
                this._sguid = value.ToByteArray();
            }
        }

        [CompilerGenerated]
        private sealed class <CountNodes>c__AnonStorey24F
        {
            internal int count;

            internal bool <>m__1E(GraphNode node)
            {
                this.count++;
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <GetNearest>c__AnonStorey251
        {
            internal NNConstraint constraint;
            internal float maxDistSqr;
            internal float minConstDist;
            internal GraphNode minConstNode;
            internal float minDist;
            internal GraphNode minNode;
            internal Vector3 position;

            internal bool <>m__20(GraphNode node)
            {
                Vector3 vector = this.position - ((Vector3) node.position);
                float sqrMagnitude = vector.sqrMagnitude;
                if (sqrMagnitude < this.minDist)
                {
                    this.minDist = sqrMagnitude;
                    this.minNode = node;
                }
                if (((sqrMagnitude < this.minConstDist) && (sqrMagnitude < this.maxDistSqr)) && this.constraint.Suitable(node))
                {
                    this.minConstDist = sqrMagnitude;
                    this.minConstNode = node;
                }
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <OnDrawGizmos>c__AnonStorey252
        {
            internal NavGraph <>f__this;
            internal PathHandler data;
            internal GraphNodeDelegate drawConnection;
            internal GraphNode node;

            internal void <>m__22(GraphNode otherNode)
            {
                Gizmos.DrawLine((Vector3) this.node.position, (Vector3) otherNode.position);
            }

            internal bool <>m__23(GraphNode _node)
            {
                this.node = _node;
                Gizmos.color = this.<>f__this.NodeColor(this.node, AstarPath.active.debugPathData);
                if (!AstarPath.active.showSearchTree || NavGraph.InSearchTree(this.node, AstarPath.active.debugPath))
                {
                    PathNode node = (this.data == null) ? null : this.data.GetPathNode(this.node);
                    if ((AstarPath.active.showSearchTree && (node != null)) && (node.parent != null))
                    {
                        Gizmos.DrawLine((Vector3) this.node.position, (Vector3) node.parent.node.position);
                    }
                    else
                    {
                        this.node.GetConnections(this.drawConnection);
                    }
                }
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <RelocateNodes>c__AnonStorey250
        {
            internal Matrix4x4 m;

            internal bool <>m__1F(GraphNode node)
            {
                node.position = (Int3) this.m.MultiplyPoint((Vector3) node.position);
                return true;
            }
        }
    }
}

