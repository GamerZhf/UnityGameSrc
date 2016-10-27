using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TransformAnimation : MonoBehaviour
{
    [CompilerGenerated]
    private RectTransform <RectTm>k__BackingField;
    [CompilerGenerated]
    private Transform <Tm>k__BackingField;
    private int m_lastId;
    private bool m_paused;
    private Queue<int> m_queue = new Queue<int>();
    private Dictionary<int, TransformAnimationTask> m_tasks = new Dictionary<int, TransformAnimationTask>();
    private bool m_useFixedUpdate;

    public int addTask(TransformAnimationTask animationTask)
    {
        this.m_lastId++;
        this.m_tasks.Add(this.m_lastId, animationTask);
        this.m_queue.Enqueue(this.m_lastId);
        return this.m_lastId;
    }

    protected void Awake()
    {
        this.Tm = base.transform;
        this.RectTm = base.GetComponent<RectTransform>();
    }

    protected void FixedUpdate()
    {
        if (this.m_useFixedUpdate)
        {
            this.tick();
        }
    }

    protected void OnDisable()
    {
        this.m_paused = false;
        this.stopAll();
    }

    public void pause()
    {
        this.m_paused = true;
    }

    private int peekNext()
    {
        while (this.m_queue.Count > 0)
        {
            int key = this.m_queue.Peek();
            if (this.m_tasks.ContainsKey(key))
            {
                return key;
            }
            this.m_queue.Dequeue();
        }
        return -1;
    }

    public void play()
    {
        this.m_paused = false;
    }

    public void stopAll()
    {
        foreach (KeyValuePair<int, TransformAnimationTask> pair in this.m_tasks)
        {
            if (pair.Value.CancelCallback != null)
            {
                pair.Value.CancelCallback(pair.Key);
            }
        }
        this.m_tasks.Clear();
        this.m_queue.Clear();
    }

    public void stopTask(int id)
    {
        TransformAnimationTask task;
        this.m_tasks.TryGetValue(id, out task);
        if (task != null)
        {
            if (task.CancelCallback != null)
            {
                task.CancelCallback(id);
            }
            this.m_tasks.Remove(id);
        }
    }

    public void stopTask(TransformAnimationTask animationTask)
    {
        foreach (KeyValuePair<int, TransformAnimationTask> pair in this.m_tasks)
        {
            if (pair.Value.Equals(this.m_tasks))
            {
                if (pair.Value.CancelCallback != null)
                {
                    pair.Value.CancelCallback(pair.Key);
                }
                this.m_tasks.Remove(pair.Key);
                break;
            }
        }
    }

    private void tick()
    {
        if (!this.m_paused)
        {
            int key = this.peekNext();
            if (key != -1)
            {
                TransformAnimationTask task = this.m_tasks[key];
                float progress = task.Progress;
                task.apply();
                if (((progress == 0f) && (task.Progress > 0f)) && (task.StartCallback != null))
                {
                    task.StartCallback(this);
                }
                if (task.Completed)
                {
                    this.m_tasks.Remove(key);
                    this.m_queue.Dequeue();
                    if (task.EndCallback != null)
                    {
                        task.EndCallback(this);
                    }
                }
            }
        }
    }

    protected void Update()
    {
        if (!this.m_useFixedUpdate)
        {
            this.tick();
        }
    }

    public TransformAnimationTask activeOrNextAnimationTask
    {
        get
        {
            int num = this.peekNext();
            return ((num == -1) ? null : this.m_tasks[num]);
        }
    }

    public bool HasTasks
    {
        get
        {
            return (this.m_queue.Count > 0);
        }
    }

    public RectTransform RectTm
    {
        [CompilerGenerated]
        get
        {
            return this.<RectTm>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<RectTm>k__BackingField = value;
        }
    }

    public Transform Tm
    {
        [CompilerGenerated]
        get
        {
            return this.<Tm>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<Tm>k__BackingField = value;
        }
    }

    public bool UseFixedUpdate
    {
        get
        {
            return this.m_useFixedUpdate;
        }
        set
        {
            this.m_useFixedUpdate = value;
        }
    }
}

