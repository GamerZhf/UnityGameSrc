namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class EscapeSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorCE rce = new <ExecuteRoutine>c__IteratorCE();
            rce.c = c;
            rce.executionStats = executionStats;
            rce.<$>c = c;
            rce.<$>executionStats = executionStats;
            return rce;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorCE : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal IEnumerator <ie>__2;
            internal bool <midPointReached>__1;
            internal float <timer>__0;
            internal CharacterInstance c;
            internal SkillExecutionStats executionStats;

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
                        this.<timer>__0 = 0f;
                        this.<midPointReached>__1 = false;
                        this.<ie>__2 = this.c.PhysicsBody.leapBackRoutine(0f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00ED;
                }
                if (this.<ie>__2.MoveNext())
                {
                    this.<timer>__0 += Time.fixedDeltaTime;
                    if (!this.<midPointReached>__1 && (this.<timer>__0 >= 0.225f))
                    {
                        Binder.EventBus.CharacterSkillExecutionMidpoint(this.c, SkillType.Escape, this.executionStats);
                        Binder.DeathSystem.killCharacter(this.c, null, false, false, SkillType.Escape);
                        this.<midPointReached>__1 = true;
                    }
                    this.$current = this.<ie>__2.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_00ED;
                this.$PC = -1;
            Label_00ED:
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

