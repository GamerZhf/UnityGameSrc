namespace Pathfinding
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class WorkItemProcessor : IWorkItemContext
    {
        [CompilerGenerated]
        private bool <workItemsInProgress>k__BackingField;
        private readonly AstarPath astar;
        private bool queuedWorkItemFloodFill;
        private readonly IndexedQueue<AstarWorkItem> workItems = new IndexedQueue<AstarWorkItem>();
        private bool workItemsInProgressRightNow;

        public WorkItemProcessor(AstarPath astar)
        {
            this.astar = astar;
        }

        public void AddWorkItem(AstarWorkItem itm)
        {
            this.workItems.Enqueue(itm);
        }

        public void EnsureValidFloodFill()
        {
            if (this.queuedWorkItemFloodFill)
            {
                this.astar.FloodFill();
            }
        }

        public void OnFloodFill()
        {
            this.queuedWorkItemFloodFill = false;
        }

        void IWorkItemContext.QueueFloodFill()
        {
            this.queuedWorkItemFloodFill = true;
        }

        public bool ProcessWorkItems(bool force)
        {
            if (this.workItemsInProgressRightNow)
            {
                throw new Exception("Processing work items recursively. Please do not wait for other work items to be completed inside work items. If you think this is not caused by any of your scripts, this might be a bug.");
            }
            this.workItemsInProgressRightNow = true;
            while (this.workItems.Count > 0)
            {
                bool flag;
                if (!this.workItemsInProgress)
                {
                    this.workItemsInProgress = true;
                    this.queuedWorkItemFloodFill = false;
                }
                AstarWorkItem item = this.workItems[0];
                if (item.init != null)
                {
                    item.init();
                    item.init = null;
                }
                if (item.initWithContext != null)
                {
                    item.initWithContext(this);
                    item.initWithContext = null;
                }
                this.workItems[0] = item;
                try
                {
                    if (item.update != null)
                    {
                        flag = item.update(force);
                    }
                    else if (item.updateWithContext != null)
                    {
                        flag = item.updateWithContext(this, force);
                    }
                    else
                    {
                        flag = true;
                    }
                }
                catch
                {
                    this.workItems.Dequeue();
                    this.workItemsInProgressRightNow = false;
                    throw;
                }
                if (!flag)
                {
                    if (force)
                    {
                        Debug.LogError("Misbehaving WorkItem. 'force'=true but the work item did not complete.\nIf force=true is passed to a WorkItem it should always return true.");
                    }
                    this.workItemsInProgressRightNow = false;
                    return false;
                }
                this.workItems.Dequeue();
            }
            this.EnsureValidFloodFill();
            this.workItemsInProgressRightNow = false;
            this.workItemsInProgress = false;
            return true;
        }

        public bool workItemsInProgress
        {
            [CompilerGenerated]
            get
            {
                return this.<workItemsInProgress>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<workItemsInProgress>k__BackingField = value;
            }
        }

        private class IndexedQueue<T>
        {
            private T[] buffer;
            private int length;
            private int start;

            public IndexedQueue()
            {
                this.buffer = new T[4];
            }

            public T Dequeue()
            {
                if (this.length == 0)
                {
                    throw new InvalidOperationException();
                }
                T local = this.buffer[this.start];
                this.start = (this.start + 1) % this.buffer.Length;
                this.length--;
                return local;
            }

            public void Enqueue(T item)
            {
                if (this.length == this.buffer.Length)
                {
                    T[] localArray = new T[this.buffer.Length * 2];
                    for (int i = 0; i < this.length; i++)
                    {
                        localArray[i] = this[i];
                    }
                    this.buffer = localArray;
                    this.start = 0;
                }
                this.buffer[(this.start + this.length) % this.buffer.Length] = item;
                this.length++;
            }

            public int Count
            {
                get
                {
                    return this.length;
                }
            }

            public T this[int index]
            {
                get
                {
                    if ((index < 0) || (index >= this.length))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    return this.buffer[(this.start + index) % this.buffer.Length];
                }
                set
                {
                    if ((index < 0) || (index >= this.length))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    this.buffer[(this.start + index) % this.buffer.Length] = value;
                }
            }
        }
    }
}

