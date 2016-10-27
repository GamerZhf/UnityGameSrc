namespace Pathfinding.Serialization
{
    using Pathfinding;
    using Pathfinding.Ionic.Zip;
    using Pathfinding.Serialization.JsonFx;
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using UnityEngine;

    public class AstarSerializer
    {
        private static StringBuilder _stringBuilder = new StringBuilder();
        private const string binaryExt = ".binary";
        private uint checksum;
        private AstarData data;
        private UTF8Encoding encoding;
        private Dictionary<NavGraph, int> graphIndexInZip;
        private int graphIndexOffset;
        private NavGraph[] graphs;
        private const string jsonExt = ".json";
        private GraphMeta meta;
        public JsonReaderSettings readerSettings;
        private SerializeSettings settings;
        public JsonWriterSettings writerSettings;
        private ZipFile zip;
        private MemoryStream zipStream;

        public AstarSerializer(AstarData data)
        {
            this.checksum = uint.MaxValue;
            this.encoding = new UTF8Encoding();
            this.data = data;
            this.settings = SerializeSettings.Settings;
        }

        public AstarSerializer(AstarData data, SerializeSettings settings)
        {
            this.checksum = uint.MaxValue;
            this.encoding = new UTF8Encoding();
            this.data = data;
            this.settings = settings;
        }

        private void AddChecksum(byte[] bytes)
        {
            this.checksum = Checksum.GetChecksum(bytes, this.checksum);
        }

        private bool AnyDestroyedNodesInGraphs()
        {
            <AnyDestroyedNodesInGraphs>c__AnonStorey24B storeyb = new <AnyDestroyedNodesInGraphs>c__AnonStorey24B();
            storeyb.result = false;
            for (int i = 0; i < this.graphs.Length; i++)
            {
                this.graphs[i].GetNodes(new GraphNodeDelegateCancelable(storeyb.<>m__1A));
            }
            return storeyb.result;
        }

        public void CloseDeserialize()
        {
            this.zipStream.Dispose();
            this.zip.Dispose();
            this.zip = null;
            this.zipStream = null;
        }

        public byte[] CloseSerialize()
        {
            byte[] bytes = this.SerializeMeta();
            this.AddChecksum(bytes);
            this.zip.AddEntry("meta.json", bytes);
            DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            IEnumerator<ZipEntry> enumerator = this.zip.Entries.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ZipEntry current = enumerator.Current;
                    current.AccessedTime = time;
                    current.CreationTime = time;
                    current.LastModified = time;
                    current.ModifiedTime = time;
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            MemoryStream outputStream = new MemoryStream();
            this.zip.Save(outputStream);
            bytes = outputStream.ToArray();
            outputStream.Dispose();
            this.zip.Dispose();
            this.zip = null;
            return bytes;
        }

        public void DeserializeEditorSettings(GraphEditorBase[] graphEditors)
        {
            if (graphEditors != null)
            {
                for (int i = 0; i < graphEditors.Length; i++)
                {
                    if (graphEditors[i] != null)
                    {
                        for (int j = 0; j < this.graphs.Length; j++)
                        {
                            if (graphEditors[i].target == this.graphs[j])
                            {
                                int num3 = this.graphIndexInZip[this.graphs[j]];
                                ZipEntry entry = this.zip["graph" + num3 + "_editor.json"];
                                if (entry != null)
                                {
                                    JsonReader reader = new JsonReader(GetString(entry), this.readerSettings);
                                    GraphEditorBase base2 = graphEditors[i];
                                    reader.PopulateObject<GraphEditorBase>(ref base2);
                                    graphEditors[i] = base2;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void DeserializeExtraInfo()
        {
            bool flag = false;
            for (int i = 0; i < this.graphs.Length; i++)
            {
                flag |= this.DeserializeExtraInfo(this.graphs[i]);
            }
            if (flag)
            {
                if (this.AnyDestroyedNodesInGraphs())
                {
                    Debug.LogError("Graph contains destroyed nodes. This is a bug.");
                }
                GraphNode[] nodeArray = this.DeserializeNodeReferenceMap();
                for (int j = 0; j < this.graphs.Length; j++)
                {
                    this.DeserializeNodeReferences(this.graphs[j], nodeArray);
                }
                this.DeserializeNodeLinks(nodeArray);
            }
        }

        private bool DeserializeExtraInfo(NavGraph graph)
        {
            int num = this.graphIndexInZip[graph];
            ZipEntry entry = this.zip["graph" + num + "_extra.binary"];
            if (entry == null)
            {
                return false;
            }
            GraphSerializationContext ctx = new GraphSerializationContext(GetBinaryReader(entry), null, graph.graphIndex, this.meta);
            graph.DeserializeExtraInfo(ctx);
            return true;
        }

        private NavGraph DeserializeGraph(int zipIndex, int graphIndex)
        {
            System.Type graphType = this.meta.GetGraphType(zipIndex);
            if (object.Equals(graphType, null))
            {
                return null;
            }
            ZipEntry entry = this.zip["graph" + zipIndex + ".json"];
            if (entry == null)
            {
                object[] objArray1 = new object[] { "Could not find data for graph ", zipIndex, " in zip. Entry 'graph", zipIndex, ".json' does not exist" };
                throw new FileNotFoundException(string.Concat(objArray1));
            }
            NavGraph graph = this.data.CreateGraph(graphType);
            graph.graphIndex = (uint) graphIndex;
            new JsonReader(GetString(entry), this.readerSettings).PopulateObject<NavGraph>(ref graph);
            if (graph.guid.ToString() != this.meta.guids[zipIndex])
            {
                object[] objArray2 = new object[] { "Guid in graph file not equal to guid defined in meta file. Have you edited the data manually?\n", graph.guid, " != ", this.meta.guids[zipIndex] };
                throw new Exception(string.Concat(objArray2));
            }
            return graph;
        }

        public NavGraph[] DeserializeGraphs()
        {
            List<NavGraph> list = new List<NavGraph>();
            this.graphIndexInZip = new Dictionary<NavGraph, int>();
            for (int i = 0; i < this.meta.graphs; i++)
            {
                int graphIndex = list.Count + this.graphIndexOffset;
                NavGraph item = this.DeserializeGraph(i, graphIndex);
                if (item != null)
                {
                    list.Add(item);
                    this.graphIndexInZip[item] = i;
                }
            }
            this.graphs = list.ToArray();
            return this.graphs;
        }

        private GraphMeta DeserializeMeta(ZipEntry entry)
        {
            if (entry == null)
            {
                throw new Exception("No metadata found in serialized data.");
            }
            JsonReader reader = new JsonReader(GetString(entry), this.readerSettings);
            return (GraphMeta) reader.Deserialize(typeof(GraphMeta));
        }

        private void DeserializeNodeLinks(GraphNode[] int2Node)
        {
            ZipEntry entry = this.zip["node_link2.binary"];
            if (entry != null)
            {
                GraphSerializationContext ctx = new GraphSerializationContext(GetBinaryReader(entry), int2Node, 0, this.meta);
                NodeLink2.DeserializeReferences(ctx);
            }
        }

        private GraphNode[] DeserializeNodeReferenceMap()
        {
            <DeserializeNodeReferenceMap>c__AnonStorey24C storeyc = new <DeserializeNodeReferenceMap>c__AnonStorey24C();
            ZipEntry entry = this.zip["graph_references.binary"];
            if (entry == null)
            {
                throw new Exception("Node references not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
            }
            storeyc.reader = GetBinaryReader(entry);
            int num = storeyc.reader.ReadInt32();
            storeyc.int2Node = new GraphNode[num + 1];
            try
            {
                for (int i = 0; i < this.graphs.Length; i++)
                {
                    this.graphs[i].GetNodes(new GraphNodeDelegateCancelable(storeyc.<>m__1B));
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Some graph(s) has thrown an exception during GetNodes, or some graph(s) have deserialized more or fewer nodes than were serialized", exception);
            }
            if (storeyc.reader.BaseStream.Position != storeyc.reader.BaseStream.Length)
            {
                object[] objArray1 = new object[] { storeyc.reader.BaseStream.Length / 4L, " nodes were serialized, but only data for ", storeyc.reader.BaseStream.Position / 4L, " nodes was found. The data looks corrupt." };
                throw new Exception(string.Concat(objArray1));
            }
            storeyc.reader.Close();
            return storeyc.int2Node;
        }

        private void DeserializeNodeReferences(NavGraph graph, GraphNode[] int2Node)
        {
            <DeserializeNodeReferences>c__AnonStorey24D storeyd = new <DeserializeNodeReferences>c__AnonStorey24D();
            int num = this.graphIndexInZip[graph];
            ZipEntry entry = this.zip["graph" + num + "_references.binary"];
            if (entry == null)
            {
                throw new Exception("Node references for graph " + num + " not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
            }
            BinaryReader binaryReader = GetBinaryReader(entry);
            storeyd.ctx = new GraphSerializationContext(binaryReader, int2Node, graph.graphIndex, this.meta);
            graph.GetNodes(new GraphNodeDelegateCancelable(storeyd.<>m__1C));
        }

        private static Version FullyDefinedVersion(Version v)
        {
            return new Version(Mathf.Max(v.Major, 0), Mathf.Max(v.Minor, 0), Mathf.Max(v.Build, 0), Mathf.Max(v.Revision, 0));
        }

        private static BinaryReader GetBinaryReader(ZipEntry entry)
        {
            MemoryStream stream = new MemoryStream();
            entry.Extract(stream);
            stream.Position = 0L;
            return new BinaryReader(stream);
        }

        public uint GetChecksum()
        {
            return this.checksum;
        }

        private static int GetMaxNodeIndexInAllGraphs(NavGraph[] graphs)
        {
            <GetMaxNodeIndexInAllGraphs>c__AnonStorey248 storey = new <GetMaxNodeIndexInAllGraphs>c__AnonStorey248();
            storey.maxIndex = 0;
            for (int i = 0; i < graphs.Length; i++)
            {
                if (graphs[i] != null)
                {
                    graphs[i].GetNodes(new GraphNodeDelegateCancelable(storey.<>m__17));
                }
            }
            return storey.maxIndex;
        }

        private static string GetString(ZipEntry entry)
        {
            MemoryStream stream = new MemoryStream();
            entry.Extract(stream);
            stream.Position = 0L;
            StreamReader reader = new StreamReader(stream);
            string str = reader.ReadToEnd();
            stream.Position = 0L;
            reader.Dispose();
            return str;
        }

        private static StringBuilder GetStringBuilder()
        {
            _stringBuilder.Length = 0;
            return _stringBuilder;
        }

        public static byte[] LoadFromFile(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                byte[] array = new byte[(int) stream.Length];
                stream.Read(array, 0, (int) stream.Length);
                return array;
            }
        }

        public bool OpenDeserialize(byte[] bytes)
        {
            this.readerSettings = new JsonReaderSettings();
            this.readerSettings.AddTypeConverter(new VectorConverter());
            this.readerSettings.AddTypeConverter(new BoundsConverter());
            this.readerSettings.AddTypeConverter(new LayerMaskConverter());
            this.readerSettings.AddTypeConverter(new MatrixConverter());
            this.readerSettings.AddTypeConverter(new GuidConverter());
            this.readerSettings.AddTypeConverter(new UnityObjectConverter());
            this.zipStream = new MemoryStream();
            this.zipStream.Write(bytes, 0, bytes.Length);
            this.zipStream.Position = 0L;
            try
            {
                this.zip = ZipFile.Read(this.zipStream);
            }
            catch (Exception exception)
            {
                Debug.LogError("Caught exception when loading from zip\n" + exception);
                this.zipStream.Dispose();
                return false;
            }
            this.meta = this.DeserializeMeta(this.zip["meta.json"]);
            if (FullyDefinedVersion(this.meta.version) > FullyDefinedVersion(AstarPath.Version))
            {
                Debug.LogWarning(string.Concat(new object[] { "Trying to load data from a newer version of the A* Pathfinding Project\nCurrent version: ", AstarPath.Version, " Data version: ", this.meta.version, "\nThis is usually fine as the stored data is usually backwards and forwards compatible.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n" }));
            }
            else if (FullyDefinedVersion(this.meta.version) < FullyDefinedVersion(AstarPath.Version))
            {
                Debug.LogWarning(string.Concat(new object[] { "Trying to load data from an older version of the A* Pathfinding Project\nCurrent version: ", AstarPath.Version, " Data version: ", this.meta.version, "\nThis is usually fine, it just means you have upgraded to a new version.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n" }));
            }
            return true;
        }

        public void OpenSerialize()
        {
            this.zip = new ZipFile();
            this.zip.AlternateEncoding = Encoding.UTF8;
            this.zip.AlternateEncodingUsage = ZipOption.Always;
            this.writerSettings = new JsonWriterSettings();
            this.writerSettings.AddTypeConverter(new VectorConverter());
            this.writerSettings.AddTypeConverter(new BoundsConverter());
            this.writerSettings.AddTypeConverter(new LayerMaskConverter());
            this.writerSettings.AddTypeConverter(new MatrixConverter());
            this.writerSettings.AddTypeConverter(new GuidConverter());
            this.writerSettings.AddTypeConverter(new UnityObjectConverter());
            this.writerSettings.PrettyPrint = this.settings.prettyPrint;
            this.meta = new GraphMeta();
        }

        public void PostDeserialization()
        {
            for (int i = 0; i < this.graphs.Length; i++)
            {
                this.graphs[i].PostDeserialization();
            }
        }

        public static void SaveToFile(string path, byte[] data)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                stream.Write(data, 0, data.Length);
            }
        }

        public byte[] Serialize(NavGraph graph)
        {
            StringBuilder stringBuilder = GetStringBuilder();
            new JsonWriter(stringBuilder, this.writerSettings).Write(graph);
            return this.encoding.GetBytes(stringBuilder.ToString());
        }

        public void SerializeEditorSettings(GraphEditorBase[] editors)
        {
            if ((editors != null) && this.settings.editorSettings)
            {
                for (int i = 0; i < editors.Length; i++)
                {
                    if (editors[i] == null)
                    {
                        return;
                    }
                    StringBuilder stringBuilder = GetStringBuilder();
                    new JsonWriter(stringBuilder, this.writerSettings).Write(editors[i]);
                    byte[] bytes = this.encoding.GetBytes(stringBuilder.ToString());
                    if (bytes.Length > 2)
                    {
                        this.AddChecksum(bytes);
                        this.zip.AddEntry("graph" + i + "_editor.json", bytes);
                    }
                }
            }
        }

        public void SerializeExtraInfo()
        {
            if (this.settings.nodes)
            {
                if (this.graphs == null)
                {
                    throw new InvalidOperationException("Cannot serialize extra info with no serialized graphs (call SerializeGraphs first)");
                }
                byte[] bytes = SerializeNodeIndices(this.graphs);
                this.AddChecksum(bytes);
                this.zip.AddEntry("graph_references.binary", bytes);
                for (int i = 0; i < this.graphs.Length; i++)
                {
                    if (this.graphs[i] != null)
                    {
                        bytes = SerializeGraphExtraInfo(this.graphs[i]);
                        this.AddChecksum(bytes);
                        this.zip.AddEntry("graph" + i + "_extra.binary", bytes);
                        bytes = SerializeGraphNodeReferences(this.graphs[i]);
                        this.AddChecksum(bytes);
                        this.zip.AddEntry("graph" + i + "_references.binary", bytes);
                    }
                }
                bytes = this.SerializeNodeLinks();
                this.AddChecksum(bytes);
                this.zip.AddEntry("node_link2.binary", bytes);
            }
        }

        private static byte[] SerializeGraphExtraInfo(NavGraph graph)
        {
            MemoryStream output = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(output);
            GraphSerializationContext ctx = new GraphSerializationContext(writer);
            graph.SerializeExtraInfo(ctx);
            byte[] buffer = output.ToArray();
            writer.Close();
            return buffer;
        }

        private static byte[] SerializeGraphNodeReferences(NavGraph graph)
        {
            <SerializeGraphNodeReferences>c__AnonStorey24A storeya = new <SerializeGraphNodeReferences>c__AnonStorey24A();
            MemoryStream output = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(output);
            storeya.ctx = new GraphSerializationContext(writer);
            graph.GetNodes(new GraphNodeDelegateCancelable(storeya.<>m__19));
            writer.Close();
            return output.ToArray();
        }

        public void SerializeGraphs(NavGraph[] _graphs)
        {
            if (this.graphs != null)
            {
                throw new InvalidOperationException("Cannot serialize graphs multiple times.");
            }
            this.graphs = _graphs;
            if (this.zip == null)
            {
                throw new NullReferenceException("You must not call CloseSerialize before a call to this function");
            }
            if (this.graphs == null)
            {
                this.graphs = new NavGraph[0];
            }
            for (int i = 0; i < this.graphs.Length; i++)
            {
                if (this.graphs[i] != null)
                {
                    byte[] bytes = this.Serialize(this.graphs[i]);
                    this.AddChecksum(bytes);
                    this.zip.AddEntry("graph" + i + ".json", bytes);
                }
            }
        }

        private byte[] SerializeMeta()
        {
            if (this.graphs == null)
            {
                throw new Exception("No call to SerializeGraphs has been done");
            }
            this.meta.version = AstarPath.Version;
            this.meta.graphs = this.graphs.Length;
            this.meta.guids = new string[this.graphs.Length];
            this.meta.typeNames = new string[this.graphs.Length];
            this.meta.nodeCounts = new int[this.graphs.Length];
            for (int i = 0; i < this.graphs.Length; i++)
            {
                if (this.graphs[i] != null)
                {
                    this.meta.guids[i] = this.graphs[i].guid.ToString();
                    this.meta.typeNames[i] = this.graphs[i].GetType().FullName;
                }
            }
            StringBuilder stringBuilder = GetStringBuilder();
            new JsonWriter(stringBuilder, this.writerSettings).Write(this.meta);
            return this.encoding.GetBytes(stringBuilder.ToString());
        }

        private static byte[] SerializeNodeIndices(NavGraph[] graphs)
        {
            <SerializeNodeIndices>c__AnonStorey249 storey = new <SerializeNodeIndices>c__AnonStorey249();
            MemoryStream output = new MemoryStream();
            storey.wr = new BinaryWriter(output);
            int maxNodeIndexInAllGraphs = GetMaxNodeIndexInAllGraphs(graphs);
            storey.wr.Write(maxNodeIndexInAllGraphs);
            storey.maxNodeIndex2 = 0;
            for (int i = 0; i < graphs.Length; i++)
            {
                if (graphs[i] != null)
                {
                    graphs[i].GetNodes(new GraphNodeDelegateCancelable(storey.<>m__18));
                }
            }
            if (storey.maxNodeIndex2 != maxNodeIndexInAllGraphs)
            {
                throw new Exception("Some graphs are not consistent in their GetNodes calls, sequential calls give different results.");
            }
            byte[] buffer = output.ToArray();
            storey.wr.Close();
            return buffer;
        }

        private byte[] SerializeNodeLinks()
        {
            MemoryStream output = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(output);
            GraphSerializationContext ctx = new GraphSerializationContext(writer);
            NodeLink2.SerializeReferences(ctx);
            return output.ToArray();
        }

        [Obsolete("Not used anymore. You can safely remove the call to this function.")]
        public void SerializeNodes()
        {
        }

        public void SetGraphIndexOffset(int offset)
        {
            this.graphIndexOffset = offset;
        }

        [CompilerGenerated]
        private sealed class <AnyDestroyedNodesInGraphs>c__AnonStorey24B
        {
            internal bool result;

            internal bool <>m__1A(GraphNode node)
            {
                if (node.Destroyed)
                {
                    this.result = true;
                }
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <DeserializeNodeReferenceMap>c__AnonStorey24C
        {
            internal GraphNode[] int2Node;
            internal BinaryReader reader;

            internal bool <>m__1B(GraphNode node)
            {
                int index = this.reader.ReadInt32();
                this.int2Node[index] = node;
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <DeserializeNodeReferences>c__AnonStorey24D
        {
            internal GraphSerializationContext ctx;

            internal bool <>m__1C(GraphNode node)
            {
                node.DeserializeReferences(this.ctx);
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <GetMaxNodeIndexInAllGraphs>c__AnonStorey248
        {
            internal int maxIndex;

            internal bool <>m__17(GraphNode node)
            {
                this.maxIndex = Math.Max(node.NodeIndex, this.maxIndex);
                if (node.NodeIndex == -1)
                {
                    Debug.LogError("Graph contains destroyed nodes. This is a bug.");
                }
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <SerializeGraphNodeReferences>c__AnonStorey24A
        {
            internal GraphSerializationContext ctx;

            internal bool <>m__19(GraphNode node)
            {
                node.SerializeReferences(this.ctx);
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <SerializeNodeIndices>c__AnonStorey249
        {
            internal int maxNodeIndex2;
            internal BinaryWriter wr;

            internal bool <>m__18(GraphNode node)
            {
                this.maxNodeIndex2 = Math.Max(node.NodeIndex, this.maxNodeIndex2);
                this.wr.Write(node.NodeIndex);
                return true;
            }
        }
    }
}

