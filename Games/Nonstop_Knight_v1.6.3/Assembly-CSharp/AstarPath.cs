using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

[HelpURL("http://arongranberg.com/astar/docs/class_astar_path.php"), ExecuteInEditMode, AddComponentMenu("Pathfinding/Pathfinder")]
public class AstarPath : MonoBehaviour
{
    [CompilerGenerated]
    private static Action<Path> <>f__am$cache3B;
    [CompilerGenerated]
    private bool <isScanning>k__BackingField;
    [CompilerGenerated]
    private float <lastScanTime>k__BackingField;
    public static AstarPath active;
    public AstarData astarData;
    public bool batchGraphUpdates;
    public static readonly string Branch = "HEAD_Pro";
    public AstarColor colorSettings;
    public float debugFloor;
    public GraphDebugMode debugMode;
    [NonSerialized]
    public Path debugPath;
    public float debugRoof = 20000f;
    public static readonly AstarDistribution Distribution = AstarDistribution.WebsiteDownload;
    public EuclideanEmbedding euclideanEmbedding = new EuclideanEmbedding();
    public bool fullGetNearestSearch;
    public float graphUpdateBatchingInterval = 0.2f;
    private bool graphUpdateRoutineRunning;
    private readonly GraphUpdateProcessor graphUpdates;
    private bool graphUpdatesWorkItemAdded;
    public Heuristic heuristic = Heuristic.Euclidean;
    public float heuristicScale = 1f;
    private string inGameDebugPath;
    private float lastGraphUpdate = -9999f;
    public PathLog logPathResults = PathLog.Normal;
    public bool manualDebugFloorRoof;
    public float maxFrameTime = 1f;
    public float maxNearestNodeDistance = 100f;
    [Obsolete("Minimum area size is mostly obsolete since the limit has been raised significantly, and the edge cases are handled automatically")]
    public int minAreaSize;
    private ushort nextFreePathID = 1;
    public static Action On65KOverflow;
    public static Action OnAwakeSettings;
    public Action OnDrawGizmosCallback;
    public static OnGraphDelegate OnGraphPostScan;
    public static OnGraphDelegate OnGraphPreScan;
    public static OnScanDelegate OnGraphsUpdated;
    [Obsolete]
    public Action OnGraphsWillBeUpdated;
    [Obsolete]
    public Action OnGraphsWillBeUpdated2;
    public static OnScanDelegate OnLatePostScan;
    public static OnPathDelegate OnPathPostSearch;
    public static OnPathDelegate OnPathPreSearch;
    public static OnScanDelegate OnPostScan;
    public static OnScanDelegate OnPreScan;
    private static Action OnThreadSafeCallback;
    public Action OnUnloadGizmoMeshes;
    private PathProcessor pathProcessor;
    private readonly PathReturnQueue pathReturnQueue;
    public bool prioritizeGraphs;
    public float prioritizeGraphsLimit = 1f;
    private static readonly object safeUpdateLock = new object();
    public bool scanOnStartup = true;
    public bool showGraphs;
    public bool showNavGraphs = true;
    public bool showSearchTree;
    public bool showUnwalkableNodes = true;
    [SerializeField]
    protected string[] tagNames;
    public ThreadCount threadCount;
    public float unwalkableNodeDebugSize = 0.3f;
    private static int waitForPathDepth = 0;
    private readonly WorkItemProcessor workItems;
    private bool workItemsQueued;

    private AstarPath()
    {
        this.pathProcessor = new PathProcessor(this, this.pathReturnQueue, 0, true);
        this.pathReturnQueue = new PathReturnQueue(this);
        this.workItems = new WorkItemProcessor(this);
        this.graphUpdates = new GraphUpdateProcessor(this);
        this.graphUpdates.OnGraphsUpdated += new Action(this.<AstarPath>m__4);
    }

    [CompilerGenerated]
    private void <AstarPath>m__4()
    {
        if (OnGraphsUpdated != null)
        {
            OnGraphsUpdated(this);
        }
    }

    public void AddWorkItem(AstarWorkItem itm)
    {
        this.workItems.AddWorkItem(itm);
        if (!this.workItemsQueued)
        {
            this.workItemsQueued = true;
            if (!this.isScanning)
            {
                this.InterruptPathfinding();
            }
        }
    }

    private void Awake()
    {
        active = this;
        if (UnityEngine.Object.FindObjectsOfType(typeof(AstarPath)).Length > 1)
        {
            UnityEngine.Debug.LogError("You should NOT have more than one AstarPath component in the scene at any time.\nThis can cause serious errors since the AstarPath component builds around a singleton pattern.");
        }
        base.useGUILayout = false;
        if (Application.isPlaying)
        {
            if (OnAwakeSettings != null)
            {
                OnAwakeSettings();
            }
            GraphModifier.FindAllModifiers();
            RelevantGraphSurface.FindAllGraphSurfaces();
            this.InitializePathProcessor();
            this.InitializeProfiler();
            this.SetUpReferences();
            this.InitializeAstarData();
            this.FlushWorkItems(true, false);
            this.euclideanEmbedding.dirty = true;
            if (this.scanOnStartup && (!this.astarData.cacheStartup || (this.astarData.file_cachedStartup == null)))
            {
                this.Scan();
            }
        }
    }

    public void BlockUntilPathQueueBlocked()
    {
        this.pathProcessor.BlockUntilPathQueueBlocked();
    }

