namespace Pathfinding
{
    using Pathfinding.Serialization;
    using Pathfinding.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.Serialization;

    [Serializable]
    public class AstarData
    {
        [CompilerGenerated]
        private System.Type[] <graphTypes>k__BackingField;
        [CompilerGenerated]
        private GridGraph <gridGraph>k__BackingField;
        [CompilerGenerated]
        private LayerGridGraph <layerGridGraph>k__BackingField;
        [CompilerGenerated]
        private NavMeshGraph <navmesh>k__BackingField;
        [CompilerGenerated]
        private PointGraph <pointGraph>k__BackingField;
        [CompilerGenerated]
        private RecastGraph <recastGraph>k__BackingField;
        [SerializeField]
        public bool cacheStartup;
        public byte[] data_backup;
        public byte[] data_cachedStartup;
        [SerializeField]
        private string dataString;
        public static readonly System.Type[] DefaultGraphTypes = new System.Type[] { typeof(GridGraph), typeof(PointGraph), typeof(NavMeshGraph), typeof(RecastGraph), typeof(LayerGridGraph) };
        public TextAsset file_cachedStartup;
        [NonSerialized]
        public NavGraph[] graphs = new NavGraph[0];
        [FormerlySerializedAs("data"), SerializeField]
        private byte[] upgradeData;

        public void AddGraph(NavGraph graph)
        {
            AstarPath.active.BlockUntilPathQueueBlocked();
            for (int i = 0; i < this.graphs.Length; i++)
            {
                if (this.graphs[i] == null)
                {
                    this.graphs[i] = graph;
                    graph.active = active;
                    graph.Awake();
                    graph.graphIndex = (uint) i;
                    this.UpdateShortcuts();
                    return;
                }
            }
            if ((this.graphs != null) && (this.graphs.Length >= 0xffL))
            {
                throw new Exception("Graph Count Limit Reached. You cannot have more than " + 0xff + " graphs.");
            }
            List<NavGraph> list = new List<NavGraph>(this.graphs);
            list.Add(graph);
            this.graphs = list.ToArray();
            this.UpdateShortcuts();
            graph.active = active;
            graph.Awake();
            graph.graphIndex = (uint) (this.graphs.Length - 1);
        }

        [Obsolete("Use AddGraph(System.Type) instead")]
        public NavGraph AddGraph(string type)
        {
            NavGraph graph = null;
            for (int i = 0; i < this.graphTypes.Length; i++)
            {
                if (this.graphTypes[i].Name == type)
                {
                    graph = this.CreateGraph(this.graphTypes[i]);
                }
            }
            if (graph == null)
            {
                UnityEngine.Debug.LogError("No NavGraph of type '" + type + "' could be found");
                return null;
            }
            this.AddGraph(graph);
            return graph;
        }

        public NavGraph AddGraph(System.Type type)
        {
            NavGraph graph = null;
            for (int i = 0; i < this.graphTypes.Length; i++)
            {
                if (object.Equals(this.graphTypes[i], type))
                {
                    graph = this.CreateGraph(this.graphTypes[i]);
                }
            }
            if (graph == null)
            {
                UnityEngine.Debug.LogError(string.Concat(new object[] { "No NavGraph of type '", type, "' could be found, ", this.graphTypes.Length, " graph types are avaliable" }));
                return null;
            }
            this.AddGraph(graph);
            return graph;
        }

        public void Awake()
        {
            this.graphs = new NavGraph[0];
            if (this.cacheStartup && (this.file_cachedStartup != null))
            {
                this.LoadFromCache();
            }
            else
            {
                this.DeserializeGraphs();
            }
        }

        private void ClearGraphs()
        {
            if (this.graphs != null)
            {
                for (int i = 0; i < this.graphs.Length; i++)
                {
                    if (this.graphs[i] != null)
                    {
                        this.graphs[i].OnDestroy();
                    }
                }
                this.graphs = null;
                this.UpdateShortcuts();
            }
        }

        [Obsolete("Use CreateGraph(System.Type) instead")]
        public NavGraph CreateGraph(string type)
        {
            UnityEngine.Debug.Log("Creating Graph of type '" + type + "'");
            for (int i = 0; i < this.graphTypes.Length; i++)
            {
                if (this.graphTypes[i].Name == type)
                {
                    return this.CreateGraph(this.graphTypes[i]);
                }
            }
            UnityEngine.Debug.LogError("Graph type (" + type + ") wasn't found");
            return null;
        }

