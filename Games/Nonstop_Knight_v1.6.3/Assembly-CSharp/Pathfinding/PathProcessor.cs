namespace Pathfinding
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    internal class PathProcessor
    {
        private readonly AstarPath astar;
        private int nextNodeIndex = 1;
        private readonly Stack<int> nodeIndexPool = new Stack<int>();
        public readonly ThreadControlQueue queue;
        private readonly PathReturnQueue returnQueue;
        private IEnumerator threadCoroutine;
        private readonly PathThreadInfo[] threadInfos;
        private readonly Thread[] threads;

        public event Action<Path> OnPathPostSearch;

        public event Action<Path> OnPathPreSearch;

        public PathProcessor(AstarPath astar, PathReturnQueue returnQueue, int processors, bool multithreaded)
        {
            this.astar = astar;
            this.returnQueue = returnQueue;
            if (processors < 0)
            {
                throw new ArgumentOutOfRangeException("processors");
            }
            if (!multithreaded && (processors != 1))
            {
                throw new Exception("Only a single non-multithreaded processor is allowed");
            }
            this.queue = new ThreadControlQueue(processors);
            this.threadInfos = new PathThreadInfo[processors];
            for (int i = 0; i < processors; i++)
            {
                this.threadInfos[i] = new PathThreadInfo(i, astar, new PathHandler(i, processors));
            }
            if (multithreaded)
            {
                this.threads = new Thread[processors];
                for (int j = 0; j < processors; j++)
                {
                    <PathProcessor>c__AnonStorey247 storey = new <PathProcessor>c__AnonStorey247();
                    storey.<>f__this = this;
                    storey.threadIndex = j;
                    Thread thread = new Thread(new ThreadStart(storey.<>m__16));
                    thread.Name = "Pathfinding Thread " + j;
                    thread.IsBackground = true;
                    this.threads[j] = thread;
                    thread.Start();
                }
            }
            else
            {
                this.threadCoroutine = this.CalculatePaths(this.threadInfos[0]);
            }
        }

        public void AbortThreads()
        {
            if (this.threads != null)
            {
                for (int i = 0; i < this.threads.Length; i++)
                {
                    if ((this.threads[i] != null) && this.threads[i].IsAlive)
                    {
                        this.threads[i].Abort();
                    }
                }
            }
        }

        public void BlockUntilPathQueueBlocked()
        {
            this.queue.Block();
            if (Application.isPlaying)
            {
                while (!this.queue.AllReceiversBlocked)
                {
                    if (this.IsUsingMultithreading)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
                        this.TickNonMultithreaded();
                    }
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator CalculatePaths(PathThreadInfo threadInfo)
        {
            <CalculatePaths>c__IteratorB rb = new <CalculatePaths>c__IteratorB();
            rb.threadInfo = threadInfo;
            rb.<$>threadInfo = threadInfo;
            rb.<>f__this = this;
            return rb;
        }

        private void CalculatePathsThreaded(PathThreadInfo threadInfo)
        {
            try
            {
                PathHandler runData = threadInfo.runData;
                if (runData.nodes == null)
                {
                    throw new NullReferenceException("NodeRuns must be assigned to the threadInfo.runData.nodes field before threads are started\nthreadInfo is an argument to the thread functions");
                }
                long num = (long) (this.astar.maxFrameTime * 10000f);
                long targetTick = DateTime.UtcNow.Ticks + num;
                while (true)
                {
                    Path path = this.queue.Pop();
                    num = (long) (this.astar.maxFrameTime * 10000f);
                    path.PrepareBase(runData);
                    path.AdvanceState(PathState.Processing);
                    if (this.OnPathPreSearch != null)
                    {
                        this.OnPathPreSearch(path);
                    }
                    long ticks = DateTime.UtcNow.Ticks;
                    long num4 = 0L;
                    path.Prepare();
                    if (!path.IsDone())
                    {
                        this.astar.debugPath = path;
                        path.Initialize();
                        while (!path.IsDone())
                        {
                            path.CalculateStep(targetTick);
                            path.searchIterations++;
                            if (path.IsDone())
                            {
                                break;
                            }
                            num4 += DateTime.UtcNow.Ticks - ticks;
                            Thread.Sleep(0);
                            ticks = DateTime.UtcNow.Ticks;
                            targetTick = ticks + num;
                            if (this.queue.IsTerminating)
                            {
                                path.Error();
                            }
                        }
                        num4 += DateTime.UtcNow.Ticks - ticks;
                        path.duration = num4 * 0.0001f;
                    }
                    path.Cleanup();
                    if (path.immediateCallback != null)
                    {
                        path.immediateCallback(path);
                    }
                    if (this.OnPathPostSearch != null)
                    {
                        this.OnPathPostSearch(path);
                    }
                    this.returnQueue.Enqueue(path);
                    path.AdvanceState(PathState.ReturnQueue);
                    if (DateTime.UtcNow.Ticks > targetTick)
                    {
                        Thread.Sleep(1);
                        targetTick = DateTime.UtcNow.Ticks + num;
                    }
                }
            }
            catch (Exception exception)
            {
                if ((exception is ThreadAbortException) || (exception is ThreadControlQueue.QueueTerminationException))
                {
                    if (this.astar.logPathResults == PathLog.Heavy)
                    {
                        UnityEngine.Debug.LogWarning("Shutting down pathfinding thread #" + threadInfo.threadIndex);
                    }
                    return;
                }
                UnityEngine.Debug.LogException(exception);
                UnityEngine.Debug.LogError("Unhandled exception during pathfinding. Terminating.");
                this.queue.TerminateReceivers();
            }
            UnityEngine.Debug.LogError("Error : This part should never be reached.");
            this.queue.ReceiverTerminated();
        }

        public void DestroyNode(GraphNode node)
        {
            if (node.NodeIndex != -1)
            {
                this.nodeIndexPool.Push(node.NodeIndex);
                for (int i = 0; i < this.threadInfos.Length; i++)
                {
                    this.threadInfos[i].runData.DestroyNode(node);
                }
            }
        }

        public int GetNewNodeIndex()
        {
            return ((this.nodeIndexPool.Count <= 0) ? this.nextNodeIndex++ : this.nodeIndexPool.Pop());
        }

        public void InitializeNode(GraphNode node)
        {
            if (!this.queue.AllReceiversBlocked)
            {
                throw new Exception("Trying to initialize a node when it is not safe to initialize any nodes. Must be done during a graph update. See http://arongranberg.com/astar/docs/graph-updates.php#direct");
            }
            for (int i = 0; i < this.threadInfos.Length; i++)
            {
                this.threadInfos[i].runData.InitializeNode(node);
            }
        }

        public void JoinThreads()
        {
            if (this.threads != null)
            {
                for (int i = 0; i < this.threads.Length; i++)
                {
                    if (!this.threads[i].Join(50))
                    {
                        UnityEngine.Debug.LogError("Could not terminate pathfinding thread[" + i + "] in 50ms, trying Thread.Abort");
                        this.threads[i].Abort();
                    }
                }
            }
        }

        public void TickNonMultithreaded()
        {
            if (this.threadCoroutine != null)
            {
                try
                {
                    this.threadCoroutine.MoveNext();
                }
                catch (Exception exception)
                {
                    this.threadCoroutine = null;
                    if (!(exception is ThreadControlQueue.QueueTerminationException))
                    {
                        UnityEngine.Debug.LogException(exception);
                        UnityEngine.Debug.LogError("Unhandled exception during pathfinding. Terminating.");
                        this.queue.TerminateReceivers();
                        try
                        {
                            this.queue.PopNoBlock(false);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        public bool IsUsingMultithreading
        {
            get
            {
                return (this.threads != null);
            }
        }

        public int NumThreads
        {
            get
            {
                return this.threadInfos.Length;
            }
        }

        [CompilerGenerated]
        private sealed class <CalculatePaths>c__IteratorB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal PathThreadInfo <$>threadInfo;
            internal PathProcessor <>f__this;
            internal bool <blockedBefore>__5;
            internal long <maxTicks>__2;
            internal int <numPaths>__0;
            internal Path <p>__4;
            internal PathHandler <runData>__1;
            internal long <startTicks>__7;
            internal long <targetTick>__3;
            internal OnPathDelegate <tmpImmediateCallback>__9;
            internal Action<Path> <tmpOnPathPostSearch>__10;
            internal Action<Path> <tmpOnPathPreSearch>__6;
            internal long <totalTicks>__8;
            internal PathThreadInfo threadInfo;

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
                        this.<numPaths>__0 = 0;
                        this.<runData>__1 = this.threadInfo.runData;
                        if (this.<runData>__1.nodes == null)
                        {
                            throw new NullReferenceException("NodeRuns must be assigned to the threadInfo.runData.nodes field before threads are started\nthreadInfo is an argument to the thread functions");
                        }
                        this.<maxTicks>__2 = (long) (this.<>f__this.astar.maxFrameTime * 10000f);
                        this.<targetTick>__3 = DateTime.UtcNow.Ticks + this.<maxTicks>__2;
                        break;

                    case 1:
                        goto Label_0106;

                    case 2:
                        goto Label_024F;

                    case 3:
                        this.<targetTick>__3 = DateTime.UtcNow.Ticks + this.<maxTicks>__2;
                        this.<numPaths>__0 = 0;
                        break;

                    default:
                        goto Label_03C9;
                }
            Label_0093:
                this.<p>__4 = null;
                this.<blockedBefore>__5 = false;
            Label_0106:
                while (this.<p>__4 == null)
                {
                    try
                    {
                        this.<p>__4 = this.<>f__this.queue.PopNoBlock(this.<blockedBefore>__5);
                        this.<blockedBefore>__5 |= this.<p>__4 == null;
                    }
                    catch (ThreadControlQueue.QueueTerminationException)
                    {
                        goto Label_03C9;
                    }
                    if (this.<p>__4 == null)
                    {
                        this.$current = null;
                        this.$PC = 1;
                        goto Label_03CB;
                    }
                }
                this.<maxTicks>__2 = (long) (this.<>f__this.astar.maxFrameTime * 10000f);
                this.<p>__4.PrepareBase(this.<runData>__1);
                this.<p>__4.AdvanceState(PathState.Processing);
                this.<tmpOnPathPreSearch>__6 = this.<>f__this.OnPathPreSearch;
                if (this.<tmpOnPathPreSearch>__6 != null)
                {
                    this.<tmpOnPathPreSearch>__6(this.<p>__4);
                }
                this.<numPaths>__0++;
                this.<startTicks>__7 = DateTime.UtcNow.Ticks;
                this.<totalTicks>__8 = 0L;
                this.<p>__4.Prepare();
                if (!this.<p>__4.IsDone())
                {
                    this.<>f__this.astar.debugPath = this.<p>__4;
                    this.<p>__4.Initialize();
                    while (!this.<p>__4.IsDone())
                    {
                        this.<p>__4.CalculateStep(this.<targetTick>__3);
                        this.<p>__4.searchIterations++;
                        if (this.<p>__4.IsDone())
                        {
                            break;
                        }
                        this.<totalTicks>__8 += DateTime.UtcNow.Ticks - this.<startTicks>__7;
                        this.$current = null;
                        this.$PC = 2;
                        goto Label_03CB;
                    Label_024F:
                        this.<startTicks>__7 = DateTime.UtcNow.Ticks;
                        if (this.<>f__this.queue.IsTerminating)
                        {
                            this.<p>__4.Error();
                        }
                        this.<targetTick>__3 = DateTime.UtcNow.Ticks + this.<maxTicks>__2;
                    }
                    this.<totalTicks>__8 += DateTime.UtcNow.Ticks - this.<startTicks>__7;
                    this.<p>__4.duration = this.<totalTicks>__8 * 0.0001f;
                }
                this.<p>__4.Cleanup();
                this.<tmpImmediateCallback>__9 = this.<p>__4.immediateCallback;
                if (this.<tmpImmediateCallback>__9 != null)
                {
                    this.<tmpImmediateCallback>__9(this.<p>__4);
                }
                this.<tmpOnPathPostSearch>__10 = this.<>f__this.OnPathPostSearch;
                if (this.<tmpOnPathPostSearch>__10 != null)
                {
                    this.<tmpOnPathPostSearch>__10(this.<p>__4);
                }
                this.<>f__this.returnQueue.Enqueue(this.<p>__4);
                this.<p>__4.AdvanceState(PathState.ReturnQueue);
                if (DateTime.UtcNow.Ticks <= this.<targetTick>__3)
                {
                    goto Label_0093;
                }
                this.$current = null;
                this.$PC = 3;
                goto Label_03CB;
                this.$PC = -1;
            Label_03C9:
                return false;
            Label_03CB:
                return true;
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
        private sealed class <PathProcessor>c__AnonStorey247
        {
            internal PathProcessor <>f__this;
            internal int threadIndex;

            internal void <>m__16()
            {
                this.<>f__this.CalculatePathsThreaded(this.<>f__this.threadInfos[this.threadIndex]);
            }
        }
    }
}