    public static int CalculateThreadCount(ThreadCount count)
    {
        if ((count != ThreadCount.AutomaticLowLoad) && (count != ThreadCount.AutomaticHighLoad))
        {
            return (int) count;
        }
        int num = Mathf.Max(1, SystemInfo.processorCount);
        int systemMemorySize = SystemInfo.systemMemorySize;
        if (systemMemorySize <= 0)
        {
            UnityEngine.Debug.LogError("Machine reporting that is has <= 0 bytes of RAM. This is definitely not true, assuming 1 GiB");
            systemMemorySize = 0x400;
        }
        if (num <= 1)
        {
            return 0;
        }
        if (systemMemorySize <= 0x200)
        {
            return 0;
        }
        if (count == ThreadCount.AutomaticHighLoad)
        {
            if (systemMemorySize <= 0x400)
            {
                num = Math.Min(num, 2);
            }
            return num;
        }
        num /= 2;
        num = Mathf.Max(1, num);
        if (systemMemorySize <= 0x400)
        {
            num = Math.Min(num, 2);
        }
        return Math.Min(num, 6);
    }

    [DebuggerHidden]
    private IEnumerator DelayedGraphUpdate()
    {
        <DelayedGraphUpdate>c__Iterator7 iterator = new <DelayedGraphUpdate>c__Iterator7();
        iterator.<>f__this = this;
        return iterator;
    }

    internal void DestroyNode(GraphNode node)
    {
        this.pathProcessor.DestroyNode(node);
    }

    private bool DrawUnwalkableNode(GraphNode node)
    {
        if (!node.Walkable)
        {
            Gizmos.DrawCube((Vector3) node.position, (Vector3) (Vector3.one * this.unwalkableNodeDebugSize));
        }
        return true;
    }

    [Obsolete("This method has been moved. Use the method on the context object that can be sent with work item delegates instead")]
    public void EnsureValidFloodFill()
    {
        throw new Exception("This method has been moved. Use the method on the context object that can be sent with work item delegates instead");
    }

    public static string[] FindTagNames()
    {
        if (active != null)
        {
            return active.GetTagNames();
        }
        AstarPath path = UnityEngine.Object.FindObjectOfType<AstarPath>();
        if (path != null)
        {
            active = path;
            return path.GetTagNames();
        }
        return new string[] { "There is no AstarPath component in the scene" };
    }

    [ContextMenu("Flood Fill Graphs")]
    public void FloodFill()
    {
        this.graphUpdates.FloodFill();
        this.workItems.OnFloodFill();
    }

    public void FloodFill(GraphNode seed)
    {
        this.graphUpdates.FloodFill(seed);
    }

    public void FloodFill(GraphNode seed, uint area)
    {
        this.graphUpdates.FloodFill(seed, area);
    }

    public void FlushGraphUpdates()
    {
        if (this.IsAnyGraphUpdateQueued)
        {
            this.QueueGraphUpdates();
            this.FlushWorkItems(true, true);
        }
    }

    public void FlushThreadSafeCallbacks()
    {
        if (OnThreadSafeCallback != null)
        {
            this.BlockUntilPathQueueBlocked();
            this.PerformBlockingActions(false, true);
        }
    }

    public void FlushWorkItems([Optional, DefaultParameterValue(true)] bool unblockOnComplete, [Optional, DefaultParameterValue(false)] bool block)
    {
        this.BlockUntilPathQueueBlocked();
        this.PerformBlockingActions(block, unblockOnComplete);
    }

    public GraphNode GetNearest(Ray ray)
    {
        <GetNearest>c__AnonStorey242 storey = new <GetNearest>c__AnonStorey242();
        if (this.graphs == null)
        {
            return null;
        }
        storey.minDist = float.PositiveInfinity;
        storey.nearestNode = null;
        storey.lineDirection = ray.direction;
        storey.lineOrigin = ray.origin;
        for (int i = 0; i < this.graphs.Length; i++)
        {
            this.graphs[i].GetNodes(new GraphNodeDelegateCancelable(storey.<>m__9));
        }
        return storey.nearestNode;
    }

    public NNInfo GetNearest(Vector3 position)
    {
        return this.GetNearest(position, NNConstraint.None);
    }

    public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
    {
        return this.GetNearest(position, constraint, null);
    }

    public NNInfo GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
    {
        NavGraph[] graphs = this.graphs;
        float positiveInfinity = float.PositiveInfinity;
        NNInfoInternal internalInfo = new NNInfoInternal();
        int index = -1;
        if (graphs != null)
        {
            for (int i = 0; i < graphs.Length; i++)
            {
                NavGraph graph = graphs[i];
                if ((graph != null) && constraint.SuitableGraph(i, graph))
                {
                    NNInfoInternal nearestForce;
                    if (this.fullGetNearestSearch)
                    {
                        nearestForce = graph.GetNearestForce(position, constraint);
                    }
                    else
                    {
                        nearestForce = graph.GetNearest(position, constraint);
                    }
                    if (nearestForce.node != null)
                    {
                        Vector3 vector = nearestForce.clampedPosition - position;
                        float magnitude = vector.magnitude;
                        if (this.prioritizeGraphs && (magnitude < this.prioritizeGraphsLimit))
                        {
                            positiveInfinity = magnitude;
                            internalInfo = nearestForce;
                            index = i;
                            break;
                        }
                        if (magnitude < positiveInfinity)
                        {
                            positiveInfinity = magnitude;
                            internalInfo = nearestForce;
                            index = i;
                        }
                    }
                }
            }
        }
        if (index != -1)
        {
            if (internalInfo.constrainedNode != null)
            {
                internalInfo.node = internalInfo.constrainedNode;
                internalInfo.clampedPosition = internalInfo.constClampedPosition;
            }
            if ((!this.fullGetNearestSearch && (internalInfo.node != null)) && !constraint.Suitable(internalInfo.node))
            {
                NNInfoInternal internal4 = graphs[index].GetNearestForce(position, constraint);
                if (internal4.node != null)
                {
                    internalInfo = internal4;
                }
            }
            if (constraint.Suitable(internalInfo.node))
            {
                if (!constraint.constrainDistance)
                {
                    goto Label_01A7;
                }
                Vector3 vector2 = internalInfo.clampedPosition - position;
                if (vector2.sqrMagnitude <= this.maxNearestNodeDistanceSqr)
                {
                    goto Label_01A7;
                }
            }
        }
        return new NNInfo();
    Label_01A7:
        return new NNInfo(internalInfo);
    }

