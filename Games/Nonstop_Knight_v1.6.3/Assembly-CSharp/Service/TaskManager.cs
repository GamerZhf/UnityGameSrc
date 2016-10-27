namespace Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class TaskManager : MonoBehaviour
    {
        [DebuggerHidden]
        public IEnumerator ExecStacked(IEnumerator meth, ExceptionActionDelegate onException)
        {
            <ExecStacked>c__Iterator235 iterator = new <ExecStacked>c__Iterator235();
            iterator.meth = meth;
            iterator.onException = onException;
            iterator.<$>meth = meth;
            iterator.<$>onException = onException;
            return iterator;
        }

        public Coroutine StartTask(IEnumerator routine, [Optional, DefaultParameterValue(null)] ExceptionActionDelegate onException)
        {
            return base.StartCoroutine(this.ExecStacked(routine, onException));
        }

        [CompilerGenerated]
        private sealed class <ExecStacked>c__Iterator235 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal IEnumerator <$>meth;
            internal TaskManager.ExceptionActionDelegate <$>onException;
            internal object <curr>__3;
            internal Exception <ex>__2;
            internal IEnumerator <running>__1;
            internal Stack<IEnumerator> <stack>__0;
            internal IEnumerator meth;
            internal TaskManager.ExceptionActionDelegate onException;

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
                        this.<stack>__0 = new Stack<IEnumerator>();
                        this.<stack>__0.Push(this.meth);
                        goto Label_0121;

                    case 1:
                        if (this.<curr>__3 is IEnumerator)
                        {
                            this.<stack>__0.Push(this.<curr>__3 as IEnumerator);
                            this.<running>__1 = this.<curr>__3 as IEnumerator;
                        }
                        break;

                    default:
                        goto Label_0139;
                }
            Label_0053:
                try
                {
                    if (!this.<running>__1.MoveNext())
                    {
                        goto Label_0115;
                    }
                }
                catch (Exception exception)
                {
                    this.<ex>__2 = exception;
                    if ((this.onException != null) && !this.onException(this.<ex>__2))
                    {
                        goto Label_0139;
                    }
                    UnityEngine.Debug.LogError(this.<ex>__2);
                    goto Label_0115;
                }
                this.<curr>__3 = this.<running>__1.Current;
                this.$current = this.<curr>__3;
                this.$PC = 1;
                return true;
            Label_0115:
                this.<stack>__0.Pop();
            Label_0121:
                if (this.<stack>__0.Count > 0)
                {
                    this.<running>__1 = this.<stack>__0.Peek();
                    goto Label_0053;
                }
                this.$PC = -1;
            Label_0139:
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

        public delegate bool ExceptionActionDelegate(Exception ex);
    }
}