        public NavGraph CreateGraph(System.Type type)
        {
            NavGraph graph = Activator.CreateInstance(type) as NavGraph;
            graph.active = active;
            return graph;
        }

        public void DeserializeGraphs()
        {
            if (this.data != null)
            {
                this.DeserializeGraphs(this.data);
            }
        }

        public void DeserializeGraphs(byte[] bytes)
        {
            AstarPath.active.BlockUntilPathQueueBlocked();
            this.ClearGraphs();
            this.DeserializeGraphsAdditive(bytes);
        }

        public void DeserializeGraphsAdditive(byte[] bytes)
        {
            AstarPath.active.BlockUntilPathQueueBlocked();
            try
            {
                if (bytes == null)
                {
                    throw new ArgumentNullException("bytes");
                }
                AstarSerializer sr = new AstarSerializer(this);
                if (sr.OpenDeserialize(bytes))
                {
                    this.DeserializeGraphsPartAdditive(sr);
                    sr.CloseDeserialize();
                    this.UpdateShortcuts();
                }
                else
                {
                    UnityEngine.Debug.Log("Invalid data file (cannot read zip).\nThe data is either corrupt or it was saved using a 3.0.x or earlier version of the system");
                }
                active.VerifyIntegrity();
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogError("Caught exception while deserializing data.\n" + exception);
                this.graphs = new NavGraph[0];
                this.data_backup = bytes;
            }
        }

        public void DeserializeGraphsPart(AstarSerializer sr)
        {
            this.ClearGraphs();
            this.DeserializeGraphsPartAdditive(sr);
        }

        public void DeserializeGraphsPartAdditive(AstarSerializer sr)
        {
            if (this.graphs == null)
            {
                this.graphs = new NavGraph[0];
            }
            List<NavGraph> list = new List<NavGraph>(this.graphs);
            sr.SetGraphIndexOffset(list.Count);
            list.AddRange(sr.DeserializeGraphs());
            this.graphs = list.ToArray();
            sr.DeserializeExtraInfo();
            <DeserializeGraphsPartAdditive>c__AnonStorey240 storey = new <DeserializeGraphsPartAdditive>c__AnonStorey240();
            storey.i = 0;
            while (storey.i < this.graphs.Length)
            {
                if (this.graphs[storey.i] != null)
                {
                    this.graphs[storey.i].GetNodes(new GraphNodeDelegateCancelable(storey.<>m__3));
                }
                storey.i++;
            }
            for (int i = 0; i < this.graphs.Length; i++)
            {
                for (int j = i + 1; j < this.graphs.Length; j++)
                {
                    if (((this.graphs[i] != null) && (this.graphs[j] != null)) && (this.graphs[i].guid == this.graphs[j].guid))
                    {
                        UnityEngine.Debug.LogWarning("Guid Conflict when importing graphs additively. Imported graph will get a new Guid.\nThis message is (relatively) harmless.");
                        this.graphs[i].guid = Pathfinding.Util.Guid.NewGuid();
                        break;
                    }
                }
            }
            sr.PostDeserialization();
        }

        public NavGraph FindGraphOfType(System.Type type)
        {
            if (this.graphs != null)
            {
                for (int i = 0; i < this.graphs.Length; i++)
                {
                    if ((this.graphs[i] != null) && object.Equals(this.graphs[i].GetType(), type))
                    {
                        return this.graphs[i];
                    }
                }
            }
            return null;
        }

        [DebuggerHidden]
        public IEnumerable FindGraphsOfType(System.Type type)
        {
            <FindGraphsOfType>c__Iterator4 iterator = new <FindGraphsOfType>c__Iterator4();
            iterator.type = type;
            iterator.<$>type = type;
            iterator.<>f__this = this;
            iterator.$PC = -2;
            return iterator;
        }

        public void FindGraphTypes()
        {
            this.graphTypes = DefaultGraphTypes;
        }

        public byte[] GetData()
        {
            return this.data;
        }

        public static NavGraph GetGraph(GraphNode node)
        {
            if (node == null)
            {
                return null;
            }
            AstarPath active = AstarPath.active;
            if (active == null)
            {
                return null;
            }
            AstarData astarData = active.astarData;
            if ((astarData == null) || (astarData.graphs == null))
            {
                return null;
            }
            uint graphIndex = node.GraphIndex;
            if (graphIndex >= astarData.graphs.Length)
            {
                return null;
            }
            return astarData.graphs[graphIndex];
        }