    internal int GetNewNodeIndex()
    {
        return this.pathProcessor.GetNewNodeIndex();
    }

    internal ushort GetNextPathID()
    {
        ushort num;
        if (this.nextFreePathID == 0)
        {
            this.nextFreePathID = (ushort) (this.nextFreePathID + 1);
            UnityEngine.Debug.Log("65K cleanup (this message is harmless, it just means you have searched a lot of paths)");
            if (On65KOverflow != null)
            {
                Action action = On65KOverflow;
                On65KOverflow = null;
                action();
            }
        }
        this.nextFreePathID = (ushort) ((num = this.nextFreePathID) + 1);
        return num;
    }

    public string[] GetTagNames()
    {
        if ((this.tagNames == null) || (this.tagNames.Length != 0x20))
        {
            this.tagNames = new string[0x20];
            for (int i = 0; i < this.tagNames.Length; i++)
            {
                this.tagNames[i] = string.Empty + i;
            }
            this.tagNames[0] = "Basic Ground";
        }
        return this.tagNames;
    }

    private void InitializeAstarData()
    {
        this.astarData.FindGraphTypes();
        this.astarData.Awake();
        this.astarData.UpdateShortcuts();
        for (int i = 0; i < this.astarData.graphs.Length; i++)
        {
            if (this.astarData.graphs[i] != null)
            {
                this.astarData.graphs[i].Awake();
            }
        }
    }

    internal void InitializeNode(GraphNode node)
    {
        this.pathProcessor.InitializeNode(node);
    }

    private void InitializePathProcessor()
    {
        int a = CalculateThreadCount(this.threadCount);
        int processors = Mathf.Max(a, 1);
        bool multithreaded = a > 0;
        this.pathProcessor = new PathProcessor(this, this.pathReturnQueue, processors, multithreaded);
        if (<>f__am$cache3B == null)
        {
            <>f__am$cache3B = delegate (Path path) {
                OnPathDelegate onPathPreSearch = OnPathPreSearch;
                if (onPathPreSearch != null)
                {
                    onPathPreSearch(path);
                }
            };
        }
        this.pathProcessor.OnPathPreSearch += <>f__am$cache3B;
        this.pathProcessor.OnPathPostSearch += delegate (Path path) {
            this.LogPathResults(path);
            OnPathDelegate onPathPostSearch = OnPathPostSearch;
            if (onPathPostSearch != null)
            {
                onPathPostSearch(path);
            }
        };
        if (multithreaded)
        {
            this.graphUpdates.EnableMultithreading();
        }
    }

    private void InitializeProfiler()
    {
    }

    private void InterruptPathfinding()
    {
        this.pathProcessor.queue.Block();
    }

    internal void Log(string s)
    {
        if (object.ReferenceEquals(active, null))
        {
            UnityEngine.Debug.Log("No AstarPath object was found : " + s);
        }
        else if ((active.logPathResults != PathLog.None) && (active.logPathResults != PathLog.OnlyErrors))
        {
            UnityEngine.Debug.Log(s);
        }
    }

    private void LogPathResults(Path p)
    {
        if ((this.logPathResults != PathLog.None) && ((this.logPathResults != PathLog.OnlyErrors) || p.error))
        {
            string message = p.DebugString(this.logPathResults);
            if (this.logPathResults == PathLog.InGame)
            {
                this.inGameDebugPath = message;
            }
            else
            {
                UnityEngine.Debug.Log(message);
            }
        }
    }

