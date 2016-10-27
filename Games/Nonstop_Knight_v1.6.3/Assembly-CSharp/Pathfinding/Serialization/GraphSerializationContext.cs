namespace Pathfinding.Serialization
{
    using Pathfinding;
    using System;
    using System.IO;
    using UnityEngine;

    public class GraphSerializationContext
    {
        public readonly uint graphIndex;
        private readonly GraphNode[] id2NodeMapping;
        public readonly GraphMeta meta;
        public readonly BinaryReader reader;
        public readonly BinaryWriter writer;

        public GraphSerializationContext(BinaryWriter writer)
        {
            this.writer = writer;
        }

        public GraphSerializationContext(BinaryReader reader, GraphNode[] id2NodeMapping, uint graphIndex, GraphMeta meta)
        {
            this.reader = reader;
            this.id2NodeMapping = id2NodeMapping;
            this.graphIndex = graphIndex;
            this.meta = meta;
        }

        public Int3 DeserializeInt3()
        {
            return new Int3(this.reader.ReadInt32(), this.reader.ReadInt32(), this.reader.ReadInt32());
        }

        public GraphNode DeserializeNodeReference()
        {
            int index = this.reader.ReadInt32();
            if (this.id2NodeMapping == null)
            {
                throw new Exception("Calling DeserializeNodeReference when serializing");
            }
            if (index == -1)
            {
                return null;
            }
            GraphNode node = this.id2NodeMapping[index];
            if (node == null)
            {
                throw new Exception("Invalid id (" + index + ")");
            }
            return node;
        }

        public Vector3 DeserializeVector3()
        {
            return new Vector3(this.reader.ReadSingle(), this.reader.ReadSingle(), this.reader.ReadSingle());
        }

        public void SerializeInt3(Int3 v)
        {
            this.writer.Write(v.x);
            this.writer.Write(v.y);
            this.writer.Write(v.z);
        }

        public void SerializeNodeReference(GraphNode node)
        {
            this.writer.Write((node != null) ? node.NodeIndex : -1);
        }

        public void SerializeVector3(Vector3 v)
        {
            this.writer.Write(v.x);
            this.writer.Write(v.y);
            this.writer.Write(v.z);
        }
    }
}