        public int GetGraphIndex(NavGraph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
            int index = -1;
            if (this.graphs != null)
            {
                index = Array.IndexOf<NavGraph>(this.graphs, graph);
                if (index == -1)
                {
                    UnityEngine.Debug.LogError("Graph doesn't exist");
                }
            }
            return index;
        }

        [Obsolete("If really necessary. Use System.Type.GetType instead.")]
        public System.Type GetGraphType(string type)
        {
            for (int i = 0; i < this.graphTypes.Length; i++)
            {
                if (this.graphTypes[i].Name == type)
                {
                    return this.graphTypes[i];
                }
            }
            return null;
        }

        [Obsolete("Obsolete because it is not used by the package internally and the use cases are few. Iterate through the graphs array instead."), DebuggerHidden]
        public IEnumerable GetRaycastableGraphs()
        {
            <GetRaycastableGraphs>c__Iterator6 iterator = new <GetRaycastableGraphs>c__Iterator6();
            iterator.<>f__this = this;
            iterator.$PC = -2;
            return iterator;
        }

        [DebuggerHidden]
        public IEnumerable GetUpdateableGraphs()
        {
            <GetUpdateableGraphs>c__Iterator5 iterator = new <GetUpdateableGraphs>c__Iterator5();
            iterator.<>f__this = this;
            iterator.$PC = -2;
            return iterator;
        }

        public void LoadFromCache()
        {
            AstarPath.active.BlockUntilPathQueueBlocked();
            if (this.file_cachedStartup != null)
            {
                byte[] bytes = this.file_cachedStartup.bytes;
                this.DeserializeGraphs(bytes);
                GraphModifier.TriggerEvent(GraphModifier.EventType.PostCacheLoad);
            }
            else
            {
                UnityEngine.Debug.LogError("Can't load from cache since the cache is empty");
            }
        }

        public void OnDestroy()
        {
            this.ClearGraphs();
        }

        public bool RemoveGraph(NavGraph graph)
        {
            active.FlushWorkItems(false, true);
            active.BlockUntilPathQueueBlocked();
            graph.OnDestroy();
            int index = Array.IndexOf<NavGraph>(this.graphs, graph);
            if (index == -1)
            {
                return false;
            }
            this.graphs[index] = null;
            this.UpdateShortcuts();
            return true;
        }

        public byte[] SerializeGraphs()
        {
            return this.SerializeGraphs(SerializeSettings.Settings);
        }

        public byte[] SerializeGraphs(SerializeSettings settings)
        {
            uint num;
            return this.SerializeGraphs(settings, out num);
        }

        public byte[] SerializeGraphs(SerializeSettings settings, out uint checksum)
        {
            AstarPath.active.BlockUntilPathQueueBlocked();
            AstarSerializer sr = new AstarSerializer(this, settings);
            sr.OpenSerialize();
            this.SerializeGraphsPart(sr);
            byte[] buffer = sr.CloseSerialize();
            checksum = sr.GetChecksum();
            return buffer;
        }

        public void SerializeGraphsPart(AstarSerializer sr)
        {
            sr.SerializeGraphs(this.graphs);
            sr.SerializeExtraInfo();
        }

        public void SetData(byte[] data)
        {
            this.data = data;
        }

        public void UpdateShortcuts()
        {
            this.navmesh = (NavMeshGraph) this.FindGraphOfType(typeof(NavMeshGraph));
            this.gridGraph = (GridGraph) this.FindGraphOfType(typeof(GridGraph));
            this.layerGridGraph = (LayerGridGraph) this.FindGraphOfType(typeof(LayerGridGraph));
            this.pointGraph = (PointGraph) this.FindGraphOfType(typeof(PointGraph));
            this.recastGraph = (RecastGraph) this.FindGraphOfType(typeof(RecastGraph));
        }

        public static AstarPath active
        {
            get
            {
                return AstarPath.active;
            }
        }

        private byte[] data
        {
            get
            {
                if ((this.upgradeData != null) && (this.upgradeData.Length > 0))
                {
                    this.data = this.upgradeData;
                    this.upgradeData = null;
                }
                return ((this.dataString == null) ? null : Convert.FromBase64String(this.dataString));
            }
            set
            {
                this.dataString = (value == null) ? null : Convert.ToBase64String(value);
            }
        }

