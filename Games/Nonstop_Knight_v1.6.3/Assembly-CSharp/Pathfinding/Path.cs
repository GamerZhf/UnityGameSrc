namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using UnityEngine;

    public abstract class Path
    {
        private string _errorLog = string.Empty;
        [CompilerGenerated]
        private DateTime <callTime>k__BackingField;
        [CompilerGenerated]
        private PathHandler <pathHandler>k__BackingField;
        [CompilerGenerated]
        private ushort <pathID>k__BackingField;
        public OnPathDelegate callback;
        private List<object> claimed = new List<object>();
        protected PathNode currentR;
        public float duration;
        public int enabledTags = -1;
        protected bool hasBeenReset;
        public Heuristic heuristic;
        public float heuristicScale = 1f;
        protected Int3 hTarget;
        protected GraphNode hTargetNode;
        public OnPathDelegate immediateCallback;
        protected int[] internalTagPenalties;
        protected int[] manualTagPenalties;
        protected float maxFrameTime;
        internal Path next;
        public NNConstraint nnConstraint = PathNNConstraint.Default;
        public List<GraphNode> path;
        private PathCompleteState pathCompleteState;
        internal bool pooled;
        private bool releasedNotSilent;
        public int searchedNodes;
        public int searchIterations;
        private PathState state;
        private object stateLock = new object();
        public List<Vector3> vectorPath;
        private static readonly int[] ZeroTagPenalties = new int[0x20];

        protected Path()
        {
        }

        public void AdvanceState(PathState s)
        {
            object stateLock = this.stateLock;
            lock (stateLock)
            {
                this.state = (PathState) Math.Max((int) this.state, (int) s);
            }
        }

        public uint CalculateHScore(GraphNode node)
        {
            uint num;
            switch (this.heuristic)
            {
                case Heuristic.Manhattan:
                {
                    Int3 position = node.position;
                    int introduced7 = Math.Abs((int) (this.hTarget.x - position.x));
                    num = (uint) (((introduced7 + Math.Abs((int) (this.hTarget.y - position.y))) + Math.Abs((int) (this.hTarget.z - position.z))) * this.heuristicScale);
                    if (this.hTargetNode != null)
                    {
                        num = Math.Max(num, AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
                    }
                    return num;
                }
                case Heuristic.DiagonalManhattan:
                {
                    Int3 num3 = this.GetHTarget() - node.position;
                    num3.x = Math.Abs(num3.x);
                    num3.y = Math.Abs(num3.y);
                    num3.z = Math.Abs(num3.z);
                    int num4 = Math.Min(num3.x, num3.z);
                    int num5 = Math.Max(num3.x, num3.z);
                    num = (uint) (((((14 * num4) / 10) + (num5 - num4)) + num3.y) * this.heuristicScale);
                    if (this.hTargetNode != null)
                    {
                        num = Math.Max(num, AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
                    }
                    return num;
                }
                case Heuristic.Euclidean:
                {
                    Int3 num6 = this.GetHTarget() - node.position;
                    num = (uint) (num6.costMagnitude * this.heuristicScale);
                    if (this.hTargetNode != null)
                    {
                        num = Math.Max(num, AstarPath.active.euclideanEmbedding.GetHeuristic(node.NodeIndex, this.hTargetNode.NodeIndex));
                    }
                    return num;
                }
            }
            return 0;
        }

        public abstract void CalculateStep(long targetTick);
        public bool CanTraverse(GraphNode node)
        {
            return (node.Walkable && (((this.enabledTags >> node.Tag) & 1) != 0));
        }

        public void Claim(object o)
        {
            if (object.ReferenceEquals(o, null))
            {
                throw new ArgumentNullException("o");
            }
            for (int i = 0; i < this.claimed.Count; i++)
            {
                if (object.ReferenceEquals(this.claimed[i], o))
                {
                    throw new ArgumentException("You have already claimed the path with that object (" + o + "). Are you claiming the path with the same object twice?");
                }
            }
            this.claimed.Add(o);
        }

        public virtual void Cleanup()
        {
        }

        public virtual string DebugString(PathLog logMode)
        {
            if ((logMode == PathLog.None) || (!this.error && (logMode == PathLog.OnlyErrors)))
            {
                return string.Empty;
            }
            StringBuilder debugStringBuilder = this.pathHandler.DebugStringBuilder;
            debugStringBuilder.Length = 0;
            this.DebugStringPrefix(logMode, debugStringBuilder);
            this.DebugStringSuffix(logMode, debugStringBuilder);
            return debugStringBuilder.ToString();
        }

        protected void DebugStringPrefix(PathLog logMode, StringBuilder text)
        {
            text.Append(!this.error ? "Path Completed : " : "Path Failed : ");
            text.Append("Computation Time ");
            text.Append(this.duration.ToString((logMode != PathLog.Heavy) ? "0.00 ms " : "0.000 ms "));
            text.Append("Searched Nodes ").Append(this.searchedNodes);
            if (!this.error)
            {
                text.Append(" Path Length ");
                text.Append((this.path != null) ? this.path.Count.ToString() : "Null");
                if (logMode == PathLog.Heavy)
                {
                    text.Append("\nSearch Iterations ").Append(this.searchIterations);
                }
            }
        }

        protected void DebugStringSuffix(PathLog logMode, StringBuilder text)
        {
            if (this.error)
            {
                text.Append("\nError: ").Append(this.errorLog);
            }
            if ((logMode == PathLog.Heavy) && !AstarPath.active.IsUsingMultithreading)
            {
                text.Append("\nCallback references ");
                if (this.callback != null)
                {
                    text.Append(this.callback.Target.GetType().FullName).AppendLine();
                }
                else
                {
                    text.AppendLine("NULL");
                }
            }
            text.Append("\nPath Number ").Append(this.pathID).Append(" (unique id)");
        }

        public void Error()
        {
            this.CompleteState = PathCompleteState.Error;
        }

        private void ErrorCheck()
        {
            if (!this.hasBeenReset)
            {
                throw new Exception("The path has never been reset. Use pooling API or call Reset() after creating the path with the default constructor.");
            }
            if (this.pooled)
            {
                throw new Exception("The path is currently in a path pool. Are you sending the path for calculation twice?");
            }
            if (this.pathHandler == null)
            {
                throw new Exception("Field pathHandler is not set. Please report this bug.");
            }
            if (this.GetState() > PathState.Processing)
            {
                throw new Exception("This path has already been processed. Do not request a path with the same path object twice.");
            }
        }

        internal void ForceLogError(string msg)
        {
            this.Error();
            this._errorLog = this._errorLog + msg;
            UnityEngine.Debug.LogError(msg);
        }

        public virtual uint GetConnectionSpecialCost(GraphNode a, GraphNode b, uint currentCost)
        {
            return currentCost;
        }

        public Int3 GetHTarget()
        {
            return this.hTarget;
        }

        public PathState GetState()
        {
            return this.state;
        }

        public uint GetTagPenalty(int tag)
        {
            return (uint) this.internalTagPenalties[tag];
        }

        public float GetTotalLength()
        {
            if (this.vectorPath == null)
            {
                return float.PositiveInfinity;
            }
            float num = 0f;
            for (int i = 0; i < (this.vectorPath.Count - 1); i++)
            {
                num += Vector3.Distance(this.vectorPath[i], this.vectorPath[i + 1]);
            }
            return num;
        }

        public uint GetTraversalCost(GraphNode node)
        {
            return (this.GetTagPenalty((int) node.Tag) + node.Penalty);
        }

        protected bool HasExceededTime(int searchedNodes, long targetTime)
        {
            return (DateTime.UtcNow.Ticks >= targetTime);
        }

        public abstract void Initialize();
        public bool IsDone()
        {
            return (this.CompleteState != PathCompleteState.NotCalculated);
        }

        internal void Log(string msg)
        {
            if (AstarPath.active.logPathResults != PathLog.None)
            {
                this._errorLog = this._errorLog + msg;
            }
        }

        [Conditional("DISABLED")]
        internal void LogError(string msg)
        {
            if (AstarPath.active.logPathResults != PathLog.None)
            {
                this._errorLog = this._errorLog + msg;
            }
            if ((AstarPath.active.logPathResults != PathLog.None) && (AstarPath.active.logPathResults != PathLog.InGame))
            {
                UnityEngine.Debug.LogWarning(msg);
            }
        }

        public virtual void OnEnterPool()
        {
            if (this.vectorPath != null)
            {
                ListPool<Vector3>.Release(this.vectorPath);
            }
            if (this.path != null)
            {
                ListPool<GraphNode>.Release(this.path);
            }
            this.vectorPath = null;
            this.path = null;
        }

        public abstract void Prepare();
        internal void PrepareBase(PathHandler pathHandler)
        {
            if (pathHandler.PathID > this.pathID)
            {
                pathHandler.ClearPathIDs();
            }
            this.pathHandler = pathHandler;
            pathHandler.InitializeForPath(this);
            if ((this.internalTagPenalties == null) || (this.internalTagPenalties.Length != 0x20))
            {
                this.internalTagPenalties = ZeroTagPenalties;
            }
            try
            {
                this.ErrorCheck();
            }
            catch (Exception exception)
            {
                this.ForceLogError(string.Concat(new object[] { "Exception in path ", this.pathID, "\n", exception }));
            }
        }

        public void Release(object o, [Optional, DefaultParameterValue(false)] bool silent)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            for (int i = 0; i < this.claimed.Count; i++)
            {
                if (object.ReferenceEquals(this.claimed[i], o))
                {
                    this.claimed.RemoveAt(i);
                    if (!silent)
                    {
                        this.releasedNotSilent = true;
                    }
                    if ((this.claimed.Count == 0) && this.releasedNotSilent)
                    {
                        PathPool.Pool(this);
                    }
                    return;
                }
            }
            if (this.claimed.Count == 0)
            {
                throw new ArgumentException("You are releasing a path which is not claimed at all (most likely it has been pooled already). Are you releasing the path with the same object (" + o + ") twice?\nCheck out the documentation on path pooling for help.");
            }
            throw new ArgumentException("You are releasing a path which has not been claimed with this object (" + o + "). Are you releasing the path with the same object twice?\nCheck out the documentation on path pooling for help.");
        }

        [Obsolete("Use Release(o, true) instead")]
        public void ReleaseSilent(object o)
        {
            this.Release(o, true);
        }

        public virtual void Reset()
        {
            if (object.ReferenceEquals(AstarPath.active, null))
            {
                throw new NullReferenceException("No AstarPath object found in the scene. Make sure there is one or do not create paths in Awake");
            }
            this.hasBeenReset = true;
            this.state = PathState.Created;
            this.releasedNotSilent = false;
            this.pathHandler = null;
            this.callback = null;
            this._errorLog = string.Empty;
            this.pathCompleteState = PathCompleteState.NotCalculated;
            this.path = ListPool<GraphNode>.Claim();
            this.vectorPath = ListPool<Vector3>.Claim();
            this.currentR = null;
            this.duration = 0f;
            this.searchIterations = 0;
            this.searchedNodes = 0;
            this.nnConstraint = PathNNConstraint.Default;
            this.next = null;
            this.heuristic = AstarPath.active.heuristic;
            this.heuristicScale = AstarPath.active.heuristicScale;
            this.enabledTags = -1;
            this.tagPenalties = null;
            this.callTime = DateTime.UtcNow;
            this.pathID = AstarPath.active.GetNextPathID();
            this.hTarget = Int3.zero;
            this.hTargetNode = null;
        }

        public virtual void ReturnPath()
        {
            if (this.callback != null)
            {
                this.callback(this);
            }
        }

        protected virtual void Trace(PathNode from)
        {
            PathNode parent = from;
            int num = 0;
            while (parent != null)
            {
                parent = parent.parent;
                num++;
                if (num > 0x800)
                {
                    UnityEngine.Debug.LogWarning("Infinite loop? >2048 node path. Remove this message if you really have that long paths (Path.cs, Trace method)");
                    break;
                }
            }
            if (this.path.Capacity < num)
            {
                this.path.Capacity = num;
            }
            if (this.vectorPath.Capacity < num)
            {
                this.vectorPath.Capacity = num;
            }
            parent = from;
            for (int i = 0; i < num; i++)
            {
                this.path.Add(parent.node);
                parent = parent.parent;
            }
            int num3 = num / 2;
            for (int j = 0; j < num3; j++)
            {
                GraphNode node2 = this.path[j];
                this.path[j] = this.path[(num - j) - 1];
                this.path[(num - j) - 1] = node2;
            }
            for (int k = 0; k < num; k++)
            {
                this.vectorPath.Add((Vector3) this.path[k].position);
            }
        }

        [DebuggerHidden]
        public IEnumerator WaitForPath()
        {
            <WaitForPath>c__IteratorC rc = new <WaitForPath>c__IteratorC();
            rc.<>f__this = this;
            return rc;
        }

        public DateTime callTime
        {
            [CompilerGenerated]
            get
            {
                return this.<callTime>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<callTime>k__BackingField = value;
            }
        }

        public PathCompleteState CompleteState
        {
            get
            {
                return this.pathCompleteState;
            }
            protected set
            {
                this.pathCompleteState = value;
            }
        }

        public bool error
        {
            get
            {
                return (this.CompleteState == PathCompleteState.Error);
            }
        }

        public string errorLog
        {
            get
            {
                return this._errorLog;
            }
        }

        public virtual bool FloodingPath
        {
            get
            {
                return false;
            }
        }

        public PathHandler pathHandler
        {
            [CompilerGenerated]
            get
            {
                return this.<pathHandler>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<pathHandler>k__BackingField = value;
            }
        }

        public ushort pathID
        {
            [CompilerGenerated]
            get
            {
                return this.<pathID>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<pathID>k__BackingField = value;
            }
        }

        [Obsolete("Has been renamed to 'pooled' to use more widely underestood terminology")]
        internal bool recycled
        {
            get
            {
                return this.pooled;
            }
            set
            {
                this.pooled = value;
            }
        }

        public int[] tagPenalties
        {
            get
            {
                return this.manualTagPenalties;
            }
            set
            {
                if ((value == null) || (value.Length != 0x20))
                {
                    this.manualTagPenalties = null;
                    this.internalTagPenalties = ZeroTagPenalties;
                }
                else
                {
                    this.manualTagPenalties = value;
                    this.internalTagPenalties = value;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <WaitForPath>c__IteratorC : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Path <>f__this;

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
                        if (this.<>f__this.GetState() == PathState.Created)
                        {
                            throw new InvalidOperationException("This path has not been started yet");
                        }
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_006C;
                }
                while (this.<>f__this.GetState() != PathState.Returned)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.$PC = -1;
            Label_006C:
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
    }
}

