namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    internal class GraphUpdateProcessor
    {
        [CompilerGenerated]
        private static GraphNodeDelegateCancelable <>f__am$cacheA;
        private readonly AstarPath astar;
        private readonly ManualResetEvent asyncGraphUpdatesComplete = new ManualResetEvent(true);
        private readonly AutoResetEvent exitAsyncThread = new AutoResetEvent(false);
        private readonly AutoResetEvent graphUpdateAsyncEvent = new AutoResetEvent(false);
        private readonly Queue<GraphUpdateObject> graphUpdateQueue = new Queue<GraphUpdateObject>();
        private readonly Queue<GUOSingle> graphUpdateQueueAsync = new Queue<GUOSingle>();
        private readonly Queue<GUOSingle> graphUpdateQueueRegular = new Queue<GUOSingle>();
        private Thread graphUpdateThread;
        private uint lastUniqueAreaIndex;

        public event Action OnGraphsUpdated;

        public GraphUpdateProcessor(AstarPath astar)
        {
            this.astar = astar;
        }

        public void DisableMultithreading()
        {
            if ((this.graphUpdateThread != null) && this.graphUpdateThread.IsAlive)
            {
                this.exitAsyncThread.Set();
                if (!this.graphUpdateThread.Join(0x4e20))
                {
                    Debug.LogError("Graph update thread did not exit in 20 seconds");
                }
                this.graphUpdateThread = null;
            }
        }

        public void EnableMultithreading()
        {
            if ((this.graphUpdateThread == null) || !this.graphUpdateThread.IsAlive)
            {
                this.graphUpdateThread = new Thread(new ThreadStart(this.ProcessGraphUpdatesAsync));
                this.graphUpdateThread.IsBackground = true;
                this.graphUpdateThread.Priority = System.Threading.ThreadPriority.Lowest;
                this.graphUpdateThread.Start(this);
            }
        }

        [ContextMenu("Flood Fill Graphs")]
        public void FloodFill()
        {
            <FloodFill>c__AnonStorey243 storey = new <FloodFill>c__AnonStorey243();
            NavGraph[] graphs = this.astar.graphs;
            if (graphs != null)
            {
                for (int i = 0; i < graphs.Length; i++)
                {
                    NavGraph graph = graphs[i];
                    if (graph != null)
                    {
                        if (<>f__am$cacheA == null)
                        {
                            <>f__am$cacheA = delegate (GraphNode node) {
                                node.Area = 0;
                                return true;
                            };
                        }
                        graph.GetNodes(<>f__am$cacheA);
                    }
                }
                this.lastUniqueAreaIndex = 0;
                storey.area = 0;
                storey.forcedSmallAreas = 0;
                storey.stack = StackPool<GraphNode>.Claim();
                for (int j = 0; j < graphs.Length; j++)
                {
                    NavGraph graph2 = graphs[j];
                    if (graph2 != null)
                    {
                        GraphNodeDelegateCancelable del = new GraphNodeDelegateCancelable(storey.<>m__F);
                        graph2.GetNodes(del);
                    }
                }
                this.lastUniqueAreaIndex = storey.area;
                if (storey.forcedSmallAreas > 0)
                {
                    Debug.LogError(string.Concat(new object[] { storey.forcedSmallAreas, " areas had to share IDs. This usually doesn't affect pathfinding in any significant way (you might get 'Searched whole area but could not find target' as a reason for path failure) however some path requests may take longer to calculate (specifically those that fail with the 'Searched whole area' error).The maximum number of areas is ", 0x1ffff, "." }));
                }
                StackPool<GraphNode>.Release(storey.stack);
            }
        }

        public void FloodFill(GraphNode seed)
        {
            this.FloodFill(seed, this.lastUniqueAreaIndex + 1);
            this.lastUniqueAreaIndex++;
        }

        public void FloodFill(GraphNode seed, uint area)
        {
            if (area > 0x1ffff)
            {
                Debug.LogError("Too high area index - The maximum area index is " + 0x1ffff);
            }
            else if (area < 0)
            {
                Debug.LogError("Too low area index - The minimum area index is 0");
            }
            else
            {
                Stack<GraphNode> stack = StackPool<GraphNode>.Claim();
                stack.Push(seed);
                seed.Area = area;
                while (stack.Count > 0)
                {
                    stack.Pop().FloodFill(stack, area);
                }
                StackPool<GraphNode>.Release(stack);
            }
        }

        public AstarWorkItem GetWorkItem()
        {
            return new AstarWorkItem(new Action(this.QueueGraphUpdatesInternal), new Func<bool, bool>(this.ProcessGraphUpdates));
        }

        private bool ProcessGraphUpdates(bool force)
        {
            if (force)
            {
                this.asyncGraphUpdatesComplete.WaitOne();
            }
            else if (!this.asyncGraphUpdatesComplete.WaitOne(0))
            {
                return false;
            }
            if (this.graphUpdateQueueAsync.Count != 0)
            {
                throw new Exception("Queue should be empty at this stage");
            }
            while (this.graphUpdateQueueRegular.Count > 0)
            {
                GUOSingle item = this.graphUpdateQueueRegular.Peek();
                GraphUpdateThreading threading = (item.order != GraphUpdateOrder.FloodFill) ? item.graph.CanUpdateAsync(item.obj) : GraphUpdateThreading.SeparateThread;
                bool flag = force;
                if ((!Application.isPlaying || (this.graphUpdateThread == null)) || !this.graphUpdateThread.IsAlive)
                {
                    flag = true;
                }
                if (!flag && (threading == GraphUpdateThreading.SeparateAndUnityInit))
                {
                    if (this.graphUpdateQueueAsync.Count > 0)
                    {
                        this.asyncGraphUpdatesComplete.Reset();
                        this.graphUpdateAsyncEvent.Set();
                        return false;
                    }
                    item.graph.UpdateAreaInit(item.obj);
                    this.graphUpdateQueueRegular.Dequeue();
                    this.graphUpdateQueueAsync.Enqueue(item);
                    this.asyncGraphUpdatesComplete.Reset();
                    this.graphUpdateAsyncEvent.Set();
                    return false;
                }
                if (!flag && (threading == GraphUpdateThreading.SeparateThread))
                {
                    this.graphUpdateQueueRegular.Dequeue();
                    this.graphUpdateQueueAsync.Enqueue(item);
                }
                else
                {
                    if (this.graphUpdateQueueAsync.Count > 0)
                    {
                        if (force)
                        {
                            throw new Exception("This should not happen");
                        }
                        this.asyncGraphUpdatesComplete.Reset();
                        this.graphUpdateAsyncEvent.Set();
                        return false;
                    }
                    this.graphUpdateQueueRegular.Dequeue();
                    if (item.order == GraphUpdateOrder.FloodFill)
                    {
                        this.FloodFill();
                        continue;
                    }
                    if (threading == GraphUpdateThreading.SeparateAndUnityInit)
                    {
                        try
                        {
                            item.graph.UpdateAreaInit(item.obj);
                        }
                        catch (Exception exception)
                        {
                            Debug.LogError("Error while initializing GraphUpdates\n" + exception);
                        }
                    }
                    try
                    {
                        item.graph.UpdateArea(item.obj);
                        continue;
                    }
                    catch (Exception exception2)
                    {
                        Debug.LogError("Error while updating graphs\n" + exception2);
                        continue;
                    }
                }
            }
            if (this.graphUpdateQueueAsync.Count > 0)
            {
                this.asyncGraphUpdatesComplete.Reset();
                this.graphUpdateAsyncEvent.Set();
                return false;
            }
            GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
            if (this.OnGraphsUpdated != null)
            {
                this.OnGraphsUpdated();
            }
            return true;
        }

        private void ProcessGraphUpdatesAsync()
        {
            AutoResetEvent[] waitHandles = new AutoResetEvent[] { this.graphUpdateAsyncEvent, this.exitAsyncThread };
        Label_0019:
            if (WaitHandle.WaitAny(waitHandles) == 1)
            {
                this.graphUpdateQueueAsync.Clear();
                this.asyncGraphUpdatesComplete.Set();
            }
            else
            {
                while (this.graphUpdateQueueAsync.Count > 0)
                {
                    GUOSingle num2 = this.graphUpdateQueueAsync.Dequeue();
                    try
                    {
                        if (num2.order != GraphUpdateOrder.GraphUpdate)
                        {
                            if (num2.order != GraphUpdateOrder.FloodFill)
                            {
                                throw new NotSupportedException(string.Empty + num2.order);
                            }
                            this.FloodFill();
                        }
                        else
                        {
                            num2.graph.UpdateArea(num2.obj);
                        }
                        continue;
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError("Exception while updating graphs:\n" + exception);
                        continue;
                    }
                }
                this.asyncGraphUpdatesComplete.Set();
                goto Label_0019;
            }
        }

        private void QueueGraphUpdatesInternal()
        {
            bool flag = false;
            while (this.graphUpdateQueue.Count > 0)
            {
                GraphUpdateObject obj2 = this.graphUpdateQueue.Dequeue();
                if (obj2.requiresFloodFill)
                {
                    flag = true;
                }
                IEnumerator enumerator = this.astar.astarData.GetUpdateableGraphs().GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        IUpdatableGraph current = (IUpdatableGraph) enumerator.Current;
                        NavGraph graph = current as NavGraph;
                        if ((obj2.nnConstraint == null) || obj2.nnConstraint.SuitableGraph(this.astar.astarData.GetGraphIndex(graph), graph))
                        {
                            GUOSingle item = new GUOSingle();
                            item.order = GraphUpdateOrder.GraphUpdate;
                            item.obj = obj2;
                            item.graph = current;
                            this.graphUpdateQueueRegular.Enqueue(item);
                        }
                    }
                    continue;
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable == null)
                    {
                    }
                    disposable.Dispose();
                }
            }
            if (flag)
            {
                GUOSingle num2 = new GUOSingle();
                num2.order = GraphUpdateOrder.FloodFill;
                this.graphUpdateQueueRegular.Enqueue(num2);
            }
            GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
        }

        public void UpdateGraphs(GraphUpdateObject ob)
        {
            this.graphUpdateQueue.Enqueue(ob);
        }

        public bool IsAnyGraphUpdateQueued
        {
            get
            {
                return (this.graphUpdateQueue.Count > 0);
            }
        }

        [CompilerGenerated]
        private sealed class <FloodFill>c__AnonStorey243
        {
            internal uint area;
            internal int forcedSmallAreas;
            internal Stack<GraphNode> stack;

            internal bool <>m__F(GraphNode node)
            {
                if (node.Walkable && (node.Area == 0))
                {
                    this.area++;
                    uint area = this.area;
                    if (this.area > 0x1ffff)
                    {
                        this.area--;
                        area = this.area;
                        if (this.forcedSmallAreas == 0)
                        {
                            this.forcedSmallAreas = 1;
                        }
                        this.forcedSmallAreas++;
                    }
                    this.stack.Clear();
                    this.stack.Push(node);
                    int num2 = 1;
                    node.Area = area;
                    while (this.stack.Count > 0)
                    {
                        num2++;
                        this.stack.Pop().FloodFill(this.stack, area);
                    }
                }
                return true;
            }
        }

        private enum GraphUpdateOrder
        {
            GraphUpdate,
            FloodFill
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct GUOSingle
        {
            public GraphUpdateProcessor.GraphUpdateOrder order;
            public IUpdatableGraph graph;
            public GraphUpdateObject obj;
        }
    }
}

