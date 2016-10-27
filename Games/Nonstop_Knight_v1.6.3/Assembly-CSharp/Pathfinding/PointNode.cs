namespace Pathfinding
{
    using Pathfinding.Serialization;
    using System;
    using UnityEngine;

    public class PointNode : GraphNode
    {
        public uint[] connectionCosts;
        public GraphNode[] connections;
        public GameObject gameObject;
        public PointNode next;

        public PointNode(AstarPath astar) : base(astar)
        {
        }

        public override void AddConnection(GraphNode node, uint cost)
        {
            if (node == null)
            {
                throw new ArgumentNullException();
            }
            if (this.connections != null)
            {
                for (int j = 0; j < this.connections.Length; j++)
                {
                    if (this.connections[j] == node)
                    {
                        this.connectionCosts[j] = cost;
                        return;
                    }
                }
            }
            int index = (this.connections == null) ? 0 : this.connections.Length;
            GraphNode[] nodeArray = new GraphNode[index + 1];
            uint[] numArray = new uint[index + 1];
            for (int i = 0; i < index; i++)
            {
                nodeArray[i] = this.connections[i];
                numArray[i] = this.connectionCosts[i];
            }
            nodeArray[index] = node;
            numArray[index] = cost;
            this.connections = nodeArray;
            this.connectionCosts = numArray;
        }

        public override void ClearConnections(bool alsoReverse)
        {
            if (alsoReverse && (this.connections != null))
            {
                for (int i = 0; i < this.connections.Length; i++)
                {
                    this.connections[i].RemoveConnection(this);
                }
            }
            this.connections = null;
            this.connectionCosts = null;
        }

        public override bool ContainsConnection(GraphNode node)
        {
            for (int i = 0; i < this.connections.Length; i++)
            {
                if (this.connections[i] == node)
                {
                    return true;
                }
            }
            return false;
        }

        public override void DeserializeNode(GraphSerializationContext ctx)
        {
            base.DeserializeNode(ctx);
            base.position = ctx.DeserializeInt3();
        }

        public override void DeserializeReferences(GraphSerializationContext ctx)
        {
            int num = ctx.reader.ReadInt32();
            if (num == -1)
            {
                this.connections = null;
                this.connectionCosts = null;
            }
            else
            {
                this.connections = new GraphNode[num];
                this.connectionCosts = new uint[num];
                for (int i = 0; i < num; i++)
                {
                    this.connections[i] = ctx.DeserializeNodeReference();
                    this.connectionCosts[i] = ctx.reader.ReadUInt32();
                }
            }
        }

        public override void GetConnections(GraphNodeDelegate del)
        {
            if (this.connections != null)
            {
                for (int i = 0; i < this.connections.Length; i++)
                {
                    del(this.connections[i]);
                }
            }
        }

        public override void Open(Path path, PathNode pathNode, PathHandler handler)
        {
            if (this.connections != null)
            {
                for (int i = 0; i < this.connections.Length; i++)
                {
                    GraphNode node = this.connections[i];
                    if (path.CanTraverse(node))
                    {
                        PathNode node2 = handler.GetPathNode(node);
                        if (node2.pathID != handler.PathID)
                        {
                            node2.parent = pathNode;
                            node2.pathID = handler.PathID;
                            node2.cost = this.connectionCosts[i];
                            node2.H = path.CalculateHScore(node);
                            node.UpdateG(path, node2);
                            handler.heap.Add(node2);
                        }
                        else
                        {
                            uint num2 = this.connectionCosts[i];
                            if (((pathNode.G + num2) + path.GetTraversalCost(node)) < node2.G)
                            {
                                node2.cost = num2;
                                node2.parent = pathNode;
                                node.UpdateRecursiveG(path, node2, handler);
                            }
                            else if ((((node2.G + num2) + path.GetTraversalCost(this)) < pathNode.G) && node.ContainsConnection(this))
                            {
                                pathNode.parent = node2;
                                pathNode.cost = num2;
                                this.UpdateRecursiveG(path, pathNode, handler);
                            }
                        }
                    }
                }
            }
        }

        public override void RemoveConnection(GraphNode node)
        {
            if (this.connections != null)
            {
                for (int i = 0; i < this.connections.Length; i++)
                {
                    if (this.connections[i] == node)
                    {
                        int length = this.connections.Length;
                        GraphNode[] nodeArray = new GraphNode[length - 1];
                        uint[] numArray = new uint[length - 1];
                        for (int j = 0; j < i; j++)
                        {
                            nodeArray[j] = this.connections[j];
                            numArray[j] = this.connectionCosts[j];
                        }
                        for (int k = i + 1; k < length; k++)
                        {
                            nodeArray[k - 1] = this.connections[k];
                            numArray[k - 1] = this.connectionCosts[k];
                        }
                        this.connections = nodeArray;
                        this.connectionCosts = numArray;
                        return;
                    }
                }
            }
        }

        public override void SerializeNode(GraphSerializationContext ctx)
        {
            base.SerializeNode(ctx);
            ctx.SerializeInt3(base.position);
        }

        public override void SerializeReferences(GraphSerializationContext ctx)
        {
            if (this.connections == null)
            {
                ctx.writer.Write(-1);
            }
            else
            {
                ctx.writer.Write(this.connections.Length);
                for (int i = 0; i < this.connections.Length; i++)
                {
                    ctx.SerializeNodeReference(this.connections[i]);
                    ctx.writer.Write(this.connectionCosts[i]);
                }
            }
        }

        public void SetPosition(Int3 value)
        {
            base.position = value;
        }

        public override void UpdateRecursiveG(Path path, PathNode pathNode, PathHandler handler)
        {
            base.UpdateG(path, pathNode);
            handler.heap.Add(pathNode);
            for (int i = 0; i < this.connections.Length; i++)
            {
                GraphNode node = this.connections[i];
                PathNode node2 = handler.GetPathNode(node);
                if ((node2.parent == pathNode) && (node2.pathID == handler.PathID))
                {
                    node.UpdateRecursiveG(path, node2, handler);
                }
            }
        }
    }
}