    private void OnApplicationQuit()
    {
        this.OnDestroy();
        this.pathProcessor.AbortThreads();
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            if (this.logPathResults == PathLog.Heavy)
            {
                UnityEngine.Debug.Log("+++ AstarPath Component Destroyed - Cleaning Up Pathfinding Data +++");
            }
            if (active == this)
            {
                this.BlockUntilPathQueueBlocked();
                this.euclideanEmbedding.dirty = false;
                this.FlushWorkItems(false, true);
                this.pathProcessor.queue.TerminateReceivers();
                if (this.logPathResults == PathLog.Heavy)
                {
                    UnityEngine.Debug.Log("Processing Possible Work Items");
                }
                this.graphUpdates.DisableMultithreading();
                this.pathProcessor.JoinThreads();
                if (this.logPathResults == PathLog.Heavy)
                {
                    UnityEngine.Debug.Log("Returning Paths");
                }
                this.pathReturnQueue.ReturnPaths(false);
                if (this.logPathResults == PathLog.Heavy)
                {
                    UnityEngine.Debug.Log("Destroying Graphs");
                }
                this.astarData.OnDestroy();
                if (this.logPathResults == PathLog.Heavy)
                {
                    UnityEngine.Debug.Log("Cleaning up variables");
                }
                this.OnDrawGizmosCallback = null;
                OnAwakeSettings = null;
                OnGraphPreScan = null;
                OnGraphPostScan = null;
                OnPathPreSearch = null;
                OnPathPostSearch = null;
                OnPreScan = null;
                OnPostScan = null;
                OnLatePostScan = null;
                On65KOverflow = null;
                OnGraphsUpdated = null;
                OnThreadSafeCallback = null;
                active = null;
            }
        }
    }

    private void OnDisable()
    {
        if (this.OnUnloadGizmoMeshes != null)
        {
            this.OnUnloadGizmoMeshes();
        }
    }

    private void OnDrawGizmos()
    {
        if (!this.isScanning)
        {
            if (active == null)
            {
                active = this;
            }
            else if (active != this)
            {
                return;
            }
            if ((this.graphs != null) && !this.workItems.workItemsInProgress)
            {
                if (this.showNavGraphs && !this.manualDebugFloorRoof)
                {
                    this.RecalculateDebugLimits();
                }
                for (int i = 0; i < this.graphs.Length; i++)
                {
                    if ((this.graphs[i] != null) && this.graphs[i].drawGizmos)
                    {
                        this.graphs[i].OnDrawGizmos(this.showNavGraphs);
                    }
                }
                if (this.showNavGraphs)
                {
                    this.euclideanEmbedding.OnDrawGizmos();
                    if (this.showUnwalkableNodes)
                    {
                        Gizmos.color = AstarColor.UnwalkableNode;
                        GraphNodeDelegateCancelable del = new GraphNodeDelegateCancelable(this.DrawUnwalkableNode);
                        for (int j = 0; j < this.graphs.Length; j++)
                        {
                            if ((this.graphs[j] != null) && this.graphs[j].drawGizmos)
                            {
                                this.graphs[j].GetNodes(del);
                            }
                        }
                    }
                }
                if (this.OnDrawGizmosCallback != null)
                {
                    this.OnDrawGizmosCallback();
                }
            }
        }
    }

    private void OnGUI()
    {
        if ((this.logPathResults == PathLog.InGame) && (this.inGameDebugPath != string.Empty))
        {
            GUI.Label(new Rect(5f, 5f, 400f, 600f), this.inGameDebugPath);
        }
    }

    private void PerformBlockingActions([Optional, DefaultParameterValue(false)] bool force, [Optional, DefaultParameterValue(true)] bool unblockOnComplete)
    {
        if (this.pathProcessor.queue.AllReceiversBlocked)
        {
            this.pathReturnQueue.ReturnPaths(false);
            if (OnThreadSafeCallback != null)
            {
                Action onThreadSafeCallback = OnThreadSafeCallback;
                OnThreadSafeCallback = null;
                onThreadSafeCallback();
            }
            if (this.pathProcessor.queue.AllReceiversBlocked && this.workItems.ProcessWorkItems(force))
            {
                this.workItemsQueued = false;
                if (unblockOnComplete)
                {
                    if (this.euclideanEmbedding.dirty)
                    {
                        this.euclideanEmbedding.RecalculateCosts();
                    }
                    this.pathProcessor.queue.Unblock();
                }
            }
        }
    }

    public void QueueGraphUpdates()
    {
        if (!this.graphUpdatesWorkItemAdded)
        {
            <QueueGraphUpdates>c__AnonStorey241 storey = new <QueueGraphUpdates>c__AnonStorey241();
            storey.<>f__this = this;
            this.graphUpdatesWorkItemAdded = true;
            storey.workItem = this.graphUpdates.GetWorkItem();
            this.AddWorkItem(new AstarWorkItem(new Action(storey.<>m__6), storey.workItem.update));
        }
    }

    [Obsolete("This method has been moved. Use the method on the context object that can be sent with work item delegates instead")]
    public void QueueWorkItemFloodFill()
    {
        throw new Exception("This method has been moved. Use the method on the context object that can be sent with work item delegates instead");
    }

    private void RecalculateDebugLimits()
    {
        this.debugFloor = float.PositiveInfinity;
        this.debugRoof = float.NegativeInfinity;
        for (int i = 0; i < this.graphs.Length; i++)
        {
            if ((this.graphs[i] != null) && this.graphs[i].drawGizmos)
            {
                this.graphs[i].GetNodes(delegate (GraphNode node) {
                    if ((!this.showSearchTree || (this.debugPathData == null)) || NavGraph.InSearchTree(node, this.debugPath))
                    {
                        PathNode node2 = (this.debugPathData == null) ? null : this.debugPathData.GetPathNode(node);
                        if ((node2 != null) || (this.debugMode == GraphDebugMode.Penalty))
                        {
                            switch (this.debugMode)
                            {
                                case GraphDebugMode.G:
                                    this.debugFloor = Mathf.Min(this.debugFloor, (float) node2.G);
                                    this.debugRoof = Mathf.Max(this.debugRoof, (float) node2.G);
                                    break;

                                case GraphDebugMode.H:
                                    this.debugFloor = Mathf.Min(this.debugFloor, (float) node2.H);
                                    this.debugRoof = Mathf.Max(this.debugRoof, (float) node2.H);
                                    break;

                                case GraphDebugMode.F:
                                    this.debugFloor = Mathf.Min(this.debugFloor, (float) node2.F);
                                    this.debugRoof = Mathf.Max(this.debugRoof, (float) node2.F);
                                    break;

                                case GraphDebugMode.Penalty:
                                    this.debugFloor = Mathf.Min(this.debugFloor, (float) node.Penalty);
                                    this.debugRoof = Mathf.Max(this.debugRoof, (float) node.Penalty);
                                    break;
                            }
                        }
                    }
                    return true;
                });
            }
        }
        if (float.IsInfinity(this.debugFloor))
        {
            this.debugFloor = 0f;
            this.debugRoof = 1f;
        }
        if ((this.debugRoof - this.debugFloor) < 1f)
        {
            this.debugRoof++;
        }
    }

    public static void RegisterSafeUpdate(Action callback)
    {
        if ((callback != null) && Application.isPlaying)
        {
            if (active.pathProcessor.queue.AllReceiversBlocked)
            {
                active.pathProcessor.queue.Lock();
                try
                {
                    if (active.pathProcessor.queue.AllReceiversBlocked)
                    {
                        callback();
                        return;
                    }
                }
                finally
                {
                    active.pathProcessor.queue.Unlock();
                }
            }
            object safeUpdateLock = AstarPath.safeUpdateLock;
            lock (safeUpdateLock)
            {
                OnThreadSafeCallback = (Action) Delegate.Combine(OnThreadSafeCallback, callback);
            }
            active.pathProcessor.queue.Block();
        }
    }

    [Obsolete("The threadSafe parameter has been deprecated")]
    public static void RegisterSafeUpdate(Action callback, bool threadSafe)
    {
        RegisterSafeUpdate(callback);
    }

    public void Scan()
    {
        IEnumerator<Progress> enumerator = this.ScanAsync().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Progress current = enumerator.Current;
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    [DebuggerHidden]
    public IEnumerable<Progress> ScanAsync()
    {
        <ScanAsync>c__Iterator9 iterator = new <ScanAsync>c__Iterator9();
        iterator.<>f__this = this;
        iterator.$PC = -2;
        return iterator;
    }

    [DebuggerHidden]
    private IEnumerable<Progress> ScanGraph(NavGraph graph)
    {
        <ScanGraph>c__IteratorA ra = new <ScanGraph>c__IteratorA();
        ra.graph = graph;
        ra.<$>graph = graph;
        ra.$PC = -2;
        return ra;
    }

    [Obsolete("ScanLoop is now named ScanAsync and is an IEnumerable<Progress>. Use foreach to iterate over the progress insead")]
    public void ScanLoop(OnScanStatus statusCallback)
    {
        IEnumerator<Progress> enumerator = this.ScanAsync().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Progress current = enumerator.Current;
                statusCallback(current);
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
    }

    public void SetUpReferences()
    {
        active = this;
        if (this.astarData == null)
        {
            this.astarData = new AstarData();
        }
        if (this.colorSettings == null)
        {
            this.colorSettings = new AstarColor();
        }
        this.colorSettings.OnEnable();
    }

    public static void StartPath(Path p, [Optional, DefaultParameterValue(false)] bool pushToFront)
    {
        AstarPath active = AstarPath.active;
        if (object.ReferenceEquals(active, null))
        {
            UnityEngine.Debug.LogError("There is no AstarPath object in the scene or it has not been initialized yet");
        }
        else
        {
            if (p.GetState() != PathState.Created)
            {
                object[] objArray1 = new object[] { "The path has an invalid state. Expected ", PathState.Created, " found ", p.GetState(), "\nMake sure you are not requesting the same path twice" };
                throw new Exception(string.Concat(objArray1));
            }
            if (active.pathProcessor.queue.IsTerminating)
            {
                p.Error();
            }
            else if ((active.graphs == null) || (active.graphs.Length == 0))
            {
                UnityEngine.Debug.LogError("There are no graphs in the scene");
                p.Error();
                UnityEngine.Debug.LogError(p.errorLog);
            }
            else
            {
                p.Claim(active);
                p.AdvanceState(PathState.PathQueue);
                if (pushToFront)
                {
                    active.pathProcessor.queue.PushFront(p);
                }
                else
                {
                    active.pathProcessor.queue.Push(p);
                }
            }
        }
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            if (!this.isScanning)
            {
                this.PerformBlockingActions(false, true);
            }
            this.pathProcessor.TickNonMultithreaded();
            this.pathReturnQueue.ReturnPaths(true);
        }
    }

    public void UpdateGraphs(GraphUpdateObject ob)
    {
        this.graphUpdates.UpdateGraphs(ob);
        if (this.batchGraphUpdates && ((Time.realtimeSinceStartup - this.lastGraphUpdate) < this.graphUpdateBatchingInterval))
        {
            if (!this.graphUpdateRoutineRunning)
            {
                base.StartCoroutine(this.DelayedGraphUpdate());
            }
        }
        else
        {
            this.QueueGraphUpdates();
        }
    }

    public void UpdateGraphs(Bounds bounds)
    {
        this.UpdateGraphs(new GraphUpdateObject(bounds));
    }

    public void UpdateGraphs(GraphUpdateObject ob, float t)
    {
        base.StartCoroutine(this.UpdateGraphsInteral(ob, t));
    }

    public void UpdateGraphs(Bounds bounds, float t)
    {
        this.UpdateGraphs(new GraphUpdateObject(bounds), t);
    }

    [DebuggerHidden]
    private IEnumerator UpdateGraphsInteral(GraphUpdateObject ob, float t)
    {
        <UpdateGraphsInteral>c__Iterator8 iterator = new <UpdateGraphsInteral>c__Iterator8();
        iterator.t = t;
        iterator.ob = ob;
        iterator.<$>t = t;
        iterator.<$>ob = ob;
        iterator.<>f__this = this;
        return iterator;
    }

    internal void VerifyIntegrity()
    {
        if (active != this)
        {
            throw new Exception("Singleton pattern broken. Make sure you only have one AstarPath object in the scene");
        }
        if (this.astarData == null)
        {
            throw new NullReferenceException("AstarData is null... Astar not set up correctly?");
        }
        if (this.astarData.graphs == null)
        {
            this.astarData.graphs = new NavGraph[0];
        }
    }

    public static void WaitForPath(Path p)
    {
        if (active == null)
        {
            throw new Exception("Pathfinding is not correctly initialized in this scene (yet?). AstarPath.active is null.\nDo not call this function in Awake");
        }
        if (p == null)
        {
            throw new ArgumentNullException("Path must not be null");
        }
        if (!active.pathProcessor.queue.IsTerminating)
        {
            if (p.GetState() == PathState.Created)
            {
                throw new Exception("The specified path has not been started yet.");
            }
            waitForPathDepth++;
            if (waitForPathDepth == 5)
            {
                UnityEngine.Debug.LogError("You are calling the WaitForPath function recursively (maybe from a path callback). Please don't do this.");
            }
            if (p.GetState() < PathState.ReturnQueue)
            {
                if (active.IsUsingMultithreading)
                {
                    while (p.GetState() < PathState.ReturnQueue)
                    {
                        if (active.pathProcessor.queue.IsTerminating)
                        {
                            waitForPathDepth--;
                            throw new Exception("Pathfinding Threads seems to have crashed.");
                        }
                        Thread.Sleep(1);
                        active.PerformBlockingActions(false, true);
                    }
                }
                else
                {
                    while (p.GetState() < PathState.ReturnQueue)
                    {
                        if (active.pathProcessor.queue.IsEmpty && (p.GetState() != PathState.Processing))
                        {
                            waitForPathDepth--;
                            throw new Exception("Critical error. Path Queue is empty but the path state is '" + p.GetState() + "'");
                        }
                        active.pathProcessor.TickNonMultithreaded();
                        active.PerformBlockingActions(false, true);
                    }
                }
            }
            active.pathReturnQueue.ReturnPaths(false);
            waitForPathDepth--;
        }
    }

    public PathHandler debugPathData
    {
        get
        {
            if (this.debugPath == null)
            {
                return null;
            }
            return this.debugPath.pathHandler;
        }
    }

    public NavGraph[] graphs
    {
        get
        {
            if (this.astarData == null)
            {
                this.astarData = new AstarData();
            }
            return this.astarData.graphs;
        }
        set
        {
            if (this.astarData == null)
            {
                this.astarData = new AstarData();
            }
            this.astarData.graphs = value;
        }
    }

    [Obsolete]
    public System.Type[] graphTypes
    {
        get
        {
            return this.astarData.graphTypes;
        }
    }

    public bool IsAnyGraphUpdateQueued
    {
        get
        {
            return this.graphUpdates.IsAnyGraphUpdateQueued;
        }
    }

    [Obsolete("Fixed grammar, use IsAnyGraphUpdateQueued instead")]
    public bool IsAnyGraphUpdatesQueued
    {
        get
        {
            return this.IsAnyGraphUpdateQueued;
        }
    }

    public bool isScanning
    {
        [CompilerGenerated]
        get
        {
            return this.<isScanning>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<isScanning>k__BackingField = value;
        }
    }

    public bool IsUsingMultithreading
    {
        get
        {
            return this.pathProcessor.IsUsingMultithreading;
        }
    }

    public float lastScanTime
    {
        [CompilerGenerated]
        get
        {
            return this.<lastScanTime>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<lastScanTime>k__BackingField = value;
        }
    }

    [Obsolete("This field has been renamed to 'batchGraphUpdates'")]
    public bool limitGraphUpdates
    {
        get
        {
            return this.batchGraphUpdates;
        }
        set
        {
            this.batchGraphUpdates = value;
        }
    }

    [Obsolete("This field has been renamed to 'graphUpdateBatchingInterval'")]
    public float maxGraphUpdateFreq
    {
        get
        {
            return this.graphUpdateBatchingInterval;
        }
        set
        {
            this.graphUpdateBatchingInterval = value;
        }
    }

    public float maxNearestNodeDistanceSqr
    {
        get
        {
            return (this.maxNearestNodeDistance * this.maxNearestNodeDistance);
        }
    }

    public int NumParallelThreads
    {
        get
        {
            return this.pathProcessor.NumThreads;
        }
    }

    public static System.Version Version
    {
        get
        {
            return new System.Version(3, 8, 4);
        }
    }

    [CompilerGenerated]
    private sealed class <DelayedGraphUpdate>c__Iterator7 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal AstarPath <>f__this;

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
                    this.<>f__this.graphUpdateRoutineRunning = true;
                    this.$current = new WaitForSeconds(this.<>f__this.graphUpdateBatchingInterval - (Time.realtimeSinceStartup - this.<>f__this.lastGraphUpdate));
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.QueueGraphUpdates();
                    this.<>f__this.graphUpdateRoutineRunning = false;
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
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
    private sealed class <GetNearest>c__AnonStorey242
    {
        internal Vector3 lineDirection;
        internal Vector3 lineOrigin;
        internal float minDist;
        internal GraphNode nearestNode;

        internal bool <>m__9(GraphNode node)
        {
            Vector3 position = (Vector3) node.position;
            Vector3 vector2 = this.lineOrigin + ((Vector3) (Vector3.Dot(position - this.lineOrigin, this.lineDirection) * this.lineDirection));
            float num = Mathf.Abs((float) (vector2.x - position.x));
            num *= num;
            if (num <= this.minDist)
            {
                num = Mathf.Abs((float) (vector2.z - position.z));
                num *= num;
                if (num > this.minDist)
                {
                    return true;
                }
                Vector3 vector3 = vector2 - position;
                float sqrMagnitude = vector3.sqrMagnitude;
                if (sqrMagnitude < this.minDist)
                {
                    this.minDist = sqrMagnitude;
                    this.nearestNode = node;
                }
            }
            return true;
        }
    }

    [CompilerGenerated]
    private sealed class <QueueGraphUpdates>c__AnonStorey241
    {
        internal AstarPath <>f__this;
        internal AstarWorkItem workItem;

        internal void <>m__6()
        {
            this.<>f__this.graphUpdatesWorkItemAdded = false;
            this.<>f__this.lastGraphUpdate = Time.realtimeSinceStartup;
            this.<>f__this.debugPath = null;
            this.workItem.init();
        }
    }

    [CompilerGenerated]
    private sealed class <ScanAsync>c__Iterator9 : IEnumerator, IEnumerable<Progress>, IEnumerator<Progress>, IDisposable, IEnumerable
    {
        internal Progress $current;
        internal int $PC;
        internal IEnumerator<Progress> <$s_3>__6;
        private static GraphNodeDelegateCancelable <>f__am$cacheC;
        internal AstarPath <>f__this;
        internal Exception <e>__8;
        internal int <i>__1;
        internal int <i>__2;
        internal float <maxp>__4;
        internal float <minp>__3;
        internal Progress <progress>__7;
        internal string <progressDescriptionPrefix>__5;
        internal Stopwatch <watch>__0;

        private static bool <>m__A(GraphNode node)
        {
            node.Destroy();
            return true;
        }

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 2:
                    try
                    {
                    }
                    finally
                    {
                        if (this.<$s_3>__6 == null)
                        {
                        }
                        this.<$s_3>__6.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    if (this.<>f__this.graphs != null)
                    {
                        this.<>f__this.isScanning = true;
                        this.<>f__this.euclideanEmbedding.dirty = false;
                        this.<>f__this.VerifyIntegrity();
                        this.<>f__this.BlockUntilPathQueueBlocked();
                        this.<>f__this.pathReturnQueue.ReturnPaths(false);
                        this.<>f__this.BlockUntilPathQueueBlocked();
                        if (!Application.isPlaying)
                        {
                            GraphModifier.FindAllModifiers();
                            RelevantGraphSurface.FindAllGraphSurfaces();
                        }
                        RelevantGraphSurface.UpdateAllPositions();
                        this.<>f__this.astarData.UpdateShortcuts();
                        this.$current = new Progress(0.05f, "Pre processing graphs");
                        this.$PC = 1;
                        goto Label_04D6;
                    }
                    break;

                case 1:
                    if (AstarPath.OnPreScan != null)
                    {
                        AstarPath.OnPreScan(this.<>f__this);
                    }
                    GraphModifier.TriggerEvent(GraphModifier.EventType.PreScan);
                    this.<watch>__0 = Stopwatch.StartNew();
                    this.<i>__1 = 0;
                    while (this.<i>__1 < this.<>f__this.graphs.Length)
                    {
                        if (this.<>f__this.graphs[this.<i>__1] != null)
                        {
                            if (<>f__am$cacheC == null)
                            {
                                <>f__am$cacheC = new GraphNodeDelegateCancelable(AstarPath.<ScanAsync>c__Iterator9.<>m__A);
                            }
                            this.<>f__this.graphs[this.<i>__1].GetNodes(<>f__am$cacheC);
                        }
                        this.<i>__1++;
                    }
                    this.<i>__2 = 0;
                    while (this.<i>__2 < this.<>f__this.graphs.Length)
                    {
                        if (this.<>f__this.graphs[this.<i>__2] == null)
                        {
                            goto Label_0322;
                        }
                        this.<minp>__3 = Mathf.Lerp(0.1f, 0.8f, ((float) this.<i>__2) / ((float) this.<>f__this.graphs.Length));
                        this.<maxp>__4 = Mathf.Lerp(0.1f, 0.8f, (this.<i>__2 + 0.95f) / ((float) this.<>f__this.graphs.Length));
                        object[] objArray1 = new object[] { "Scanning graph ", this.<i>__2 + 1, " of ", this.<>f__this.graphs.Length, " - " };
                        this.<progressDescriptionPrefix>__5 = string.Concat(objArray1);
                        this.<$s_3>__6 = this.<>f__this.ScanGraph(this.<>f__this.graphs[this.<i>__2]).GetEnumerator();
                        num = 0xfffffffd;
                    Label_0287:
                        try
                        {
                            while (this.<$s_3>__6.MoveNext())
                            {
                                this.<progress>__7 = this.<$s_3>__6.Current;
                                this.$current = new Progress(Mathf.Lerp(this.<minp>__3, this.<maxp>__4, this.<progress>__7.progress), this.<progressDescriptionPrefix>__5 + this.<progress>__7.description);
                                this.$PC = 2;
                                flag = true;
                                goto Label_04D6;
                            }
                        }
                        finally
                        {
                            if (!flag)
                            {
                            }
                            if (this.<$s_3>__6 == null)
                            {
                            }
                            this.<$s_3>__6.Dispose();
                        }
                    Label_0322:
                        this.<i>__2++;
                    }
                    this.$current = new Progress(0.8f, "Post processing graphs");
                    this.$PC = 3;
                    goto Label_04D6;

                case 2:
                    goto Label_0287;

                case 3:
                    if (AstarPath.OnPostScan != null)
                    {
                        AstarPath.OnPostScan(this.<>f__this);
                    }
                    GraphModifier.TriggerEvent(GraphModifier.EventType.PostScan);
                    try
                    {
                        this.<>f__this.FlushWorkItems(false, true);
                    }
                    catch (Exception exception)
                    {
                        this.<e>__8 = exception;
                        UnityEngine.Debug.LogException(this.<e>__8);
                    }
                    this.$current = new Progress(0.9f, "Computing areas");
                    this.$PC = 4;
                    goto Label_04D6;

                case 4:
                    this.<>f__this.FloodFill();
                    this.<>f__this.VerifyIntegrity();
                    this.$current = new Progress(0.95f, "Late post processing");
                    this.$PC = 5;
                    goto Label_04D6;

                case 5:
                    this.<>f__this.isScanning = false;
                    if (AstarPath.OnLatePostScan != null)
                    {
                        AstarPath.OnLatePostScan(this.<>f__this);
                    }
                    GraphModifier.TriggerEvent(GraphModifier.EventType.LatePostScan);
                    this.<>f__this.euclideanEmbedding.dirty = true;
                    this.<>f__this.euclideanEmbedding.RecalculatePivots();
                    this.<>f__this.PerformBlockingActions(true, true);
                    this.<watch>__0.Stop();
                    this.<>f__this.lastScanTime = (float) this.<watch>__0.Elapsed.TotalSeconds;
                    GC.Collect();
                    this.<>f__this.Log("Scanning - Process took " + ((this.<>f__this.lastScanTime * 1000f)).ToString("0") + " ms to complete");
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_04D6:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        [DebuggerHidden]
        IEnumerator<Progress> IEnumerable<Progress>.GetEnumerator()
        {
            if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
            {
                return this;
            }
            AstarPath.<ScanAsync>c__Iterator9 iterator = new AstarPath.<ScanAsync>c__Iterator9();
            iterator.<>f__this = this.<>f__this;
            return iterator;
        }

        [DebuggerHidden]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.System.Collections.Generic.IEnumerable<Pathfinding.Progress>.GetEnumerator();
        }

        Progress IEnumerator<Progress>.Current
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
    private sealed class <ScanGraph>c__IteratorA : IEnumerator, IEnumerable<Progress>, IEnumerator<Progress>, IDisposable, IEnumerable
    {
        internal Progress $current;
        internal int $PC;
        internal NavGraph <$>graph;
        internal IEnumerator<Progress> <$s_4>__0;
        internal Progress <p>__1;
        internal NavGraph graph;

        internal bool <>m__B(GraphNode node)
        {
            node.GraphIndex = this.graph.graphIndex;
            return true;
        }

        [DebuggerHidden]
        public void Dispose()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 3:
                    try
                    {
                    }
                    finally
                    {
                        if (this.<$s_4>__0 == null)
                        {
                        }
                        this.<$s_4>__0.Dispose();
                    }
                    break;
            }
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            bool flag = false;
            switch (num)
            {
                case 0:
                    if (AstarPath.OnGraphPreScan == null)
                    {
                        break;
                    }
                    this.$current = new Progress(0f, "Pre processing");
                    this.$PC = 1;
                    goto Label_01B2;

                case 1:
                    AstarPath.OnGraphPreScan(this.graph);
                    break;

                case 2:
                    this.<$s_4>__0 = this.graph.ScanInternal().GetEnumerator();
                    num = 0xfffffffd;
                    goto Label_00A8;

                case 3:
                    goto Label_00A8;

                case 4:
                    this.graph.GetNodes(new GraphNodeDelegateCancelable(this.<>m__B));
                    if (AstarPath.OnGraphPostScan == null)
                    {
                        goto Label_01A9;
                    }
                    this.$current = new Progress(0.99f, "Post processing");
                    this.$PC = 5;
                    goto Label_01B2;

                case 5:
                    AstarPath.OnGraphPostScan(this.graph);
                    goto Label_01A9;

                default:
                    goto Label_01B0;
            }
            this.$current = new Progress(0f, string.Empty);
            this.$PC = 2;
            goto Label_01B2;
        Label_00A8:
            try
            {
                while (this.<$s_4>__0.MoveNext())
                {
                    this.<p>__1 = this.<$s_4>__0.Current;
                    float p = Mathf.Lerp(0f, 0.95f, this.<p>__1.progress);
                    this.$current = new Progress(p, this.<p>__1.description);
                    this.$PC = 3;
                    flag = true;
                    goto Label_01B2;
                }
            }
            finally
            {
                if (!flag)
                {
                }
                if (this.<$s_4>__0 == null)
                {
                }
                this.<$s_4>__0.Dispose();
            }
            this.$current = new Progress(0.95f, "Assigning graph indices");
            this.$PC = 4;
            goto Label_01B2;
        Label_01A9:
            this.$PC = -1;
        Label_01B0:
            return false;
        Label_01B2:
            return true;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        [DebuggerHidden]
        IEnumerator<Progress> IEnumerable<Progress>.GetEnumerator()
        {
            if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
            {
                return this;
            }
            AstarPath.<ScanGraph>c__IteratorA ra = new AstarPath.<ScanGraph>c__IteratorA();
            ra.graph = this.<$>graph;
            return ra;
        }

        [DebuggerHidden]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.System.Collections.Generic.IEnumerable<Pathfinding.Progress>.GetEnumerator();
        }

        Progress IEnumerator<Progress>.Current
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
    private sealed class <UpdateGraphsInteral>c__Iterator8 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal GraphUpdateObject <$>ob;
        internal float <$>t;
        internal AstarPath <>f__this;
        internal GraphUpdateObject ob;
        internal float t;

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
                    this.$current = new WaitForSeconds(this.t);
                    this.$PC = 1;
                    return true;

                case 1:
                    this.<>f__this.UpdateGraphs(this.ob);
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
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

    public enum AstarDistribution
    {
        WebsiteDownload,
        AssetStore
    }
}

