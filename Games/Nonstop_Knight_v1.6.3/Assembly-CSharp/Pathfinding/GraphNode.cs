namespace Pathfinding
{
    using Pathfinding.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class GraphNode
    {
        protected uint flags;
        private const uint FlagsAreaMask = 0x3fffe;
        private const int FlagsAreaOffset = 1;
        private const uint FlagsGraphMask = 0xff000000;
        private const int FlagsGraphOffset = 0x18;
        private const uint FlagsTagMask = 0xf80000;
        private const int FlagsTagOffset = 0x13;
        private const uint FlagsWalkableMask = 1;
        private const int FlagsWalkableOffset = 0;
        public const uint MaxAreaIndex = 0x1ffff;
        public const uint MaxGraphIndex = 0xff;
        private int nodeIndex;
        public Int3 position;

        protected GraphNode(AstarPath astar)
        {
            if (object.ReferenceEquals(astar, null))
            {
                throw new Exception("No active AstarPath object to bind to");
            }
            this.nodeIndex = astar.GetNewNodeIndex();
            astar.InitializeNode(this);
        }

        public abstract void AddConnection(GraphNode node, uint cost);
        public abstract void ClearConnections(bool alsoReverse);
        public virtual bool ContainsConnection(GraphNode node)
        {
            <ContainsConnection>c__AnonStorey246 storey = new <ContainsConnection>c__AnonStorey246();
            storey.node = node;
            storey.contains = false;
            this.GetConnections(new GraphNodeDelegate(storey.<>m__14));
            return storey.contains;
        }

        public virtual void DeserializeNode(GraphSerializationContext ctx)
        {
            this.Penalty = ctx.reader.ReadUInt32();
            this.Flags = ctx.reader.ReadUInt32();
            this.GraphIndex = ctx.graphIndex;
        }

        public virtual void DeserializeReferences(GraphSerializationContext ctx)
        {
        }

        internal void Destroy()
        {
            if (!this.Destroyed)
            {
                this.ClearConnections(true);
                if (AstarPath.active != null)
                {
                    AstarPath.active.DestroyNode(this);
                }
                this.nodeIndex = -1;
            }
        }

        public virtual void FloodFill(Stack<GraphNode> stack, uint region)
        {
            <FloodFill>c__AnonStorey245 storey = new <FloodFill>c__AnonStorey245();
            storey.region = region;
            storey.stack = stack;
            this.GetConnections(new GraphNodeDelegate(storey.<>m__13));
        }

        public abstract void GetConnections(GraphNodeDelegate del);
        public virtual bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
        {
            return false;
        }

        public abstract void Open(Path path, PathNode pathNode, PathHandler handler);
        public virtual Vector3 RandomPointOnSurface()
        {
            return (Vector3) this.position;
        }

        public virtual void RecalculateConnectionCosts()
        {
        }

        public abstract void RemoveConnection(GraphNode node);
        public virtual void SerializeNode(GraphSerializationContext ctx)
        {
            ctx.writer.Write(this.Penalty);
            ctx.writer.Write(this.Flags);
        }

        public virtual void SerializeReferences(GraphSerializationContext ctx)
        {
        }

        public virtual float SurfaceArea()
        {
            return 0f;
        }

        public void UpdateG(Path path, PathNode pathNode)
        {
            pathNode.G = (pathNode.parent.G + pathNode.cost) + path.GetTraversalCost(this);
        }

        public virtual void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
        {
            <UpdateRecursiveG>c__AnonStorey244 storey = new <UpdateRecursiveG>c__AnonStorey244();
            storey.handler = handler;
            storey.pathNode = pathNode;
            storey.path = path;
            this.UpdateG(storey.path, storey.pathNode);
            storey.handler.heap.Add(storey.pathNode);
            this.GetConnections(new GraphNodeDelegate(storey.<>m__12));
        }

        public uint Area
        {
            get
            {
                return (uint) ((this.flags & 0x3fffe) >> 1);
            }
            set
            {
                this.flags = (this.flags & 0xfffc0001) | (value << 1);
            }
        }

        public bool Destroyed
        {
            get
            {
                return (this.nodeIndex == -1);
            }
        }

        public uint Flags
        {
            get
            {
                return this.flags;
            }
            set
            {
                this.flags = value;
            }
        }

        public uint GraphIndex
        {
            get
            {
                return (uint) ((this.flags & -16777216) >> 0x18);
            }
            set
            {
                this.flags = (this.flags & 0xffffff) | (value << 0x18);
            }
        }

        public int NodeIndex
        {
            get
            {
                return this.nodeIndex;
            }
        }

        public uint Penalty
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public uint Tag
        {
            get
            {
                return (uint) ((this.flags & 0xf80000) >> 0x13);
            }
            set
            {
                this.flags = (this.flags & 0xff07ffff) | (value << 0x13);
            }
        }

        public bool Walkable
        {
            get
            {
                return ((this.flags & 1) != 0);
            }
            set
            {
                this.flags = (this.flags & 0xfffffffe) | (!value ? 0 : 1);
            }
        }

        [CompilerGenerated]
        private sealed class <ContainsConnection>c__AnonStorey246
        {
            internal bool contains;
            internal GraphNode node;

            internal void <>m__14(GraphNode neighbour)
            {
                this.contains |= neighbour == this.node;
            }
        }

        [CompilerGenerated]
        private sealed class <FloodFill>c__AnonStorey245
        {
            internal uint region;
            internal Stack<GraphNode> stack;

            internal void <>m__13(GraphNode other)
            {
                if (other.Area != this.region)
                {
                    other.Area = this.region;
                    this.stack.Push(other);
                }
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateRecursiveG>c__AnonStorey244
        {
            internal PathHandler handler;
            internal Path path;
            internal PathNode pathNode;

            internal void <>m__12(GraphNode other)
            {
                PathNode pathNode = this.handler.GetPathNode(other);
                if ((pathNode.parent == this.pathNode) && (pathNode.pathID == this.handler.PathID))
                {
                    other.UpdateRecursiveG(this.path, pathNode, this.handler);
                }
            }
        }
    }
}