        public System.Type[] graphTypes
        {
            [CompilerGenerated]
            get
            {
                return this.<graphTypes>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<graphTypes>k__BackingField = value;
            }
        }

        public GridGraph gridGraph
        {
            [CompilerGenerated]
            get
            {
                return this.<gridGraph>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<gridGraph>k__BackingField = value;
            }
        }

        public LayerGridGraph layerGridGraph
        {
            [CompilerGenerated]
            get
            {
                return this.<layerGridGraph>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<layerGridGraph>k__BackingField = value;
            }
        }

        public NavMeshGraph navmesh
        {
            [CompilerGenerated]
            get
            {
                return this.<navmesh>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<navmesh>k__BackingField = value;
            }
        }

        public PointGraph pointGraph
        {
            [CompilerGenerated]
            get
            {
                return this.<pointGraph>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<pointGraph>k__BackingField = value;
            }
        }

        public RecastGraph recastGraph
        {
            [CompilerGenerated]
            get
            {
                return this.<recastGraph>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<recastGraph>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <DeserializeGraphsPartAdditive>c__AnonStorey240
        {
            internal int i;

            internal bool <>m__3(GraphNode node)
            {
                node.GraphIndex = (uint) this.i;
                return true;
            }
        }

        [CompilerGenerated]
        private sealed class <FindGraphsOfType>c__Iterator4 : IEnumerator, IDisposable, IEnumerable, IEnumerable<object>, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal System.Type <$>type;
            internal AstarData <>f__this;
            internal int <i>__0;
            internal System.Type type;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (this.<>f__this.graphs != null)
                        {
                            this.<i>__0 = 0;
                            while (this.<i>__0 < this.<>f__this.graphs.Length)
                            {
                                if ((this.<>f__this.graphs[this.<i>__0] != null) && object.Equals(this.<>f__this.graphs[this.<i>__0].GetType(), this.type))
                                {
                                    this.$current = this.<>f__this.graphs[this.<i>__0];
                                    this.$PC = 1;
                                    return true;
                                }
                            Label_00A4:
                                this.<i>__0++;
                            }
                            this.$PC = -1;
                            break;
                        }
                        break;

                    case 1:
                        goto Label_00A4;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<object> IEnumerable<object>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                AstarData.<FindGraphsOfType>c__Iterator4 iterator = new AstarData.<FindGraphsOfType>c__Iterator4();
                iterator.<>f__this = this.<>f__this;
                iterator.type = this.<$>type;
                return iterator;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetRaycastableGraphs>c__Iterator6 : IEnumerator, IDisposable, IEnumerable, IEnumerable<object>, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AstarData <>f__this;
            internal int <i>__0;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (this.<>f__this.graphs != null)
                        {
                            this.<i>__0 = 0;
                            while (this.<i>__0 < this.<>f__this.graphs.Length)
                            {
                                if (this.<>f__this.graphs[this.<i>__0] is IRaycastableGraph)
                                {
                                    this.$current = this.<>f__this.graphs[this.<i>__0];
                                    this.$PC = 1;
                                    return true;
                                }
                            Label_0082:
                                this.<i>__0++;
                            }
                            this.$PC = -1;
                            break;
                        }
                        break;

                    case 1:
                        goto Label_0082;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<object> IEnumerable<object>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                AstarData.<GetRaycastableGraphs>c__Iterator6 iterator = new AstarData.<GetRaycastableGraphs>c__Iterator6();
                iterator.<>f__this = this.<>f__this;
                return iterator;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetUpdateableGraphs>c__Iterator5 : IEnumerator, IDisposable, IEnumerable, IEnumerable<object>, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AstarData <>f__this;
            internal int <i>__0;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (this.<>f__this.graphs != null)
                        {
                            this.<i>__0 = 0;
                            while (this.<i>__0 < this.<>f__this.graphs.Length)
                            {
                                if (this.<>f__this.graphs[this.<i>__0] is IUpdatableGraph)
                                {
                                    this.$current = this.<>f__this.graphs[this.<i>__0];
                                    this.$PC = 1;
                                    return true;
                                }
                            Label_0082:
                                this.<i>__0++;
                            }
                            this.$PC = -1;
                            break;
                        }
                        break;

                    case 1:
                        goto Label_0082;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<object> IEnumerable<object>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                AstarData.<GetUpdateableGraphs>c__Iterator5 iterator = new AstarData.<GetUpdateableGraphs>c__Iterator5();
                iterator.<>f__this = this.<>f__this;
                return iterator;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

