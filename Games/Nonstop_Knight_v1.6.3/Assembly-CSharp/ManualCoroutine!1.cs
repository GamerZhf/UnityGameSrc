using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ManualCoroutine<T>
{
    [CompilerGenerated]
    private bool <done>k__BackingField;
    private Stack<IEnumerator> m_stack;

    public ManualCoroutine(IEnumerator c)
    {
        this.m_stack = new Stack<IEnumerator>();
        this.done = false;
        this.m_stack.Clear();
        this.m_stack.Push(c);
    }

    public object Resume()
    {
        if (!this.m_stack.Peek().MoveNext())
        {
            this.done = true;
            return this.m_stack.Peek().Current;
        }
    Label_002D:
        while (this.m_stack.Peek().Current is IEnumerator)
        {
            this.m_stack.Push((IEnumerator) this.m_stack.Peek().Current);
            if (!this.m_stack.Peek().MoveNext())
            {
                this.done = true;
                return this.m_stack.Peek().Current;
            }
        }
        if (this.m_stack.Peek().Current == null)
        {
            this.m_stack.Pop();
            if (this.m_stack.Count == 0)
            {
                this.done = true;
                return null;
            }
            if (!this.m_stack.Peek().MoveNext())
            {
                this.done = true;
                return this.m_stack.Peek().Current;
            }
            goto Label_002D;
        }
        return (T) this.m_stack.Peek().Current;
    }

    public bool done
    {
        [CompilerGenerated]
        get
        {
            return this.<done>k__BackingField;
        }
        [CompilerGenerated]
        private set
        {
            this.<done>k__BackingField = value;
        }
    }
}

